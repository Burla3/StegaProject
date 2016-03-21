using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StegaProject {
    /// <summary>
    /// This class holds a HammingMatrix.
    /// </summary>
    class HammingMatrix {
        public int[,] Matrix { get; private set; }
        public int Cols { get; }
        public int Rows { get; }

        /// <summary>
        /// Generate HammingMatrix based on the input size.
        /// </summary>
        /// <param name="size">Size of the HammingMatrix.</param>
        public HammingMatrix(int size) {
            
            generateMatrix(size);

            Cols = Matrix.Length / (Matrix.Rank + 1);
            Rows = Matrix.Rank + 1;
        }

        private void generateMatrix(int size) {
            Matrix = new int[3, 7] {
                {1, 0, 0, 1, 1, 0, 1},
                {0, 1, 0, 1, 0, 1, 1},
                {0, 0, 1, 0, 1, 1, 1}
            };
        }
    }
}
