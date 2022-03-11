using FileUpload.Shared.Dtos.User;
using FileUpload.Shared.Wrappers;
using IdentityModel.Client;
using System.Threading.Tasks;

namespace FileUpload.MVC.Application.Interfaces.Services
{
    public interface IIdentityService
    {
        Task<Response<NoContent>> SignIn(SigninInput signinInput);
        Task<Response<NoContent>> SignUp(SignupInput signupInput);
        Task<Response<NoContent>> ValidateUserEmail(ConfirmEmailModel model);
        Task<Response<NoContent>> ResetPassword(string email);
        Task<TokenResponse> GetAccessTokenByRefreshToken();
        Task RevokeRefreshToken();
    }
}
