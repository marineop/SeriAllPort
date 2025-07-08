using System.Text;

namespace CommonWpf.Extensions
{
    public static class BytesExtension
    {
        public static string BytesToString(this byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder((bytes.Length * 3) - 1);

            for (int i = 0; i < bytes.Length; i++)
            {
                sb.AppendFormat("{0:X2}", bytes[i]);
                if (i < bytes.Length - 1)
                {
                    sb.Append(' ');
                }
            }

            return sb.ToString();
        }

        // Convert a specific range in the byte array to hex string
        public static string BytesToString(this byte[] bytes, int offset, int length)
        {
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset must not be less than 0");
            }

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length must not be less than 0");
            }

            if (offset + length >= bytes.Length)
            {
                throw new ArgumentOutOfRangeException("(offset + length) must be less than bytes.Length");
            }

            int end = Math.Min(offset + length, bytes.Length);
            StringBuilder sb = new StringBuilder((length * 3) - 1);

            for (int i = offset; i < end; i++)
            {
                sb.AppendFormat("{0:X2}", bytes[i]);
                if (i != end - 1)
                {
                    sb.Append(' ');
                }
            }

            return sb.ToString();
        }

        public static byte[] SubArray(this byte[] bytes, int offset, int length)
        {
            byte[] answer = new byte[length];
            Array.Copy(bytes, offset, answer, 0, length);

            return answer;
        }
    }
}
