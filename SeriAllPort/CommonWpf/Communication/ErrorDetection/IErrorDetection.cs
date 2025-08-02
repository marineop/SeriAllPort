using System.ComponentModel;

namespace CommonWpf.Communication.ErrorDetection
{
    public interface IErrorDetection : INotifyPropertyChanged
    {
        string Name { get; }
        bool CanEdit { get; }

        int ComputeErrorDetectionCode(byte[] input, int startIndex, int length, byte[] errorDetectionCode, Endianness endianness);
    }
}
