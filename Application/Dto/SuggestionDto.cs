using System.Runtime.Serialization;

namespace Application.Dto
{
    public class SuggestionDto
    {
        [DataMember( Name = "input" )]
        public IEnumerable<string> Input { get; private set; }

        [DataMember( Name = "weight" )]
        public int? Weight { get; private set; }
    }
}
