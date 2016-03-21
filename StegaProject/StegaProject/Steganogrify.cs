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
            Console.WriteLine("Msg to be encoded " + msgToEncode);
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
            LSBComponent[] currentLSBs;


            while (CurrentIndex < entropyComponents.Count - HammingMatrixCols) {
                currentLSBs = getNextLSBs(entropyComponents);

                //Console.Write("currentLSBs ");
                //foreach (LSBComponent currentLSB in currentLSBs) {
                //    Console.Write(currentLSB.LSB + " ");
                //}
                //Console.WriteLine();

                msgInBits = matrixVectorProduct(currentLSBs);

                //Console.Write("AfterMatrix ");
                foreach (int bit in msgInBits) {
                    decodedMsg += bit.ToString();
                    //Console.Write(bit);
                }
                //Console.WriteLine();
            }

            return decodedMsg;
        }

        public void encodeMsg(List<EntropyComponent> entropyComponents) {

            CurrentIndex = 0;

            LSBComponent[] currentLSBs;

            while (CurrentIndex < entropyComponents.Count - HammingMatrixCols) {

                currentLSBs = getNextLSBs(entropyComponents);

                currentLSBs = checkLSB(currentLSBs);

                foreach (LSBComponent currentLSB in currentLSBs) {
                    entropyComponents[currentLSB.IndexInEntropyComponents].LSB = currentLSB.LSB;
                }
            }
        }

        private LSBComponent[] getNextLSBs(List<EntropyComponent> entropyComponents) {
            int temp;
            LSBComponent[] LSBs = new LSBComponent[HammingMatrixCols];

            for (int i = 0; i < HammingMatrixCols; i++) {
                temp = -1;

                while (temp == -1) {
                    temp = entropyComponents[CurrentIndex].IsDC ? -1 : entropyComponents[CurrentIndex].LSB;
                    CurrentIndex++;
                }
                LSBs[i] = new LSBComponent(temp, CurrentIndex - 1);
                //Console.WriteLine($"LSB {LSBs[i].LSB} with {LSBs[i].IndexInEntropyComponents} passed ");

            }
            return LSBs;
        }

        private LSBComponent[] checkLSB(LSBComponent[] currentLSBs) {

            //Console.WriteLine($"LSBs before change");
            //foreach (LSBComponent currentLSB in currentLSBs) {
            //    Console.Write(currentLSB.LSB + " ");
            //}
            //Console.WriteLine();

            int[] matrixVectorProductResult = new int[HammingMatrixRows];
            int[] difference = new int[HammingMatrixRows];

            matrixVectorProductResult = matrixVectorProduct(currentLSBs);

            //Console.WriteLine("Result after H3 " + matrixVectorProductResult[0] + " " + matrixVectorProductResult[1] + " " + matrixVectorProductResult[2]);
            //Console.WriteLine("MsgToIncode " + MsgToEncodeInBits[0] + " " + MsgToEncodeInBits[1] + " " + MsgToEncodeInBits[2]);
            

            for (int i = 0; i < HammingMatrixRows; i++) {
                difference[i] = (matrixVectorProductResult[i] + MsgToEncodeInBits[i]) % 2;
            }

            //Console.WriteLine("Difference " + difference[0] + " " + difference[1] + " " + difference[2]);

            currentLSBs = changeLSB(difference, currentLSBs);

            //Console.WriteLine($"LSBs after change");
            //foreach (LSBComponent currentLSB in currentLSBs) {
            //    Console.Write(currentLSB.LSB + " ");
            //}
            //Console.WriteLine();
            //Console.WriteLine();

            return currentLSBs;
        }

        private int[] matrixVectorProduct(LSBComponent[] currentLSBs) {
            int temp;
            int[] result = new int[HammingMatrixRows];

            for (int row = 0; row < HammingMatrixRows; row++) {
                temp = 0;
                for (int col = 0; col < HammingMatrixCols; col++) {
                    temp += HammingMatrix[row, col] * currentLSBs[col].LSB;
                }
                result[row] = temp % 2;
            }
            return result;
        }

        private LSBComponent[] changeLSB(int[] difference, LSBComponent[] currentLSBs) {
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

                currentLSBs[bitToChange].LSB = currentLSBs[bitToChange].LSB == 0 ? 1 : 0;
            }
            return currentLSBs;
        }
    }
}
