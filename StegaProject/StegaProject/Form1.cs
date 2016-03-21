using System;
using System.Collections.Generic;
using System.Windows.Forms;
using HuffmanTreeBuilder;

namespace StegaProject {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();

            JPEGExtractor extractor = new JPEGExtractor(@"C:\Users\Mikke\Desktop\rainbow-4096p-q60-optimized.jpg");
            Decoder decoder = new Decoder(extractor);

            Steganogrify steganogrify = new Steganogrify("110");

            Console.WriteLine("Encoding msg");
            steganogrify.encodeMsg(decoder.EntropyComponents);

            extractor.SaveImage(decoder.getReEncodedRawHexData(), @"C:\Users\Mikke\Desktop\hamming-4096p-test.jpg");

            //Decode
            Console.WriteLine("Decoding newly created JPEG");
            extractor = new JPEGExtractor(@"C:\Users\Mikke\Desktop\hamming-4096p-test.jpg");

            decoder = new Decoder(extractor);
            Console.WriteLine("Extracting hidden msg");
            Console.WriteLine("Msg hidden in JPEG " + steganogrify.decodeMsg(decoder.EntropyComponents));
        }
    }
}
