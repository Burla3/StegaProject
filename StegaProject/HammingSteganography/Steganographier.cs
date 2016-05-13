using System;
using System.Collections;
using System.Diagnostics.Contracts;
using System.Text;
using Utilities;

namespace HammingSteganography {

    /// <summary>
    /// This class holds the implementation for a Hamming steganography method.
    /// It implements the ISteganographier interface.
    /// </summary>
    public class Steganographier : ISteganographier {
        private HammingMatrix HammingMatrix { get; }

        private const int BitsPerAscii = 8;

        /// <summary>
        /// This calls <see cref="Steganographier(HammingSteganography.HammingMatrix)"/> with a new 
        /// <see cref="HammingSteganography.HammingMatrix"/> made from the entered number of <paramref name="hammingRows"/>.
        /// </summary>
        /// <param name="hammingRows">Number of rows the generated Hamming matrix should have.</param>
        public Steganographier(int hammingRows) : this(new HammingMatrix(hammingRows))
        { }

        /// <summary>
        /// Sets the Hamming matrix used in this method to steganography.
        /// </summary>
        /// <param name="hammingMatrix">A custom <see cref="HammingSteganography.HammingMatrix"/> 
        /// which could have switched columns</param>
        public Steganographier(HammingMatrix hammingMatrix) {
            Contract.Requires<ArgumentNullException>(hammingMatrix != null);

            HammingMatrix = hammingMatrix;
        }

        /// <summary>
        /// Decodes an ASCII message from <paramref name="coverData"/>.
        /// This use the <see cref="DecodeBinaryMessage"/> to get binary data
        /// and then converts it to a <see cref="T:byte[]"/> and gets a <see cref="string"/> with <see cref="Encoding"/>.
        /// </summary>
        /// <param name="coverData">The steganographied cover data.</param>
        /// <returns>The decoded data as a string.</returns>
        public string DecodeAsciiMessage(BitArray coverData) {
            Contract.Requires<ArgumentNullException>(coverData != null);
            Contract.Requires<ArgumentException>((coverData.Count / HammingMatrix.Cols) * HammingMatrix.Rows > BitsPerAscii);

            BitArray binaryMessage = DecodeBinaryMessage(coverData);
            
            BitArrayUtilities.MakeBitArrayDivisible(binaryMessage, BitsPerAscii, false);

            int neededBytes = binaryMessage.Count / BitsPerAscii;
            byte[] messageBytes = new byte[neededBytes];

            binaryMessage = BitArrayUtilities.ChangeEndianOnBitArray(binaryMessage);
            
            binaryMessage.CopyTo(messageBytes, 0);

            return Encoding.ASCII.GetString(messageBytes);
        }

        /// <summary>
        /// Decodes a binary message from <paramref name="coverData"/>.
        /// This use the <see cref="HammingMatrix"/> to calculate the message using matrix vector product.
        /// </summary>
        /// <param name="coverData"></param>
        /// <returns>The decoded data as a <see cref="BitArray"/>.</returns>
        public BitArray DecodeBinaryMessage(BitArray coverData) {
            Contract.Requires<ArgumentNullException>(coverData != null);
            Contract.Requires<ArgumentException>(coverData.Count >= HammingMatrix.Cols);

            BitArray messageArray = new BitArray(0);

            coverData = BitArrayUtilities.ReverseBitArray(coverData);

            int maxNumberOfVectors = coverData.Count / HammingMatrix.Cols;

            // Calculates each part of the message
            for (int i = 0; i < maxNumberOfVectors; i++) {
                BitArray coverVector = BitArrayUtilities.TakeSubBitArrayFromEnd(coverData, HammingMatrix.Cols);
                BitArray resultVector = BinaryMatrixVectorProduct(HammingMatrix, coverVector);

                messageArray = BitArrayUtilities.CombineTwoBitArrays(messageArray, resultVector);
            }

            return messageArray;
        }

        /// <summary>
        /// Encodes the ASCII <paramref name="message"/> into the <paramref name="coverData"/>.
        /// This use <see cref="Encoding"/> to convert to binary and then use <see cref="EncodeBinaryMessage"/> to encode.
        /// </summary>
        /// <param name="coverData">Data where <paramref name="message"/> should be encoded.</param>
        /// <param name="message">The ASCII message that should be encoded.</param>
        /// <returns>The encoded <paramref name="coverData"/></returns>
        public BitArray EncodeAsciiMessage(BitArray coverData, string message) {
            Contract.Requires<ArgumentNullException>(coverData != null);
            Contract.Requires<ArgumentNullException>(message != null);
            
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            BitArray messageBitArray = new BitArray(messageBytes);
            messageBitArray = BitArrayUtilities.ChangeEndianOnBitArray(messageBitArray);

            BitArrayUtilities.MakeBitArrayDivisible(messageBitArray, HammingMatrix.Rows, true);

            return EncodeBinaryMessage(coverData, messageBitArray);
        }

        /// <summary>
        /// Encodes the binary <paramref name="message"/> into the <paramref name="coverData"/>.
        /// This use the <see cref="HammingMatrix"/> to calculate the needed 
        /// <paramref name="coverData"/> using matrix vector product.
        /// Changes the <paramref name="coverData"/> if there is a mismatch (most likely).
        /// </summary>
        /// <param name="coverData">Data where <paramref name="message"/> should be encoded.</param>
        /// <param name="message">The binary message that should be encoded.</param>
        /// <returns>The encoded <paramref name="coverData"/></returns>
        public BitArray EncodeBinaryMessage(BitArray coverData, BitArray message) {
            Contract.Requires<ArgumentNullException>(coverData != null);
            Contract.Requires<ArgumentNullException>(message != null);
            Contract.Requires<ArgumentOutOfRangeException>(
                coverData.Count / HammingMatrix.Cols >= message.Count / HammingMatrix.Rows);
            Contract.Requires<ArgumentException>(message.Count % HammingMatrix.Rows == 0);

            BitArray resultArray = new BitArray(0);

            coverData = BitArrayUtilities.ReverseBitArray(coverData);
            message = BitArrayUtilities.ReverseBitArray(message);

            int maxNumberOfVectors = message.Count / HammingMatrix.Rows;

            // Calculates each part of the message
            for (int i = 0; i < maxNumberOfVectors; i++) {
                BitArray coverVector = BitArrayUtilities.TakeSubBitArrayFromEnd(coverData, HammingMatrix.Cols);
                BitArray messageVector = BitArrayUtilities.TakeSubBitArrayFromEnd(message, HammingMatrix.Rows);

                BitArray resultVector = BinaryMatrixVectorProduct(HammingMatrix, coverVector);
                BitArray differenceVector = resultVector.Xor(messageVector);
                // Change coverVector if differenceVector is not equal to a zero vector.
                if (!BitArrayUtilities.CompareBitArray(differenceVector, new BitArray(HammingMatrix.Rows))) {
                    coverVector = ChangeLsb(differenceVector, coverVector);
                }

                resultArray = BitArrayUtilities.CombineTwoBitArrays(resultArray, coverVector);
            }

            // Add the rest of the coverData, which did not go through the message process to the resultArray.
            BitArrayUtilities.ReverseBitArray(coverData);
            BitArrayUtilities.CombineTwoBitArrays(resultArray, coverData);

            return resultArray;
        }

        /// <summary>
        /// Finds the column number in the <see cref="HammingMatrix"/> that is equal to <paramref name="differenceVector"/>
        /// and change the bit at that number row in <paramref name="coverVector"/>.
        /// </summary>
        /// <param name="differenceVector">The vector to find in the <see cref="HammingMatrix"/>.</param>
        /// <param name="coverVector">The vector to change a bit in.</param>
        /// <returns>The changed <paramref name="coverVector"/>.</returns>
        private BitArray ChangeLsb(BitArray differenceVector, BitArray coverVector) {

            int bitToChange = 0;

            for (int col = 0; col < HammingMatrix.Cols; col++) {
                bool realCol = true;
                // Checks if whole column is equal to differenceVector.
                for (int row = 0; row < HammingMatrix.Rows && realCol; row++) {
                    realCol = HammingMatrix.Matrix[row, col] == differenceVector[row];
                }
                if (realCol) {
                    bitToChange = col;
                    break;
                }
            }

            coverVector[bitToChange] = !coverVector[bitToChange];

            return coverVector;
        }

        /// <summary>
        /// Calculates the binary matrix vector product of <paramref name="matrix"/> and <paramref name="vector"/>.
        /// </summary>
        /// <param name="matrix">Binary matrix.</param>
        /// <param name="vector">Binary vector.</param>
        /// <returns>A binary vector.</returns>
        private static BitArray BinaryMatrixVectorProduct(HammingMatrix matrix, BitArray vector) {

            BitArray result = new BitArray(matrix.Rows);

            for (int row = 0; row < matrix.Rows; row++) {
                bool rowResult = false;
                for (int col = 0; col < matrix.Cols; col++) {
                    // (Earlier result + matrix[row, col] * vector[row]) % 2
                    rowResult = rowResult ^ (matrix.Matrix[row, col] & vector[col]);
                }
                result[row] = rowResult;
            }

            return result;
        }
    }
}
