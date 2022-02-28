namespace FileUpload.Api.Application.Interfaces.Settings
{
    public interface IDatabaseSettings
    {
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
    }
}
