
using FileUpload.Data.Entity;
using FileUpload.Data.Repository;
using FileUpload.Shared.Models;
using FileUpload.Shared.Services;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FileUpload.Api.Services
{

    public class FileService : IFileService
    {
        private readonly HttpClient _client;
        private readonly IRepository<Data.Entity.Data> _repository;
        private readonly IConfiguration configuration;
        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly UserManager<ApplicationUser> _userManager;
        public FileService(HttpClient client, IRepository<Data.Entity.Data> repository, IConfiguration configuration, ISharedIdentityService sharedIdentityService, UserManager<ApplicationUser> userManager)
        {
            _client = client;
            _repository = repository;
            this.configuration = configuration;
            _sharedIdentityService = sharedIdentityService;
            _userManager = userManager;
        }

        public async Task<Response<bool>> DeleteAsyncByMetaId(string metaId)
        {

            //if (!_context.Data.Any(x => x.UserId == _sharedIdentityService.GetUserId && x.MetaDataId == metaId))
            //{
            //    return Response<bool>.Fail(false, 404);
            //}

            //var data = await _context.Data.Where(x => x.UserId == _sharedIdentityService.GetUserId && x.MetaDataId == metaId).FirstOrDefaultAsync();


            //_context.Data.Remove(data);
            //(await _userManager.FindByIdAsync(_sharedIdentityService.GetUserId)).UsedSpace -= data.Size;
            //_context.SaveChanges();

            if (!_repository.Any(x => x.UserId == _sharedIdentityService.GetUserId && x.MetaDataId == metaId))
            {
                return Response<bool>.Fail(false, 404);
            }

            var data = await _repository.FirstOrDefaultAsync(x => x.UserId == _sharedIdentityService.GetUserId && x.MetaDataId == metaId);

            _repository.Remove(data);
            (await _userManager.FindByIdAsync(_sharedIdentityService.GetUserId)).UsedSpace -= data.Size;

            return Response<bool>.Success(true, 200);


        }

        public async Task<Response<string>> GetDirectLinkByMetaId(string metaId)
        {
            if (!_repository.Any(x => x.UserId == _sharedIdentityService.GetUserId && x.MetaDataId == metaId))
            {
                return Response<string>.Fail("Böyle bir dosya bulunamadı", 404);
            }

            var shortUri = $"https://anonfiles.com/{metaId}";

            var response = await _client.GetAsync(shortUri);

            if (!response.IsSuccessStatusCode)
            {
                return Response<string>.Fail("Geçersiz URL", 500);
            }

            HtmlWeb web = new();
            var document = web.Load(shortUri);

            var xpath = "//*[@id='download-url']";

            var directLink = document.DocumentNode.SelectSingleNode(xpath).Attributes["href"].Value;

            return Response<string>.Success(directLink, 200);
        }

        public async Task<Response<List<MyFilesViewModel>>> GetMyFiles()
        {
            if (_repository.Any())
            {
                var filesList = await _repository.Where(x => x.UserId == _sharedIdentityService.GetUserId).Select(x => new MyFilesViewModel()
                {
                    FileId = x.MetaDataId,
                    FileName = x.Name

                }).ToListAsync();

                return Response<List<MyFilesViewModel>>.Success(filesList, 200);
            }

            return Response<List<MyFilesViewModel>>.Success(200);

        }

        public async Task<Response<UploadModel>> UploadAsync(IFormFile file)
        {
            if (file == null)
            {
                var failedModel = new UploadModel() { FileName = file.FileName };
                return Response<UploadModel>.Fail(failedModel, 500);
            }

            if (file.Length <= 0)
            {
                var failedModel = new UploadModel() { FileName = file.FileName };
                return Response<UploadModel>.Fail(failedModel, 500);
            }

            var token = configuration.GetSection("token").Value;
            var content = await Helper.GetMultipartContentAsync(file);

            var response = await _client.PostAsync($"https://api.anonfiles.com/upload?token={token}", content);

            if (!response.IsSuccessStatusCode)
            {
                var failedModel = new UploadModel() { FileName = file.FileName };

                return Response<UploadModel>.Fail(failedModel, (int)response.StatusCode);
            }

            var successData = JsonConvert.DeserializeObject<UploadReturnModel>(await response.Content.ReadAsStringAsync());

            Data.Entity.Data dataEntity = new()
            {
                MetaDataId = successData.data.file.metadata.id,
                Name = successData.data.file.metadata.name,
                ShortUri = successData.data.file.url.@short,
                Size = successData.data.file.metadata.size.bytes,
                UserId = _sharedIdentityService.GetUserId
            };

            (await _userManager.FindByIdAsync(_sharedIdentityService.GetUserId)).UsedSpace += dataEntity.Size;

            await _repository.AddAsync(dataEntity);

            var model = new UploadModel() { FileId = dataEntity.MetaDataId, FileName = dataEntity.Name };
            return Response<UploadModel>.Success(model, (int)response.StatusCode);
        }

    }





}
