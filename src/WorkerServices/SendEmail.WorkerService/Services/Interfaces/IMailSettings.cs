namespace SendEmail.WorkerService.Services.Interfaces
{
    public interface IMailSettings
    {
        public string MailAdress { get; set; }
        public string MailPassword { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
