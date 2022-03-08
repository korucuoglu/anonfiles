using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUpload.Api.Application.Interfaces.Redis
{
    public interface IRedisSettings
    {
        public string Host { get; set; }
        public string Port { get; set; }
    }
}
