using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnonFilesUpload.Data.Entity
{
    public interface IDataContext
    {
         DbSet<Data> Data { get; set; }

        int SaveChanges();
    }
}
