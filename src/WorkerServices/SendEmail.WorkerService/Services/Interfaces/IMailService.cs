using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendEmail.WorkerService.Services.Interfaces
{
    public interface IMailService
    {
        Task Send(string toMailAdress, string message, string subject );
    }
}
