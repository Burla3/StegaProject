﻿using System;
using System.Collections.Generic;
using System.IO;
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

        public Decoder(JPEGExtractor extractor) {
            CurrentIndex = 0;
            HuffmanTrees = new List<HuffmanTree>();
            EntropyComponents = new List<EntropyComponent>();
            Console.WriteLine("Hallo");
            buildHuffmanTrees(extractor);
            //foreach (HuffmanTree huffmanTree in HuffmanTrees) {
            //    Console.WriteLine("New Tree");
            //    huffmanTree.printAddresses();
            //}
            Console.WriteLine("Hallo, its me");
            getBinaryData(extractor);
            Console.WriteLine("SHUT UP!");

            decodeBinaryData();
        }

        private void buildHuffmanTrees(JPEGExtractor extractor) {
            List<string> DHT = extractor.GetDHT();

            //foreach (string table in DHT) {
            //    Console.WriteLine(table);
            //}

            foreach (string table in DHT) {
                HuffmanTrees.Add(new HuffmanTree(table));
            }
        }

        private void getBinaryData(JPEGExtractor extractor) {
            string data = extractor.GetCompressedImageData();

            Console.WriteLine("JAJAJAA " + data.Length);
            //Console.WriteLine(data);

            StringBuilder sBuilder = new StringBuilder();

           

            for (int i = 0; i < data.Length; i++) {
                if (i % 1000 == 0) {
                    Console.WriteLine(i);
                }
                if (data[i] != ' ') {
                    sBuilder.Append(Convert.ToString(Convert.ToInt32(data[i].ToString(), 16), 2).PadLeft(4, '0'));
                    //BinaryData += Convert.ToString(Convert.ToInt32(data[i].ToString(), 16), 2).PadLeft(4, '0');
                }
            }

            BinaryData = sBuilder.ToString();

            //Console.WriteLine(data);
            //Console.WriteLine(BinaryData);
        }

        private void decodeBinaryData() {
            bool hitEOB;

            while (CurrentIndex < BinaryData.Length) {

                //Lum manuel supsampling for now change i
                for (int i = 0; i < 4; i++) {
                    Console.WriteLine("Lum");
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

                //Crom manuel supsampling for now change i
                for (int i = 0; i < 2; i++) {
                    Console.WriteLine("Chrom");
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
                if (BinaryData.Length - CurrentIndex < 8) {
                    Console.WriteLine("Only trash left");
                    break;
                }
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

                //Console.WriteLine($"Huffman {huffmanLeafHexValue} TreePath {amplitude}");

                EntropyComponents.Add(new EntropyComponent(huffmanTreePath, huffmanLeafHexValue, amplitude));
            } else {
                Console.WriteLine("EOB");
                EntropyComponents.Add(new EntropyComponent(huffmanTreePath, huffmanLeafHexValue, "EOB"));
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
            int lenght = Convert.ToInt32(huffmanLeafHexValue[1].ToString(), 16);

            for (int i = 0; i < lenght; i++, CurrentIndex++) {
                value += BinaryData[CurrentIndex];
            }

            return value;
        }

        public string getReEncodedRawHexData() {
            BinaryData = "";
            string HexData = "";

            StringBuilder sBuilder = new StringBuilder();

            foreach (EntropyComponent entropyComponent in EntropyComponents) {
                if (entropyComponent.HuffmanLeafHexValue == "00" || entropyComponent.Amplitude == "EOB") {
                    //BinaryData += entropyComponent.HuffmanTreePath;
                    sBuilder.Append(entropyComponent.HuffmanTreePath);
                } else {
                    //BinaryData += entropyComponent.HuffmanTreePath + entropyComponent.Amplitude;
                    sBuilder.Append(entropyComponent.HuffmanTreePath + entropyComponent.Amplitude);
                }   
            }

            while (BinaryData.Length % 8 != 0) {
                //BinaryData += "1";
                sBuilder.Append("1");
            }

            BinaryData = sBuilder.ToString();


            sBuilder.Clear();

            for (int i = 0; i < BinaryData.Length; i += 4) {
                sBuilder.Append(Convert.ToString(Convert.ToInt32(BinaryData.Substring(i, 4), 2), 16));
                //HexData += Convert.ToString(Convert.ToInt32(BinaryData.Substring(i, 4), 2), 16);
            }

            HexData = sBuilder.ToString();

            return HexData.ToUpper();
        }
    }
}