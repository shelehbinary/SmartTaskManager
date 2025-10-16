using SmartTaskManager.Core.Entities;

namespace SmartTaskManager.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetUserByEmailAsync(string email);
        Task AddAsync(User user);
    }
}
