using FileUpload.Shared.Event;
using RabbitMQ.Client;

namespace SendEmail.WorkerService.Services
{
    public class RabbitMQClientService : IDisposable
    {
        private IConnection _connection; 
        private IModel _channel;

        IConfiguration configuration;

        public RabbitMQClientService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IModel Connect()
        {
            var connectionFactory = new ConnectionFactory() { HostName = configuration.GetConnectionString("RabbitMQ"), DispatchConsumersAsync = true };

            _connection = connectionFactory.CreateConnection();

            if (_channel is { IsOpen: true })
            {
                return _channel;
            }

            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(RabbitMQInfo.ExchangeName, type: ExchangeType.Direct, true, false);
            _channel.QueueDeclare(RabbitMQInfo.QueueName, true, false, false, null);
            _channel.QueueBind(RabbitMQInfo.QueueName, RabbitMQInfo.ExchangeName, RabbitMQInfo.RouteKey);

            return _channel;
        }

        public void Dispose()
        {
            _channel?.Close(); 
            _channel?.Dispose(); 
            _connection?.Close(); 
            _connection?.Dispose();
        }
    }
}
