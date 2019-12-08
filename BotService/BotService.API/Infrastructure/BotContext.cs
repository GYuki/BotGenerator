namespace BotService.API.Infrastructure
{
    using Model;
    using Microsoft.EntityFrameworkCore;
    using MySql.Data.EntityFrameworkCore.Extensions;
    using EntityConfigurations;
    public class BotContext : DbContext
    {
        public DbSet<Bot> Bots { get; set; }
        public DbSet<Command> Commands { get; set; }
        public DbSet<Subscribe> Subscribes { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserEntityTypeConfiguration());
        }
    }
}