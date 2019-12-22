using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BotService.API.Model;

namespace BotService.API.Infrastructure.EntityConfigurations
{
    class UserEntityTypeConfiguration
        : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id)
                .UseHiLo("user_hilo")
                .IsRequired();
            
            builder.HasIndex(u => u.SenderId)
                .IsUnique(true);
            builder.Property(u => u.SenderId)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}