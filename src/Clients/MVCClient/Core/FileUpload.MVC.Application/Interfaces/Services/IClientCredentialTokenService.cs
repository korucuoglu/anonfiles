using System.Threading.Tasks;

namespace FileUpload.MVC.Application.Interfaces.Services
{
    public interface IClientCredentialTokenService
    {
        Task<string> GetToken();
    }
}
