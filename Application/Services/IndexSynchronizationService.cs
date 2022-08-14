using Application.Interfaces.Services;
using Application.OptionModels;

namespace Application.Services
{
    public class IndexSynchronizationService : IIndexSynchronizationService
    {
        private readonly IIndexDataService _indexer;
        private readonly IIndexCreationService _indexCreation;
        private readonly IReadOnlyList<IndexConfig> _indices;

        public IndexSynchronizationService( IIndexDataService indexer, IIndexCreationService indexCreation, IReadOnlyList<IndexConfig> indices )
        {
            _indices = indices;
            _indexCreation = indexCreation;
            _indexer = indexer;
        }

        public async Task SynchronizeAsync()
        {
            foreach( var index in _indices)
            {
                await _indexCreation.CreateIndexIfDoesNotExistAsync( index );
                await _indexer.IndexDataAsync( index );
            }
        }
    }
}
