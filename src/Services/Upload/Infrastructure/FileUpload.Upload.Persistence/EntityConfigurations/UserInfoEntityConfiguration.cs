using FileUpload.Upload.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileUpload.Upload.Persistence.EntityConfigurations
{
    public class UserInfoEntityConfiguration: BaseIdentityEntityConfiguration<UserInfo>
    {
        public override void Configure(EntityTypeBuilder<UserInfo> builder)
        {
            base.Configure(builder);
            builder.ToTable("userinfo");
            builder.Property(x => x.UsedSpace).HasColumnName("used_space").HasDefaultValue(0);
        }

    }
}
