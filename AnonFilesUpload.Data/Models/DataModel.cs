using System.Collections.Generic;

namespace AnonFilesUpload.Data.Models
{
    public class DataModel
    {
        public string FileId { get; set; }
        public string FileName { get; set; }
        public double Size { get; set; }
    }

    public class DataViewModel
    {
        public  List<DataModel> DataModel { get; set; }

        public double TotalSize { get; set; }
        public string UsedSpace { get; set; }
        public string RemainingSpace { get; set; }
       
    }
}
