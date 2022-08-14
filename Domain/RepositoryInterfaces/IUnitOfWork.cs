namespace Domain.RepositoryInterfaces
{
    public interface IUnitOfWork
    {
       Task CommitAsync();
    }
}
