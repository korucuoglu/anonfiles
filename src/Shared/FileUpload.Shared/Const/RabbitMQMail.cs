namespace FileUpload.Shared.Const
{
    public static class RabbitMQMail
    {
        public const string ExchangeName = "sendmail-direct-exchange";
        public const string RouteKey = "route-sendmail";
        public const string QueueName = "queue-sendmail";
    }
}
