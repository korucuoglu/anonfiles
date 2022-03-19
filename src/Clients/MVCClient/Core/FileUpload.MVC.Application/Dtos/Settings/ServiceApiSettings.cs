namespace FileUpload.MVC.Application.Dtos.Settings
{
    public class ServiceApiSettings
    {
        public string IdentityBaseUri { get; set; }
        public string GatewayBaseUri { get; set; }
        public ServiceApi Upload { get; set; }
    }

    public class ServiceApi
    {
        public string Path { get; set; }
    }
}
