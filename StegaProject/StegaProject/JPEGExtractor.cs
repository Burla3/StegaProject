using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;

namespace StegaProject {
    /// <summary>
    /// This class holds methods to extract different markers' data, compressed image data and all data from JPEG files.
    /// </summary>
    class JPEGExtractor {
        private const byte MARKERLENGTH = 2;
        private const byte FIELDLENGTHOFFSET = 2;
        private const byte MARKERANDFIELDLENGTHOFFSET = 4;
        private const string DQTMARKER = "FFDB";
        private const string DHTMARKER = "FFC4";
        private const string DRIMARKER = "FFDD";
        private const string SOFMARKER = "FFC0";
        private const string SOSMARKER = "FFDA";
        private const string EOIMARKER = "FFD9";

        private MemoryStream memoryStream = new MemoryStream();
        private byte[] imageBytes;

        private string RemoveDashes(string s) {
            return s.Replace("-", "");
        }

        private string ReplaceDashesWithSpaces(string s) {
            return s.Replace("-", " ");
        }

        private int FindMarker(string marker, int startIndex = 0) {
            int index = startIndex;

            while (index != -1 && index < imageBytes.Length - 1 &&
                   RemoveDashes(BitConverter.ToString(imageBytes, index, MARKERLENGTH)) != marker) {
                index++;
            }

            if (index < imageBytes.Length - 1) {
                return index;
            }

            return -1;
        }

        private int FindDataStart(string marker, int startIndex = 0) {
            int index = startIndex;

            index = FindMarker(marker, startIndex);

            if (index != -1) {
                return index + MARKERANDFIELDLENGTHOFFSET;
            }

            return -1;
        }

        private List<string> GetFieldData(string marker) {
            List<string> list = new List<string>();

            int index = FindMarker(marker);
            int fieldLength = GetFieldLength(marker);
            int dataStart = FindDataStart(marker);

            while (index != -1) {
                list.Add(ReplaceDashesWithSpaces(BitConverter.ToString(imageBytes, dataStart, fieldLength)));

                index++;

                index = FindMarker(marker, index);
                fieldLength = GetFieldLength(marker, index);
                dataStart = FindDataStart(marker, index);
            }

            return list;
        }

        private int GetFieldLength(string marker, int startIndex = 0) {
            int index = FindMarker(marker, startIndex);

            return int.Parse(RemoveDashes(BitConverter.ToString(imageBytes, index + MARKERLENGTH, MARKERLENGTH)),
                NumberStyles.HexNumber) - FIELDLENGTHOFFSET;
        }

        private bool DataContainsThumbnail() {
            int firstIndex = FindMarker(SOSMARKER);

            if (FindMarker(SOSMARKER, firstIndex + MARKERLENGTH) != -1) {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Calls the LoadImage method.
        /// </summary>
        /// <param name="path">Path to the image file you wish to extract from.</param>
        public JPEGExtractor(string path) {
            LoadImage(path);
        }

        /// <summary>
        /// Loads an image file into the class.
        /// </summary>
        /// <param name="path">Path to the image file you wish to extract from.</param>
        public void LoadImage(string path) {
            try {
                Image image = Image.FromFile(path);
                image.Save(memoryStream, ImageFormat.Jpeg);
                imageBytes = memoryStream.ToArray();
            }
            catch (FileNotFoundException e) {
                Console.WriteLine("Tried to access file: " + e.FileName +
                                  ". However, I was not able to find it. Exception message: " + e.Message);
            }
        }


        /// <summary>
        /// Saves a MemoryStream as a JPEG image file at the given path.
        /// </summary>
        /// <param name="imageStream">MemoryStream containing JPEG image bytes.</param>
        /// <param name="path">Path to the image file you wish to save.</param>
        public void SaveImage(string hexDataToInject, string path) {
            int index = FindMarker(SOSMARKER);

            if (DataContainsThumbnail()) {
                index = FindMarker(SOSMARKER, index + MARKERLENGTH);
            }

            int SOSFieldLength = GetFieldLength(SOSMARKER, index);

            int startIndex = index + MARKERANDFIELDLENGTHOFFSET + SOSFieldLength;

            for (int i = startIndex, j = 0; j < hexDataToInject.Length; i++, j += 2) {
                string hexValue = hexDataToInject[j] + "" + hexDataToInject[j + 1];
                imageBytes[i] = Convert.ToByte(hexValue, 16);
            }

            MemoryStream memStream = new MemoryStream(imageBytes);

            Image image = Image.FromStream(memStream);
            image.Save(path, ImageFormat.Jpeg);
        }

        /// <summary>
        /// Gets DefineQuantizationTable marker data as a List.
        /// </summary>
        /// <returns>A List containing DefineQuantizationTable marker data.</returns>
        public List<string> GetDQT() {
            return GetFieldData(DQTMARKER);
        }

        /// <summary>
        /// Gets DefineHuffmanTable marker data as a List.
        /// </summary>
        /// <returns>A List containing DefineHuffmanTable marker data.</returns>
        public List<string> GetDHT() {
            return GetFieldData(DHTMARKER);
        }

        /// <summary>
        /// Gets DefineRestartInteroperability marker data as a List.
        /// </summary>
        /// <returns>A List containing DefineRestartInteroperability marker data.</returns>
        public List<string> GetDRI() {
            return GetFieldData(DRIMARKER);
        }

        /// <summary>
        /// Gets StartOfFrame marker data as a List.
        /// </summary>
        /// <returns>A List containing StartOfFrame marker data.</returns>
        public List<string> GetSOF() {
            return GetFieldData(SOFMARKER);
        }

        /// <summary>
        /// Gets StartOfScan marker data as a List.
        /// </summary>
        /// <returns>A List containing StartOfScan marker data.</returns>
        public List<string> GetSOS() {
            return GetFieldData(SOSMARKER);
        }

        /// <summary>
        /// Gets compressed image data as a string.
        /// </summary>
        /// <returns>A string containing compressed image data.</returns>
        public string GetCompressedImageData() {
            int index = FindMarker(SOSMARKER);

            if (DataContainsThumbnail()) {
                index = FindMarker(SOSMARKER, index + MARKERLENGTH);
            }

            int SOSFieldLength = GetFieldLength(SOSMARKER, index);

            int startIndex = index + MARKERANDFIELDLENGTHOFFSET + SOSFieldLength;

            while (RemoveDashes(BitConverter.ToString(imageBytes, index, MARKERLENGTH)) != EOIMARKER) {
                index++;
            }

            int endIndex = index;

            return RemoveDashes(BitConverter.ToString(imageBytes, startIndex, endIndex - startIndex));
        }

        /// <summary>
        /// Gets all data from MemoryStream as a string.
        /// </summary>
        /// <returns>A string containing all data from MemoryStream.</returns>
        public string GetAllData() {
            return ReplaceDashesWithSpaces(BitConverter.ToString(imageBytes, 0, imageBytes.Length));
        }
    }
}
