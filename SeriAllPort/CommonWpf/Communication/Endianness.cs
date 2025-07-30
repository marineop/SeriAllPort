using System.ComponentModel;

namespace CommonWpf.Communication
{
    public enum Endianness
    {
        [Description("Little-endian")]
        LittleEndian,

        [Description("Big-endian")]
        BigEndian,
    }
}
