using FileUpload.Upload.Domain.Entities;
using FileUpload.Upload.Persistence.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileUpload.Upload.Persistence.EntityConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasOne(p => p.UserInfo)
                        .WithOne()
                        .HasForeignKey<UserInfo>(pe => pe.Id);
        }
    }
}
