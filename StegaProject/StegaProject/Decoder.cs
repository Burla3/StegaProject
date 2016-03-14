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
            JPEGExtractor extractor = new JPEGExtractor(path);
            HuffmanTrees = new List<HuffmanTree>();
            EntropyComponents = new List<EntropyComponent>();

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
                    Console.WriteLine($"DC done");
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
                    Console.WriteLine($"DC done");
                    for (int j = 0; j < 63; j++) {
                        hitEOB = decodeValue(HuffmanTable.ChromAC);
                        if (hitEOB) {
                            Console.WriteLine($"AC done");
                            break;
                        }
                    }
                }
                //TEMP FIX!! NEED REWORK!! 
               break;
            } 
        }

        private bool decodeValue(HuffmanTable table) {
            string huffmanLeafHexValue;
            string amplitude;
            string huffmanTreePath;

            getHuffmanLeafHexValue(out huffmanTreePath, out huffmanLeafHexValue, table);

            if (huffmanLeafHexValue != "00" || (table == HuffmanTable.ChromDC || table == HuffmanTable.LumDC)) {
                amplitude = getAmplitude(huffmanLeafHexValue);

                if (amplitude == "") {
                    amplitude = "0";
                }

                Console.WriteLine($"Huffman {huffmanLeafHexValue} TreePath {amplitude}");

                EntropyComponents.Add(new EntropyComponent(huffmanTreePath, huffmanLeafHexValue, amplitude));
            } else {
                return true;
            }

            return false;
        }

        private string getHuffmanLeafHexValue(out string currentHuffmanTreePath, out string huffmanLeafHexValue, HuffmanTable table) {
            currentHuffmanTreePath = "";
            huffmanLeafHexValue = "";

            while (huffmanLeafHexValue == "") {
                currentHuffmanTreePath += BinaryData[CurrentIndex];
                huffmanLeafHexValue = HuffmanTrees[(int)table].SearchFor(currentHuffmanTreePath);
                CurrentIndex++;
            }

            return huffmanLeafHexValue;
        }

        private string getAmplitude(string huffmanLeafHexValue) {
            string value = "";
            int lenght = Convert.ToInt32(huffmanLeafHexValue[1].ToString(), 10);

            for (int i = 0; i < lenght; i++, CurrentIndex++) {
                value += BinaryData[CurrentIndex];
            }

            return value;
        }
    }
}
