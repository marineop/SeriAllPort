namespace CommonWpf.Extensions
{
    public static class StringExtension
    {
        public static byte[] HexStringToBytes(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return [];
            }

            List<byte> bytes = [];
            int length = text.Length;
            int i = 0;

            while (i < length)
            {
                // Skip whitespace
                while (i < length && char.IsWhiteSpace(text[i]))
                {
                    i++;
                }

                if (i >= length)
                {
                    break;
                }

                // First nibble
                char c1 = text[i++];
                if (!c1.IsHexDigit())
                {
                    throw new FormatException($"Invalid hex character: {c1}");
                }

                int highNibble = HexValue(c1);

                // Skip whitespace between nibbles
                while (i < length && char.IsWhiteSpace(text[i]))
                {
                    i++;
                }

                int lowNibble;

                if (i < length && text[i].IsHexDigit())
                {
                    char c2 = text[i++];
                    lowNibble = HexValue(c2);
                }
                else
                {
                    // Pad with trailing zero if no second nibble
                    lowNibble = 0;
                }

                bytes.Add((byte)((highNibble << 4) | lowNibble));
            }

            return [.. bytes];
        }

        private static int HexValue(char c)
        {
            if (c >= '0' && c <= '9')
            {
                return c - '0';
            }

            if (c >= 'A' && c <= 'F')
            {
                return c - 'A' + 10;
            }

            if (c >= 'a' && c <= 'f')
            {
                return c - 'a' + 10;
            }

            throw new FormatException($"Invalid hex character: {(int)c}");
        }
    }
}
