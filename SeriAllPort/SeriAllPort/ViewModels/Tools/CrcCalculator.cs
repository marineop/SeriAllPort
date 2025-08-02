using CommonWpf;
using CommonWpf.Communication;
using CommonWpf.Communication.ErrorDetection;
using CommonWpf.Extensions;
using CommonWpf.ViewModels.TextBytes;
using System.Collections.ObjectModel;

namespace SeriAllPort.ViewModels.Tools
{
    public class CrcCalculator : ViewModel
    {
        private ObservableCollection<IErrorDetection> _crcList = [];
        public ObservableCollection<IErrorDetection> CrcList
        {
            get => _crcList;
            set
            {
                if (_crcList != value)
                {
                    _crcList = value;
                    OnPropertyChanged();
                }
            }
        }

        private IErrorDetection? _selectedCrc;
        public IErrorDetection? SelectedCrc
        {
            get => _selectedCrc;
            set
            {
                if (_selectedCrc != value)
                {
                    _selectedCrc = value;
                    OnPropertyChanged();

                    UpdateCrc();
                }
            }
        }

        private string _crcValue = string.Empty;
        public string CrcValue
        {
            get => _crcValue;
            set
            {
                if (_crcValue != value)
                {
                    _crcValue = value;
                    OnPropertyChanged();
                }
            }
        }

        private Endianness _endianness = Endianness.LittleEndian;
        public Endianness Endianness
        {
            get => _endianness;
            set
            {
                if (_endianness != value)
                {
                    _endianness = value;
                    OnPropertyChanged();

                    UpdateCrc();
                }
            }
        }

        private TextBytes _inputData = new TextBytes();
        public TextBytes InputData
        {
            get => _inputData;
            set
            {
                if (_inputData != value)
                {
                    _inputData = value;
                    OnPropertyChanged();
                }
            }
        }

        public CrcCalculator()
        {
            InputData.PropertyChanged += InputData_PropertyChanged;

            CrcList.Add(new Checksum());
            CrcList.Add(new Xor());
            CrcList.Add(new CRC() { Name = "CRC-3/GSM", CanEdit = false, Polynomial = 0x3, PolynomialSize = 3, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0x7 });
            CrcList.Add(new CRC() { Name = "CRC-3/ROHC", CanEdit = false, Polynomial = 0x3, PolynomialSize = 3, InitialValue = 0x7, ReverseIn = true, ReverseOut = true, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-4/G-704", CanEdit = false, Polynomial = 0x3, PolynomialSize = 4, InitialValue = 0x0, ReverseIn = true, ReverseOut = true, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-4/INTERLAKEN", CanEdit = false, Polynomial = 0x3, PolynomialSize = 4, InitialValue = 0xF, ReverseIn = false, ReverseOut = false, XorOut = 0xF });
            CrcList.Add(new CRC() { Name = "CRC-5/EPC-C1G2", CanEdit = false, Polynomial = 0x9, PolynomialSize = 5, InitialValue = 0x9, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-5/G-704", CanEdit = false, Polynomial = 0x15, PolynomialSize = 5, InitialValue = 0x0, ReverseIn = true, ReverseOut = true, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-5/USB", CanEdit = false, Polynomial = 0x5, PolynomialSize = 5, InitialValue = 0x1F, ReverseIn = true, ReverseOut = true, XorOut = 0x1F });
            CrcList.Add(new CRC() { Name = "CRC-6/CDMA2000-A", CanEdit = false, Polynomial = 0x27, PolynomialSize = 6, InitialValue = 0x3F, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-6/CDMA2000-B", CanEdit = false, Polynomial = 0x7, PolynomialSize = 6, InitialValue = 0x3F, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-6/DARC", CanEdit = false, Polynomial = 0x19, PolynomialSize = 6, InitialValue = 0x0, ReverseIn = true, ReverseOut = true, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-6/G-704", CanEdit = false, Polynomial = 0x3, PolynomialSize = 6, InitialValue = 0x0, ReverseIn = true, ReverseOut = true, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-6/GSM", CanEdit = false, Polynomial = 0x2F, PolynomialSize = 6, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0x3F });
            CrcList.Add(new CRC() { Name = "CRC-7/MMC", CanEdit = false, Polynomial = 0x9, PolynomialSize = 7, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-7/ROHC", CanEdit = false, Polynomial = 0x4F, PolynomialSize = 7, InitialValue = 0x7F, ReverseIn = true, ReverseOut = true, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-7/UMTS", CanEdit = false, Polynomial = 0x45, PolynomialSize = 7, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-8/AUTOSAR", CanEdit = false, Polynomial = 0x2F, PolynomialSize = 8, InitialValue = 0xFF, ReverseIn = false, ReverseOut = false, XorOut = 0xFF });
            CrcList.Add(new CRC() { Name = "CRC-8/BLUETOOTH", CanEdit = false, Polynomial = 0xA7, PolynomialSize = 8, InitialValue = 0x0, ReverseIn = true, ReverseOut = true, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-8/CDMA2000", CanEdit = false, Polynomial = 0x9B, PolynomialSize = 8, InitialValue = 0xFF, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-8/DARC", CanEdit = false, Polynomial = 0x39, PolynomialSize = 8, InitialValue = 0x0, ReverseIn = true, ReverseOut = true, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-8/DVB-S2", CanEdit = false, Polynomial = 0xD5, PolynomialSize = 8, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-8/GSM-A", CanEdit = false, Polynomial = 0x1D, PolynomialSize = 8, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-8/GSM-B", CanEdit = false, Polynomial = 0x49, PolynomialSize = 8, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0xFF });
            CrcList.Add(new CRC() { Name = "CRC-8/HITAG", CanEdit = false, Polynomial = 0x1D, PolynomialSize = 8, InitialValue = 0xFF, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-8/I-432-1", CanEdit = false, Polynomial = 0x7, PolynomialSize = 8, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0x55 });
            CrcList.Add(new CRC() { Name = "CRC-8/I-CODE", CanEdit = false, Polynomial = 0x1D, PolynomialSize = 8, InitialValue = 0xFD, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-8/LTE", CanEdit = false, Polynomial = 0x9B, PolynomialSize = 8, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-8/MAXIM-DOW", CanEdit = false, Polynomial = 0x31, PolynomialSize = 8, InitialValue = 0x0, ReverseIn = true, ReverseOut = true, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-8/MIFARE-MAD", CanEdit = false, Polynomial = 0x1D, PolynomialSize = 8, InitialValue = 0xC7, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-8/NRSC-5", CanEdit = false, Polynomial = 0x31, PolynomialSize = 8, InitialValue = 0xFF, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-8/OPENSAFETY", CanEdit = false, Polynomial = 0x2F, PolynomialSize = 8, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-8/ROHC", CanEdit = false, Polynomial = 0x7, PolynomialSize = 8, InitialValue = 0xFF, ReverseIn = true, ReverseOut = true, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-8/SAE-J1850", CanEdit = false, Polynomial = 0x1D, PolynomialSize = 8, InitialValue = 0xFF, ReverseIn = false, ReverseOut = false, XorOut = 0xFF });
            CrcList.Add(new CRC() { Name = "CRC-8/SMBUS", CanEdit = false, Polynomial = 0x7, PolynomialSize = 8, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-8/TECH-3250", CanEdit = false, Polynomial = 0x1D, PolynomialSize = 8, InitialValue = 0xFF, ReverseIn = true, ReverseOut = true, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-8/WCDMA", CanEdit = false, Polynomial = 0x9B, PolynomialSize = 8, InitialValue = 0x0, ReverseIn = true, ReverseOut = true, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-10/ATM", CanEdit = false, Polynomial = 0x233, PolynomialSize = 10, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-10/CDMA2000", CanEdit = false, Polynomial = 0x3D9, PolynomialSize = 10, InitialValue = 0x3FF, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-10/GSM", CanEdit = false, Polynomial = 0x175, PolynomialSize = 10, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0x3FF });
            CrcList.Add(new CRC() { Name = "CRC-11/FLEXRAY", CanEdit = false, Polynomial = 0x385, PolynomialSize = 11, InitialValue = 0x1A, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-11/UMTS", CanEdit = false, Polynomial = 0x307, PolynomialSize = 11, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-12/CDMA2000", CanEdit = false, Polynomial = 0xF13, PolynomialSize = 12, InitialValue = 0xFFF, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-12/DECT", CanEdit = false, Polynomial = 0x80F, PolynomialSize = 12, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-12/GSM", CanEdit = false, Polynomial = 0xD31, PolynomialSize = 12, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0xFFF });
            CrcList.Add(new CRC() { Name = "CRC-12/UMTS", CanEdit = false, Polynomial = 0x80F, PolynomialSize = 12, InitialValue = 0x0, ReverseIn = false, ReverseOut = true, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-13/BBC", CanEdit = false, Polynomial = 0x1CF5, PolynomialSize = 13, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-14/DARC", CanEdit = false, Polynomial = 0x805, PolynomialSize = 14, InitialValue = 0x0, ReverseIn = true, ReverseOut = true, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-14/GSM", CanEdit = false, Polynomial = 0x202D, PolynomialSize = 14, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0x3FFF });
            CrcList.Add(new CRC() { Name = "CRC-15/CAN", CanEdit = false, Polynomial = 0x4599, PolynomialSize = 15, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-15/MPT1327", CanEdit = false, Polynomial = 0x6815, PolynomialSize = 15, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0x1 });
            CrcList.Add(new CRC() { Name = "CRC-16/ARC", CanEdit = false, Polynomial = 0x8005, PolynomialSize = 16, InitialValue = 0x0, ReverseIn = true, ReverseOut = true, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-16/CDMA2000", CanEdit = false, Polynomial = 0xC867, PolynomialSize = 16, InitialValue = 0xFFFF, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-16/CMS", CanEdit = false, Polynomial = 0x8005, PolynomialSize = 16, InitialValue = 0xFFFF, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-16/DDS-110", CanEdit = false, Polynomial = 0x8005, PolynomialSize = 16, InitialValue = 0x800D, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-16/DECT-R", CanEdit = false, Polynomial = 0x589, PolynomialSize = 16, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0x1 });
            CrcList.Add(new CRC() { Name = "CRC-16/DECT-X", CanEdit = false, Polynomial = 0x589, PolynomialSize = 16, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-16/DNP", CanEdit = false, Polynomial = 0x3D65, PolynomialSize = 16, InitialValue = 0x0, ReverseIn = true, ReverseOut = true, XorOut = 0xFFFF });
            CrcList.Add(new CRC() { Name = "CRC-16/EN-13757", CanEdit = false, Polynomial = 0x3D65, PolynomialSize = 16, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0xFFFF });
            CrcList.Add(new CRC() { Name = "CRC-16/GENIBUS", CanEdit = false, Polynomial = 0x1021, PolynomialSize = 16, InitialValue = 0xFFFF, ReverseIn = false, ReverseOut = false, XorOut = 0xFFFF });
            CrcList.Add(new CRC() { Name = "CRC-16/GSM", CanEdit = false, Polynomial = 0x1021, PolynomialSize = 16, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0xFFFF });
            CrcList.Add(new CRC() { Name = "CRC-16/IBM-3740", CanEdit = false, Polynomial = 0x1021, PolynomialSize = 16, InitialValue = 0xFFFF, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-16/IBM-SDLC", CanEdit = false, Polynomial = 0x1021, PolynomialSize = 16, InitialValue = 0xFFFF, ReverseIn = true, ReverseOut = true, XorOut = 0xFFFF });
            CrcList.Add(new CRC() { Name = "CRC-16/ISO-IEC-14443-3-A", CanEdit = false, Polynomial = 0x1021, PolynomialSize = 16, InitialValue = 0xC6C6, ReverseIn = true, ReverseOut = true, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-16/KERMIT", CanEdit = false, Polynomial = 0x1021, PolynomialSize = 16, InitialValue = 0x0, ReverseIn = true, ReverseOut = true, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-16/LJ1200", CanEdit = false, Polynomial = 0x6F63, PolynomialSize = 16, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-16/M17", CanEdit = false, Polynomial = 0x5935, PolynomialSize = 16, InitialValue = 0xFFFF, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-16/MAXIM-DOW", CanEdit = false, Polynomial = 0x8005, PolynomialSize = 16, InitialValue = 0x0, ReverseIn = true, ReverseOut = true, XorOut = 0xFFFF });
            CrcList.Add(new CRC() { Name = "CRC-16/MCRF4XX", CanEdit = false, Polynomial = 0x1021, PolynomialSize = 16, InitialValue = 0xFFFF, ReverseIn = true, ReverseOut = true, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-16/MODBUS", CanEdit = false, Polynomial = 0x8005, PolynomialSize = 16, InitialValue = 0xFFFF, ReverseIn = true, ReverseOut = true, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-16/NRSC-5", CanEdit = false, Polynomial = 0x80B, PolynomialSize = 16, InitialValue = 0xFFFF, ReverseIn = true, ReverseOut = true, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-16/OPENSAFETY-A", CanEdit = false, Polynomial = 0x5935, PolynomialSize = 16, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-16/OPENSAFETY-B", CanEdit = false, Polynomial = 0x755B, PolynomialSize = 16, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-16/PROFIBUS", CanEdit = false, Polynomial = 0x1DCF, PolynomialSize = 16, InitialValue = 0xFFFF, ReverseIn = false, ReverseOut = false, XorOut = 0xFFFF });
            CrcList.Add(new CRC() { Name = "CRC-16/RIELLO", CanEdit = false, Polynomial = 0x1021, PolynomialSize = 16, InitialValue = 0xB2AA, ReverseIn = true, ReverseOut = true, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-16/SPI-FUJITSU", CanEdit = false, Polynomial = 0x1021, PolynomialSize = 16, InitialValue = 0x1D0F, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-16/T10-DIF", CanEdit = false, Polynomial = 0x8BB7, PolynomialSize = 16, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-16/TELEDISK", CanEdit = false, Polynomial = 0xA097, PolynomialSize = 16, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-16/TMS37157", CanEdit = false, Polynomial = 0x1021, PolynomialSize = 16, InitialValue = 0x89EC, ReverseIn = true, ReverseOut = true, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-16/UMTS", CanEdit = false, Polynomial = 0x8005, PolynomialSize = 16, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-16/USB", CanEdit = false, Polynomial = 0x8005, PolynomialSize = 16, InitialValue = 0xFFFF, ReverseIn = true, ReverseOut = true, XorOut = 0xFFFF });
            CrcList.Add(new CRC() { Name = "CRC-16/XMODEM", CanEdit = false, Polynomial = 0x1021, PolynomialSize = 16, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-17/CAN-FD", CanEdit = false, Polynomial = 0x1685B, PolynomialSize = 17, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-21/CAN-FD", CanEdit = false, Polynomial = 0x102899, PolynomialSize = 21, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-24/BLE", CanEdit = false, Polynomial = 0x65B, PolynomialSize = 24, InitialValue = 0x555555, ReverseIn = true, ReverseOut = true, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-24/FLEXRAY-A", CanEdit = false, Polynomial = 0x5D6DCB, PolynomialSize = 24, InitialValue = 0xFEDCBA, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-24/FLEXRAY-B", CanEdit = false, Polynomial = 0x5D6DCB, PolynomialSize = 24, InitialValue = 0xABCDEF, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-24/INTERLAKEN", CanEdit = false, Polynomial = 0x328B63, PolynomialSize = 24, InitialValue = 0xFFFFFF, ReverseIn = false, ReverseOut = false, XorOut = 0xFFFFFF });
            CrcList.Add(new CRC() { Name = "CRC-24/LTE-A", CanEdit = false, Polynomial = 0x864CFB, PolynomialSize = 24, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-24/LTE-B", CanEdit = false, Polynomial = 0x800063, PolynomialSize = 24, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-24/OPENPGP", CanEdit = false, Polynomial = 0x864CFB, PolynomialSize = 24, InitialValue = 0xB704CE, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-24/OS-9", CanEdit = false, Polynomial = 0x800063, PolynomialSize = 24, InitialValue = 0xFFFFFF, ReverseIn = false, ReverseOut = false, XorOut = 0xFFFFFF });
            CrcList.Add(new CRC() { Name = "CRC-30/CDMA", CanEdit = false, Polynomial = 0x2030B9C7, PolynomialSize = 30, InitialValue = 0x3FFFFFFF, ReverseIn = false, ReverseOut = false, XorOut = 0x3FFFFFFF });
            CrcList.Add(new CRC() { Name = "CRC-31/PHILIPS", CanEdit = false, Polynomial = 0x4C11DB7, PolynomialSize = 31, InitialValue = 0x7FFFFFFF, ReverseIn = false, ReverseOut = false, XorOut = 0x7FFFFFFF });
            CrcList.Add(new CRC() { Name = "CRC-32/AIXM", CanEdit = false, Polynomial = 0x814141AB, PolynomialSize = 32, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-32/AUTOSAR", CanEdit = false, Polynomial = 0xF4ACFB13, PolynomialSize = 32, InitialValue = 0xFFFFFFFF, ReverseIn = true, ReverseOut = true, XorOut = 0xFFFFFFFF });
            CrcList.Add(new CRC() { Name = "CRC-32/BASE91-D", CanEdit = false, Polynomial = 0xA833982B, PolynomialSize = 32, InitialValue = 0xFFFFFFFF, ReverseIn = true, ReverseOut = true, XorOut = 0xFFFFFFFF });
            CrcList.Add(new CRC() { Name = "CRC-32/BZIP2", CanEdit = false, Polynomial = 0x4C11DB7, PolynomialSize = 32, InitialValue = 0xFFFFFFFF, ReverseIn = false, ReverseOut = false, XorOut = 0xFFFFFFFF });
            CrcList.Add(new CRC() { Name = "CRC-32/CD-ROM-EDC", CanEdit = false, Polynomial = 0x8001801B, PolynomialSize = 32, InitialValue = 0x0, ReverseIn = true, ReverseOut = true, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-32/CKSUM", CanEdit = false, Polynomial = 0x4C11DB7, PolynomialSize = 32, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0xFFFFFFFF });
            CrcList.Add(new CRC() { Name = "CRC-32/ISCSI", CanEdit = false, Polynomial = 0x1EDC6F41, PolynomialSize = 32, InitialValue = 0xFFFFFFFF, ReverseIn = true, ReverseOut = true, XorOut = 0xFFFFFFFF });
            CrcList.Add(new CRC() { Name = "CRC-32/ISO-HDLC", CanEdit = false, Polynomial = 0x4C11DB7, PolynomialSize = 32, InitialValue = 0xFFFFFFFF, ReverseIn = true, ReverseOut = true, XorOut = 0xFFFFFFFF });
            CrcList.Add(new CRC() { Name = "CRC-32/JAMCRC", CanEdit = false, Polynomial = 0x4C11DB7, PolynomialSize = 32, InitialValue = 0xFFFFFFFF, ReverseIn = true, ReverseOut = true, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-32/MEF", CanEdit = false, Polynomial = 0x741B8CD7, PolynomialSize = 32, InitialValue = 0xFFFFFFFF, ReverseIn = true, ReverseOut = true, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-32/MPEG-2", CanEdit = false, Polynomial = 0x4C11DB7, PolynomialSize = 32, InitialValue = 0xFFFFFFFF, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-32/XFER", CanEdit = false, Polynomial = 0xAF, PolynomialSize = 32, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-40/GSM", CanEdit = false, Polynomial = 0x4820009, PolynomialSize = 40, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0xFFFFFFFFFF });
            CrcList.Add(new CRC() { Name = "CRC-64/ECMA-182", CanEdit = false, Polynomial = 0x42F0E1EBA9EA3693, PolynomialSize = 64, InitialValue = 0x0, ReverseIn = false, ReverseOut = false, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-64/GO-ISO", CanEdit = false, Polynomial = 0x1B, PolynomialSize = 64, InitialValue = 0xFFFFFFFFFFFFFFFF, ReverseIn = true, ReverseOut = true, XorOut = 0xFFFFFFFFFFFFFFFF });
            CrcList.Add(new CRC() { Name = "CRC-64/MS", CanEdit = false, Polynomial = 0x259C84CBA6426349, PolynomialSize = 64, InitialValue = 0xFFFFFFFFFFFFFFFF, ReverseIn = true, ReverseOut = true, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-64/NVME", CanEdit = false, Polynomial = 0xAD93D23594C93659, PolynomialSize = 64, InitialValue = 0xFFFFFFFFFFFFFFFF, ReverseIn = true, ReverseOut = true, XorOut = 0xFFFFFFFFFFFFFFFF });
            CrcList.Add(new CRC() { Name = "CRC-64/REDIS", CanEdit = false, Polynomial = 0xAD93D23594C935A9, PolynomialSize = 64, InitialValue = 0x0, ReverseIn = true, ReverseOut = true, XorOut = 0x0 });
            CrcList.Add(new CRC() { Name = "CRC-64/WE", CanEdit = false, Polynomial = 0x42F0E1EBA9EA3693, PolynomialSize = 64, InitialValue = 0xFFFFFFFFFFFFFFFF, ReverseIn = false, ReverseOut = false, XorOut = 0xFFFFFFFFFFFFFFFF });
            CrcList.Add(new CRC() { Name = "CRC-64/XZ", CanEdit = false, Polynomial = 0x42F0E1EBA9EA3693, PolynomialSize = 64, InitialValue = 0xFFFFFFFFFFFFFFFF, ReverseIn = true, ReverseOut = true, XorOut = 0xFFFFFFFFFFFFFFFF });

            foreach (IErrorDetection crc in CrcList)
            {
                crc.PropertyChanged += Crc_PropertyChanged;
            }

            _selectedCrc = CrcList[0];

            UpdateCrc();
        }

        private void Crc_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            UpdateCrc();
        }

        private void InputData_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(InputData.Bytes))
            {
                UpdateCrc();
            }
        }

        private void UpdateCrc()
        {
            if (SelectedCrc != null)
            {
                byte[] crc = new byte[8];
                int byteCount = SelectedCrc.ComputeErrorDetectionCode(
                    InputData.Bytes,
                    0, InputData.Bytes.Length,
                    crc,
                    Endianness);

                CrcValue = crc.SubArray(0, byteCount).BytesToString();
            }
        }
    }
}
