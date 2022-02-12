using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUpload.Shared.Models.Files
{
    public class FileFilterModel
    {

        int page;

        public int Page
        {
            get { return page; }
            set
            {
                if (value <= 0)
                { page = 1; }
                else
                { page = value; }
            }
        }

        int number;

        public int Number
        {
            get { return number; }
            set
            {
                if (value <= 0)
                { number = 10; }
                else
                { number = value; }
            }
        }

        int orderBy;

        public int OrderBy
        {
            get { return orderBy; }
            set
            {
                if (value < 0)
                { orderBy = 0; }
                else
                { orderBy = value; }
            }
        }


        public string Extension { get; set; }

    }
}
