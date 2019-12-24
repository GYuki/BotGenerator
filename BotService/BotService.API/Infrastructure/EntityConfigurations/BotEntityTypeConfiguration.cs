using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BotService.API.Model;

namespace BotService.API.Infrastructure.EntityConfigurations
{
    class BotEntityTypeConfiguration
        : IEntityTypeConfiguration<Bot>
    {
        public void Configure(EntityTypeBuilder<Bot> builder)
        {
            builder.ToTable("Bot");

            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id)
                .UseHiLo("bot_hilo")
                .IsRequired();
            
            builder.HasIndex(b => b.Token)
                .IsUnique(true);
            builder.HasIndex(b => b.Name)
                .IsUnique(true);

            builder.Property(b => b.Token)
                .IsRequired()
                .HasMaxLength(100);
            
            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(100);
            
            builder.HasOne(b => b.Owner)
                .WithMany(u => u.Bots)
                .HasForeignKey(b => b.OwnerId);
        }
    }
}