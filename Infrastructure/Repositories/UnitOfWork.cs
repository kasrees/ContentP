using Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UnitOfWork<T> : IUnitOfWork
         where T : DbContext
    {
        public T DbContext;

        public UnitOfWork( T dbContext )
        {
            DbContext = dbContext;
        }

        public async Task CommitAsync()
        {
            await DbContext.SaveChangesAsync();
        }
    }
}
