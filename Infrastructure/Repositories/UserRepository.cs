using Domain.RepositoryInterfaces;
using Domain.Entities;
using Infrastructure.Foundation;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ExtranetPageKeywordsDbContext dbContext ) : base( dbContext )
        { }
        public async Task<User?> GetByLoginAsync(string login)
        {
            return await Entities.FirstOrDefaultAsync(u => u.Login == login);
        }
    }
}
