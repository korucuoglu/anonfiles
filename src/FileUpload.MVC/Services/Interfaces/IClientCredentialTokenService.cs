using System;
using System.Threading.Tasks;

namespace FileUpload.MVC.Services.Interfaces
{
    public interface IClientCredentialTokenService
    {
        Task<string> GetToken();
    }
}
