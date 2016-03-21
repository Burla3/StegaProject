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

        private int Index { get; set; }

        public Steganogrify(string msgToEncode) {
            Console.WriteLine("Msg to be encoded " + msgToEncode);
            MsgToEncodeInBits = new int[msgToEncode.Length * 8];
            string tempString;
            int index = 0;
            for (int i = 0; i < msgToEncode.Length - 8; i++) {
                tempString = Convert.ToString(msgToEncode[i], 2).PadLeft(8, '0');
                for (int j = 0; j < tempString.Length; j++) {
                    MsgToEncodeInBits[index++] = Convert.ToInt32(tempString[j].ToString(), 2);
                }                
            }
            string msgToPrint = "";
            index = 0;
            for (int k = 0; k < msgToEncode.Length - 8; k++) {
                tempString = "";
                for (int l = 0; l < 8; l++) {
                    tempString += MsgToEncodeInBits[index++].ToString();
                }
                msgToPrint += Convert.ToChar(Convert.ToInt32(tempString, 2));
            }
            Console.WriteLine(msgToPrint);

            HammingMatrix = new int[3, 7] {
                {1, 0, 0, 1, 1, 0, 1},
                {0, 1, 0, 1, 0, 1, 1},
                {0, 0, 1, 0, 1, 1, 1}
            };

            HammingMatrixCols = HammingMatrix.Length / (HammingMatrix.Rank + 1);
            HammingMatrixRows = HammingMatrix.Rank + 1;
            Index = 0;
        }

        public string decodeMsg(List<EntropyComponent> entropyComponents) {
            CurrentIndex = 0;
            string msgAsString = "";
            int[] msgInBits = new int[HammingMatrixRows];
            LSBComponent[] currentLSBs;


            while (CurrentIndex < entropyComponents.Count - HammingMatrixCols) {
                currentLSBs = getNextLSBs(entropyComponents);

                msgInBits = matrixVectorProduct(currentLSBs);

                foreach (int bit in msgInBits) {
                    msgAsString += bit.ToString();
                }                
            }

            string decodedMsg = "";
            string temp = "";
            for (int i = 1; i < msgAsString.Length + 1; i++) {
                temp += msgAsString[i - 1].ToString();
                if (i % 8 == 0 && i != 0) {
                    decodedMsg += Convert.ToChar(Convert.ToInt32(temp, 2));
                    temp = "";
                }
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
            }
            return LSBs;
        }

        private LSBComponent[] checkLSB(LSBComponent[] currentLSBs) {

            int[] matrixVectorProductResult = new int[HammingMatrixRows];
            int[] difference = new int[HammingMatrixRows];

            matrixVectorProductResult = matrixVectorProduct(currentLSBs);

            for (int i = 0; i < HammingMatrixRows; i++) {
                difference[i] = (matrixVectorProductResult[i] + MsgToEncodeInBits[Index++]) % 2;
            }

            currentLSBs = changeLSB(difference, currentLSBs);

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
