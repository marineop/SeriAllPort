using CommonWpf.Communication;
using CommonWpf.Communication.ErrorDetection;

namespace UnitTest.ErrorDetection
{
    [TestClass]
    public sealed class ChecksumTest
    {
        [TestMethod]
        public void BitCount8()
        {
            Checksum checksum = new Checksum();
            checksum.BitCount = 8;

            byte[] input;
            byte[] answer;
            int answerLength;

            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = checksum.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xDD }, answer);
        }

        [TestMethod]
        public void BitCount16()
        {
            Checksum checksum = new Checksum();
            checksum.BitCount = 16;

            byte[] input;
            byte[] answer;
            int answerLength;

            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = checksum.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xDD, 0x01 }, answer);
        }

        [TestMethod]
        public void BitCount16_OnesComplement()
        {
            Checksum checksum = new Checksum();
            checksum.BitCount = 16;
            checksum.OnesComplement = true;

            byte[] input;
            byte[] answer;
            int answerLength;

            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = checksum.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x22, 0xFE }, answer);
        }

        [TestMethod]
        public void BitCount16BigEndian()
        {
            Checksum checksum = new Checksum();
            checksum.BitCount = 16;

            byte[] input;
            byte[] answer;
            int answerLength;

            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = checksum.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x01, 0xDD }, answer);
        }
    }
}
