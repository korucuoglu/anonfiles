using FileUpload.MVC.Application.Dtos.Settings;
using FileUpload.MVC.Application.Interfaces.Services;
using FileUpload.Shared.Dtos.User;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FileUpload.MVC.Infrastructure.Services
{

    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClientSettings _clientSettings;
        private readonly ServiceApiSettings _serviceApiSettings;
        private readonly IClientCredentialTokenService _clientCredentialTokenService;

        public IdentityService(HttpClient client, IHttpContextAccessor httpContextAccessor, IOptions<ClientSettings> clientSettings, IOptions<ServiceApiSettings> serviceApiSettings, IClientCredentialTokenService clientCredentialTokenService)
        {
            _httpClient = client;
            _httpContextAccessor = httpContextAccessor;
            _clientSettings = clientSettings.Value;
            _serviceApiSettings = serviceApiSettings.Value;
            _clientCredentialTokenService = clientCredentialTokenService;
        }


        public async Task<TokenResponse> GetAccessTokenByRefreshToken()
        {
            var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.IdentityBaseUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            if (disco.IsError)
            {
                throw disco.Exception;
            }

            var refreshToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            if (refreshToken == null)
            {
                return null;
            }

            RefreshTokenRequest refreshTokenRequest = new()
            {
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                RefreshToken = refreshToken,
                Address = disco.TokenEndpoint
            };

            var token = await _httpClient.RequestRefreshTokenAsync(refreshTokenRequest);

            if (token.IsError)
            {
                return null;
            }

            var authenticationTokens = new List<AuthenticationToken>()
            {
               new AuthenticationToken{ Name=OpenIdConnectParameterNames.AccessToken,Value=token.AccessToken},
               new AuthenticationToken{ Name=OpenIdConnectParameterNames.RefreshToken,Value=token.RefreshToken},
               new AuthenticationToken{ Name=OpenIdConnectParameterNames.ExpiresIn,Value= DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)}
            };

            var authenticationResult = await _httpContextAccessor.HttpContext.AuthenticateAsync();

            var properties = authenticationResult.Properties;
            properties.StoreTokens(authenticationTokens);

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, authenticationResult.Principal, properties);

            return token;
        }

        public async Task<bool> SignUp(SignupInput signupInput)
        {
            // ilk olarak ClientId ve Secret ile birlikte ClientCredentials token alınır. Ardından request gönderilir. 

            var ClientCredentialsToken = await _clientCredentialTokenService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ClientCredentialsToken);

            var signupInputContent = new StringContent(JsonConvert.SerializeObject(signupInput), Encoding.UTF8, "application/json");

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_serviceApiSettings.IdentityBaseUri}/api/user"))
            {
                requestMessage.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", ClientCredentialsToken);

                requestMessage.Content = signupInputContent;

                var response = await _httpClient.SendAsync(requestMessage);

                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }
            }

            // Kod buraya inerse kişi başarılı bir şekilde kayıt yapmış demektir. Kişi kayıt yaptığında aynı zamanda Login işlemini de gerçekleştirebiliriz. 

            SigninInput signinInput = new()
            {
                UserName = signupInput.UserName,
                Password = signupInput.Password
            };

            await SignIn(signinInput);

            return true;





        }

        public async Task RevokeRefreshToken()
        {
            var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.IdentityBaseUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            if (disco.IsError)
            {
                throw disco.Exception;
            }
            var refreshToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            TokenRevocationRequest tokenRevocationRequest = new()
            {
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                Address = disco.RevocationEndpoint,
                Token = refreshToken,
                TokenTypeHint = "refresh_token"
            };

            await _httpClient.RevokeTokenAsync(tokenRevocationRequest);
        }

        public async Task<bool> SignIn(SigninInput signinInput)
        {
            var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.IdentityBaseUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            if (disco.IsError)
            {
                throw disco.Exception;
            }

            var passwordTokenRequest = new PasswordTokenRequest
            {
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                UserName = signinInput.UserName,
                Password = signinInput.Password,
                Address = disco.TokenEndpoint
            };

            var token = await _httpClient.RequestPasswordTokenAsync(passwordTokenRequest);

            if (token.IsError)
            {

                return false;

            }

            var userInfoRequest = new UserInfoRequest
            {
                Token = token.AccessToken,
                Address = disco.UserInfoEndpoint
            };

            var userInfo = await _httpClient.GetUserInfoAsync(userInfoRequest);

            if (userInfo.IsError)
            {
                throw userInfo.Exception;
            }

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(userInfo.Claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", "role");

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var authenticationProperties = new AuthenticationProperties();

            authenticationProperties.StoreTokens(new List<AuthenticationToken>()
            {
               new AuthenticationToken{ Name=OpenIdConnectParameterNames.AccessToken,Value=token.AccessToken},
               new AuthenticationToken{ Name=OpenIdConnectParameterNames.RefreshToken,Value=token.RefreshToken},
               new AuthenticationToken{ Name=OpenIdConnectParameterNames.ExpiresIn,Value= DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)}
            });

            authenticationProperties.IsPersistent = true;

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);

            return true;
        }
    }
}
