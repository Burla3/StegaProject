using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HuffmanTreeBuilder;

namespace StegaProject {
    class Decoder {

        enum HuffmanTable {
            LumDC = 0, LumAC = 1, ChromDC = 2, ChromAC = 3
        }

        private List<HuffmanTree> HuffmanTrees { get; set; }
        public List<EntropyComponent> EntropyComponents { get; private set; }

        private string BinaryData { get; set; }
        private int CurrentIndex { get; set; }

        public Decoder(string path) {
            CurrentIndex = 0;
            JPEGExtractor extractor = new JPEGExtractor();
            HuffmanTrees = new List<HuffmanTree>();
            EntropyComponents = new List<EntropyComponent>();

            extractor.LoadImage(path);
            buildHuffmanTrees(extractor);
            getBinaryData(extractor);

            decodeBinaryData();
        }

        private void buildHuffmanTrees(JPEGExtractor extractor) {
            List<string> DHT = extractor.GetDHT();

            foreach (string table in DHT) {
                Console.WriteLine(table);
            }

            foreach (string table in DHT) {
                HuffmanTrees.Add(new HuffmanTree(table));
            }
        }

        private void getBinaryData(JPEGExtractor extractor) {
            string data = extractor.GetCompressedImageData();

            for (int i = 0; i < data.Length; i++) {
                if (data[i] != ' ') {
                    BinaryData += Convert.ToString(Convert.ToInt32(data[i].ToString(), 16), 2).PadLeft(4, '0');
                }
            }

            Console.WriteLine(data);
            Console.WriteLine(BinaryData);
        }

        private void decodeBinaryData() {
            bool hitEOB;

            while (CurrentIndex < BinaryData.Length) {

                //Lum
                for (int i = 0; i < 1; i++) {
                    decodeValue(HuffmanTable.LumDC);
                    for (int j = 0; j < 63; j++) {
                       hitEOB = decodeValue(HuffmanTable.LumAC);
                        if (hitEOB) {
                            Console.WriteLine($"AC done");
                            break;
                        }  
                    }
                }

                //Crom
                for (int i = 0; i < 2; i++) {
                    decodeValue(HuffmanTable.ChromDC);
                    for (int j = 0; j < 63; j++) {
                        hitEOB = decodeValue(HuffmanTable.ChromAC);
                        if (hitEOB) {
                            Console.WriteLine($"AC done");
                            break;
                        }
                    }
                }
                //TEMP FIX!! NEED REWORK!! 
               // break;
            } 
        }

        private bool decodeValue(HuffmanTable table) {
            string huffmanLeafValue;
            string value;
            string currentHuffmanCode;

            getLeafValue(out currentHuffmanCode, out huffmanLeafValue, table);

            if (huffmanLeafValue != "00") {
                value = getValueCode(huffmanLeafValue);

                if (value == "") {
                    value = "0";
                }

                Console.WriteLine($"Huffman {huffmanLeafValue} TreePath {value}");

                EntropyComponents.Add(new EntropyComponent(currentHuffmanCode, huffmanLeafValue, value));
            } else {
                return true;
            }

            return false;
        }

        private string getLeafValue(out string currentHuffmanCode, out string huffmanLeafValue, HuffmanTable table) {
            currentHuffmanCode = "";
            huffmanLeafValue = "";

            while (huffmanLeafValue == "") {
                currentHuffmanCode += BinaryData[CurrentIndex];
                huffmanLeafValue = HuffmanTrees[(int)table].SearchFor(currentHuffmanCode);
                CurrentIndex++;
            }

            return huffmanLeafValue;
        }

        private string getValueCode(string huffmanLeafValue) {
            string value = "";
            int lenght = Convert.ToInt32(huffmanLeafValue[1].ToString(), 10);

            for (int i = 0; i < lenght; i++, CurrentIndex++) {
                value += BinaryData[CurrentIndex];
            }

            return value;
        }
    }
}
