
using CommonWpf.Communication;

namespace CommonWpf.Extensions
{
    public static class LongExtension
    {
        public static int FillBytes(
            this long longValue,  // the value to be converted to bytes, will cast to long first
            IList<byte> bytes, // target byte array
            int bytesStartIndex, // where to start fill the longValue bytes
            int intByteCount, // might be 1 ~ 8
            Endianness endian = Endianness.LittleEndian)
        {
            ArgumentNullException.ThrowIfNull(bytes);

            if (intByteCount < 1 || intByteCount > 8)
            {
                throw new ArgumentOutOfRangeException(nameof(intByteCount), "Only lengths from 1 to 8 are supported.");
            }

            if (bytesStartIndex < 0 || bytesStartIndex + intByteCount > bytes.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(bytesStartIndex), "Not enough space in the target byte array.");
            }

            // Write each byte from the long to the target array
            for (int i = 0; i < intByteCount; i++)
            {
                int shift = endian == Endianness.LittleEndian
                    ? i * 8
                    : (intByteCount - 1 - i) * 8;

                bytes[bytesStartIndex + i] = (byte)((longValue >> shift) & 0xFF);
            }

            return intByteCount;
        }
    }
}
