using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CryptoHelper
{
    /// <summary>
    /// Message-Digest Algorithm 5
    /// MD5即Message-Digest Algorithm 5（信息-摘要算法5），用于确保信息传输完整一致。是计算机广泛使用的杂凑算法之一（又译摘要算法、哈希算法），主流编程语言普遍已有MD5实现。将数据（如汉字）运算为另一固定长度值，是杂凑算法的基础原理，MD5的前身有MD2、MD3和MD4
    /// </summary>
    public class Md5CryptoUtil
    {
        public static string Encrypt(string plainTxt, Encoding encoding) 
        {
            var plainBytes = encoding.GetBytes(plainTxt);
            return Encrypt(plainBytes);
        }
        public static string Encrypt(byte[] plainBytes)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            // using (var md5 = MD5.Create())
            {
                var encryptedBytes = md5.ComputeHash(plainBytes);
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
