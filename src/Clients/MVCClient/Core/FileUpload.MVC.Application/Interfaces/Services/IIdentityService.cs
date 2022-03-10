using FileUpload.Shared.Dtos.User;
using FileUpload.Shared.Wrappers;
using IdentityModel.Client;
using System.Threading.Tasks;

namespace FileUpload.MVC.Application.Interfaces.Services
{
    public interface IIdentityService
    {
        Task<bool> SignIn(SigninInput signinInput);
        Task<Response<NoContent>> SignUp(SignupInput signupInput);
        Task<bool> ValidateUserEmail(string userId, string token);
        Task<TokenResponse> GetAccessTokenByRefreshToken();
        Task RevokeRefreshToken();
    }
}
