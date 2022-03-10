using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendEmail.WorkerService.Settings
{
    public interface IMailSettings
    {
        public string MailAdress { get; set; }
        public string MailPassword { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
