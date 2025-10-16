using SmartTaskManager.Core.Interfaces.Repositories;
using SmartTaskManager.Infrastructure.Data;

namespace SmartTaskManager.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public ITaskRepository TaskRepository { get; }
        public IUserRepository UserRepository { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            TaskRepository = new TaskRepository(_context);
            UserRepository = new UserRepository(_context);
        }

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
        public void Dispose() => _context.Dispose();
    }
}
