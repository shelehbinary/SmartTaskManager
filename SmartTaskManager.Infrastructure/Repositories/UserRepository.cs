using Microsoft.EntityFrameworkCore;
using SmartTaskManager.Core;
using SmartTaskManager.Core.Entities;
using SmartTaskManager.Core.Interfaces.Repositories;
using SmartTaskManager.Infrastructure.Data;

namespace SmartTaskManager.Infrastructure.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user) => await _context.AddAsync(user);

        public async Task<User?> GetUserByEmailAsync(string email) => await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

        public async Task<User?> GetByIdAsync(Guid id) => await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }
}
