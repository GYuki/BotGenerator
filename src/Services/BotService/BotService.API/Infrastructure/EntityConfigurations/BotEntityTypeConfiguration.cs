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
                .IsRequired();
            
            builder.HasIndex(b => b.Token)
                .IsUnique(true);

            builder.Property(b => b.Token)
                .IsRequired()
                .HasMaxLength(100);
            
            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}