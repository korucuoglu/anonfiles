using FileUpload.Api.UnitTests.TestSetup;
using IdentityModel.Client;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace FileUpload.Api.UnitTests.Api.DataController
{
    public class DataControllerTest : IClassFixture<CommonTestFixture>
    {
        private readonly HttpClient _client;

        private readonly ClientCredentialsTokenRequest clientCredentialTokenRequest = new();
        private readonly PasswordTokenRequest passwordTokenRequest = new();

        public DataControllerTest(CommonTestFixture testFixture)
        {
            _client = testFixture.Client;

            var disco = _client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = "http://localhost:5001",
                Policy = new DiscoveryPolicy { RequireHttps = false }
            }).Result;

            clientCredentialTokenRequest = new ClientCredentialsTokenRequest
            {
                ClientId = "MVCClient",
                ClientSecret = "secret",
                Address = disco.TokenEndpoint
            };

            passwordTokenRequest = new PasswordTokenRequest
            {
                ClientId = "WebMvcClientForUser",
                ClientSecret = "secret",
                UserName = "test@gmail.com",
                Password = "Test16.,",
                Address = disco.TokenEndpoint
            };

        }

        [Fact]
        public async Task ÜyeGirişiYapılmadanFilesİstekYapıldığında401GeriyeDön()
        {
            var data = await _client.GetAsync("http://localhost:5002/api/data/myfiles");

            Assert.Equal(401, (int)data.StatusCode);
        }

        [Fact]
        public async Task ÜyeGirişiYapıldığındaDosyalaraGittiğindeGeriyeOkDön()
        {
            var clientCredentialToken = await _client.RequestClientCredentialsTokenAsync(clientCredentialTokenRequest);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", clientCredentialToken.AccessToken);

            Assert.Equal(200, (int)clientCredentialToken.HttpStatusCode);

            var PasswordToken = await _client.RequestPasswordTokenAsync(passwordTokenRequest);

            Assert.Equal(200, (int)PasswordToken.HttpStatusCode);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", PasswordToken.AccessToken);

            var response = await _client.GetAsync("http://localhost:5002/api/data/myfiles");

            Assert.Equal(200, (int)response.StatusCode);
        }
    }
}
