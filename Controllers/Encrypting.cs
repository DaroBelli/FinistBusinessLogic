using System.Security.Cryptography;
using System.Text;

namespace FinistBusinessLogic.Controllers
{
    public static class Encrypting
    {
        /// <summary>
        /// Зашифровывает слово по ключу из appsettings.json.
        /// </summary>
        /// <param name="value">Слово, которое подлежит зашифровке.</param>
        /// <returns></returns>
        public static string Encrypt(string value)
        {
            string? EncryptionKey = ConfigJSON.GetConfig().GetSection("EncryptionKey").Value;
            EncryptionKey ??= "";
            byte[] clearBytes = Encoding.Unicode.GetBytes(value);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using MemoryStream ms = new();
                using (CryptoStream cs = new(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                value = Convert.ToBase64String(ms.ToArray());
            }
            return value;
        }
    }
}
