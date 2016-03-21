using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using HuffmanTreeBuilder;

namespace StegaProject {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            JPEGExtractor extractor = new JPEGExtractor(@"C:\Users\benja\Desktop\rainbow-1024p-q60-optimized.jpg");
            Decoder decoder = new Decoder(extractor);

            Steganogrify steganogrify = new Steganogrify("110");

            Console.WriteLine("Encoding msg");
            steganogrify.encodeMsg(decoder.EntropyComponents);

            extractor.SaveImage(decoder.getReEncodedRawHexData(), @"C:\Users\benja\Desktop\hamming-1024p-test.jpg");

            //Decode
            Console.WriteLine("Decoding newly created JPEG");
            extractor = new JPEGExtractor(@"C:\Users\benja\Desktop\hamming-1024p-test.jpg");

            decoder = new Decoder(extractor);
            Console.WriteLine("Extracting hidden msg");
            Console.WriteLine("Msg hidden in JPEG " + steganogrify.decodeMsg(decoder.EntropyComponents));
            sw.Stop();
            Console.WriteLine($"Program completed in: {sw.Elapsed}");
        }
    }
}
