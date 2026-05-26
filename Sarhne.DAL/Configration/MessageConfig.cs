

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sarhne.DAL.Entities;

namespace Sarhne.DAL.Configration
{
    public class MessageConfig : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Messages");

            builder.HasKey(M => M.Id);

            builder.Property(m => m.Content)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(m => m.PhotoUrl)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(m => m.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(m => m.IsRead)
                .HasDefaultValue(false);

            builder.Property(m => m.IsStared)
               .HasDefaultValue(false);
        }
    }
}
