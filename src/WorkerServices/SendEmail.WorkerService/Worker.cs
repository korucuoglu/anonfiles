using SendEmail.WorkerService.Services.Interfaces;

namespace SendEmail.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMailService _mailService;

        public Worker(ILogger<Worker> logger, IMailService mailService)
        {
            _logger = logger;
            _mailService = mailService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _mailService.Send("xhnedhdaaejyujnqau@nthrw.com", "deneme mesajýdýr", "Þifre Sýfýrlama");

             await Task.CompletedTask;
        }
    }
}