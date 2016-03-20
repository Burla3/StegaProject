using System;
using System.Collections.Generic;
using System.Windows.Forms;
using HuffmanTreeBuilder;

namespace StegaProject {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();

            JPEGExtractor extractor = new JPEGExtractor(@"C:\Users\Nyggi\Desktop\rainbow-16p-q60-optimized.jpg");
            Steganogrify steganogrify = new Steganogrify("101");

            Decoder decoder = new Decoder(extractor);

            Console.WriteLine("Encoding msg");
            steganogrify.encodeMsg(decoder.EntropyComponents);

            extractor.SaveImage(decoder.getReEncodedRawHexData(), @"C:\Users\Nyggi\Desktop\hamming-16p-test.jpg");

            //Decode
            Console.WriteLine("Decoding newly created JPEG");
            extractor = new JPEGExtractor(@"C:\Users\Nyggi\Desktop\hamming-16p-test.jpg");

            decoder = new Decoder(extractor);
            Console.WriteLine("Extracting hidden msg");
            Console.WriteLine("Msg hidden in JPEG " + steganogrify.decodeMsg(decoder.EntropyComponents));
        }
    }
}
