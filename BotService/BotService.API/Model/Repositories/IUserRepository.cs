using System.Threading.Tasks;

namespace BotService.API.Model
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(int id);
        Task<User> GetUserBySenderIdAsync(string senderId);
        Task CreateUserAsync(User user);
    }
}