using FileUpload.Upload.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileUpload.Upload.Persistence.EntityConfigurations
{
    public abstract class BaseIdentityConfiguration<T> : BaseEntityConfiguration<T> where T : BaseIdentity
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.UserId).HasColumnName("user_id").IsRequired();
        }
    }
}
