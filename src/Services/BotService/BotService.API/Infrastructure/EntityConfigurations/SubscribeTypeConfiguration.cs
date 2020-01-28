using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BotService.API.Model;

namespace BotService.API.Infrastructure.EntityConfigurations
{
    public class SubscribeEntityTypeConfiguration
        : IEntityTypeConfiguration<Subscribe>
    {
        public void Configure(EntityTypeBuilder<Subscribe> builder)
        {
            builder.ToTable("Subscribe");

            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id)
                   .IsRequired();

            builder.HasIndex(s => new { s.BotId, s.ChatId })
                   .IsUnique(true);

            builder.Property(u => u.ChatId)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}