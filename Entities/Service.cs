using System.Security.Cryptography;
using System.Text;

namespace SecureSoftware.Entities
{
    public class Service
    {
        protected Secret Secret { get; set; }

        public Service()
        {
            Secret = new Secret();
        }

        public async Task<string> EncryptStringAsync(string plainText)
        {
            byte[] key = Secret.EncryptKey;
            byte[] iv = Secret.EncryptIV;

            return await Task.Run(() =>
            {
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = key;
                    aesAlg.IV = iv;

                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(plainText);
                            }
                        }

                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            });
        }

        public async Task<string> DecryptStringAsync(string cipherText)
        {
            byte[] key = Secret.EncryptKey;
            byte[] iv = Secret.EncryptIV;

            return await Task.Run(() =>
            {
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = key;
                    aesAlg.IV = iv;

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                return srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            });
        }

        public async Task<string> HashAsync(string input)
        {
            StringBuilder builder = new StringBuilder();

            return await Task.Run(() =>
            {
                using (var sha256 = SHA256.Create())
                {
                    byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                    
                    for (int i = 0; i < hashBytes.Length; i++)
                        builder.Append(hashBytes[i].ToString("x2"));

                    return builder.ToString();
                }
            });

        }

    }
}
