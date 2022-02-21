using FileUpload.MVC.Models.User;
using IdentityModel.Client;
using System.Threading.Tasks;

namespace FileUpload.MVC.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<bool> SignIn(SigninInput signinInput);
        Task<bool> SignUp(SignupInput signupInput);
        Task<TokenResponse> GetAccessTokenByRefreshToken();
        Task RevokeRefreshToken();
    }
}
