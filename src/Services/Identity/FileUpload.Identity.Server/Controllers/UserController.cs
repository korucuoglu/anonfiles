using FileUpload.Data.Entity;
using FileUpload.Identity.Server.Services;
using FileUpload.Shared.Dtos.User;
using FileUpload.Shared.Event;
using FileUpload.Shared.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace FileUpload.IdentityServer.Controllers
{
    [Authorize(LocalApi.PolicyName)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration configuration;
        private readonly RabbitMQPublisher _rabbitMQPublisher;

        public UserController(UserManager<ApplicationUser> userManager, IConfiguration configuration, RabbitMQPublisher rabbitMQPublisher)
        {
            _userManager = userManager;
            this.configuration = configuration;
            _rabbitMQPublisher = rabbitMQPublisher;
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignupInput dto)
        {
            var user = new ApplicationUser()
            {
                UserName = dto.UserName,
                Email = dto.Email,
                UserInfo = new()
                {
                    Id = Guid.NewGuid(),
                    UsedSpace = 0,
                }
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            async Task<Response<NoContent>> GetResult(IdentityResult result)
            {
                if (!result.Succeeded)
                {
                    var error = result.Errors.First().Description;

                    return Response<NoContent>.Fail(error, 500);

                }

                string confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                string encodedConfirmationToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(confirmationToken));

                string link = $"{configuration.GetSection("MVCClient").Value}/user/confirmEmail?userId={user.Id}&token={encodedConfirmationToken}";


                UserCreatedEvent userCreatedEvent = new()
                {
                    MailAdress = user.Email,
                    Message = $"<p>Mail adresini doğrulamak için <a href='{link}'>tıkla</a></p>"
                };

                _rabbitMQPublisher.Publish(userCreatedEvent);

                return Response<NoContent>.Success($"{user.Email} mail adresine doğrulama maili gönderildi", 200);
            }

            var data = await GetResult(result);

            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };
        }

        [HttpPost]
        public async Task<IActionResult> ValidateUserEmail(ConfirmEmailModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token));

            IdentityResult result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            Response<NoContent> GetResult(IdentityResult result)
            {
                if (!result.Succeeded)
                {
                    var error = result.Errors.First().Description;

                    return Response<NoContent>.Fail(error, 500);
                }

                return Response<NoContent>.Success($"{user.Email} mail adresi doğrulandı", 200);
            }

            var data = GetResult(result);

            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };


        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            async Task<Response<NoContent>> GetResult(ApplicationUser user)
            {
                if (user == null)
                {
                    return Response<NoContent>.Fail("Sistemde böyle bir mail hesabı bulunamadı", 500);
                }

                string token = await _userManager.GeneratePasswordResetTokenAsync(user);
                string encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                string link = $"{configuration.GetSection("MVCClient").Value}/user/reset-password?userId={user.Id}&token={encodedToken}";


                UserCreatedEvent userCreatedEvent = new()
                {
                    MailAdress = user.Email,
                    Message = $"<p>Şifrenizi sıfırlamak için <a href='{link}'>tıklayınız</a></p>"
                };

                _rabbitMQPublisher.Publish(userCreatedEvent);

                return Response<NoContent>.Success($"{user.Email} mail adresine doğrulama maili gönderildi", 200);
            }

            var data = await GetResult(user);
            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };
        }

        [HttpPost]
        public async Task<IActionResult> ValidateResetPassword(ResetPasswordModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token));

            IdentityResult result = await _userManager.ResetPasswordAsync(user, decodedToken, model.NewPassword);

            Response<NoContent> GetResult(IdentityResult result)
            {
                if (!result.Succeeded)
                {
                    var error = result.Errors.First().Description;

                    return Response<NoContent>.Fail(error, 500);
                }

                return Response<NoContent>.Success("Şifre başarılı bir şekilde sıfırlandı", 200);
            }

            var data = GetResult(result);

            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };
        }
    }
}
