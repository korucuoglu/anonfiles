using FileUpload.Shared.Dtos.User;
using IdentityModel.Client;
using System.Threading.Tasks;

namespace FileUpload.MVC.Application.Interfaces.Services
{
    public interface IIdentityService
    {
        Task<bool> SignIn(SigninInput signinInput);
        Task<bool> SignUp(SignupInput signupInput);
        Task<TokenResponse> GetAccessTokenByRefreshToken();
        Task RevokeRefreshToken();
    }
}
