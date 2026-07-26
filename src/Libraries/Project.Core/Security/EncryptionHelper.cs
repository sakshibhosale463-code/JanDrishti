using System.Security.Cryptography;
using System.Text;

namespace Project.Core.Security
{
    public class EncryptionHelper
    {
        private static readonly byte[] key = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes("YourSuperSecretKeyForAES256!!")); // Ensures a 32-byte key
        private static readonly byte[] iv = new byte[16];

        public static string EncryptPassword(string plainText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    byte[] inputBytes = Encoding.UTF8.GetBytes(plainText);
                    cs.Write(inputBytes, 0, inputBytes.Length);
                    cs.FlushFinalBlock();
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static string DecryptPassword(string encryptedText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(encryptedText)))
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    byte[] outputBytes = new byte[ms.Length];
                    int bytesRead = cs.Read(outputBytes, 0, outputBytes.Length);
                    return Encoding.UTF8.GetString(outputBytes, 0, bytesRead);
                }
            }
        }
    }
}
