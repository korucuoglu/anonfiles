using System;

namespace AnonFilesUpload.Shared.Models
{


    public interface ILog
    {
        public void Log(string message);
    }


    public class ConsoleLogger : ILog
    {
        public void Log(string message)
        {
            Console.WriteLine($"[ConsoleLogger] - {message}");
        }
    }

}