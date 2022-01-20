
using AnonFilesUpload.Data.Entity;
using AnonFilesUpload.Shared;
using AnonFilesUpload.Shared.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AnonFilesUpload.Api.Services
{

    public interface IFileService
    {
        public Task<Response<UploadModel>> UploadAsync(IFormFile file);
        public Task<Response<DataViewModel>> GetFilesByUserId();
        public Task<Response<string>> GetDirectLinkByMetaId(string metaId);
        public Task<Response<bool>> DeleteAsyncByMetaId(string metaId);

    }

    public class FileService : IFileService
    {
        private static HttpClient _client;
        private readonly DataContext _context;
        private readonly IConfiguration configuration;
        private readonly ISharedIdentityService _sharedIdentityService;
        public FileService(HttpClient client, DataContext context, IConfiguration configuration, ISharedIdentityService sharedIdentityService)
        {
            _client = client;
            _context = context;
            this.configuration = configuration;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<Response<bool>> DeleteAsyncByMetaId(string metaId)
        {
            if (!_context.Data.Any(x => x.UserId == _sharedIdentityService.GetUserId && x.MetaDataId == metaId))
            {
                return Response<bool>.Fail(false, 404);
            }

            var data = await _context.Data.Where(x => x.UserId == _sharedIdentityService.GetUserId && x.MetaDataId == metaId).FirstOrDefaultAsync();

            _context.Remove(data);
            await _context.SaveChangesAsync();

            return Response<bool>.Success(true, 200);
        }

        public async Task<Response<string>> GetDirectLinkByMetaId(string metaId)
        {
            if (!_context.Data.Any(x => x.UserId == _sharedIdentityService.GetUserId && x.MetaDataId == metaId))
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

        public async Task<Response<DataViewModel>> GetFilesByUserId()
        {
            var data = await _context.Data.Where(x => x.UserId == _sharedIdentityService.GetUserId).Select(x => new DataModel()
            {
                FileId = x.MetaDataId,
                FileName = x.Name,
                Size = x.Size

            }).ToListAsync();

            DataViewModel model = new()
            {
                DataModel = data,
                TotalSize = data.Sum(x => x.Size),
                UsedSpace = Helper.GetUsedSpace(data.Sum(x => x.Size)),
                RemainingSpace = Helper.GetRemainingSpace(data.Sum(x => x.Size)),

            };

            return Response<DataViewModel>.Success(model, 200);
        }

        public async Task<Response<UploadModel>> UploadAsync(IFormFile file)
        {
            if (file.Length < 0 || file == null)
            {
                return Response<UploadModel>.Fail($"Gönderilen dosya boş olamaz", 500);
            }

            var token = configuration.GetSection("token").Value;
            var content = await Helper.GetMultipartContentAsync(file);

            var response = await _client.PostAsync($"https://api.anonfiles.com/upload?token={token}", content);

            if (!response.IsSuccessStatusCode)
            {

                var failedModel = new UploadModel() { fileName = file.FileName, success = false };

                return Response<UploadModel>.Fail(failedModel, 500);



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

            await _context.Data.AddAsync(dataEntity);
            await _context.SaveChangesAsync();

            var model = new UploadModel() { fileId = dataEntity.MetaDataId, fileName = dataEntity.Name, success = true };
            return Response<UploadModel>.Success(model, 200);


        }
    }





}
