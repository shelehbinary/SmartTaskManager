namespace SmartTaskManager.Core.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        ITaskRepository TaskRepository { get; }
        IUserRepository UserRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
