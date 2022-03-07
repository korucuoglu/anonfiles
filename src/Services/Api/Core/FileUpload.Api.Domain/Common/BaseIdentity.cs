using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUpload.Domain.Common
{
    public abstract class BaseIdentity: BaseEntity
    {
        public Guid ApplicationUserId { get; set; }
    }
}
