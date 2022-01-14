using AnonFilesUpload.Api.Models;
using AnonFilesUpload.Data.Entity;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AnonFilesUpload.Api.Services
{

    public class FileUploadDto
    {
        public IFormFile file { get; set; }
    }

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

        private async  Task<MultipartContent> GetMultipartContentAsync(IFormFile file, string key)
        {
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                var multipartContent = new MultipartFormDataContent();
                multipartContent.Add(new ByteArrayContent(ms.ToArray()), key , file.FileName);

                return multipartContent;
            };
        }

        public async Task<Response<UploadReturnModel>> UploadAsync(IFormFile file)
        {

            var multipartContent = await GetMultipartContentAsync(file, "file");

            var token = configuration.GetSection("token").Value;

            var response = await _client.PostAsync($"https://api.anonfiles.com/upload?token={token}", multipartContent);

            if (response.IsSuccessStatusCode)
            {
                 var successData = JsonConvert.DeserializeObject<UploadReturnModel>(await response.Content.ReadAsStringAsync());

                Data.Entity.Data dataEntity = new()
                {
                    MetaDataId = successData.data.file.metadata.id,
                    Name = successData.data.file.metadata.name,
                    ShortUri = successData.data.file.url.@short,
                    DirectLink = await GetDirectLinkAsync(successData.data.file.url.@short),
                };

                await _context.Data.AddAsync(dataEntity);
                await _context.SaveChangesAsync();


                return Response<UploadReturnModel>.Success(successData, 200);

            }

            var faiedData = JsonConvert.DeserializeObject<UploadReturnFailModel>(await response.Content.ReadAsStringAsync());

            return Response<UploadReturnModel>.Fail(faiedData.error.message, 404);
            
        }

        public async Task<string> GetDirectLinkAsync(string shortUri)
        {

            var response = await _client.GetAsync(shortUri);

            if(response.IsSuccessStatusCode)
            {
                HtmlWeb web = new HtmlWeb();
                var document = web.Load(shortUri);

                var xpath = "//*[@id='download-url']";

                var res1 = document.DocumentNode.SelectSingleNode(xpath).Attributes["href"].Value;

                return await Task.FromResult(res1);
            }

            return await Task.FromResult("");
        }

        public async Task<Response<string>> DirectLinkControl(Data.Entity.Data data)
        {

            var newDirectLink = await GetDirectLinkAsync(data.ShortUri);

            if (newDirectLink == "")
            {
                return Response<string>.Success($"DirectLink alınamadı. Lütfen {data.ShortUri} linkinin güncel olduğundan emin olun", 200);
            }

            if (data.DirectLink!= newDirectLink)
            {

                data.DirectLink = newDirectLink;
                await _context.SaveChangesAsync();

                return Response<string>.Success( $"indirme linki {newDirectLink} olarak güncellendi", 200);
            }

            return Response<string>.Success("DirectLink zaten günceldir", 200);
        }

        //public async Task<Entity.Data> GetDataByUri(string directLink)
        //{
        //    return await _context.Data.FirstOrDefaultAsync(x => x.DirectLink == directLink);
        //}

    }
}
