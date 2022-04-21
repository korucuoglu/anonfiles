using FileUpload.Data.Entity;
using FileUpload.Identity.Server.Services;
using FileUpload.Shared.Base;
using FileUpload.Shared.Dtos.User;
using FileUpload.Shared.Event;
using FileUpload.Shared.Services;
using FileUpload.Shared.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace FileUpload.IdentityServer.Controllers
{
    [Authorize(LocalApi.PolicyName)]
    public class UserController : BaseApiController
    {
        private readonly UserManager<User> _userManager;
        private readonly RabbitMQPublisher _rabbitMQPublisher;
        private readonly IHashService _hashService;

        public UserController(UserManager<User> userManager, RabbitMQPublisher rabbitMQPublisher, IHashService hashService)
        {
            _userManager = userManager;
            _rabbitMQPublisher = rabbitMQPublisher;
            _hashService = hashService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup(SignupInput dto)
        {
            var user = new User()
            {
                UserName = dto.UserName,
                Email = dto.Email,
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            async Task<Response<NoContent>> GetResult(IdentityResult result)
            {
                if (!result.Succeeded)
                {

                    return Response<NoContent>.Fail(result.Errors.Select(x => x.Description).ToList(), 500);
                }
                await _userManager.AddClaimAsync(user, new Claim("role", "user"));
                await _userManager.AddClaimAsync(user, new Claim("email", user.Email));

                string confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                string encodedConfirmationToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(confirmationToken));

                string link = $"http://localhost:5003/user/confirmEmail?userId={_hashService.Encode(user.Id)}&token={encodedConfirmationToken}";

                UserCreatedEvent userCreatedEvent = new()
                {
                    MailAdress = user.Email,
                    Message = $"<p>Mail adresini doğrulamak için <a href='{link}'>tıkla</a></p>",
                    Subject = "Mail Onaylama"
                };

                _rabbitMQPublisher.Publish(userCreatedEvent);

                return Response<NoContent>.Success($"{user.Email} mail adresine doğrulama maili gönderildi", 200);
            }

            var data = await GetResult(result);

            return Result(data);
        }

        [HttpGet("validate-mail")]
        public async Task<IActionResult> ValidateUserEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(_hashService.Decode(userId).ToString());

            async Task<Response<NoContent>> GetResult(User user)
            {
                if (user == null)
                {
                    return Response<NoContent>.Fail("Geçersiz link", 404);
                }

                var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

                IdentityResult result = await _userManager.ConfirmEmailAsync(user, decodedToken);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(x => x.Description).ToList();

                    return Response<NoContent>.Fail(errors, 500);
                }

                return Response<NoContent>.Success($"{user.Email} mail adresi doğrulandı", 200);
            }

            var data = await GetResult(user);

            return Result(data);
        }

        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Mail);

            async Task<Response<NoContent>> GetResult(User user)
            {
                if (user == null)
                {
                    return Response<NoContent>.Fail("Sistemde böyle bir mail hesabı bulunamadı", 500);
                }

                string token = await _userManager.GeneratePasswordResetTokenAsync(user);
                string encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                string link = $"http://localhost:5003/user/reset-passwordConfirm?userId={_hashService.Encode(user.Id)}&token={encodedToken}";

                UserCreatedEvent userCreatedEvent = new()
                {
                    MailAdress = user.Email,
                    Message = $"<p>Şifrenizi sıfırlamak için <a href='{link}'>tıklayınız</a></p>",
                    Subject = "Şifre Sıfırlama"
                };

                _rabbitMQPublisher.Publish(userCreatedEvent);

                return Response<NoContent>.Success($"{user.Email} adresine gerekli bilgiler gönderildi", 200);
            }

            var data = await GetResult(user);
            return Result(data);
        }

        [HttpPost("resetpassword-confirm")]
        public async Task<IActionResult> ResetPasswordConfirm(ResetPasswordConfirmModel model)
        {
            var user = await _userManager.FindByIdAsync(_hashService.Decode(model.UserId).ToString());

            async Task<Response<NoContent>> GetResult(User user)
            {
                if (user == null)
                {
                    return Response<NoContent>.Fail("Girilen bilgiler geçersizdir.", 500);
                }

                var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token));

                IdentityResult result = await _userManager.ResetPasswordAsync(user, decodedToken, model.NewPassword);

                if (!result.Succeeded)
                {
                    var error = result.Errors.First().Description;

                    return Response<NoContent>.Fail(error, 500);
                }

                return Response<NoContent>.Success("Şifre başarılı bir şekilde sıfırlandı", 200);
            }

            var data = await GetResult(user);

            return Result(data);
        }
    }
}
