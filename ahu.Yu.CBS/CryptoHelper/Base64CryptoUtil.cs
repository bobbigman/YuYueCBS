using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoHelper
{
    public class Base64CryptoUtil
    {
        public static string Encrypt(byte[] plainBytes)
        {
            return Convert.ToBase64String(plainBytes);
        }
        public static byte[] Decrypt(string encrypted)
        {
            return Convert.FromBase64String(encrypted);
        }
        public static string Encrypt(string plainText, Encoding encoding) 
        {
            return Convert.ToBase64String(encoding.GetBytes(plainText));
        }
        public static string Decrypt(string encrypted, Encoding encoding) 
        {
            return encoding.GetString(Convert.FromBase64String(encrypted));
        }
    }
}
