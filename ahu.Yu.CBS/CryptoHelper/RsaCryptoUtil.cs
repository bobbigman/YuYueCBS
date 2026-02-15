using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CryptoHelper
{
    public class RSACryptoUtil
    {
        /// <summary>
        /// 生成密钥
        /// </summary>
        /// <returns></returns>
        public static CryptoKey GenerateXmlKeys()
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                var key = new CryptoKey
                {
                    PrivateKey = rsa.ToXmlString(true),
                    PublicKey = rsa.ToXmlString(false)
                };
                return key;
            }
        }
        public static CryptoKey GeneratePkcs8Keys()
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                var keyPair = DotNetUtilities.GetRsaKeyPair(rsa);

                var key = new CryptoKey
                {
                    PrivateKey = GeneratePrivateKey(keyPair.Private),
                    PublicKey = GeneratePublicKey(keyPair.Public)
                };
                return key;
            }
        }

        readonly string privateKey;
        readonly string publicKey;
        readonly Type type;
        readonly Encoding encoding;
        public RSACryptoUtil(string privateKey, string publicKey, Type type = Type.Xml) : this(privateKey, publicKey, Encoding.UTF8, type)
        {

        }
        public RSACryptoUtil(string privateKey, string publicKey, Encoding encoding, Type type = Type.Xml) 
        {
            this.privateKey = privateKey;
            this.publicKey = publicKey;
            this.encoding = encoding;
            this.type = type;
        }

        /// <summary>
        /// 签名算法
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string Sign(string data)
        {
            var dataBytes = encoding.GetBytes(data);
            using (var rsa = new RSACryptoServiceProvider())
            {
                SetPrivateKey(rsa, privateKey);
                var signature = rsa.SignData(dataBytes, new MD5CryptoServiceProvider());
                return Convert.ToBase64String(signature);
            }
        }

        /// <summary>
        /// 签名校验
        /// </summary>
        /// <param name="data"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public bool Verify(string data, string signature)
        {
            var dataBytes = encoding.GetBytes(data);
            var signBytes = Convert.FromBase64String(signature);
            using (var rsa = new RSACryptoServiceProvider())
            {
                SetPublicKey(rsa, publicKey);
                return rsa.VerifyData(dataBytes, new MD5CryptoServiceProvider(), signBytes);
            }
        }

        /// <summary>
        /// 公钥加密，分段加密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public string Encrypt(string data)
        {
            var plainBytes = encoding.GetBytes(data);
            byte[] encryContent = null;
            using (var rsa = new RSACryptoServiceProvider())
            {
                SetPublicKey(rsa, publicKey);
                int bufferSize = (rsa.KeySize / 8) - 11;
                byte[] buffer = new byte[bufferSize];
                using (MemoryStream input = new MemoryStream(plainBytes))
                using (MemoryStream output = new MemoryStream()) 
                {
                    while (true) 
                    {
                        int readLine = input.Read(buffer, 0, bufferSize);
                        if (readLine <= 0) 
                        {
                            break;
                        }
                        byte[] temp = new byte[readLine];
                        Array.Copy(buffer, 0, temp, 0, readLine);
                        byte[] encrypt = rsa.Encrypt(temp, false);
                        output.Write(encrypt, 0, encrypt.Length);
                    }
                    encryContent = output.ToArray();
                }
                return Convert.ToBase64String(encryContent);
            }
        }

        /// <summary>
        /// 私钥加密，分段加密
        /// </summary>
        /// <param name="privateKey"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string PrivateKeyEncrypt(string data)
        {
            var plainBytes = encoding.GetBytes(data);
            byte[] encryContent = null;
            using (var rsa = new RSACryptoServiceProvider())
            {
                SetPrivateKey(rsa, privateKey);
                AsymmetricCipherKeyPair keyPair = DotNetUtilities.GetRsaKeyPair(rsa);
                IBufferedCipher c = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding");//使用RSA/ECB/PKCS1Padding格式
                c.Init(true, keyPair.Private);//第一个参数为true表示加密，为false表示解密；第二个参数表示密钥

                int bufferSize = (rsa.KeySize / 8) - 11;
                byte[] buffer = new byte[bufferSize];
                using (MemoryStream input = new MemoryStream(plainBytes))
                using (MemoryStream output = new MemoryStream())
                {
                    while (true)
                    {
                        int readLine = input.Read(buffer, 0, bufferSize);
                        if (readLine <= 0)
                        {
                            break;
                        }
                        byte[] temp = new byte[readLine];
                        Array.Copy(buffer, 0, temp, 0, readLine);
                        byte[] encrypt = c.DoFinal(temp);
                        output.Write(encrypt, 0, encrypt.Length);
                    }
                    encryContent = output.ToArray();
                }
                return Convert.ToBase64String(encryContent);
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="encryptedBytes"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public string Decrypt(string encryptedData)
        {
            var encryptedBytes = Convert.FromBase64String(encryptedData);
            byte[] dencryContent = null;
            using (var rsa = new RSACryptoServiceProvider())
            {
                SetPrivateKey(rsa, privateKey);
                int keySize = rsa.KeySize / 8;
                byte[] buffer = new byte[keySize];
                using (MemoryStream input = new MemoryStream(encryptedBytes))
                using (MemoryStream output = new MemoryStream())
                {
                    while (true)
                    {
                        int readline = input.Read(buffer, 0, keySize);
                        if (readline <= 0) 
                        {
                            break;
                        }
                        byte[] temp = new byte[readline];
                        Array.Copy(buffer, 0, temp, 0, readline);
                        byte[] decrypt = rsa.Decrypt(temp, false);
                        output.Write(decrypt, 0, decrypt.Length);
                    }
                    dencryContent = output.ToArray();
                }
                return encoding.GetString(dencryContent);
            }
        }

        /// <summary>
        /// 公钥加密，分段加密
        /// </summary>
        /// <returns></returns>
        public string PublicKeyDecrypt(string encryptedData)
        {
            var encryptedBytes = Convert.FromBase64String(encryptedData);
            byte[] dencryContent = null;
            using (var rsa = new RSACryptoServiceProvider())
            {
                SetPublicKey(rsa, publicKey);
                AsymmetricKeyParameter pbk = DotNetUtilities.GetRsaPublicKey(rsa.ExportParameters(false));
                IBufferedCipher c = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding");
                c.Init(false, pbk);

                int keySize = rsa.KeySize / 8;
                byte[] buffer = new byte[keySize];
                using (MemoryStream input = new MemoryStream(encryptedBytes))
                using (MemoryStream output = new MemoryStream())
                {
                    while (true)
                    {
                        int readline = input.Read(buffer, 0, keySize);
                        if (readline <= 0)
                        {
                            break;
                        }
                        byte[] temp = new byte[readline];
                        Array.Copy(buffer, 0, temp, 0, readline);
                        byte[] decrypt = c.DoFinal(temp);
                        output.Write(decrypt, 0, decrypt.Length);
                    }
                    dencryContent = output.ToArray();
                }
                return encoding.GetString(dencryContent);
            }
        }

        public enum Type 
        {
            Xml,
            Pkcs8
        }

        private static string GeneratePrivateKey(AsymmetricKeyParameter key)
        {
            var builder = new StringBuilder();

            using (var writer = new StringWriter(builder))
            {
                var pkcs8Gen = new Pkcs8Generator(key);
                var pemObj = pkcs8Gen.Generate();

                var pemWriter = new PemWriter(writer);
                pemWriter.WriteObject(pemObj);
            }

            return builder.ToString();
        }

        private static string GeneratePublicKey(AsymmetricKeyParameter key)
        {
            var builder = new StringBuilder();

            using (var writer = new StringWriter(builder))
            {
                var pemWriter = new PemWriter(writer);
                pemWriter.WriteObject(key);
            }

            return builder.ToString();
        }

        private static RSAParameters ParsePrivateKey(string privateKey)
        {
            using (var reader = new StringReader(privateKey))
            {
                var pemReader = new PemReader(reader);
                var key = (RsaPrivateCrtKeyParameters)pemReader.ReadObject();

                var parameter = new RSAParameters
                {
                    Modulus = key.Modulus.ToByteArrayUnsigned(),
                    Exponent = key.PublicExponent.ToByteArrayUnsigned(),
                    D = key.Exponent.ToByteArrayUnsigned(),
                    P = key.P.ToByteArrayUnsigned(),
                    Q = key.Q.ToByteArrayUnsigned(),
                    DP = key.DP.ToByteArrayUnsigned(),
                    DQ = key.DQ.ToByteArrayUnsigned(),
                    InverseQ = key.QInv.ToByteArrayUnsigned()
                };

                return parameter;
            }
        }

        private static RSAParameters ParsePublicKey(string publicKey)
        {
            using (var reader = new StringReader(publicKey))
            {
                var pemReader = new PemReader(reader);
                var key = (RsaKeyParameters)pemReader.ReadObject();

                var parameter = new RSAParameters
                {
                    Modulus = key.Modulus.ToByteArrayUnsigned(),
                    Exponent = key.Exponent.ToByteArrayUnsigned()
                };

                return parameter;
            }
        }
        private void SetPrivateKey(RSACryptoServiceProvider rsa, string privateKey) 
        {
            if (type == Type.Xml)
            {
                rsa.FromXmlString(privateKey);
            }
            else if (type == Type.Pkcs8)
            {
                var key = ParsePrivateKey(privateKey);
                rsa.ImportParameters(key);
            }
            else
            {
                throw new EncoderFallbackException("未知私钥格式");
            }
        }
        private void SetPublicKey(RSACryptoServiceProvider rsa, string publicKey)
        {
            if (type == Type.Xml)
            {
                rsa.FromXmlString(publicKey);
            }
            else if (type == Type.Pkcs8)
            {
                var key = ParsePublicKey(publicKey);
                rsa.ImportParameters(key);
            }
            else
            {
                throw new EncoderFallbackException("未知公钥格式");
            }
        }
    }

}
