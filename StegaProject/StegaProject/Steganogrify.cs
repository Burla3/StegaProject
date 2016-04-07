using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace StegaProject {
    class Steganogrify {

        private HammingMatrix HammingMatrix { get; }

        private int[] MsgToEncodeInBits { get; }
        public bool EndOfString { get; set; }
        
        public Steganogrify(string msgToEncode, int sizeOfMatrix) {
            MsgToEncodeInBits = new int[(msgToEncode.Length + 1) * 8];
            string tempString;
            int index = 0;

            for (int i = 0; i < msgToEncode.Length; i++) {
                tempString = Convert.ToString(msgToEncode[i], 2).PadLeft(8, '0');
                foreach (char c in tempString) {
                    MsgToEncodeInBits[index++] = Convert.ToInt32(c.ToString(), 2);
                }          
            }
            for (int i = index; i < index + 8; i++) {
                MsgToEncodeInBits[i] = 0;
            }
                    
            HammingMatrix = new HammingMatrix(sizeOfMatrix);
        }

        public string decodeMsg(List<EntropyComponent> entropyComponents, int componentsThatCanBeChanged) {
            string msgAsString = "";
            int[] msgInBits;
            List<LSBComponent> LSBsThatCanBeChanged = getLSBsThatCanBeChanged(entropyComponents);

            for (int i = 0; i < LSBsThatCanBeChanged.Count / HammingMatrix.Cols; i++) {
                msgInBits = matrixVectorProduct(LSBsThatCanBeChanged, i);

                foreach (int bit in msgInBits) {
                    msgAsString += bit.ToString();
                }     
            }
            Console.WriteLine();

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
            List<LSBComponent> LSBsThatCanBeChanged = getLSBsThatCanBeChanged(entropyComponents);

            for (int i = 0; i < MsgToEncodeInBits.Length / HammingMatrix.Rows; i++) {
                LSBsThatCanBeChanged = checkLSB(LSBsThatCanBeChanged, i);
            }

            foreach (LSBComponent currentLSB in LSBsThatCanBeChanged) {
                ((ACComponent)entropyComponents[currentLSB.IndexInEntropyComponents]).LSB = currentLSB.LSB;
            }
        }

        private List<LSBComponent> getLSBsThatCanBeChanged(List<EntropyComponent> entropyComponents) {
            List<LSBComponent> LSBsThatCanBeChanged = new List<LSBComponent>();
            int index = 0;

            foreach (EntropyComponent entropyComponent in entropyComponents) {
                if (entropyComponent is ACComponent) {
                    LSBsThatCanBeChanged.Add(new LSBComponent(((ACComponent)entropyComponent).LSB, index));
                }
                index++;
            }
            return LSBsThatCanBeChanged;
        }         

        private List<LSBComponent> checkLSB(List<LSBComponent> LSBsThatCanBeChanged, int currentIteration) {
            int[] difference = new int[HammingMatrix.Rows];
            int[] matrixVectorProductResult = matrixVectorProduct(LSBsThatCanBeChanged, currentIteration);

            for (int i = 0; i < HammingMatrix.Rows; i++) {
                difference[i] = (matrixVectorProductResult[i] + MsgToEncodeInBits[HammingMatrix.Rows * currentIteration + i]) % 2;
                //difference[i] = (matrixVectorProductResult[i] + ((HammingMatrix.Rows * currentIteration + i) < MsgToEncodeInBits.Length ? MsgToEncodeInBits[HammingMatrix.Rows * currentIteration + i] : matrixVectorProductResult[i])) % 2;
            }

            LSBsThatCanBeChanged = changeLSB(difference, LSBsThatCanBeChanged, currentIteration);

            return LSBsThatCanBeChanged;
        }

        private int[] matrixVectorProduct(List<LSBComponent> LSBsThatCanBeChanged, int currentIteration) {
            int[] result = new int[HammingMatrix.Rows];
            int rowResult;

            for (int row = 0; row < HammingMatrix.Rows; row++) {
                rowResult = 0;
                for (int col = 0; col < HammingMatrix.Cols; col++) {
                    rowResult += HammingMatrix.Matrix[row, col] * LSBsThatCanBeChanged[HammingMatrix.Cols * currentIteration + col].LSB;
                }
                result[row] = rowResult % 2;
            }
            return result;
        }

        private List<LSBComponent> changeLSB(int[] difference, List<LSBComponent> LSBsThatCanBeChanged, int currentIteration) {
            bool haveToChangeLSB = false;

            foreach (int LSB in difference) {
                if (LSB != 0) {
                    haveToChangeLSB = true;
                    break;
                }
            }

            if (haveToChangeLSB) {
                bool realCol = false;
                int bitToChange = 0;

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
                LSBsThatCanBeChanged[HammingMatrix.Cols * currentIteration + bitToChange].LSB = LSBsThatCanBeChanged[HammingMatrix.Cols * currentIteration + bitToChange].LSB == 0 ? 1 : 0;               
            }
            return LSBsThatCanBeChanged;
        }
    }
}
