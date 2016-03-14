using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HuffmanTreeBuilder;

namespace StegaProject {
    class Decoder {

        private List<HuffmanTree> buildHuffmanTrees;

        private List<string> huffmanTrees;

        private string binaryData;


        public Decoder(string path) {

            JPEGExtractor s = new JPEGExtractor();
            s.LoadImage(path);

            string data = s.GetCompressedImageData();

            huffmanTrees = s.GetDHT();

            foreach (string huffmanTree in huffmanTrees) {
                Console.WriteLine(huffmanTree);
            }

            buildHuffmanTrees = new List<HuffmanTree>();

            foreach (string tree in huffmanTrees) {
                buildHuffmanTrees.Add(new HuffmanTree(tree));
            }

            for (int i = 0; i < data.Length; i++) {
                if (data[i] != ' ') {
                    binaryData += Convert.ToString(Convert.ToInt32(data[i].ToString(), 16), 2).PadLeft(4, '0');
                }
            }

            Console.WriteLine(data);
            Console.WriteLine(binaryData);

            //int currentIndex = 0;
            //string currentHuffmanCode = "";
            //string huffmanLeaveValue = "";

            //while (currentIndex <= binaryData.Length) {

            //    for

            //    while (huffmanLeaveValue == "") {
            //            currentHuffmanCode += binaryData[currentIndex];
            //            huffmanLeaveValue = buildHuffmanTrees[0].SearchFor(currentHuffmanCode);
            //        }


            //}
        }
    }
}
