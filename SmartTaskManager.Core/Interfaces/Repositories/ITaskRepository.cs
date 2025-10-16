using SmartTaskManager.Core.Entities;

namespace SmartTaskManager.Core.Interfaces.Repositories
{
    public interface ITaskRepository
    {
        Task<TaskItem?> GetByIdAsync(Guid id);
        Task<IEnumerable<TaskItem>> GetTasksByUserIdAsync(Guid userId);
        Task AddAsync(TaskItem task);
        void Update(TaskItem task);
        void Delete(TaskItem task);
    }
}
