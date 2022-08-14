using System.Runtime.Serialization;

namespace Application.Models
{
    public class Suggestion
    {
        [DataMember( Name = "input" )]
        public IEnumerable<string> Input { get; private set; }

        [DataMember( Name = "weight" )]
        public int? Weight { get; private set; }

        public Suggestion( string data, int? weight )
        {
            Input = new List<string>( data.Split() );
            Weight = weight;
        }
    }
}
