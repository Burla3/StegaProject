using System;
using System.Collections.Generic;
using System.Windows.Forms;
using HuffmanTreeBuilder;

namespace StegaProject {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();

            Decoder decoder = new Decoder(@"C:\Users\Mikke\Desktop\small-rainbow-60-web-optimized.jpg");

            foreach (EntropyComponent entropyComponent in decoder.EntropyComponents) {
                Console.WriteLine($"Amplitude {entropyComponent.Amplitude}, HuffmanTreePath {entropyComponent.HuffmanTreePath}, " +
                                  $"HuffmanLeafHexValue {entropyComponent.HuffmanLeafHexValue}, DecimalValue {entropyComponent.getDecimalValue()}," +
                                  $" LSB {entropyComponent.LSB}");
            }

        }
    }
}
