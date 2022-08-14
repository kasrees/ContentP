namespace Application.Models
{
    public class SearchItem
    {
        public string Url { get; private set; }
        public string Tag { get; private set; }
        public string Data { get; private set; }
        public int Weight { get; private set; }

        public SearchItem( string url, string tag, string data, int weight )
        {
            Validate( url );
            Validate( tag );
            Validate( data );
            Weight = weight;

            Url = url;
            Tag = tag;
            Data = PrepareData( data );
        }

        private static void Validate( string content )
        {
            if ( string.IsNullOrWhiteSpace( content ) )
            {
                throw new ArgumentNullException( nameof( content ) );
            }
        }

        private static string PrepareData( string data )
        {
            char[] trimChars = new char[] { '\n', '\t' };
            return data.Trim( trimChars ).Trim();
        }

        public DataElement ToDataElement()
        {
            return new DataElement( Tag, Data, Weight );
        }
    }
}
