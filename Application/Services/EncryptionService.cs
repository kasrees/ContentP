using System.Security.Cryptography;
using System.Text;

namespace Application.Services
{
    public class EncryptionService
    {
        //https://docs.microsoft.com/ru-ru/dotnet/api/system.security.cryptography.aesmanaged.createencryptor?view=net-6.0
        //https://docs.microsoft.com/ru-ru/dotnet/standard/security/encrypting-data
        public static string AesEncryptString(string key, string text)
        {
            byte[] encrypted;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = new byte[16];
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(encrypted);
        }
    }
}
