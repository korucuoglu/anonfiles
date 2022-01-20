using AnonFilesUpload.MVC.Models.User;
using IdentityModel.Client;
using System.Threading.Tasks;

namespace AnonFilesUpload.MVC.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<bool> SignIn(SigninInput signinInput);

        public string GetUserId { get; }

        Task<TokenResponse> GetAccessTokenByRefreshToken();

        Task RevokeRefreshToken();
    }
}
