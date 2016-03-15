using System;
using System.Collections.Generic;
using System.Windows.Forms;
using HuffmanTreeBuilder;

namespace StegaProject {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();

            JPEGExtractor extractor = new JPEGExtractor(@"C:\Users\Mikke\Desktop\IMG_20160309_160619.jpg");
            Steganogrify steganogrify = new Steganogrify();

            Decoder decoder = new Decoder(extractor);

            //foreach (EntropyComponent entropyComponent in decoder.EntropyComponents) {
            //    if (entropyComponent.Amplitude != "EOB") {
            //        Console.WriteLine(
            //            $"Amplitude {entropyComponent.Amplitude}, HuffmanTreePath {entropyComponent.HuffmanTreePath}, " +
            //            $"HuffmanLeafHexValue {entropyComponent.HuffmanLeafHexValue}, DecimalValue {entropyComponent.getDecimalValue()}," +
            //            $" LSB {entropyComponent.LSB}");
            //    }
            //    else {
            //        Console.WriteLine("EOB");
            //    }
            //}

            steganogrify.changeLSB(decoder.EntropyComponents);
            
            //Console.WriteLine(decoder.getReEncodedRawHexData());

            extractor.SaveImage(decoder.getReEncodedRawHexData(), @"C:\Users\Mikke\Desktop\small-test.jpg");
        }
    }
}
