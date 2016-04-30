using System.Collections;
using NUnit.Framework;
using HammingSteganography;

namespace HammingSteganographyTests {

    [TestFixture]
    public class SteganographierTests {
        private Steganographier Steganographier { get; set; }

        [SetUp]
        public void Init() {
            Steganographier = new Steganographier(3);
        }

        [Test]
        public void DecodeAsciiMessage_DoubleSimpleValue_Calculated() {
            BitArray coverData = new BitArray(new[] {false, true, true, false, true, true, false,
                                                     true, false, false, false, true, false, false,
                                                     true, true, true, false, true, false, true,
                                                     true, false, true, true, true, false, false,
                                                     false, false, false, false, true, true, true,
                                                     false, false, false, true, true, true, true});

            string resultMessage = Steganographier.DecodeAsciiMessage(coverData);

            const string expectedMessage = "Q8";

            Assert.AreEqual(expectedMessage, resultMessage);
        }

        [Test]
        public void DecodeAsciiMessage_SingleSimpleValue_Calculated() {
            BitArray coverData = new BitArray(new[] {false, true, true, false, true, true, false,
                                                     true, false, false, false, true, false, false,
                                                     true, true, true, false, true, false, true});

            string resultMessage = Steganographier.DecodeAsciiMessage(coverData);

            const string expectedMessage = "Q";

            Assert.AreEqual(expectedMessage, resultMessage);
        }

        [Test]
        public void DecodeBinaryMessage_SimpleEvenValue_Calculated() {
            BitArray coverData = new BitArray(new[] {true, false, true, false, true, true, false,
                                                     true, false, false, false, true, true, false});

            BitArray resultArray = Steganographier.DecodeBinaryMessage(coverData);

            BitArray expectedMessage = new BitArray(new[] { false, false, true, false, true, false });

            Assert.AreEqual(expectedMessage, resultArray);
        }

        [Test]
        public void DecodeBinaryMessage_SimpleOddValue_Calculated() {
            BitArray coverData = new BitArray(new[] {false, true, true, false, true, true, false,
                                                     true, false, false, false, true, false, false,
                                                     true, true, true, false, true, false, true});

            BitArray resultArray = Steganographier.DecodeBinaryMessage(coverData);

            BitArray expectedMessage = new BitArray(new[] { false, true, false, true, false, false, false, true, false });

            Assert.AreEqual(expectedMessage, resultArray);
        }

        [Test]
        public void DecodeBinaryMessage_NotDivisibelCoverData_Calculated() {
            BitArray coverData = new BitArray(new[] {false, true, true, false, true, true, false,
                                                     true, false, false, false, true, false, false,
                                                     true, true, true, false, true, false, true,
                                                     false, true, false, false});

            BitArray resultArray = Steganographier.DecodeBinaryMessage(coverData);

            BitArray expectedMessage = new BitArray(new[] { false, true, false, true, false, false, false, true, false });

            Assert.AreEqual(expectedMessage, resultArray);
        }

        [Test]
        public void EncodeBinaryData_SimpleEvenValue_Calculated() {
            BitArray coverData = new BitArray(new[] {false, false, true, false, true, true, false,
                                                     true, false, false, false, true, true, false});
            BitArray message = new BitArray(new[] { false, false, true, false, true, false});

            BitArray resultArray = Steganographier.EncodeBinaryMessage(coverData, message);

            BitArray expectedArray = new BitArray(new[] {true, false, true, false, true, true, false,
                                                         true, false, false, false, true, true, false});

            Assert.AreEqual(expectedArray, resultArray);
        }

        [Test]
        public void EncodeBinaryData_SimpleOddValue_Calculated() {
            BitArray coverData = new BitArray(new[] {false, false, true, false, true, true, false,
                                                     true, false, false, false, true, true, false,
                                                     false, true, true, false, true, false, true});
            BitArray message = new BitArray(new[] { false, true, false, true, false, false, false, true, false });

            BitArray resultArray = Steganographier.EncodeBinaryMessage(coverData, message);

            BitArray expectedArray = new BitArray(new[] {false, true, true, false, true, true, false,
                                                         true, false, false, false, true, false, false,
                                                         true, true, true, false, true, false, true});

            Assert.AreEqual(expectedArray, resultArray);
        }

        [Test]
        public void EncodeBinaryData_NotDivisibelCoverData_Calculated() {
            BitArray coverData = new BitArray(new[] {false, false, true, false, true, true, false,
                                                     true, false, false, false, true, true, false,
                                                     false, true, true, false, true, false, true,
                                                     false, true, false, false});
            BitArray message = new BitArray(new[] { false, true, false, true, false, false, false, true, false });

            BitArray resultArray = Steganographier.EncodeBinaryMessage(coverData, message);

            BitArray expectedArray = new BitArray(new[] {false, true, true, false, true, true, false,
                                                         true, false, false, false, true, false, false,
                                                         true, true, true, false, true, false, true,
                                                         false, true, false, false});

            Assert.AreEqual(expectedArray, resultArray);
        }

        [Test]
        public void EncodeAsciiData_SingleSimpleValue_Calculated() {
            BitArray coverData = new BitArray(new[] {false, false, true, false, true, true, false,
                                                     true, false, false, false, true, true, false,
                                                     false, true, true, false, true, false, true});
            const string message = "Q"; // Binary message 01010001[0]

            BitArray resultArray = Steganographier.EncodeAsciiMessage(coverData, message);

            BitArray expectedArray = new BitArray(new[] {false, true, true, false, true, true, false,
                                                         true, false, false, false, true, false, false,
                                                         true, true, true, false, true, false, true});

            Assert.AreEqual(expectedArray, resultArray);
        }

        [Test]
        public void EncodeAsciiData_DoubleSimpleValue_Calculated() {
            BitArray coverData = new BitArray(new[] {false, false, true, false, true, true, false,
                                                     true, false, false, false, true, true, false,
                                                     false, true, true, false, true, false, true,
                                                     true, false, true, true, true, false, true,
                                                     false, true, false, false, true, true, true,
                                                     false, false, false, false, true, true, true});
            const string message = "Q8"; // Binary message 01010001,00111000[00]

            BitArray resultArray = Steganographier.EncodeAsciiMessage(coverData, message);

            BitArray expectedArray = new BitArray(new[] {false, true, true, false, true, true, false,
                                                         true, false, false, false, true, false, false,
                                                         true, true, true, false, true, false, true,
                                                         true, false, true, true, true, false, false,
                                                         false, false, false, false, true, true, true,
                                                         false, false, false, true, true, true, true});

            Assert.AreEqual(expectedArray, resultArray);
        }
    }
}
