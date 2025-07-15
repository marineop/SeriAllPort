using System.ComponentModel;

namespace CommonWpf.Communication.Prococol
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
