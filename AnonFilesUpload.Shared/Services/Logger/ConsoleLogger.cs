using System;

namespace AnonFilesUpload.Shared.Services
{


    public class ConsoleLogger : ILogger
    {
        public void Write(string message)
        {
            Console.WriteLine($"[ConsoleLogger] - {message}");
        }
    }
}
