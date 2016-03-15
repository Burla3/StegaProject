using System;
using System.Collections.Generic;
using System.Windows.Forms;
using HuffmanTreeBuilder;

namespace StegaProject {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();

            JPEGExtractor extractor = new JPEGExtractor(@"C:\Users\Nyggi\Desktop\small-rainbow-60-web-optimized.jpg");
            Steganogrify steganogrify = new Steganogrify();

            Decoder decoder = new Decoder(extractor);

            steganogrify.changeLSB(decoder.EntropyComponents);

            extractor.SaveImage(decoder.getReEncodedRawHexData(), @"C:\Users\Nyggi\Desktop\dsmall-test.jpg");
        }
    }
}
