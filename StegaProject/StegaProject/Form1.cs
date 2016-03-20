using System;
using System.Collections.Generic;
using System.Windows.Forms;
using HuffmanTreeBuilder;

namespace StegaProject {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();

            JPEGExtractor extractor = new JPEGExtractor(@"C:\Users\Mikke\Desktop\rainbow-16p-q60-optimized.jpg");
            Steganogrify steganogrify = new Steganogrify("101");

            Decoder decoder = new Decoder(extractor);

            steganogrify.encodeMsg(decoder.EntropyComponents);
            Console.WriteLine("Done");

            extractor.SaveImage(decoder.getReEncodedRawHexData(), @"C:\Users\Mikke\Desktop\hamming-16p-test.jpg");

            extractor = new JPEGExtractor(@"C:\Users\Mikke\Desktop\hamming-16p-test.jpg");

            decoder = new Decoder(extractor);

            Console.WriteLine(steganogrify.decodeMsg(decoder.EntropyComponents));
        }
    }
}
