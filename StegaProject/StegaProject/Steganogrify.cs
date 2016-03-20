using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StegaProject {
    class Steganogrify {

        private int[,] HammingMatrix { get; set; }

        private int[] MsgToEncodeInBits { get; set; }
    
        private int HammingMatrixCols { get; set; }
    
        private int HammingMatrixRows { get; set; }

        private int CurrentIndex { get; set; }

        public Steganogrify(string msgToEncode) {
            MsgToEncodeInBits = new int[msgToEncode.Length];
            for (int i = 0; i < msgToEncode.Length; i++) {
                MsgToEncodeInBits[i] = Convert.ToInt32(msgToEncode[i].ToString(), 2);
            }

            HammingMatrix = new int[3, 7] {
                {1, 0, 0, 1, 1, 0, 1},
                {0, 1, 0, 1, 0, 1, 1},
                {0, 0, 1, 0, 1, 1, 1}
            };

            HammingMatrixCols = HammingMatrix.Length / (HammingMatrix.Rank + 1);
            HammingMatrixRows = HammingMatrix.Rank + 1;
        }

        public string decodeMsg(List<EntropyComponent> entropyComponents) {
            CurrentIndex = 0;
            string decodedMsg = "";
            int[] msgInBits = new int[HammingMatrixRows];
            int[] currentLSBs = new int[HammingMatrixCols];


            while (CurrentIndex < entropyComponents.Count - HammingMatrixCols) {
                currentLSBs = getNextLSBs(entropyComponents);

                Console.Write("currentLSBs ");
                foreach (int currentLSB in currentLSBs) {
                    Console.Write(currentLSB + " ");
                }
                Console.WriteLine();

                msgInBits = matrixVectorProduct(currentLSBs);

                Console.Write("AfterMatrix ");
                foreach (int bit in msgInBits) {
                    decodedMsg += bit.ToString();
                    Console.Write(bit);
                }
                Console.WriteLine();
            }

            return decodedMsg;
        }

        public void encodeMsg(List<EntropyComponent> entropyComponents) {

            CurrentIndex = 0;

            int[] currentLSBs = new int[HammingMatrixCols];

            while (CurrentIndex < entropyComponents.Count - HammingMatrixCols) {

                currentLSBs = getNextLSBs(entropyComponents);

                currentLSBs = checkLSB(currentLSBs);

                for (int overrideIndex = CurrentIndex - HammingMatrixCols, j = 0; overrideIndex < CurrentIndex; overrideIndex++, j++) {
                    entropyComponents[overrideIndex].LSB = currentLSBs[j];
                }
            }
        }

        private int[] getNextLSBs(List<EntropyComponent> entropyComponents) {
            int temp;
            int[] LSBs = new int[HammingMatrixCols];

            for (int i = 0; i < HammingMatrixCols; i++) {
                temp = -1;

                while (temp == -1) {
                    temp = entropyComponents[CurrentIndex].LSB;
                    CurrentIndex++;
                }
                LSBs[i] = temp;
            }
            return LSBs;
        }

        private int[] checkLSB(int[] currentLSBs) {

            Console.WriteLine($"LSBs before change");
            foreach (int currentLSB in currentLSBs) {
                Console.Write(currentLSB + " ");
            }
            Console.WriteLine();

            int[] matrixVectorProductResult = new int[HammingMatrixRows];
            int[] difference = new int[HammingMatrixRows];

            matrixVectorProductResult = matrixVectorProduct(currentLSBs);

            //Console.WriteLine("Result after H3 " + result[0] + " " + result[1] + " " + result[2]);
            //Console.WriteLine("MsgToIncode " + MsgToEncodeInBits[0] + " " + MsgToEncodeInBits[1] + " " + MsgToEncodeInBits[2]);
            

            for (int i = 0; i < HammingMatrixRows; i++) {
                difference[i] = (matrixVectorProductResult[i] + MsgToEncodeInBits[i]) % 2;
            }

            Console.WriteLine("Difference " + difference[0] + " " + difference[1] + " " + difference[2]);

            currentLSBs = changeLSB(difference, currentLSBs);

            Console.WriteLine($"LSBs after change");
            foreach (int currentLSB in currentLSBs) {
                Console.Write(currentLSB + " ");
            }
            Console.WriteLine();

            return currentLSBs;
        }

        private int[] matrixVectorProduct(int[] currentLSBs) {
            int temp = 0;
            int[] result = new int[HammingMatrixRows];
            for (int row = 0; row < HammingMatrixRows; row++) {
                for (int col = 0; col < HammingMatrixCols; col++) {
                    temp = HammingMatrix[row, col] * currentLSBs[row];
                }
                result[row] = temp % 2;
            }
            return result;
        }

        private int[] changeLSB(int[] difference, int[] currentLSBs) {
            bool haveToChangeLSB = false;
            int index = 0;

            while (index < HammingMatrixRows && !haveToChangeLSB) {
                haveToChangeLSB = difference[index] != 0;
                index++;
            }

            if (haveToChangeLSB) {

                bool realCol = false;
                int bitToChange = -1;

                for (int col = 0; col < HammingMatrixCols; col++) {
                    for (int row = 0; row < HammingMatrixRows; row++) {
                        realCol = HammingMatrix[row, col] == difference[row];
                        if (!realCol) {
                            break;
                        }
                    }
                    if (realCol) {
                        bitToChange = col;
                        break;
                    }
                }

                currentLSBs[bitToChange] = currentLSBs[bitToChange] == 0 ? 1 : 0;
            }
            return currentLSBs;
        }
    }
}
