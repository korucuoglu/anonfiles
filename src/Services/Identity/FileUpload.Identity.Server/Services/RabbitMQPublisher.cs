using FileUpload.Shared.Event;
using System.Text;
using System.Text.Json;

namespace FileUpload.Identity.Server.Services
{
    public class RabbitMQPublisher
    {
        private readonly RabbitMQClientService _rabbitMQClientService;

        public RabbitMQPublisher(RabbitMQClientService rabbitMQClientService)
        {
            _rabbitMQClientService = rabbitMQClientService;
        }

        public void Publish(UserCreatedEvent userCreatedEvent)
        {
            var channel = _rabbitMQClientService.Connect();

            var bodyString = JsonSerializer.Serialize(userCreatedEvent);

            var bodyByte = Encoding.UTF8.GetBytes(bodyString);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(RabbitMQInfo.ExchangeName, RabbitMQInfo.RouteKey, false, properties, bodyByte);
        }
    }
}
