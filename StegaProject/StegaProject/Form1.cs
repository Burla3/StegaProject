using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using HuffmanTreeBuilder;

namespace StegaProject {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();

            JPEGExtractor extractor = new JPEGExtractor(@"C:\Users\Nyggi\Desktop\rainbow-1024p-q60-optimized.jpg");

            Stopwatch sw = new Stopwatch();
            sw.Start();

            Decoder decoder = new Decoder(extractor);

            Steganogrify steganogrify =
                new Steganogrify(
                    "Steganography is the practice of concealing a file, message, image, or video within another file, message, image, or video. The word steganography combines the Greek words steganos, meaning covered, concealed, or protected, and graphein meaning writing. The first recorded use of the term was in 1499 by Johannes Trithemius in his Steganographia, a treatise on cryptography and steganography, disguised as a book on magic. Generally, the hidden messages appear to be (or be part of) something else: images, articles, shopping lists, or some other cover text. For example, the hidden message may be in invisible ink between the visible lines of a private letter. Some implementations of steganography that lack a shared secret are forms of security through obscurity, whereas key-dependent steganographic schemes adhere to Kerckhoffs's principle. The advantage of steganography over cryptography alone is that the intended secret message does not attract attention to itself as an object of scrutiny. Plainly visible encrypted messagesno matter how unbreakablearouse interest, and may in themselves be incriminating in countries where encryption is illegal. Thus, whereas cryptography is the practice of protecting the contents of a message alone, steganography is concerned with concealing the fact that a secret message is being sent, as well as concealing the contents of the message. Steganography includes the concealment of information within computer files. In digital steganography, electronic communications may include steganographic coding inside of a transport layer, such as a document file, image file, program or protocol. Media files are ideal for steganographic transmission because of their large size. For example, a sender might start with an innocuous image file and adjust the color of every 100th pixel to correspond to a letter in the alphabet, a change so subtle that someone not specifically looking for it is unlikely to notice it. Steganography is the practice of concealing a file, message, image, or video within another file, message, image, or video. The word steganography combines the Greek words steganos, meaning covered, concealed, or protected, and graphein meaning writing. The first recorded use of the term was in 1499 by Johannes Trithemius in his Steganographia, a treatise on cryptography and steganography, disguised as a book on magic. Generally, the hidden messages appear to be (or be part of) something else: images, articles, shopping lists, or some other cover text. For example, the hidden message may be in invisible ink between the visible lines of a private letter. Some implementations of steganography that lack a shared secret are forms of security through obscurity, whereas key-dependent steganographic schemes adhere to Kerckhoffs's principle. The advantage of steganography over cryptography alone is that the intended secret message does not attract attention to itself as an object of scrutiny. Plainly visible encrypted messagesno matter how unbreakablearouse interest, and may in themselves be incriminating in countries where encryption is illegal. Thus, whereas cryptography is the practice of protecting the contents of a message alone, steganography is concerned with concealing the fact that a secret message is being sent, as well as concealing the contents of the message. Steganography includes the concealment of information within computer files. In digital steganography, electronic communications may include steganographic coding inside of a transport layer, such as a document file, image file, program or protocol. Media files are ideal for steganographic transmission because of their large size. For example, a sender might start with an innocuous image file and adjust the color of every 100th pixel to correspond to a letter in the alphabet, a change so subtle that someone not specifically looking for it is unlikely to notice it.");

            Console.WriteLine("Encoding msg");
            steganogrify.encodeMsg(decoder.EntropyComponents);

            extractor.SaveImage(decoder.getReEncodedRawHexData(), @"C:\Users\Nyggi\Desktop\hamming-1024p-test.jpg");

            //Decode
            Console.WriteLine("Decoding newly created JPEG");
            extractor = new JPEGExtractor(@"C:\Users\Nyggi\Desktop\hamming-1024p-test.jpg");

            decoder = new Decoder(extractor);
            Console.WriteLine("Extracting hidden msg");
            Console.WriteLine("Msg hidden in JPEG: " + steganogrify.decodeMsg(decoder.EntropyComponents));

            sw.Stop();
            Console.WriteLine($"Program completed in: {sw.Elapsed}");
        }
    }
}
