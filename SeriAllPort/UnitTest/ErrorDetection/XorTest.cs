using CommonWpf.Communication;
using CommonWpf.Communication.ErrorDetection;

namespace UnitTest.ErrorDetection
{
    [TestClass]
    public sealed class XorTest
    {
        [TestMethod]
        public void BitCount8()
        {
            Xor xor = new Xor();
            xor.BitCount = 8;

            byte[] input;
            byte[] answer;
            int answerLength;

            input = [0x55, 0xAA];
            answer = new byte[1];
            answerLength = xor.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xFF }, answer);
        }

        [TestMethod]
        public void BitCount8_OnesComplement()
        {
            Xor xor = new Xor();
            xor.BitCount = 8;
            xor.OnesComplement = true;

            byte[] input;
            byte[] answer;
            int answerLength;

            input = [0x55, 0xAA];
            answer = new byte[1];
            answerLength = xor.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x00 }, answer);
        }
    }
}
