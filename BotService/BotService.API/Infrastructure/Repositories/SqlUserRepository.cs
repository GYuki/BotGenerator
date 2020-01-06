using BotService.API.Model;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

namespace BotService.API.Infrastructure.Repositories
{
    public class SqlUserRepository: IUserRepository
    {
        private readonly BotContext _context;

        public SqlUserRepository(BotContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserAsync(int id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task CreateUserAsync(User _user)
        {
            var user = new User
            {
                SenderId = _user.SenderId
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();
        }
    }
}