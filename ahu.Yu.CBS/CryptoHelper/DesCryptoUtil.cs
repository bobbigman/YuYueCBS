using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CryptoHelper
{
    /// <summary>
    /// 数据加密标准 - Data Encryption Standard（DES）
    /// </summary>
    public class DesCryptoUtil
    {
        readonly byte[] key;
        readonly byte[] iv;
        readonly Encoding encoding;
        public DesCryptoUtil(string key, string iv) : this(key, iv, Encoding.UTF8)
        {
        }
        public DesCryptoUtil(string key, string iv, Encoding encoding) 
        {
            this.key = Convert.FromBase64String(key);
            this.iv = Convert.FromBase64String(iv);
            this.encoding = encoding;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="plainTxt">要加密文字</param>
        /// <returns></returns>
        public string Encrypt(string plainTxt) 
        {
            using (var des = DES.Create("DES"))
            using (var memoryStream = new MemoryStream())
            using (var cryptoStream = new CryptoStream(memoryStream, des.CreateEncryptor(key, iv), CryptoStreamMode.Write))
            {
                var plainBytes = encoding.GetBytes(plainTxt);
                cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                cryptoStream.FlushFinalBlock();
                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="encrypted">密文，</param>
        /// <returns></returns>
        public string Decrypt(string encrypted)
        {
            using (var des = DES.Create("DES"))
            using (var mStream = new MemoryStream(Convert.FromBase64String(encrypted)))
            using (var cryptoStream = new CryptoStream(mStream, des.CreateDecryptor(key, iv), CryptoStreamMode.Read))
            using (var reader = new StreamReader(cryptoStream, encoding))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
