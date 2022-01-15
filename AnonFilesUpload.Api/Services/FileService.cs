
using AnonFilesUpload.Data.Entity;
using AnonFilesUpload.Data.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace AnonFilesUpload.Api.Services
{


    public class FileService
    {

        private static HttpClient _client;
        private readonly DataContext _context;
        private readonly IConfiguration configuration;

        public FileService(HttpClient client, DataContext context, IConfiguration configuration)
        {
            _client = client;
            _context = context;
            this.configuration = configuration;
        }

        private async Task<MultipartContent> GetMultipartContentAsync(IFormFile file, string key)
        {
            using (var ms = new MemoryStream())
            {
                var multipartContent = new MultipartFormDataContent();
                await file.CopyToAsync(ms);
                multipartContent.Add(new ByteArrayContent(ms.ToArray()), key, file.FileName);

                return multipartContent;
            };
        }

        public async Task<AjaxReturningModel> UploadAsync(IFormFile file)
        {
            var token = configuration.GetSection("token").Value;

            if (file == null || file.Length < 0)
            {
                var model = new AjaxReturningModel() { fileName = file.FileName, success = false , message = $"{file.FileName} boş olamaz" };

                return model;
            }

            var multipartContent = await GetMultipartContentAsync(file, "file");

            var response = await _client.PostAsync($"https://api.anonfiles.com/upload?token={token}", multipartContent);

            if (response.IsSuccessStatusCode)
            {
                var successData = JsonConvert.DeserializeObject<UploadReturnModel>(await response.Content.ReadAsStringAsync());

                Data.Entity.Data dataEntity = new()
                {
                    MetaDataId = successData.data.file.metadata.id,
                    Name = successData.data.file.metadata.name,
                    ShortUri = successData.data.file.url.@short,
                    Size = successData.data.file.metadata.size.bytes
                };

                await _context.Data.AddAsync(dataEntity);
                await _context.SaveChangesAsync();

                var model = new AjaxReturningModel() { fileId = dataEntity.MetaDataId, fileName = dataEntity.Name, success = true, message = $"{file.FileName} başarıyla yüklendi." };
                return model;

            }
            else
            {
                var faiedData = JsonConvert.DeserializeObject<UploadReturnFailModel>(await response.Content.ReadAsStringAsync());

                var model = new AjaxReturningModel() { fileName = file.FileName, success = false, message = faiedData.error.message };
                return model;
            }
           
        }

        public async Task<string> GetDirectLinkAsync(string shortUri)
        {

            var response = await _client.GetAsync(shortUri);

            if (response.IsSuccessStatusCode)
            {

                HtmlWeb web = new HtmlWeb();
                var document = web.Load(shortUri);

                var xpath = "//*[@id='download-url']";

                var res1 = document.DocumentNode.SelectSingleNode(xpath).Attributes["href"].Value;

                return await Task.FromResult(res1);
            }

            return await Task.FromResult("");
        }


        public async Task<Response<T>> GetById<T>(int id) where T : BaseEntity
        {
            var _table = _context.Set<T>();
            var data = await _table.FirstOrDefaultAsync(x => x.Id == id);

            return Response<T>.Success(data, 200);
        }

        public async Task<Response<NoContent>> DeleteByIdAsync<T>(int id) where T : BaseEntity
        {
            var entity = await GetById<T>(id);

            if (entity.Data != null)
            {
                _context.Set<T>().Remove(entity.Data);
                await _context.SaveChangesAsync();
                return Response<NoContent>.Success(200);
            }

            return Response<NoContent>.Fail($"{id} değerine sahip veri bulunamadı", 500);
        }

        //public async Task<Response<string>> DirectLinkControl(Data.Entity.Data data)
        //{

        //    var newDirectLink = await GetDirectLinkAsync(data.ShortUri);

        //    if (newDirectLink == "")
        //    {
        //        return Response<string>.Success($"DirectLink alınamadı. Lütfen {data.ShortUri} linkinin güncel olduğundan emin olun", 200);
        //    }

        //    if (data.DirectLink != newDirectLink)
        //    {

        //        data.DirectLink = newDirectLink;
        //        await _context.SaveChangesAsync();

        //        return Response<string>.Success($"indirme linki {newDirectLink} olarak güncellendi", 200);
        //    }

        //    return Response<string>.Success("DirectLink zaten günceldir", 200);
        //}

        //public async Task<Response<UploadReturnModel>> UploadAsync(IFormFile file)
        //{
        //    var token = configuration.GetSection("token").Value;

        //    var multipartContent = await GetMultipartContentAsync(file, "file");

        //    var response = await _client.PostAsync($"https://api.anonfiles.com/upload?token={token}", multipartContent);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var successData = JsonConvert.DeserializeObject<UploadReturnModel>(await response.Content.ReadAsStringAsync());

        //        Data.Entity.Data dataEntity = new()
        //        {
        //            MetaDataId = successData.data.file.metadata.id,
        //            Name = successData.data.file.metadata.name,
        //            ShortUri = successData.data.file.url.@short,
        //            Size = successData.data.file.metadata.size.bytes
        //        };
        //        await _context.Data.AddAsync(dataEntity);
        //        await _context.SaveChangesAsync();

        //        return Response<UploadReturnModel>.Success(successData, 200);

        //    }

        //    var faiedData = JsonConvert.DeserializeObject<UploadReturnFailModel>(await response.Content.ReadAsStringAsync());

        //    return Response<UploadReturnModel>.Fail(faiedData.error.message, 404);

        //}


    }
}
