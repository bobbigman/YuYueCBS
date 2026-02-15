using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CryptoHelper
{
    public class AesCryptoUtil
    {
        readonly byte[] key;
        readonly byte[] iv;
        readonly Encoding encoding;
        readonly PaddingMode padding;
        readonly CipherMode cipher;
        public AesCryptoUtil(string key, string iv) : this(key, iv, CipherMode.CBC, PaddingMode.PKCS7, Encoding.UTF8)
        {
        }
        public AesCryptoUtil(string key, string iv, CipherMode cipher, PaddingMode padding, Encoding encoding)
        {
            this.key = Convert.FromBase64String(key);
            this.iv = Convert.FromBase64String(iv);
            this.encoding = encoding;
            this.cipher = cipher;
            this.padding = padding;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="plainTxt">要加密文字</param>
        /// <returns></returns>
        public string Encrypt(string plainTxt)
        {
            using (var aes = Aes.Create()) {
                aes.Mode = cipher;
                aes.Padding = padding;
                using (var memoryStream = new MemoryStream())
                using (var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(key, iv), CryptoStreamMode.Write))
                {
                    var plainBytes = encoding.GetBytes(plainTxt);
                    cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="encrypted">密文，</param>
        /// <returns></returns>
        public string Decrypt(string encrypted)
        {
            using (var aes = Aes.Create())
            {
                aes.Mode = cipher;
                aes.Padding = padding;
                using (var mStream = new MemoryStream(Convert.FromBase64String(encrypted)))
                using (var cryptoStream = new CryptoStream(mStream, aes.CreateDecryptor(key, iv), CryptoStreamMode.Read))
                using (var reader = new StreamReader(cryptoStream, encoding))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
