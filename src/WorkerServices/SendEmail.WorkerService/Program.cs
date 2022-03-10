using Microsoft.Extensions.Options;
using SendEmail.WorkerService;
using SendEmail.WorkerService.Settings;

IHost host = Host.CreateDefaultBuilder(args).
    ConfigureServices((hostContext, services) =>
    {
        services.Configure<MailSettings>(hostContext.Configuration.GetSection("MailSettings"));

        services.AddSingleton<IMailSettings>(sp =>
        {
            return sp.GetRequiredService<IOptions<MailSettings>>().Value;
        });

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
