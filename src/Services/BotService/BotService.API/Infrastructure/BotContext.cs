namespace BotService.API.Infrastructure
{
    using Model;
    using Microsoft.EntityFrameworkCore;
    using Pomelo.EntityFrameworkCore.MySql.Extensions;
    using EntityConfigurations;
    public class BotContext : DbContext
    {
        public BotContext(DbContextOptions<BotContext> options) : base(options)
        {
        }
        public DbSet<Bot> Bots { get; set; }
        public DbSet<Subscribe> Subscribes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new BotEntityTypeConfiguration());
            builder.ApplyConfiguration(new SubscribeEntityTypeConfiguration());
        }
    }
}