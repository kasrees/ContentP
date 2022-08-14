using Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class Repository<TEnitiy> : IRepository<TEnitiy> where TEnitiy : class
    {
        protected readonly DbContext _DbContext;
        protected DbSet<TEnitiy> Entities => _DbContext.Set<TEnitiy>();

        public Repository( DbContext dbContext )
        {
            _DbContext = dbContext;
        }

        public void Add( TEnitiy entity )
        {
            _DbContext.Add( entity );
        }

        public void AddRange( List<TEnitiy> entities )
        {
            _DbContext.AddRange( entities );
        }

        public void Remove( TEnitiy entity )
        {
            _DbContext.Remove( entity );
        }
    }
}
