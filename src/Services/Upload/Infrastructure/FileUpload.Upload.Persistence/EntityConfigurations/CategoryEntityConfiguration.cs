using FileUpload.Upload.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileUpload.Upload.Persistence.EntityConfigurations
{
    public class CategoryEntityConfiguration: BaseIdentityEntityConfiguration<Category>
    {
        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            base.Configure(builder);

            builder.ToTable("categories");

            builder.Property(x => x.Title).HasColumnName("title").IsRequired();
            builder.HasIndex(x => new { x.UserId, x.Title }).IsUnique(true);

        }

    }
}
