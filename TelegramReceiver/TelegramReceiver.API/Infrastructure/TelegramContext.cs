namespace TelegramReceiver.API.Infrastructure
{
    using Models;
    using Microsoft.EntityFrameworkCore;
    using EntityConfigurations;

    public class TelegramContext : DbContext
    {
        public TelegramContext(DbContextOptions<TelegramContext> options)
            : base(options)
        {
        }

        public DbSet<SendMessage> SendMessages { get; set; }
        public DbSet<Command> Commands { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new SendMessageEntityTypeConfiguration());
            builder.ApplyConfiguration(new CommandEntityTypeConfiguration());
        }
    }
}