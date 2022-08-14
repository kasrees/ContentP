using Nest;

namespace Application.Models
{
    [ElasticsearchType( IdProperty = nameof( Url ) )]
    public class Page
    {
        public string Url { get; private set; }
        public string Language { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public IReadOnlyList<DataElement> DataElements { get; private set; }
        public DateTime LastUpdate { get; private set; }
        public bool IsGenerated { get; private set; }
        public bool IsHidden { get; private set; }

        public Page( string url, string languageShort, IEnumerable<SearchItem> elementList )
        {
            if ( string.IsNullOrWhiteSpace( url ) )
            {
                throw new ArgumentException( $"'{nameof( url )}' cannot be null or whitespace.", nameof( url ) );
            }
            if ( !elementList.Any() )
            {
                throw new ArgumentNullException( nameof( elementList ), $"Can't create {nameof( Page )} wtih empty element list." );
            }

            Url = $"{languageShort}-{url}";
            Language = languageShort;
            Title = CreateTitle( elementList );
            Description = "";
            DataElements = CreateDataElements( elementList );
            LastUpdate = DateTime.Now;
            IsGenerated = true;
            IsHidden = false;
        }

        private string CreateTitle( IEnumerable<SearchItem> elementList )
        {
            return elementList
                .Where( elem => elem.Tag == "h1" )
                .Select( a => a.Data )
                .FirstOrDefault() ?? "";
        }

        private IReadOnlyList<DataElement> CreateDataElements( IEnumerable<SearchItem> elementList )
        {
            return elementList
                .Select( elem => elem.ToDataElement() )
                .ToList();
        }
    }
}
