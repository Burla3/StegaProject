using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StegaProject {
    class EntropyComponent {

        public EntropyComponent(string huffman, string valueCode) {
            HuffmannCode = huffman;
            ValueCode = valueCode;
        }

        public string HuffmannCode { get; private set; }

        public string ValueCode { get; private set; }

        public int LSB {

            get { return int.Parse(ValueCode.Substring(ValueCode.Length - 1)); }

            set { ValueCode = ValueCode.Remove(ValueCode.Length - 1, 1) + value.ToString(); }
        }
    }
}
