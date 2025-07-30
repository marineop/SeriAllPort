using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonWpf.Communication.ErrorDetection
{
    public interface IErrorDetection
    {
        public string Name { get; }
        public bool CanEdit { get; }

        int ComputeErrorDetectionCode(byte[] input, int startIndex, int length, byte[] errorDetectionCode, Endianness endianness);
    }
}
