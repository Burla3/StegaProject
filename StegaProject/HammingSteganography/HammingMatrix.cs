using System;
using System.Diagnostics.Contracts;

namespace HammingSteganography {
    /// <summary>
    /// This class holds a binary Hamming matrix used for steganography.
    /// </summary>
    public class HammingMatrix {

        /// <summary>
        /// This is the matrix.
        /// </summary>
        public bool[,] Matrix { get; private set; }
        /// <summary>
        /// Number of columns in the matrix.
        /// </summary>
        public int Cols { get; }
        /// <summary>
        /// Number of rows in the matrix.
        /// </summary>
        public int Rows { get; }

        /// <summary>
        /// Generate binary Hamming matrix based on the input <paramref name="rows"/>.
        /// </summary>
        /// <param name="rows">Number of rows of the HammingMatrix.</param>
        public HammingMatrix(int rows) {
            Contract.Requires<ArgumentOutOfRangeException>(rows > 0 && rows < 32);

            Cols = (int) Math.Pow(2, rows) - 1;
            Rows = rows;

            GenerateMatrix();
        }

        /// <summary>
        /// Generate binary Hamming matrix with <see cref="Rows"/>.
        /// </summary>
        private void GenerateMatrix() {
            Matrix = new bool[Rows, Cols];

            for (int cols = 0; cols < Cols; cols++) {
                string temp = Convert.ToString(cols + 1, 2).PadLeft(Rows, '0');
                for (int rows = 0; rows < Rows; rows++) {
                    Matrix[rows, cols] = temp[rows] == '1';
                }
            }
        }

        /// <summary>
        /// Switch two columns in the Hamming matrix.
        /// </summary>
        /// <param name="column1">Column 1.</param>
        /// <param name="column2">Column 2.</param>
        public void SwitchColumn(int column1, int column2) {
            Contract.Requires<ArgumentOutOfRangeException>(column1 >= 0 && column1 < Cols);
            Contract.Requires<ArgumentOutOfRangeException>(column2 >= 0 && column2 < Cols);

            for (int i = 0; i < Rows; i++) {
                bool temp = Matrix[i, column1];
                Matrix[i, column1] = Matrix[i, column2];
                Matrix[i, column2] = temp;
            }
        }
    }
}
