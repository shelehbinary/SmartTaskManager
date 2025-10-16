using Microsoft.EntityFrameworkCore;
using SmartTaskManager.Core.Entities;
using SmartTaskManager.Core.Interfaces.Repositories;
using SmartTaskManager.Infrastructure.Data;

namespace SmartTaskManager.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TaskItem?> GetByIdAsync(Guid id) => await _context.Tasks.FindAsync(id);
        public async Task<IEnumerable<TaskItem>> GetTasksByUserIdAsync(Guid userId) => await _context.Tasks.Where(t => t.UserId == userId).ToListAsync();
        public async Task AddAsync(TaskItem task) => await _context.Tasks.AddAsync(task);
        public void Update(TaskItem task) => _context.Tasks.Update(task);
        public void Delete(TaskItem task) => _context.Tasks.Remove(task);
    }
}
