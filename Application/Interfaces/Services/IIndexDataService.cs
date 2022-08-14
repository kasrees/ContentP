using Application.OptionModels;

namespace Application.Interfaces.Services
{
    public interface IIndexDataService
    {
        Task IndexDataAsync( IndexConfig indexOptions );
    }
}
