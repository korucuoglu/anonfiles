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
        Task<Response<NoContent>> ValidateUserEmail(string UserId, string token);
        Task<Response<NoContent>> ResetPassword(string email);
        Task<Response<NoContent>> ResetPasswordConfirm(ResetPasswordConfirmModel model);
        Task<TokenResponse> GetAccessTokenByRefreshToken();
        Task RevokeRefreshToken();
    }
}
