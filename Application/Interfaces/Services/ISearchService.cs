using Application.Dto;

namespace Application.Interfaces.Services
{
    public interface ISearchService
    {
        public ResultDto Search( string query, int size );
    }
}
