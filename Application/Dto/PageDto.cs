using Nest;

namespace Application.Dto
{
    [ElasticsearchType( IdProperty = nameof( Url ) )]
    public class PageDto
    {
        public string Url { get; set; }
        public string Language { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IReadOnlyList<DataElementDto> DataElements { get; set; }
        public DateTime LastUpdate { get; set; }
        public bool IsGenerated { get; set; }
        public bool IsHidden { get; set; }
    }
}
