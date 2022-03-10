using SendEmail.WorkerService.Services.Interfaces;

namespace SendEmail.WorkerService.Services
{
    public class MailSettings : IMailSettings
    {
        public string MailAdress { get; set; }
        public string MailPassword { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
