using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TelegramReceiver.API.Models;


namespace TelegramReceiver.API.Infrastructure.EntityConfigurations
{
    public class SendMessageEntityTypeConfiguration
        : IEntityTypeConfiguration<SendMessage>
    {
        public void Configure(EntityTypeBuilder<SendMessage> builder)
        {
            builder.ToTable("SendMessage");

            builder.HasKey(sm => sm.Id);
            builder.Property(sm => sm.Id)
                .IsRequired();
        }
    }
}