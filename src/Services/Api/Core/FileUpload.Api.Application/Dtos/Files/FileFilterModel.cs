using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUpload.Api.Application.Dtos.Files
{
    public class FileFilterModel
    {

        public FileFilterModel()
        {
            CategoryIds = new List<string>();
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
        public List<string> CategoryIds { get; set; }

    }
}
