using FileUpload.Shared.Event;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SendEmail.WorkerService.Services;
using SendEmail.WorkerService.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace SendEmail.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMailService _mailService;
        private readonly RabbitMQClientService _rabbitMQClientService;
        private IModel _channel;

        public Worker(ILogger<Worker> logger, IMailService mailService, RabbitMQClientService rabbitMQClientService)
        {
            _logger = logger;
            _mailService = mailService;
            _rabbitMQClientService = rabbitMQClientService;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _channel = _rabbitMQClientService.Connect();
            _channel.BasicQos(0, 1, false);

            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);

            _channel.BasicConsume(RabbitMQInfo.QueueName, false, consumer);

            consumer.Received += Consumer_Received;

            await Task.CompletedTask;

        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
        {
            var userCreatedEvent = JsonSerializer.Deserialize<UserCreatedEvent>(Encoding.UTF8.GetString(@event.Body.ToArray()));

             await _mailService.Send(userCreatedEvent.MailAdress, userCreatedEvent.Message, "Confirm Mail Adres");

            _channel.BasicAck(@event.DeliveryTag, false);

        }
    }
}