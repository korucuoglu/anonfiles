
namespace AnonFilesUpload.Shared.Models
{
    public class AjaxReturningModel
    {
        public string fileId { get; set; }
        public string fileName { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
    }

}
