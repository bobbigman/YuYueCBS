using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CryptoHelper
{
    public class SHA1CryptoUtil
    {
        public static string Encrypt(string plainTxt, Encoding encoding)
        {
            var plainBytes = encoding.GetBytes(plainTxt);
            return Encrypt(plainBytes);
        }
        public static string Encrypt(byte[] plainBytes)
        {
            using (var sha1 = new SHA1CryptoServiceProvider())
            //using (var sha1 = SHA1.Create())
            {
                var encryptedBytes = sha1.ComputeHash(plainBytes);
                var sb = new StringBuilder();
                foreach (var b in encryptedBytes)
                {
                    sb.Append(b.ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
