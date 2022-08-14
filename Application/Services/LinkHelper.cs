namespace Application.Services
{
    public static class LinkHelper
    {
        public static bool isValidAbsoluteUri(string link)
        {
            Uri? uri;
            bool isCorrectAbsoluteUri = Uri.TryCreate(link, UriKind.Absolute, out uri) && uri.Scheme == Uri.UriSchemeHttps;
            return isCorrectAbsoluteUri && !Equals(uri, null);
        }

        public static string? getRelativeUriFromAbsolute(string link)
        {
            Uri? uri;
            bool isValidAbsoluteUri = Uri.TryCreate(link, UriKind.Absolute, out uri) && uri.Scheme == Uri.UriSchemeHttps;
            if (!isValidAbsoluteUri || Equals(uri, null))
            {
                return null;
            }
            return uri?.PathAndQuery + uri?.Fragment;
        }
    }
}
