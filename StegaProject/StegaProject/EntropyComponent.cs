using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StegaProject {
    class EntropyComponent {

        public EntropyComponent(string huffmanCode, string huffmanLeafCode, string valueCode) {
            HuffmannCode = huffmanCode;
            HuffmanLeafCode = huffmanLeafCode;
            ValueCode = valueCode;
        }

        public string HuffmannCode { get; private set; }

        public string ValueCode { get; private set; }

        public string HuffmanLeafCode { get; private set; }

        public int LSB {

            get { return int.Parse(ValueCode.Substring(ValueCode.Length - 1)); }

            set {  ValueCode = ValueCode.Remove(ValueCode.Length - 1, 1) + value.ToString(); }
        }

        public int getDecimalValue() {
            int decimalValue;

            if (ValueCode[0] == '0') {
                decimalValue = -((int)Math.Pow(2, ValueCode.Length) - (Convert.ToInt32(ValueCode, 2) + 1));
            } else {
                decimalValue = Convert.ToInt32(ValueCode, 2);
            }

            return decimalValue;
        }
    }
}
