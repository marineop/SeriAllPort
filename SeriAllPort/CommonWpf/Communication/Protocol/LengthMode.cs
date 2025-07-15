using System.ComponentModel;

namespace CommonWpf.Communication.Protocol
{
    public enum LengthMode
    {
        [Description("Fixed Length")]
        FixedLength,

        [Description("Fixed Data")]
        FixedData,

        [Description("Variable Length")]
        VariableLength
    }
}
