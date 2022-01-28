using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnonFilesUpload.Shared.Models
{
    public interface ILogger
    {
        public void Write(string message);
    }

    public class ConsoleLogger : ILogger
    {
        public void Write(string message)
        {
            Console.WriteLine($"[ConsoleLogger] - {message}");
        }
    }
}
