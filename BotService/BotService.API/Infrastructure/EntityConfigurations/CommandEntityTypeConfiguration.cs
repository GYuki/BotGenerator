using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BotService.API.Model;

namespace BotService.API.Infrastructure.EntityConfigurations
{
    class CommandEntityTypeConfiguration
        : IEntityTypeConfiguration<Command>
    {
        public void Configure(EntityTypeBuilder<Command> builder)
        {
            builder.ToTable("Command");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .UseHiLo("command_hilo")
                .IsRequired();
            
            builder.Property(c => c.Request)
                .IsRequired()
                .HasMaxLength(20);
            
            builder.Property(c => c.Response)
                .IsRequired()
                .HasMaxLength(200);
            
            builder.HasOne(c => c.Bot)
                .WithMany(b => b.Commands)
                .HasForeignKey(c => c.BotId);

            builder.HasIndex(c => new { c.BotId, c.Request });
        }
    }
}