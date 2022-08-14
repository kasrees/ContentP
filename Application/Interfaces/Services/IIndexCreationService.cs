using Application.OptionModels;

namespace Application.Interfaces.Services
{
    public interface IIndexCreationService
    {
        Task CreateIndexIfDoesNotExistAsync( IndexConfig indexOptions );
        Task DeleteIndexIfExistsAsync( IndexConfig indexOptions );
    }
}
