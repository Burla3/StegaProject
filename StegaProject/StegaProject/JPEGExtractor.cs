using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;

namespace StegaProject {
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

        private string RemoveDashes( string s ) {
            return s.Replace( "-", "" );
        }

        private string ReplaceDashesWithSpaces( string s ) {
            return s.Replace( "-", " " );
        }

        private int FindMarker( string marker, int startIndex = 0 ) {
            int index = startIndex;

            while ( index != -1 && index < imageBytes.Length - 1 && RemoveDashes( BitConverter.ToString( imageBytes, index, MARKERLENGTH ) ) != marker ) {
                index++;
            }

            if ( index < imageBytes.Length - 1 ) {
                return index;
            }

            return -1;
        }

        private int FindDataStart( string marker, int startIndex = 0 ) {
            int index = startIndex;

            index = FindMarker( marker, startIndex );

            if ( index != -1 ) {
                return index + MARKERANDFIELDLENGTHOFFSET;
            }

            return -1;
        }

        private List<string> GetFieldData( string marker ) {
            List<string> list = new List<string>();

            int index = FindMarker( marker );
            int fieldLength = GetFieldLength( marker );
            int dataStart = FindDataStart( marker );

            while ( index != -1 ) {
                list.Add( ReplaceDashesWithSpaces( BitConverter.ToString( imageBytes, dataStart, fieldLength ) ) );

                index++;

                index = FindMarker( marker, index );
                fieldLength = GetFieldLength( marker, index );
                dataStart = FindDataStart( marker, index );
            }

            return list;
        }

        private int GetFieldLength( string marker, int startIndex = 0 ) {
            int index = FindMarker( marker, startIndex );

            return int.Parse( RemoveDashes( BitConverter.ToString( imageBytes, index + MARKERLENGTH, MARKERLENGTH ) ),
                NumberStyles.HexNumber ) - FIELDLENGTHOFFSET;
        }

        private bool DataContainsThumbnail() {
            int firstIndex = FindMarker( SOSMARKER );

            if ( FindMarker( SOSMARKER, firstIndex + MARKERLENGTH ) != -1 ) {
                return true;
            }

            return false;
        }

        public void LoadImage( string path ) {
            try {
                Image image = Image.FromFile( path );
                image.Save( memoryStream, ImageFormat.Jpeg );
                imageBytes = memoryStream.ToArray();
            } catch ( FileNotFoundException e ) {
                Console.WriteLine( "Tried to access file: " + e.FileName + ". However, I was not able to find it. Exception message: " + e.Message );
            }
        }

        public void SaveImage( MemoryStream imageStream, string path ) {
            Image image = Image.FromStream( imageStream );
            image.Save( path, ImageFormat.Jpeg );
        }

        public List<string> GetDQT() {
            return GetFieldData( DQTMARKER );
        }

        public List<string> GetDHT() {
            return GetFieldData( DHTMARKER );
        }

        public List<string> GetDRI() {
            return GetFieldData( DRIMARKER );
        }

        public List<string> GetSOF() {
            return GetFieldData( SOFMARKER );
        }

        public List<string> GetSOS() {
            return GetFieldData( SOSMARKER );
        }

        public string GetCompressedImageData() {
            int index = FindMarker( SOSMARKER );

            if ( DataContainsThumbnail() ) {
                index = FindMarker( SOSMARKER, index + MARKERLENGTH );
            }

            int SOSFieldLength = GetFieldLength( SOSMARKER, index );

            int startIndex = index + MARKERANDFIELDLENGTHOFFSET + SOSFieldLength;

            while ( RemoveDashes( BitConverter.ToString( imageBytes, index, MARKERLENGTH ) ) != EOIMARKER ) {
                index++;
            }

            int endIndex = index;

            return ReplaceDashesWithSpaces( BitConverter.ToString( imageBytes, startIndex, endIndex - startIndex ) );
        }

        public string GetAllData() {
            return ReplaceDashesWithSpaces( BitConverter.ToString( imageBytes, 0, imageBytes.Length ) );
        }
    }
}
