using FileUpload.Upload.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileUpload.Upload.Persistence.EntityConfigurations
{
    public class FileEntityConfiguration : BaseIdentityConfiguration<File>
    {
        public override void Configure(EntityTypeBuilder<File> builder)
        {
            base.Configure(builder);

            builder.ToTable("files");

            builder.Property(x => x.FileKey).HasColumnName("file_key").IsRequired();
            builder.Property(x => x.FileName).HasColumnName("file_name").IsRequired();
            builder.Property(x => x.Size).HasColumnName("size").HasDefaultValue(0);









        }

    }
}
