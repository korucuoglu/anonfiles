using FileUpload.Shared.Dtos.Files.Enums;
using System;
using System.Collections.Generic;

namespace FileUpload.Shared.Dtos.Files
{
    public class FileFilterModel
    {

        public FileFilterModel()
        {
            Page = 1;
            PageSize = 10;
            OrderBy = 1;
            Extension = String.Empty;
            CategoryIds = new List<int>();
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
            CategoryIds = model.CategoryIds;
            Extension = model.Extension;
        }

        public int Page { get; set; } 
        public int PageSize { get; set; } 
        public int OrderBy { get; set; } 
        public string Extension { get; set; }
        public List<int> CategoryIds { get; set; }

    }
}
