using NUnit.Framework;
using HammingSteganography;

namespace HammingSteganographyTests {
    [TestFixture]
    public class HammingMatrixTests {
        private HammingMatrix HammingMatrix { get; set; }
        
        [SetUp]
        public void Init() {
            HammingMatrix = new HammingMatrix(3);
        }

        [Test]
        public void HammingMatrix_SimpleValue_Calculated() {
            bool[,] expectedMatrix = { {true,  false, true,  false, true,  false,  true}
                                     , {false, true,  true,  false, false, true,  true}
                                     , {false, false, false, true,  true,  true, true} };

            Assert.AreEqual(3, HammingMatrix.Rows);
            Assert.AreEqual(7, HammingMatrix.Cols);
            Assert.AreEqual(expectedMatrix, HammingMatrix.Matrix);
        }

        [Test]
        public void SwitchColumn_SimpleValue_Calculated() {
            bool[,] expectedMatrix = { {true,  true, true,  false, true,  false, false}
                                     , {false, true, true,  false, false, true,  true}
                                     , {false, true, false, true,  true,  true,  false} };

            HammingMatrix.SwitchColumn(1, 6);

            Assert.AreEqual(expectedMatrix, HammingMatrix.Matrix);
        }
    }
}
