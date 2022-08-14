using Domain.Entities;

namespace Domain.RepositoryInterfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByLoginAsync(string login);
    }
}
