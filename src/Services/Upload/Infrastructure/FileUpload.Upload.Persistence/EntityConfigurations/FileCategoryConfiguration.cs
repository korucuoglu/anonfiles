using FileUpload.Upload.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileUpload.Upload.Persistence.EntityConfigurations
{
    public class FileCategoryConfiguration : IEntityTypeConfiguration<FileCategory>
    {
        public void Configure(EntityTypeBuilder<FileCategory> builder)
        {
            builder.ToTable("filecategory");

            builder.HasKey(x => new { x.CategoryId, x.FileId });

            builder.Property(x => x.CategoryId).HasColumnName("category_id");
            builder.Property(x => x.FileId).HasColumnName("file_id");

            builder.HasOne(x => x.File)
              .WithMany(x => x.FilesCategories)
              .HasForeignKey(x => x.FileId);

           builder.HasOne(x => x.Category)
              .WithMany(x => x.FilesCategories)
              .HasForeignKey(x => x.CategoryId);
        }
    }
}
