using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sarhne.DAL.Entities;


namespace Sarhne.DAL.Configration
{
    public class UserSettingConfig : IEntityTypeConfiguration<UserSetting>
    {
        public void Configure(EntityTypeBuilder<UserSetting> builder)
        {
            builder.ToTable("UserSettings");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.AllowAnonymousMessages)
                .HasDefaultValue(true);

            builder.Property(x => x.ShowLastSeen)
                .HasDefaultValue(true);

            builder.Property(x => x.ShowProfileViews)
                .HasDefaultValue(false);
        }
    }
}
