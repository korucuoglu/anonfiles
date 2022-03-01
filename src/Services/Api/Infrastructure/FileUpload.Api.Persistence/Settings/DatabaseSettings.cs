using FileUpload.Api.Application.Interfaces.Settings;

namespace FileUpload.Api.Persistence.Settings
{
    
    public class DatabaseSettings : IDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
