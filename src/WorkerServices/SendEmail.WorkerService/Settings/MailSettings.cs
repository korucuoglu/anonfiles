using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendEmail.WorkerService.Settings
{
    public class MailSettings : IMailSettings
    {
        public string MailAdress { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string MailPassword { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Host { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Port { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
