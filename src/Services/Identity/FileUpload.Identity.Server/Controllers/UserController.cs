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
using System.Web;
using static IdentityServer4.IdentityServerConstants;

namespace FileUpload.IdentityServer.Controllers
{
    [Authorize(LocalApi.PolicyName)]
    // Default gelen Policy'yi ekledik. Bu sayede token olmadan buraya istek yapılamayacaktır.
    // Token olmadan istek yapıldığında 401 hatası vereceğiz. 

    [Route("api/[controller]")]
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

                return Response<NoContent>.Success(200);
            }

            var data = await GetResult(result);

            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };
        }

        [HttpGet]
        public async Task<IActionResult> ValidateUserEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            IdentityResult result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            Response<NoContent> GetResult(IdentityResult result)
            {
                if (!result.Succeeded)
                {
                    var error = result.Errors.First().Description;

                    return Response<NoContent>.Fail(error, 500);
                }

                return Response<NoContent>.Success(200);
            }

            var data = GetResult(result);

            return new ObjectResult(data)
            {
                StatusCode = data.StatusCode
            };


        }
    }
}
