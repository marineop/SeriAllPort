using CommonWpf.Communication;
using CommonWpf.Extensions;

namespace UnitTest
{
    [TestClass]
    public sealed class BytesExtensionTest
    {
        [TestMethod]
        public void Int16()
        {
            byte[] inputData = [0xFF, 0xFF];
            long result = inputData.ToInt(0, inputData.Length, Endianness.LittleEndian);
            long expected = -1;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Int24_n1()
        {
            byte[] inputData = [0xFF, 0xFF, 0xFF];
            long result = inputData.ToInt(0, inputData.Length, Endianness.LittleEndian);
            long expected = -1;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Int24_n2()
        {
            byte[] inputData = [0xFE, 0xFF, 0xFF];
            long result = inputData.ToInt(0, inputData.Length, Endianness.LittleEndian);
            long expected = -2;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Int24_max()
        {
            byte[] inputData = [0xFF, 0xFF, 0x7F];
            long result = inputData.ToInt(0, inputData.Length, Endianness.LittleEndian);
            long expected = 8388607;
            Assert.AreEqual(expected, result);
        }

        public void Int24_smallest()
        {
            byte[] inputData = [0x00, 0x00, 0x80];
            long result = inputData.ToInt(0, inputData.Length, Endianness.LittleEndian);
            long expected = -8388608;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Int32()
        {
            int[] list = [-1, 0, 1, 2147483647, 2147483646, -2147483648, -2147483647, 8388607, 8388608, -8388608, -8388607];
            foreach (int expected in list)
            {
                byte[] inputData = BitConverter.GetBytes(expected);
                long result = inputData.ToInt(0, inputData.Length, Endianness.LittleEndian);

                Assert.AreEqual(expected, result);
            }
        }
    }
}
