using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StegaProject {
    class EntropyComponent {
        public EntropyComponent(string huffmanTreePath, string huffmanLeafHexValue, string amplitude, bool isDc) {
            HuffmanTreePath = huffmanTreePath;
            HuffmanLeafHexValue = huffmanLeafHexValue;
            Amplitude = amplitude;
            IsDC = isDc;
        }

        public bool IsDC { get; private set; }

        public string HuffmanTreePath { get; private set; }

        public string Amplitude { get; set; }

        public string HuffmanLeafHexValue { get; private set; }

        public int LSB {
            get { return this.Amplitude == "EOB" ? -1 : int.Parse(Amplitude.Substring(Amplitude.Length - 1)) ; }

            set {  Amplitude = Amplitude == "EOB" ? "EOB" : Amplitude.Remove(Amplitude.Length - 1, 1) + value.ToString(); }
        }

        public int getDecimalValue() {
            int decimalValue;

            if (Amplitude != "EOB") {
                if (Amplitude[0] == '0') {
                    decimalValue = -((int) Math.Pow(2, Amplitude.Length) - (Convert.ToInt32(Amplitude, 2) + 1));
                }
                else {
                    decimalValue = Convert.ToInt32(Amplitude, 2);
                }
            }
            else {
                decimalValue = -9999;
            }

            return decimalValue;
        }
    }
}
