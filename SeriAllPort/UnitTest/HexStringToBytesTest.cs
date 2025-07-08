using CommonWpf.Extensions;

namespace UnitTest
{
    [TestClass]
    public sealed class HexStringToBytesTest
    {
        [TestMethod]
        public void EmptyString()
        {
            byte[]? result = "".HexStringToBytes();
            byte[] expexted = [];
            Assert.IsNotNull(result);
            Assert.AreEqual(expexted.Length, result?.Length);
            if (result != null)
            {
                Assert.IsTrue(expexted.SequenceEqual(result));
            }
        }

        [TestMethod]
        public void NullString()
        {
            string? s = null;
#pragma warning disable CS8604 // Possible null reference argument.
            byte[]? result = s.HexStringToBytes();
#pragma warning restore CS8604 // Possible null reference argument.
            byte[] expexted = [];
            Assert.IsNotNull(result);
            Assert.AreEqual(expexted.Length, result?.Length);
            if (result != null)
            {
                Assert.IsTrue(expexted.SequenceEqual(result));
            }
        }

        [TestMethod]
        public void SpaceString()
        {
            byte[]? result = "  ".HexStringToBytes();
            byte[] expexted = [];
            Assert.IsNotNull(result);
            Assert.AreEqual(expexted.Length, result?.Length);
            if (result != null)
            {
                Assert.IsTrue(expexted.SequenceEqual(result));
            }
        }

        [TestMethod]
        public void Normal()
        {
            byte[]? result = "AB CD EF 12 34 56 78 09".HexStringToBytes();
            byte[] expexted = [0xAB, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78, 0x09];
            Assert.IsNotNull(result);
            Assert.AreEqual(expexted.Length, result?.Length);
            if (result != null)
            {
                Assert.IsTrue(expexted.SequenceEqual(result));
            }
        }

        [TestMethod]
        public void Odd()
        {
            byte[]? result = "ABC".HexStringToBytes();
            byte[] expexted = [0xAB, 0xC0];
            Assert.IsNotNull(result);
            Assert.AreEqual(expexted.Length, result?.Length);
            if (result != null)
            {
                Assert.IsTrue(expexted.SequenceEqual(result));
            }
        }

        [TestMethod]
        public void LowerCase()
        {
            byte[]? result = "ab cd ef 12 34 56 78 09".HexStringToBytes();
            byte[] expexted = [0xAB, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78, 0x09];
            Assert.IsNotNull(result);
            Assert.AreEqual(expexted.Length, result?.Length);
            if (result != null)
            {
                Assert.IsTrue(expexted.SequenceEqual(result));
            }
        }

        [TestMethod]
        public void MoreSpaces()
        {
            byte[]? result = "  AB\t   C  1  ".HexStringToBytes();
            byte[] expexted = [0xAB, 0xC1];
            Assert.IsNotNull(result);
            Assert.AreEqual(expexted.Length, result?.Length);
            if (result != null)
            {
                Assert.IsTrue(expexted.SequenceEqual(result));
            }
        }

        [TestMethod]
        public void MoreSpace1()
        {
            byte[]? result = "AB\t   C  1  ".HexStringToBytes();
            byte[] expexted = [0xAB, 0xC1];
            Assert.IsNotNull(result);
            Assert.AreEqual(expexted.Length, result?.Length);
            if (result != null)
            {
                Assert.IsTrue(expexted.SequenceEqual(result));
            }
        }

        [TestMethod]
        public void MoreSpaces2()
        {
            byte[]? result = "  AB\t   C  1".HexStringToBytes();
            byte[] expexted = [0xAB, 0xC1];
            Assert.IsNotNull(result);
            Assert.AreEqual(expexted.Length, result?.Length);
            if (result != null)
            {
                Assert.IsTrue(expexted.SequenceEqual(result));
            }
        }

        [TestMethod]
        public void InvalidChar()
        {
            _ = Assert.ThrowsExactly<FormatException>(" w ".HexStringToBytes);
        }

        [TestMethod]
        public void Char1()
        {
            byte[]? result = "  1".HexStringToBytes();
            byte[] expexted = [0x10];
            Assert.IsNotNull(result);
            Assert.AreEqual(expexted.Length, result?.Length);
            if (result != null)
            {
                Assert.IsTrue(expexted.SequenceEqual(result));
            }
        }

        [TestMethod]
        public void Char1_1()
        {
            byte[]? result = "1  ".HexStringToBytes();
            byte[] expexted = [0x10];
            Assert.IsNotNull(result);
            Assert.AreEqual(expexted.Length, result?.Length);
            if (result != null)
            {
                Assert.IsTrue(expexted.SequenceEqual(result));
            }
        }

        [TestMethod]
        public void Char1_2()
        {
            byte[]? result = "   1  ".HexStringToBytes();
            byte[] expexted = [0x10];
            Assert.IsNotNull(result);
            Assert.AreEqual(expexted.Length, result?.Length);
            if (result != null)
            {
                Assert.IsTrue(expexted.SequenceEqual(result));
            }
        }

        [TestMethod]
        public void Char2()
        {
            byte[]? result = "   1    f ".HexStringToBytes();
            byte[] expexted = [0x1F];
            Assert.IsNotNull(result);
            Assert.AreEqual(expexted.Length, result?.Length);
            if (result != null)
            {
                Assert.IsTrue(expexted.SequenceEqual(result));
            }
        }

        [TestMethod]
        public void Char2_1()
        {
            byte[]? result = "1    f ".HexStringToBytes();
            byte[] expexted = [0x1F];
            Assert.IsNotNull(result);
            Assert.AreEqual(expexted.Length, result?.Length);
            if (result != null)
            {
                Assert.IsTrue(expexted.SequenceEqual(result));
            }
        }

        [TestMethod]
        public void Char2_2()
        {
            byte[]? result = "   1    f".HexStringToBytes();
            byte[] expexted = [0x1F];
            Assert.IsNotNull(result);
            Assert.AreEqual(expexted.Length, result?.Length);
            if (result != null)
            {
                Assert.IsTrue(expexted.SequenceEqual(result));
            }
        }

        [TestMethod]
        public void Char2_3()
        {
            byte[]? result = "   1f ".HexStringToBytes();
            byte[] expexted = [0x1F];
            Assert.IsNotNull(result);
            Assert.AreEqual(expexted.Length, result?.Length);
            if (result != null)
            {
                Assert.IsTrue(expexted.SequenceEqual(result));
            }
        }

        [TestMethod]
        public void Char3()
        {
            byte[]? result = "   1  0  f   ".HexStringToBytes();
            byte[] expexted = [0x10, 0xF0];
            Assert.IsNotNull(result);
            Assert.AreEqual(expexted.Length, result?.Length);
            if (result != null)
            {
                Assert.IsTrue(expexted.SequenceEqual(result));
            }
        }

        [TestMethod]
        public void Random()
        {
            byte[]? result = " 03 45 3c 85 c2 7e 45 ea 11 35 6f 83 d6 56 5f fc a3 34 48 69 ".HexStringToBytes();
            byte[] expexted = [0x03, 0x45, 0x3c, 0x85, 0xc2, 0x7e, 0x45, 0xea, 0x11, 0x35, 0x6f, 0x83, 0xd6, 0x56, 0x5f, 0xfc, 0xa3, 0x34, 0x48, 0x69];
            Assert.IsNotNull(result);
            Assert.AreEqual(expexted.Length, result?.Length);
            if (result != null)
            {
                Assert.IsTrue(expexted.SequenceEqual(result));
            }
        }
    }
}
