namespace Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string GetParameterlessUrl( this string url )
        {
            int index = url.IndexOf( '?' );
            return url[ ..( index == -1 ? url.Length : index ) ];
        }
    }
}
