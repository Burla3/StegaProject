using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StegaProject {
    class Steganogrify {

        private HammingMatrix HammingMatrix { get; }

        private int[] MsgToEncodeInBits { get; }

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
                    
            HammingMatrix = new HammingMatrix(3);

            Index = 0;
        }

        public string decodeMsg(List<EntropyComponent> entropyComponents) {
            CurrentIndex = 0;
            string msgAsString = "";
            int[] msgInBits = new int[HammingMatrix.Rows];
            LSBComponent[] currentLSBs;

            while (CurrentIndex < entropyComponents.Count - HammingMatrix.Cols) {
                currentLSBs = getNextLSBs(entropyComponents);

                msgInBits = matrixVectorProduct(currentLSBs);

                foreach (int bit in msgInBits) {
                    msgAsString += bit.ToString();
                }                
            }

            string decodedMsg = "";
            string bitsToConvertToChar = "";
            for (int i = 1; i < msgAsString.Length + 1; i++) {
                bitsToConvertToChar += msgAsString[i - 1].ToString();
                if (i % 8 == 0 && i != 0) {
                    decodedMsg += Convert.ToChar(Convert.ToInt32(bitsToConvertToChar, 2));
                    bitsToConvertToChar = "";
                }
            }           
            return decodedMsg;
        }

        public void encodeMsg(List<EntropyComponent> entropyComponents) {
            LSBComponent[] currentLSBs;
            CurrentIndex = 0;

            while (CurrentIndex < entropyComponents.Count - HammingMatrix.Cols) {

                currentLSBs = getNextLSBs(entropyComponents);

                currentLSBs = checkLSB(currentLSBs);

                foreach (LSBComponent currentLSB in currentLSBs) {
                    entropyComponents[currentLSB.IndexInEntropyComponents].LSB = currentLSB.LSB;
                }
            }
        }

        private LSBComponent[] getNextLSBs(List<EntropyComponent> entropyComponents) {
            LSBComponent[] LSBs = new LSBComponent[HammingMatrix.Cols];
            int LSB;

            for (int i = 0; i < HammingMatrix.Cols; i++) {
                LSB = -1;

                while (LSB == -1) {
                    LSB = entropyComponents[CurrentIndex].IsDC ? -1 : entropyComponents[CurrentIndex].LSB;
                    CurrentIndex++;
                }
                LSBs[i] = new LSBComponent(LSB, CurrentIndex - 1);
            }
            return LSBs;
        }

        private LSBComponent[] checkLSB(LSBComponent[] currentLSBs) {
            int[] matrixVectorProductResult = new int[HammingMatrix.Rows];
            int[] difference = new int[HammingMatrix.Rows];

            matrixVectorProductResult = matrixVectorProduct(currentLSBs);

            for (int i = 0; i < HammingMatrix.Rows; i++) {
                difference[i] = (matrixVectorProductResult[i] + MsgToEncodeInBits[Index++]) % 2;
            }

            currentLSBs = changeLSB(difference, currentLSBs);

            return currentLSBs;
        }

        private int[] matrixVectorProduct(LSBComponent[] currentLSBs) {
            int rowResult;
            int[] result = new int[HammingMatrix.Rows];

            for (int row = 0; row < HammingMatrix.Rows; row++) {
                rowResult = 0;
                for (int col = 0; col < HammingMatrix.Cols; col++) {
                    rowResult += HammingMatrix.Matrix[row, col] * currentLSBs[col].LSB;
                }
                result[row] = rowResult % 2;
            }
            return result;
        }

        private LSBComponent[] changeLSB(int[] difference, LSBComponent[] currentLSBs) {
            bool haveToChangeLSB = false;
            int index = 0;

            while (index < HammingMatrix.Rows && !haveToChangeLSB) {
                haveToChangeLSB = difference[index] != 0;
                index++;
            }

            if (haveToChangeLSB) {
                bool realCol = false;
                int bitToChange = -1;

                for (int col = 0; col < HammingMatrix.Cols; col++) {
                    for (int row = 0; row < HammingMatrix.Rows; row++) {
                        realCol = HammingMatrix.Matrix[row, col] == difference[row];
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
