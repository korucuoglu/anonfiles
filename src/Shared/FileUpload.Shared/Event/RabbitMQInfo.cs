using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUpload.Shared.Event
{
    public static class RabbitMQInfo
    {
        public const string ExchangeName = "sendmail-direct-exchange";
        public const string RouteKey = "route-sendmail";
        public const string QueueName = "queue-sendmail";
    }
}
