namespace Application.Models
{
    public class DataElement
    {
        public string Tag { get; private set; }
        public string Data { get; private set; }
        public int Weight { get; private set; }
        public Suggestion Suggest { get; private set; }

        public DataElement( string tag, string data, int weight )
        {
            Tag = tag;
            Data = data;
            Weight = weight;
            Suggest = new Suggestion( data, weight );
        }
    }
}
