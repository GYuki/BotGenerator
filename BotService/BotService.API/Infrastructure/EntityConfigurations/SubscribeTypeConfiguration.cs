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

            builder.HasKey(s => new { s.BotId, s.ListenerId });

            builder.HasOne(subscribe => subscribe.Listener)
                .WithMany(user => user.Subscribes)
                .HasForeignKey(subscribe => subscribe.ListenerId);
            
            builder.HasOne(subscribe => subscribe.Bot)
                .WithMany(bot => bot.Subscribes)
                .HasForeignKey(subscribe => subscribe.BotId);
        }
    }
}