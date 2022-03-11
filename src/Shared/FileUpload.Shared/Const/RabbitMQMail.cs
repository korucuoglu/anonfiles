using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUpload.Shared.Const
{
    public static class RabbitMQMail
    {
        public const string ExchangeName = "sendmail-direct-exchange";
        public const string RouteKey = "route-sendmail";
        public const string QueueName = "queue-sendmail";
    }
}
