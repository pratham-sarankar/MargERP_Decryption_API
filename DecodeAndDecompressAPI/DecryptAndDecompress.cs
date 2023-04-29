using System.IO.Compression;
using System.Security.Cryptography;  
using System.Text;

namespace DecodeAndDecompressAPI
{
    public class DecryptAndDecompress
    {
        public static string Decrypt(string data, string key)
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged(); 
            rijndaelCipher.Mode = CipherMode.CBC;                                  
            rijndaelCipher.Padding = PaddingMode.PKCS7;
            rijndaelCipher.KeySize = 0x80;
            rijndaelCipher.BlockSize = 0x80;
            byte[] encryptedData = null;
            encryptedData = Convert.FromBase64String(data);

            byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyBytes = new byte[0x10];
            int len = pwdBytes.Length;
            if (len > keyBytes.Length)
            {
                len = keyBytes.Length;
            }
            Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            rijndaelCipher.IV = keyBytes;
            byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock
                        (encryptedData, 0, encryptedData.Length);

            return Encoding.UTF8.GetString(plainText);
        }

        public static string Decompress(string compressedstring)
        {
            string strResult = "";
            byte[] input = Convert.FromBase64String(compressedstring);
            using (MemoryStream inputStream = new MemoryStream(input))
            {
                using (DeflateStream gzip = new DeflateStream(inputStream, CompressionMode.Decompress))
                {
                    using (StreamReader reader = new StreamReader(gzip, System.Text.Encoding.UTF8))
                    {
                        strResult = reader.ReadToEnd();
                    }
                }
            }
            return strResult;
        }
    }
}