using System.Collections;

namespace HammingSteganography {
    /// <summary>
    /// This interface holds the basics for a steganography method.
    /// </summary>
    public interface ISteganographier {

        /// <summary>
        /// Decodes an ASCII message from <paramref name="coverData"/>.
        /// </summary>
        /// <param name="coverData">The steganographied cover data.</param>
        /// <returns>The decoded data as a string.</returns>
        string DecodeAsciiMessage(BitArray coverData);

        /// <summary>
        /// Decodes a binary message from <paramref name="coverData"/>.
        /// </summary>
        /// <param name="coverData"></param>
        /// <returns>The decoded data as a <see cref="BitArray"/>.</returns>
        BitArray DecodeBinaryMessage(BitArray coverData);

        /// <summary>
        /// Encodes the ASCII <paramref name="message"/> into the <paramref name="coverData"/>.
        /// </summary>
        /// <param name="coverData">Data where <paramref name="message"/> should be encoded.</param>
        /// <param name="message">The ASCII message that should be encoded.</param>
        /// <returns>The encoded <paramref name="coverData"/></returns>
        BitArray EncodeAsciiMessage(BitArray coverData, string message);

        /// <summary>
        /// Encodes the binary <paramref name="message"/> into the <paramref name="coverData"/>.
        /// </summary>
        /// <param name="coverData">Data where <paramref name="message"/> should be encoded.</param>
        /// <param name="message">The binary message that should be encoded.</param>
        /// <returns>The encoded <paramref name="coverData"/></returns>
        BitArray EncodeBinaryMessage(BitArray coverData, BitArray message);
    }
}
