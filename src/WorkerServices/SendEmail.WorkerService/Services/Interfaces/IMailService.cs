namespace SendEmail.WorkerService.Services.Interfaces
{
    public interface IMailService
    {
        Task Send(string toMailAdress, string message, string subject);
    }
}
