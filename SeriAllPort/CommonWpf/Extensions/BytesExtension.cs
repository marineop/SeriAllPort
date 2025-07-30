using CommonWpf.Communication;
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

        public static string BytesToString(this byte[] bytes, int offset, int length)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(offset);

            ArgumentOutOfRangeException.ThrowIfNegative(length);

            if (offset + length >= bytes.Length)
            {
                throw new IndexOutOfRangeException("(offset + length) must be less than bytes.Length");
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

        public static int ToInt(this byte[] bytes, int startIndex, int length, Endianness endian = Endianness.LittleEndian)
        {
            return (new ReadOnlySpan<byte>(bytes)).ToInt(startIndex, length, endian);
        }

        public static long ToLong(this byte[] bytes, int startIndex, int length, Endianness endian = Endianness.LittleEndian)
        {
            return (new ReadOnlySpan<byte>(bytes)).ToLong(startIndex, length, endian);
        }

        public static uint ToUint(this byte[] bytes, int startIndex, int length, Endianness endian = Endianness.LittleEndian)
        {
            return (new ReadOnlySpan<byte>(bytes)).ToUint(startIndex, length, endian);
        }

        public static ulong ToUlong(this byte[] bytes, int startIndex, int length, Endianness endian = Endianness.LittleEndian)
        {
            return (new ReadOnlySpan<byte>(bytes)).ToUlong(startIndex, length, endian);
        }
    }
}
