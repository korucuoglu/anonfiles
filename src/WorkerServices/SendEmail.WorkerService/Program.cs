using Microsoft.Extensions.Options;
using SendEmail.WorkerService;
using SendEmail.WorkerService.Services;
using SendEmail.WorkerService.Services.Interfaces;

IHost host = Host.CreateDefaultBuilder(args).
    ConfigureServices((hostContext, services) =>
    {
        services.Configure<MailSettings>(hostContext.Configuration.GetSection("MailSettings"));

        services.AddSingleton<IMailSettings>(sp =>
        {
            return sp.GetRequiredService<IOptions<MailSettings>>().Value;
        });
        services.AddSingleton<IMailService, MailService>();
        services.AddSingleton<RabbitMQClientService>();

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
