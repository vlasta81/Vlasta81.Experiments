using System.Text;

namespace ConfigurationStorage
{
    /// <summary>
    /// Provides methods for encoding and decoding Base64 strings.
    /// </summary>
    internal static class Base64
    {
        /// <summary>
        /// Encodes the given plain text string to a Base64 encoded string.
        /// </summary>
        /// <param name="plainText">The plain text string to encode.</param>
        /// <returns>A Base64 encoded string.</returns>
        public static string Encode(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Encodes the given byte array to a Base64 encoded string.
        /// </summary>
        /// <param name="byteText">The byte array to encode.</param>
        /// <returns>A Base64 encoded string.</returns>
        public static string Encode(byte[] byteText)
        {
            return Convert.ToBase64String(byteText);
        }

        /// <summary>
        /// Decodes the given Base64 encoded string to a plain text string.
        /// </summary>
        /// <param name="base64EncodedData">The Base64 encoded string to decode.</param>
        /// <returns>A plain text string.</returns>
        public static string Decode(string base64EncodedData)
        {
            byte[] base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
