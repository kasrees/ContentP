using Domain.Entities;

namespace Application.Mappers
{
    public class KeywordMapper
    {
        public static KeywordDto Map(int id, string phrase, int order)
        {
            return new KeywordDto
            {
                Id = id,
                Phrase = phrase,
                Order = order
            };
        }
    }
}
