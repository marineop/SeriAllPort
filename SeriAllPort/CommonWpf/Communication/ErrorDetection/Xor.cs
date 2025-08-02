namespace CommonWpf.Communication.ErrorDetection
{
    public class Xor : ViewModel, IErrorDetection
    {
        private int _bitCount = 8;
        public int BitCount
        {
            get => _bitCount;
            set
            {
                if (_bitCount != value)
                {
                    _bitCount = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _OnesComplement = false;
        public bool OnesComplement
        {
            get => _OnesComplement;
            set
            {
                if (_OnesComplement != value)
                {
                    _OnesComplement = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Name => "XOR";

        public bool CanEdit => true;

        public int ComputeErrorDetectionCode(byte[] input, int startIndex, int length, byte[] errorDetectionCode, Endianness endianness)
        {
            uint checksum = 0;
            for (int i = 0; i < length; ++i)
            {
                checksum ^= input[startIndex + i];
            }

            if (OnesComplement)
            {
                checksum = ~checksum;
            }

            uint mask = (1U << BitCount) - 1;
            checksum &= mask;

            int byteCount = (BitCount + 7) >> 3;

            for (int i = 0; i < byteCount; ++i)
            {
                int shift;
                if (endianness == Endianness.LittleEndian)
                {
                    shift = 8 * i;
                }
                else
                {
                    shift = 8 * (byteCount - 1 - i);
                }

                errorDetectionCode[i] = (byte)((checksum >> shift) & 0xFF);
            }

            return byteCount;

        }
    }
}
