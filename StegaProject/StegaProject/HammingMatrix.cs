
using System;

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

            Cols = (int)Math.Pow(2, size) - 1;
            Rows = size;

            generateMatrix();
        }

        private void generateMatrix() {
            Matrix = new int[Rows, Cols];
            string temp;

            for (int cols = 0; cols < Cols; cols++) {
                temp = Convert.ToString(cols + 1, 2).PadLeft(Rows, '0');
                for (int rows = 0; rows < Rows; rows++) {
                    Matrix[rows, cols] = int.Parse(temp[rows].ToString());
                }
            }
        }
    }
}
