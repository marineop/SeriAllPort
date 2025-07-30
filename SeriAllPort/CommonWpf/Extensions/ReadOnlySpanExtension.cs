using CommonWpf.Communication;

namespace CommonWpf.Extensions
{
    public static class ReadOnlySpanExtension
    {
        public static int ToInt(this ReadOnlySpan<byte> bytes, int startIndex, int length, Endianness endian = Endianness.LittleEndian)
        {
            return (int)ToLong(bytes, startIndex, length, endian);
        }

        public static long ToLong(this ReadOnlySpan<byte> bytes, int startIndex, int length, Endianness endian = Endianness.LittleEndian)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            if (length < 1 || length > 8)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Only lengths from 1 to 8 are supported.");
            }

            if (startIndex < 0 || startIndex + length > bytes.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex), "Not enough bytes from start index.");
            }

            long result = 0;

            if (endian == Endianness.LittleEndian)
            {
                for (int i = 0; i < length; i++)
                {
                    result |= ((long)bytes[startIndex + i]) << (8 * i);
                }
            }
            else // Big endian
            {
                for (int i = 0; i < length; i++)
                {
                    result <<= 8;
                    result |= bytes[startIndex + i];
                }
            }

            // Sign extend if MSB of the highest byte is set (negative number)
            long signBitMask = 1L << (length * 8 - 1);
            if ((result & signBitMask) != 0)
            {
                long extensionMask = ~((1L << (length * 8)) - 1);
                result |= extensionMask;
            }

            return result;
        }

        public static uint ToUint(this ReadOnlySpan<byte> bytes, int startIndex, int length, Endianness endian = Endianness.LittleEndian)
        {
            return (uint)ToUlong(bytes, startIndex, length, endian);
        }

        public static ulong ToUlong(this ReadOnlySpan<byte> bytes, int startIndex, int length, Endianness endian = Endianness.LittleEndian)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            if (length < 1 || length > 8)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Only lengths from 1 to 8 are supported.");
            }

            if (startIndex < 0 || startIndex + length > bytes.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex), "Not enough bytes from start index.");
            }

            ulong result = 0;

            if (endian == Endianness.LittleEndian)
            {
                for (int i = 0; i < length; i++)
                {
                    result <<= 8;
                    result |= bytes[startIndex + (length - i - 1)];
                }
            }
            else // Big endian
            {
                for (int i = 0; i < length; i++)
                {
                    result <<= 8;
                    result |= bytes[startIndex + i];
                }
            }

            return result;
        }
    }
}
