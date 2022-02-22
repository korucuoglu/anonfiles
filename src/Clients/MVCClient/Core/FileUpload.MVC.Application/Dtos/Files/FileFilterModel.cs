using FileUpload.MVC.Application.Dtos.Files.Enums;
using System;

namespace FileUpload.MVC.Application.Dtos.Files
{
    public class FileFilterModel
    {

        public FileFilterModel()
        {
                
        }
        public FileFilterModel(FileFilterModel model)
        {
            if (model.Page <= 0)
            {
                model.Page = 1;
            }

            if (model.PageSize <= 0)
            {
                model.PageSize = 10;
            }
            var orderByCount = Enum.GetNames(typeof(EnumOrderBy)).Length;

            if (model.OrderBy <= 0 || model.OrderBy > orderByCount)
            {
                model.OrderBy = 1;
            }


            Page = model.Page;
            PageSize = model.PageSize;
            OrderBy = model.OrderBy;
            Category = model.Category;
            Extension = model.Extension;

            

            
         
        }

        public int Page { get; set; } 
        public int PageSize { get; set; } 
        public int OrderBy { get; set; } 
        public string Extension { get; set; }
        public string Category { get; set; }

    }
}
