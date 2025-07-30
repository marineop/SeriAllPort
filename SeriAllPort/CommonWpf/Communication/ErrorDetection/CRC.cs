namespace CommonWpf.Communication.ErrorDetection
{
    public class CRC : ViewModel, IErrorDetection
    {
        private ulong _polynomial;
        public ulong Polynomial
        {
            get => _polynomial;
            set
            {
                if (_polynomial != value)
                {
                    _polynomial = value;
                    OnPropertyChanged();
                }

                PolynomialText = ToHexString(_polynomial);
            }
        }

        private int _polynomialSize;
        public int PolynomialSize
        {
            get => _polynomialSize;
            set
            {
                if (_polynomialSize != value)
                {
                    _polynomialSize = value;
                    OnPropertyChanged();
                }
            }
        }

        private ulong _initialValue;
        public ulong InitialValue
        {
            get => _initialValue;
            set
            {
                if (_initialValue != value)
                {
                    _initialValue = value;
                    OnPropertyChanged();
                }

                InitialValueText = ToHexString(_initialValue);
            }
        }

        private bool _reverseIn;
        public bool ReverseIn
        {
            get => _reverseIn;
            set
            {
                if (_reverseIn != value)
                {
                    _reverseIn = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _reverseOut;
        public bool ReverseOut
        {
            get => _reverseOut;
            set
            {
                if (_reverseOut != value)
                {
                    _reverseOut = value;
                    OnPropertyChanged();
                }
            }
        }

        private ulong _xorOut;
        public ulong XorOut
        {
            get => _xorOut;
            set
            {
                if (_xorOut != value)
                {
                    _xorOut = value;
                    OnPropertyChanged();

                    
                }

                XorOutText = ToHexString(_xorOut);
            }
        }

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _canEdit;
        public bool CanEdit
        {
            get => _canEdit;
            set
            {
                if (_canEdit != value)
                {
                    _canEdit = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _polynomialText = string.Empty;
        public string PolynomialText
        {
            get => _polynomialText;
            set
            {
                if (_polynomialText != value)
                {
                    _polynomialText = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _initialValueText = string.Empty;
        public string InitialValueText
        {
            get => _initialValueText;
            set
            {
                if (_initialValueText != value)
                {
                    _initialValueText = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _xorOutText = string.Empty;
        public string XorOutText
        {
            get => _xorOutText;
            set
            {
                if (_xorOutText != value)
                {
                    _xorOutText = value;
                    OnPropertyChanged();
                }
            }
        }

        public int ComputeErrorDetectionCode(
            byte[] input,
            int index,
            int length,
            byte[] answer,
            Endianness endianness)
        {
            if (PolynomialSize < 1 || PolynomialSize > 64)
            {
                throw new ArgumentOutOfRangeException(nameof(PolynomialSize));
            }

            if (index < 0 || length < 0 || index + length > input.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            ulong crc = InitialValue;
            ulong topBit = 1UL << (PolynomialSize - 1);
            ulong mask = (topBit << 1) - 1;

            int startIndex = 0;
            int endIndex = 8;
            int offset = 1;

            if (!ReverseIn)
            {
                startIndex = 7;
                endIndex = -1;
                offset = -1;

            }

            for (int i = 0; i < length; ++i)
            {
                byte byteNow = input[index + i];
                for (int bit = startIndex; bit != endIndex; bit += offset)
                {
                    bool dataBit = (byteNow & (1 << bit)) != 0;
                    bool crcTop = (crc & topBit) != 0;
                    crc <<= 1;
                    if (dataBit ^ crcTop)
                    {
                        crc ^= Polynomial;
                    }
                }
            }

            crc &= mask;

            if (ReverseOut)
            {
                crc = ReverseBits(crc, PolynomialSize);
            }

            crc ^= XorOut;

            int crcByteCount = (PolynomialSize + 7) / 8;
            for (int i = 0; i < crcByteCount; i++)
            {
                if (endianness == Endianness.LittleEndian)
                {
                    answer[i] = (byte)(crc >> (i << 3));
                }
                else
                {
                    answer[i] = (byte)(crc >> ((crcByteCount - 1 - i) << 3));
                }
            }

            return crcByteCount;
        }

        private static ulong ReverseBits(ulong value, int bitCount)
        {
            ulong result = 0;
            for (int i = 0; i < bitCount; i++)
            {
                result = (result << 1) | (value & 1);
                value >>= 1;
            }

            return result;
        }

        private string ToHexString(ulong newVlaue)
        {
            int hexDigits = (PolynomialSize + 3) / 4;
            return $"0x{newVlaue.ToString($"X{hexDigits}")}";
        }
    }
}

