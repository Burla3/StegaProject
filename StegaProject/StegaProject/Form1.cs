using System;
using System.IO;
using System.Windows.Forms;

namespace StegaProject {
    public partial class Form1 : Form {
        public JPEGExtractor Extractor { get; set; }
        public Decoder Decoder { get; set; }
        public string LoadPath { get; set; }

        public Form1() {
            InitializeComponent();

            EncodeMsg.Enabled = false;
            ExtractMessage.Enabled = false;
            TextBox.Enabled = false;
            LengthOfText.Enabled = false;
            SizeOfHammingMatrix.Enabled = false;
        }

        private void LoadImage_Click(object sender, EventArgs e) {
            OpenFileDialog file = new OpenFileDialog();
            file.InitialDirectory = "c:\\";
            file.Filter = "jpg files (*.jpg)|*.jpg|All files (*.*)|*.";
            file.FilterIndex = 1;
            file.RestoreDirectory = true;
            TextBox.Clear();

            if (file.ShowDialog() == DialogResult.OK) {
                LoadPath = file.FileName;

                LoadedPicture.ImageLocation = LoadPath;
                LoadedPicture.SizeMode = PictureBoxSizeMode.StretchImage;
                LoadedPicture.Load();
                Extractor = new JPEGExtractor(LoadPath);
                Decoder = new Decoder(Extractor);

                TextBox.ReadOnly = false;
                EncodeMsg.Enabled = true;
                ExtractMessage.Enabled = true;
                TextBox.Enabled = true;
                LengthOfText.Enabled = true;
                SizeOfHammingMatrix.Enabled = true;
                LengthOfText.Text = "0 / " + ((int)(Decoder.ComponentsThatCanBeChanged / ((int)Math.Pow(2, (int)SizeOfHammingMatrix.Value) - 1) * SizeOfHammingMatrix.Value / 8)).ToString();
            }                      
        }

        private void EncodeMsg_Click(object sender, EventArgs e) {
            //"Steganography is the practice of concealing a file, message, image, or video within another file, message, image, or video. The word steganography combines the Greek words steganos, meaning covered, concealed, or protected, and graphein meaning writing. The first recorded use of the term was in 1499 by Johannes Trithemius in his Steganographia, a treatise on cryptography and steganography, disguised as a book on magic. Generally, the hidden messages appear to be (or be part of) something else: images, articles, shopping lists, or some other cover text. For example, the hidden message may be in invisible ink between the visible lines of a private letter. Some implementations of steganography that lack a shared secret are forms of security through obscurity, whereas key-dependent steganographic schemes adhere to Kerckhoffs's principle. The advantage of steganography over cryptography alone is that the intended secret message does not attract attention to itself as an object of scrutiny. Plainly visible encrypted messagesno matter how unbreakablearouse interest, and may in themselves be incriminating in countries where encryption is illegal. Thus, whereas cryptography is the practice of protecting the contents of a message alone, steganography is concerned with concealing the fact that a secret message is being sent, as well as concealing the contents of the message. Steganography includes the concealment of information within computer files. In digital steganography, electronic communications may include steganographic coding inside of a transport layer, such as a document file, image file, program or protocol. Media files are ideal for steganographic transmission because of their large size. For example, a sender might start with an innocuous image file and adjust the color of every 100th pixel to correspond to a letter in the alphabet, a change so subtle that someone not specifically looking for it is unlikely to notice it. Steganography is the practice of concealing a file, message, image, or video within another file, message, image, or video. The word steganography combines the Greek words steganos, meaning covered, concealed, or protected, and graphein meaning writing. The first recorded use of the term was in 1499 by Johannes Trithemius in his Steganographia, a treatise on cryptography and steganography, disguised as a book on magic. Generally, the hidden messages appear to be (or be part of) something else: images, articles, shopping lists, or some other cover text. For example, the hidden message may be in invisible ink between the visible lines of a private letter. Some implementations of steganography that lack a shared secret are forms of security through obscurity, whereas key-dependent steganographic schemes adhere to Kerckhoffs's principle. The advantage of steganography over cryptography alone is that the intended secret message does not attract attention to itself as an object of scrutiny. Plainly visible encrypted messagesno matter how unbreakablearouse interest, and may in themselves be incriminating in countries where encryption is illegal. Thus, whereas cryptography is the practice of protecting the contents of a message alone, steganography is concerned with concealing the fact that a secret message is being sent, as well as concealing the contents of the message. Steganography includes the concealment of information within computer files. In digital steganography, electronic communications may include steganographic coding inside of a transport layer, such as a document file, image file, program or protocol. Media files are ideal for steganographic transmission because of their large size. For example, a sender might start with an innocuous image file and adjust the color of every 100th pixel to correspond to a letter in the alphabet, a change so subtle that someone not specifically looking for it is unlikely to notice it."   
            Steganogrify steganogrify = new Steganogrify(TextBox.Text, (int)SizeOfHammingMatrix.Value);

            steganogrify.encodeMsg(Decoder.EntropyComponents);
            Console.WriteLine(Path.GetDirectoryName(LoadPath) + @"\test.jpg");
            Extractor.SaveImage(Decoder.getReEncodedRawHexData(), Path.GetDirectoryName(LoadPath) + @"\test.jpg");
            TextBox.ReadOnly = true;
        }

        private void ExtractMessage_Click(object sender, EventArgs e) {
            Steganogrify steganogrify = new Steganogrify("", (int)SizeOfHammingMatrix.Value);

            Console.WriteLine("Extracting hidden msg");
            TextBox.Text = steganogrify.decodeMsg(Decoder.EntropyComponents, Decoder.ComponentsThatCanBeChanged);
        }

        private void SizeOfHammingMatrix_ValueChanged(object sender, EventArgs e)
        {
            int maxTextLength = (int)
                (Decoder.ComponentsThatCanBeChanged/((int) Math.Pow(2, (int) SizeOfHammingMatrix.Value) - 1)*
                 SizeOfHammingMatrix.Value/8);
            LengthOfText.Text = TextBox.Text.Length + " / " + maxTextLength;
            TextBox.MaxLength = maxTextLength;
            if (TextBox.TextLength > maxTextLength) {
                TextBox.Text = TextBox.Text.Remove(maxTextLength);
            }
        }

        private void TextBox_TextChanged(object sender, EventArgs e) {
            int maxTextLength =
                (int)
                    (Decoder.ComponentsThatCanBeChanged/((int) Math.Pow(2, (int) SizeOfHammingMatrix.Value) - 1)*
                     SizeOfHammingMatrix.Value/8);
            LengthOfText.Text = TextBox.Text.Length + " / " + maxTextLength;
            TextBox.MaxLength = maxTextLength;
        }
    }
}
