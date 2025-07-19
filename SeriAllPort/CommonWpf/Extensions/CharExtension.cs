namespace CommonWpf.Extensions
{
    public static class CharExtension
    {
        public static bool IsHexDigit(this char c)
        {
            return (c >= '0' && c <= '9')
                || (c >= 'A' && c <= 'F')
                || (c >= 'a' && c <= 'f');
        }
    }
}
