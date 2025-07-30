using CommonWpf.Communication;
using CommonWpf.Communication.ErrorDetection;

namespace UnitTest.ErrorDetection
{
    [TestClass]
    public sealed class CRCTest
    {

        [TestMethod]
        public void CRC_3_GSM()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x3;
            crc.PolynomialSize = 3;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x7;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x04 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x04 }, answer);
        }
        [TestMethod]
        public void CRC_3_ROHC()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x3;
            crc.PolynomialSize = 3;
            crc.InitialValue = 0x7;
            crc.ReverseIn = true;
            crc.ReverseOut = true;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x06 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x06 }, answer);
        }
        [TestMethod]
        public void CRC_4_G_704()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x3;
            crc.PolynomialSize = 4;
            crc.InitialValue = 0x0;
            crc.ReverseIn = true;
            crc.ReverseOut = true;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x07 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x07 }, answer);
        }
        [TestMethod]
        public void CRC_4_INTERLAKEN()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x3;
            crc.PolynomialSize = 4;
            crc.InitialValue = 0xF;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0xF;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x0B }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x0B }, answer);
        }
        [TestMethod]
        public void CRC_5_EPC_C1G2()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x9;
            crc.PolynomialSize = 5;
            crc.InitialValue = 0x9;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x00 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x00 }, answer);
        }
        [TestMethod]
        public void CRC_5_G_704()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x15;
            crc.PolynomialSize = 5;
            crc.InitialValue = 0x0;
            crc.ReverseIn = true;
            crc.ReverseOut = true;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x07 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x07 }, answer);
        }
        [TestMethod]
        public void CRC_5_USB()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x5;
            crc.PolynomialSize = 5;
            crc.InitialValue = 0x1F;
            crc.ReverseIn = true;
            crc.ReverseOut = true;
            crc.XorOut = 0x1F;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x19 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x19 }, answer);
        }
        [TestMethod]
        public void CRC_6_CDMA2000_A()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x27;
            crc.PolynomialSize = 6;
            crc.InitialValue = 0x3F;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x0D }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x0D }, answer);
        }
        [TestMethod]
        public void CRC_6_CDMA2000_B()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x7;
            crc.PolynomialSize = 6;
            crc.InitialValue = 0x3F;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x3B }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x3B }, answer);
        }
        [TestMethod]
        public void CRC_6_DARC()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x19;
            crc.PolynomialSize = 6;
            crc.InitialValue = 0x0;
            crc.ReverseIn = true;
            crc.ReverseOut = true;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x26 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x26 }, answer);
        }
        [TestMethod]
        public void CRC_6_G_704()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x3;
            crc.PolynomialSize = 6;
            crc.InitialValue = 0x0;
            crc.ReverseIn = true;
            crc.ReverseOut = true;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x06 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x06 }, answer);
        }
        [TestMethod]
        public void CRC_6_GSM()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x2F;
            crc.PolynomialSize = 6;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x3F;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x13 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x13 }, answer);
        }
        [TestMethod]
        public void CRC_7_MMC()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x9;
            crc.PolynomialSize = 7;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x75 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x75 }, answer);
        }
        [TestMethod]
        public void CRC_7_ROHC()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x4F;
            crc.PolynomialSize = 7;
            crc.InitialValue = 0x7F;
            crc.ReverseIn = true;
            crc.ReverseOut = true;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x53 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x53 }, answer);
        }
        [TestMethod]
        public void CRC_7_UMTS()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x45;
            crc.PolynomialSize = 7;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x61 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x61 }, answer);
        }
        [TestMethod]
        public void CRC_8_AUTOSAR()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x2F;
            crc.PolynomialSize = 8;
            crc.InitialValue = 0xFF;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0xFF;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xDF }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xDF }, answer);
        }
        [TestMethod]
        public void CRC_8_BLUETOOTH()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0xA7;
            crc.PolynomialSize = 8;
            crc.InitialValue = 0x0;
            crc.ReverseIn = true;
            crc.ReverseOut = true;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x26 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x26 }, answer);
        }
        [TestMethod]
        public void CRC_8_CDMA2000()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x9B;
            crc.PolynomialSize = 8;
            crc.InitialValue = 0xFF;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xDA }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xDA }, answer);
        }
        [TestMethod]
        public void CRC_8_DARC()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x39;
            crc.PolynomialSize = 8;
            crc.InitialValue = 0x0;
            crc.ReverseIn = true;
            crc.ReverseOut = true;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x15 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x15 }, answer);
        }
        [TestMethod]
        public void CRC_8_DVB_S2()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0xD5;
            crc.PolynomialSize = 8;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xBC }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xBC }, answer);
        }
        [TestMethod]
        public void CRC_8_GSM_A()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x1D;
            crc.PolynomialSize = 8;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x37 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x37 }, answer);
        }
        [TestMethod]
        public void CRC_8_GSM_B()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x49;
            crc.PolynomialSize = 8;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0xFF;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x94 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x94 }, answer);
        }
        [TestMethod]
        public void CRC_8_HITAG()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x1D;
            crc.PolynomialSize = 8;
            crc.InitialValue = 0xFF;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xB4 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xB4 }, answer);
        }
        [TestMethod]
        public void CRC_8_I_432_1()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x7;
            crc.PolynomialSize = 8;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x55;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xA1 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xA1 }, answer);
        }
        [TestMethod]
        public void CRC_8_I_CODE()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x1D;
            crc.PolynomialSize = 8;
            crc.InitialValue = 0xFD;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x7E }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x7E }, answer);
        }
        [TestMethod]
        public void CRC_8_LTE()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x9B;
            crc.PolynomialSize = 8;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xEA }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xEA }, answer);
        }
        [TestMethod]
        public void CRC_8_MAXIM_DOW()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x31;
            crc.PolynomialSize = 8;
            crc.InitialValue = 0x0;
            crc.ReverseIn = true;
            crc.ReverseOut = true;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xA1 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xA1 }, answer);
        }
        [TestMethod]
        public void CRC_8_MIFARE_MAD()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x1D;
            crc.PolynomialSize = 8;
            crc.InitialValue = 0xC7;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x99 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x99 }, answer);
        }
        [TestMethod]
        public void CRC_8_NRSC_5()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x31;
            crc.PolynomialSize = 8;
            crc.InitialValue = 0xFF;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xF7 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xF7 }, answer);
        }
        [TestMethod]
        public void CRC_8_OPENSAFETY()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x2F;
            crc.PolynomialSize = 8;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x3E }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x3E }, answer);
        }
        [TestMethod]
        public void CRC_8_ROHC()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x7;
            crc.PolynomialSize = 8;
            crc.InitialValue = 0xFF;
            crc.ReverseIn = true;
            crc.ReverseOut = true;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xD0 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xD0 }, answer);
        }
        [TestMethod]
        public void CRC_8_SAE_J1850()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x1D;
            crc.PolynomialSize = 8;
            crc.InitialValue = 0xFF;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0xFF;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x4B }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x4B }, answer);
        }
        [TestMethod]
        public void CRC_8_SMBUS()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x7;
            crc.PolynomialSize = 8;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xF4 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xF4 }, answer);
        }
        [TestMethod]
        public void CRC_8_TECH_3250()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x1D;
            crc.PolynomialSize = 8;
            crc.InitialValue = 0xFF;
            crc.ReverseIn = true;
            crc.ReverseOut = true;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x97 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x97 }, answer);
        }
        [TestMethod]
        public void CRC_8_WCDMA()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x9B;
            crc.PolynomialSize = 8;
            crc.InitialValue = 0x0;
            crc.ReverseIn = true;
            crc.ReverseOut = true;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x25 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[1];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x25 }, answer);
        }
        [TestMethod]
        public void CRC_10_ATM()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x233;
            crc.PolynomialSize = 10;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x99, 0x01 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x01, 0x99 }, answer);
        }
        [TestMethod]
        public void CRC_10_CDMA2000()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x3D9;
            crc.PolynomialSize = 10;
            crc.InitialValue = 0x3FF;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x33, 0x02 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x02, 0x33 }, answer);
        }
        [TestMethod]
        public void CRC_10_GSM()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x175;
            crc.PolynomialSize = 10;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x3FF;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x2A, 0x01 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x01, 0x2A }, answer);
        }
        [TestMethod]
        public void CRC_11_FLEXRAY()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x385;
            crc.PolynomialSize = 11;
            crc.InitialValue = 0x1A;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xA3, 0x05 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x05, 0xA3 }, answer);
        }
        [TestMethod]
        public void CRC_11_UMTS()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x307;
            crc.PolynomialSize = 11;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x61, 0x00 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x00, 0x61 }, answer);
        }
        [TestMethod]
        public void CRC_12_CDMA2000()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0xF13;
            crc.PolynomialSize = 12;
            crc.InitialValue = 0xFFF;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x4D, 0x0D }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x0D, 0x4D }, answer);
        }
        [TestMethod]
        public void CRC_12_DECT()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x80F;
            crc.PolynomialSize = 12;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x5B, 0x0F }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x0F, 0x5B }, answer);
        }
        [TestMethod]
        public void CRC_12_GSM()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0xD31;
            crc.PolynomialSize = 12;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0xFFF;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x34, 0x0B }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x0B, 0x34 }, answer);
        }
        [TestMethod]
        public void CRC_12_UMTS()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x80F;
            crc.PolynomialSize = 12;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = true;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xAF, 0x0D }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x0D, 0xAF }, answer);
        }
        [TestMethod]
        public void CRC_13_BBC()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x1CF5;
            crc.PolynomialSize = 13;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xFA, 0x04 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x04, 0xFA }, answer);
        }
        [TestMethod]
        public void CRC_14_DARC()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x805;
            crc.PolynomialSize = 14;
            crc.InitialValue = 0x0;
            crc.ReverseIn = true;
            crc.ReverseOut = true;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x2D, 0x08 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x08, 0x2D }, answer);
        }
        [TestMethod]
        public void CRC_14_GSM()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x202D;
            crc.PolynomialSize = 14;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x3FFF;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xAE, 0x30 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x30, 0xAE }, answer);
        }
        [TestMethod]
        public void CRC_15_CAN()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x4599;
            crc.PolynomialSize = 15;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x9E, 0x05 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x05, 0x9E }, answer);
        }
        [TestMethod]
        public void CRC_15_MPT1327()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x6815;
            crc.PolynomialSize = 15;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x1;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x66, 0x25 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x25, 0x66 }, answer);
        }
        [TestMethod]
        public void CRC_16_ARC()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x8005;
            crc.PolynomialSize = 16;
            crc.InitialValue = 0x0;
            crc.ReverseIn = true;
            crc.ReverseOut = true;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x3D, 0xBB }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xBB, 0x3D }, answer);
        }
        [TestMethod]
        public void CRC_16_CDMA2000()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0xC867;
            crc.PolynomialSize = 16;
            crc.InitialValue = 0xFFFF;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x06, 0x4C }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x4C, 0x06 }, answer);
        }
        [TestMethod]
        public void CRC_16_CMS()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x8005;
            crc.PolynomialSize = 16;
            crc.InitialValue = 0xFFFF;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xE7, 0xAE }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xAE, 0xE7 }, answer);
        }
        [TestMethod]
        public void CRC_16_DDS_110()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x8005;
            crc.PolynomialSize = 16;
            crc.InitialValue = 0x800D;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xCF, 0x9E }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x9E, 0xCF }, answer);
        }
        [TestMethod]
        public void CRC_16_DECT_R()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x589;
            crc.PolynomialSize = 16;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x1;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x7E, 0x00 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x00, 0x7E }, answer);
        }
        [TestMethod]
        public void CRC_16_DECT_X()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x589;
            crc.PolynomialSize = 16;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x7F, 0x00 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x00, 0x7F }, answer);
        }
        [TestMethod]
        public void CRC_16_DNP()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x3D65;
            crc.PolynomialSize = 16;
            crc.InitialValue = 0x0;
            crc.ReverseIn = true;
            crc.ReverseOut = true;
            crc.XorOut = 0xFFFF;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x82, 0xEA }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xEA, 0x82 }, answer);
        }
        [TestMethod]
        public void CRC_16_EN_13757()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x3D65;
            crc.PolynomialSize = 16;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0xFFFF;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xB7, 0xC2 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xC2, 0xB7 }, answer);
        }
        [TestMethod]
        public void CRC_16_GENIBUS()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x1021;
            crc.PolynomialSize = 16;
            crc.InitialValue = 0xFFFF;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0xFFFF;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x4E, 0xD6 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xD6, 0x4E }, answer);
        }
        [TestMethod]
        public void CRC_16_GSM()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x1021;
            crc.PolynomialSize = 16;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0xFFFF;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x3C, 0xCE }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xCE, 0x3C }, answer);
        }
        [TestMethod]
        public void CRC_16_IBM_3740()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x1021;
            crc.PolynomialSize = 16;
            crc.InitialValue = 0xFFFF;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xB1, 0x29 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x29, 0xB1 }, answer);
        }
        [TestMethod]
        public void CRC_16_IBM_SDLC()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x1021;
            crc.PolynomialSize = 16;
            crc.InitialValue = 0xFFFF;
            crc.ReverseIn = true;
            crc.ReverseOut = true;
            crc.XorOut = 0xFFFF;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x6E, 0x90 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x90, 0x6E }, answer);
        }
        [TestMethod]
        public void CRC_16_ISO_IEC_14443_3_A()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x1021;
            crc.PolynomialSize = 16;
            crc.InitialValue = 0xC6C6;
            crc.ReverseIn = true;
            crc.ReverseOut = true;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x05, 0xBF }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xBF, 0x05 }, answer);
        }
        [TestMethod]
        public void CRC_16_KERMIT()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x1021;
            crc.PolynomialSize = 16;
            crc.InitialValue = 0x0;
            crc.ReverseIn = true;
            crc.ReverseOut = true;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x89, 0x21 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x21, 0x89 }, answer);
        }
        [TestMethod]
        public void CRC_16_LJ1200()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x6F63;
            crc.PolynomialSize = 16;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xF4, 0xBD }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xBD, 0xF4 }, answer);
        }
        [TestMethod]
        public void CRC_16_M17()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x5935;
            crc.PolynomialSize = 16;
            crc.InitialValue = 0xFFFF;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x2B, 0x77 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x77, 0x2B }, answer);
        }
        [TestMethod]
        public void CRC_16_MAXIM_DOW()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x8005;
            crc.PolynomialSize = 16;
            crc.InitialValue = 0x0;
            crc.ReverseIn = true;
            crc.ReverseOut = true;
            crc.XorOut = 0xFFFF;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xC2, 0x44 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x44, 0xC2 }, answer);
        }
        [TestMethod]
        public void CRC_16_MCRF4XX()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x1021;
            crc.PolynomialSize = 16;
            crc.InitialValue = 0xFFFF;
            crc.ReverseIn = true;
            crc.ReverseOut = true;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x91, 0x6F }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x6F, 0x91 }, answer);
        }
        [TestMethod]
        public void CRC_16_MODBUS()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x8005;
            crc.PolynomialSize = 16;
            crc.InitialValue = 0xFFFF;
            crc.ReverseIn = true;
            crc.ReverseOut = true;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x37, 0x4B }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x4B, 0x37 }, answer);
        }
        [TestMethod]
        public void CRC_16_NRSC_5()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x80B;
            crc.PolynomialSize = 16;
            crc.InitialValue = 0xFFFF;
            crc.ReverseIn = true;
            crc.ReverseOut = true;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x66, 0xA0 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xA0, 0x66 }, answer);
        }
        [TestMethod]
        public void CRC_16_OPENSAFETY_A()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x5935;
            crc.PolynomialSize = 16;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x38, 0x5D }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x5D, 0x38 }, answer);
        }
        [TestMethod]
        public void CRC_16_OPENSAFETY_B()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x755B;
            crc.PolynomialSize = 16;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xFE, 0x20 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x20, 0xFE }, answer);
        }
        [TestMethod]
        public void CRC_16_PROFIBUS()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x1DCF;
            crc.PolynomialSize = 16;
            crc.InitialValue = 0xFFFF;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0xFFFF;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x19, 0xA8 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xA8, 0x19 }, answer);
        }
        [TestMethod]
        public void CRC_16_RIELLO()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x1021;
            crc.PolynomialSize = 16;
            crc.InitialValue = 0xB2AA;
            crc.ReverseIn = true;
            crc.ReverseOut = true;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xD0, 0x63 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x63, 0xD0 }, answer);
        }
        [TestMethod]
        public void CRC_16_SPI_FUJITSU()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x1021;
            crc.PolynomialSize = 16;
            crc.InitialValue = 0x1D0F;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xCC, 0xE5 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xE5, 0xCC }, answer);
        }
        [TestMethod]
        public void CRC_16_T10_DIF()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x8BB7;
            crc.PolynomialSize = 16;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xDB, 0xD0 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xD0, 0xDB }, answer);
        }
        [TestMethod]
        public void CRC_16_TELEDISK()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0xA097;
            crc.PolynomialSize = 16;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xB3, 0x0F }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x0F, 0xB3 }, answer);
        }
        [TestMethod]
        public void CRC_16_TMS37157()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x1021;
            crc.PolynomialSize = 16;
            crc.InitialValue = 0x89EC;
            crc.ReverseIn = true;
            crc.ReverseOut = true;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xB1, 0x26 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x26, 0xB1 }, answer);
        }
        [TestMethod]
        public void CRC_16_UMTS()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x8005;
            crc.PolynomialSize = 16;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xE8, 0xFE }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xFE, 0xE8 }, answer);
        }
        [TestMethod]
        public void CRC_16_USB()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x8005;
            crc.PolynomialSize = 16;
            crc.InitialValue = 0xFFFF;
            crc.ReverseIn = true;
            crc.ReverseOut = true;
            crc.XorOut = 0xFFFF;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xC8, 0xB4 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xB4, 0xC8 }, answer);
        }
        [TestMethod]
        public void CRC_16_XMODEM()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x1021;
            crc.PolynomialSize = 16;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xC3, 0x31 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[2];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x31, 0xC3 }, answer);
        }
        [TestMethod]
        public void CRC_17_CAN_FD()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x1685B;
            crc.PolynomialSize = 17;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[3];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x03, 0x4F, 0x00 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[3];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x00, 0x4F, 0x03 }, answer);
        }
        [TestMethod]
        public void CRC_21_CAN_FD()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x102899;
            crc.PolynomialSize = 21;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[3];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x41, 0xD8, 0x0E }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[3];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x0E, 0xD8, 0x41 }, answer);
        }
        [TestMethod]
        public void CRC_24_BLE()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x65B;
            crc.PolynomialSize = 24;
            crc.InitialValue = 0x555555;
            crc.ReverseIn = true;
            crc.ReverseOut = true;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[3];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x56, 0x5A, 0xC2 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[3];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xC2, 0x5A, 0x56 }, answer);
        }
        [TestMethod]
        public void CRC_24_FLEXRAY_A()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x5D6DCB;
            crc.PolynomialSize = 24;
            crc.InitialValue = 0xFEDCBA;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[3];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xBD, 0x79, 0x79 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[3];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x79, 0x79, 0xBD }, answer);
        }
        [TestMethod]
        public void CRC_24_FLEXRAY_B()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x5D6DCB;
            crc.PolynomialSize = 24;
            crc.InitialValue = 0xABCDEF;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[3];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xB8, 0x23, 0x1F }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[3];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x1F, 0x23, 0xB8 }, answer);
        }
        [TestMethod]
        public void CRC_24_INTERLAKEN()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x328B63;
            crc.PolynomialSize = 24;
            crc.InitialValue = 0xFFFFFF;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0xFFFFFF;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[3];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xE6, 0xF3, 0xB4 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[3];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xB4, 0xF3, 0xE6 }, answer);
        }
        [TestMethod]
        public void CRC_24_LTE_A()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x864CFB;
            crc.PolynomialSize = 24;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[3];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x03, 0xE7, 0xCD }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[3];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xCD, 0xE7, 0x03 }, answer);
        }
        [TestMethod]
        public void CRC_24_LTE_B()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x800063;
            crc.PolynomialSize = 24;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[3];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x52, 0xEF, 0x23 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[3];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x23, 0xEF, 0x52 }, answer);
        }
        [TestMethod]
        public void CRC_24_OPENPGP()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x864CFB;
            crc.PolynomialSize = 24;
            crc.InitialValue = 0xB704CE;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[3];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x02, 0xCF, 0x21 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[3];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x21, 0xCF, 0x02 }, answer);
        }
        [TestMethod]
        public void CRC_24_OS_9()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x800063;
            crc.PolynomialSize = 24;
            crc.InitialValue = 0xFFFFFF;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0xFFFFFF;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[3];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xA5, 0x0F, 0x20 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[3];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x20, 0x0F, 0xA5 }, answer);
        }
        [TestMethod]
        public void CRC_30_CDMA()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x2030B9C7;
            crc.PolynomialSize = 30;
            crc.InitialValue = 0x3FFFFFFF;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x3FFFFFFF;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[4];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xBF, 0x4A, 0xC3, 0x04 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[4];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x04, 0xC3, 0x4A, 0xBF }, answer);
        }
        [TestMethod]
        public void CRC_31_PHILIPS()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x4C11DB7;
            crc.PolynomialSize = 31;
            crc.InitialValue = 0x7FFFFFFF;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x7FFFFFFF;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[4];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x6C, 0xE4, 0xE9, 0x0C }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[4];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x0C, 0xE9, 0xE4, 0x6C }, answer);
        }
        [TestMethod]
        public void CRC_32_AIXM()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x814141AB;
            crc.PolynomialSize = 32;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[4];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x7F, 0xBF, 0x10, 0x30 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[4];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x30, 0x10, 0xBF, 0x7F }, answer);
        }
        [TestMethod]
        public void CRC_32_AUTOSAR()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0xF4ACFB13;
            crc.PolynomialSize = 32;
            crc.InitialValue = 0xFFFFFFFF;
            crc.ReverseIn = true;
            crc.ReverseOut = true;
            crc.XorOut = 0xFFFFFFFF;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[4];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x6A, 0xD0, 0x97, 0x16 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[4];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x16, 0x97, 0xD0, 0x6A }, answer);
        }
        [TestMethod]
        public void CRC_32_BASE91_D()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0xA833982B;
            crc.PolynomialSize = 32;
            crc.InitialValue = 0xFFFFFFFF;
            crc.ReverseIn = true;
            crc.ReverseOut = true;
            crc.XorOut = 0xFFFFFFFF;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[4];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x76, 0x55, 0x31, 0x87 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[4];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x87, 0x31, 0x55, 0x76 }, answer);
        }
        [TestMethod]
        public void CRC_32_BZIP2()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x4C11DB7;
            crc.PolynomialSize = 32;
            crc.InitialValue = 0xFFFFFFFF;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0xFFFFFFFF;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[4];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x18, 0x19, 0x89, 0xFC }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[4];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xFC, 0x89, 0x19, 0x18 }, answer);
        }
        [TestMethod]
        public void CRC_32_CD_ROM_EDC()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x8001801B;
            crc.PolynomialSize = 32;
            crc.InitialValue = 0x0;
            crc.ReverseIn = true;
            crc.ReverseOut = true;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[4];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xC4, 0xED, 0xC2, 0x6E }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[4];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x6E, 0xC2, 0xED, 0xC4 }, answer);
        }
        [TestMethod]
        public void CRC_32_CKSUM()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x4C11DB7;
            crc.PolynomialSize = 32;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0xFFFFFFFF;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[4];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x80, 0x76, 0x5E, 0x76 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[4];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x76, 0x5E, 0x76, 0x80 }, answer);
        }
        [TestMethod]
        public void CRC_32_ISCSI()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x1EDC6F41;
            crc.PolynomialSize = 32;
            crc.InitialValue = 0xFFFFFFFF;
            crc.ReverseIn = true;
            crc.ReverseOut = true;
            crc.XorOut = 0xFFFFFFFF;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[4];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x83, 0x92, 0x06, 0xE3 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[4];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xE3, 0x06, 0x92, 0x83 }, answer);
        }
        [TestMethod]
        public void CRC_32_ISO_HDLC()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x4C11DB7;
            crc.PolynomialSize = 32;
            crc.InitialValue = 0xFFFFFFFF;
            crc.ReverseIn = true;
            crc.ReverseOut = true;
            crc.XorOut = 0xFFFFFFFF;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[4];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x26, 0x39, 0xF4, 0xCB }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[4];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xCB, 0xF4, 0x39, 0x26 }, answer);
        }
        [TestMethod]
        public void CRC_32_JAMCRC()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x4C11DB7;
            crc.PolynomialSize = 32;
            crc.InitialValue = 0xFFFFFFFF;
            crc.ReverseIn = true;
            crc.ReverseOut = true;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[4];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xD9, 0xC6, 0x0B, 0x34 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[4];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x34, 0x0B, 0xC6, 0xD9 }, answer);
        }
        [TestMethod]
        public void CRC_32_MEF()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x741B8CD7;
            crc.PolynomialSize = 32;
            crc.InitialValue = 0xFFFFFFFF;
            crc.ReverseIn = true;
            crc.ReverseOut = true;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[4];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x51, 0x2F, 0xC2, 0xD2 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[4];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xD2, 0xC2, 0x2F, 0x51 }, answer);
        }
        [TestMethod]
        public void CRC_32_MPEG_2()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x4C11DB7;
            crc.PolynomialSize = 32;
            crc.InitialValue = 0xFFFFFFFF;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[4];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xE7, 0xE6, 0x76, 0x03 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[4];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x03, 0x76, 0xE6, 0xE7 }, answer);
        }
        [TestMethod]
        public void CRC_32_XFER()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0xAF;
            crc.PolynomialSize = 32;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[4];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x38, 0xE3, 0x0B, 0xBD }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[4];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xBD, 0x0B, 0xE3, 0x38 }, answer);
        }
        [TestMethod]
        public void CRC_40_GSM()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x4820009;
            crc.PolynomialSize = 40;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0xFFFFFFFFFF;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[5];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x46, 0xC6, 0x4F, 0x16, 0xD4 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[5];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xD4, 0x16, 0x4F, 0xC6, 0x46 }, answer);
        }
        [TestMethod]
        public void CRC_64_ECMA_182()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x42F0E1EBA9EA3693;
            crc.PolynomialSize = 64;
            crc.InitialValue = 0x0;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[8];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x47, 0x73, 0x49, 0x0B, 0x5F, 0xDF, 0x40, 0x6C }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[8];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x6C, 0x40, 0xDF, 0x5F, 0x0B, 0x49, 0x73, 0x47 }, answer);
        }
        [TestMethod]
        public void CRC_64_GO_ISO()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x1B;
            crc.PolynomialSize = 64;
            crc.InitialValue = 0xFFFFFFFFFFFFFFFF;
            crc.ReverseIn = true;
            crc.ReverseOut = true;
            crc.XorOut = 0xFFFFFFFFFFFFFFFF;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[8];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x01, 0x10, 0xA4, 0x75, 0xC7, 0x56, 0x09, 0xB9 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[8];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xB9, 0x09, 0x56, 0xC7, 0x75, 0xA4, 0x10, 0x01 }, answer);
        }
        [TestMethod]
        public void CRC_64_MS()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x259C84CBA6426349;
            crc.PolynomialSize = 64;
            crc.InitialValue = 0xFFFFFFFFFFFFFFFF;
            crc.ReverseIn = true;
            crc.ReverseOut = true;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[8];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xEA, 0xCE, 0x4E, 0x02, 0x4F, 0xB7, 0xD4, 0x75 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[8];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x75, 0xD4, 0xB7, 0x4F, 0x02, 0x4E, 0xCE, 0xEA }, answer);
        }
        [TestMethod]
        public void CRC_64_NVME()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0xAD93D23594C93659;
            crc.PolynomialSize = 64;
            crc.InitialValue = 0xFFFFFFFFFFFFFFFF;
            crc.ReverseIn = true;
            crc.ReverseOut = true;
            crc.XorOut = 0xFFFFFFFFFFFFFFFF;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[8];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x88, 0x98, 0x79, 0x0A, 0x86, 0x14, 0x8B, 0xAE }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[8];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xAE, 0x8B, 0x14, 0x86, 0x0A, 0x79, 0x98, 0x88 }, answer);
        }
        [TestMethod]
        public void CRC_64_REDIS()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0xAD93D23594C935A9;
            crc.PolynomialSize = 64;
            crc.InitialValue = 0x0;
            crc.ReverseIn = true;
            crc.ReverseOut = true;
            crc.XorOut = 0x0;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[8];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xCA, 0xD9, 0xB8, 0xC4, 0x14, 0xD9, 0xC6, 0xE9 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[8];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xE9, 0xC6, 0xD9, 0x14, 0xC4, 0xB8, 0xD9, 0xCA }, answer);
        }
        [TestMethod]
        public void CRC_64_WE()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x42F0E1EBA9EA3693;
            crc.PolynomialSize = 64;
            crc.InitialValue = 0xFFFFFFFFFFFFFFFF;
            crc.ReverseIn = false;
            crc.ReverseOut = false;
            crc.XorOut = 0xFFFFFFFFFFFFFFFF;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[8];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x0A, 0xF0, 0xA4, 0xF1, 0xE3, 0x59, 0xEC, 0x62 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[8];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x62, 0xEC, 0x59, 0xE3, 0xF1, 0xA4, 0xF0, 0x0A }, answer);
        }
        [TestMethod]
        public void CRC_64_XZ()
        {
            CRC crc = new CRC();
            crc.Polynomial = 0x42F0E1EBA9EA3693;
            crc.PolynomialSize = 64;
            crc.InitialValue = 0xFFFFFFFFFFFFFFFF;
            crc.ReverseIn = true;
            crc.ReverseOut = true;
            crc.XorOut = 0xFFFFFFFFFFFFFFFF;

            byte[] input;
            byte[] answer;
            int answerLength;


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[8];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.LittleEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0xFA, 0x39, 0x19, 0xDF, 0xBB, 0xC9, 0x5D, 0x99 }, answer);


            input = [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39];
            answer = new byte[8];
            answerLength = crc.ComputeErrorDetectionCode(input, 0, input.Length, answer, Endianness.BigEndian);

            Assert.AreEqual(answer.Length, answerLength);
            CollectionAssert.AreEqual(new byte[] { 0x99, 0x5D, 0xC9, 0xBB, 0xDF, 0x19, 0x39, 0xFA }, answer);
        }
    }
}
