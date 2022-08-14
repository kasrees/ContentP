namespace Domain.Entities
{
    public class Language
    {
        public int Id { get; private set; }
        public string Code { get; private set; }

        public Language(string code)
        {
            Code = code;
        }
    }
}
