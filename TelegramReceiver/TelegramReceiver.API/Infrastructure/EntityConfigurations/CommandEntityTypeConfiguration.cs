using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TelegramReceiver.API.Models;


namespace TelegramReceiver.API.Infrastructure.EntityConfigurations
{
    public class CommandEntityTypeConfiguration
        : IEntityTypeConfiguration<Command>
    {
        public void Configure(EntityTypeBuilder<Command> builder)
        {
            builder.ToTable("Command");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .IsRequired();
            
            builder.HasIndex(c => c.Token)
                .IsUnique(true);
            
            builder.Property(c => c.Token)
                .IsRequired()
                .HasMaxLength(100);
            
            builder.HasIndex(c => new { c.Token, c.Request })
                .IsUnique(true);
        }
    }
}