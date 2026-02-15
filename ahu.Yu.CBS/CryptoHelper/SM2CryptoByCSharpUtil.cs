using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Text;

namespace CryptoHelper
{
    /// <summary>
    /// Sm2算法   
    /// 对标国际RSA算法
    /// </summary>
    public class SM2CryptoByCSharpUtil
    {
        public enum Type 
        {
            Generate,
            Pkcs8
        }

        public SM2CryptoByCSharpUtil(string privateKey, string publicKey, Type type = Type.Generate) : this(privateKey, publicKey, Encoding.UTF8, type)
        {
        
        }

        public SM2CryptoByCSharpUtil(string privateKey, string publicKey, Encoding encoding, Type type = Type.Generate)
        {
            if (type == Type.Generate)
            {
                this.privateKey = Hex.Decode(privateKey);
                this.publicKey = Hex.Decode(publicKey);
            }
            else if (type == Type.Pkcs8)
            {
                this.privateKey = ((ECPrivateKeyParameters)PrivateKeyFactory.CreateKey(Convert.FromBase64String(privateKey))).D.ToByteArray();
                this.publicKey = ((ECPublicKeyParameters)PublicKeyFactory.CreateKey(Convert.FromBase64String(publicKey))).Q.GetEncoded();
            }
            else 
            {
                throw new EncoderFallbackException("未知密钥格式");
            }
            this.encoding = encoding;
        }

        /// <summary>
        /// 公钥
        /// </summary>
        readonly byte[] publicKey;

        /// <summary>
        /// 私钥
        /// </summary>
        readonly byte[] privateKey;

        /// <summary>
        /// 编码
        /// </summary>
        readonly Encoding encoding;

        /// <summary>
        /// 获取密钥
        /// </summary>
        /// <param name="privateKey">私钥</param>
        /// <param name="publicKey">公钥</param>
        public static CryptoKey GenerateKeys(Encoding encoding = null)
        {
            if (encoding == null) 
            {
                encoding = Encoding.UTF8;
            }
            SM2 sm2 = SM2.Instance;
            AsymmetricCipherKeyPair key = sm2.EccKeyPairGenerator.GenerateKeyPair();
            ECPrivateKeyParameters ecpriv = (ECPrivateKeyParameters)key.Private;
            ECPublicKeyParameters ecpub = (ECPublicKeyParameters)key.Public;
            return new CryptoKey
            {
                PublicKey = encoding.GetString(Hex.Encode(ecpub.Q.GetEncoded())).ToUpper(),
                PrivateKey = encoding.GetString(Hex.Encode(ecpriv.D.ToByteArray())).ToUpper()
            };
        }

        #region 解密
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <returns></returns>
        public string Decrypt(string ciphertext)
        {
            return encoding.GetString(Decrypt(privateKey, Hex.Decode(ciphertext))) ;
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="privateKey"></param>
        /// <param name="encryptedData"></param>
        /// <returns></returns>
        private byte[] Decrypt(byte[] privateKey, byte[] encryptedData)
        {
            if (null == privateKey || privateKey.Length == 0)
            {
                return null;
            }
            if (encryptedData == null || encryptedData.Length == 0)
            {
                return null;
            }

            String data = encoding.GetString(Hex.Encode(encryptedData));

            byte[] c1Bytes = Hex.Decode(encoding.GetBytes(data.Substring(0, 130)));
            int c2Len = encryptedData.Length - 97;
            byte[] c2 = Hex.Decode(encoding.GetBytes(data.Substring(130, 2 * c2Len)));
            byte[] c3 = Hex.Decode(encoding.GetBytes(data.Substring(130 + 2 * c2Len, 64)));

            SM2 sm2 = SM2.Instance;
            BigInteger userD = new BigInteger(1, privateKey);

            ECPoint c1 = sm2.EccCurve.DecodePoint(c1Bytes);
            //c1.Normalize();
            Cipher cipher = new Cipher();
            cipher.InitDec(userD, c1);
            cipher.Decrypt(c2);
            cipher.Dofinal(c3);

            return c2;
        }
        #endregion

        #region 加密
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string Encrypt(string plainTxt)
        {
            var dataBytes = encoding.GetBytes(plainTxt);
            return Encrypt(publicKey, dataBytes);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private string Encrypt(byte[] publicKey, byte[] data)
        {
            if (null == publicKey || publicKey.Length == 0)
            {
                return null;
            }
            if (data == null || data.Length == 0)
            {
                return null;
            }

            byte[] source = new byte[data.Length];
            Array.Copy(data, 0, source, 0, data.Length);

            Cipher cipher = new Cipher();
            SM2 sm2 = SM2.Instance;

            ECPoint userKey = sm2.EccCurve.DecodePoint(publicKey);
            //userKey.Normalize();
            ECPoint c1 = cipher.InitEnc(sm2, userKey);
            cipher.Encrypt(source);

            byte[] c3 = new byte[32];
            cipher.Dofinal(c3);

            String sc1 = encoding.GetString(Hex.Encode(c1.GetEncoded()));
            String sc2 = encoding.GetString(Hex.Encode(source));
            String sc3 = encoding.GetString(Hex.Encode(c3));

            return (sc1 + sc2 + sc3).ToUpper();
        }
        #endregion

    }

    /// <summary>
    /// 加密处理中心
    /// </summary>
    public class SM2
    {
        public static SM2 Instance
        {
            get
            {
                return new SM2();
            }

        }
        public static SM2 InstanceTest
        {
            get
            {
                return new SM2();
            }

        }

        #region 曲线参数
        /// <summary>
        /// 曲线参数
        /// </summary>
        public static readonly string[] CurveParameter = {
            "FFFFFFFEFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FFFFFFFFFFFFFFFF",// p,0
            "FFFFFFFEFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FFFFFFFFFFFFFFFC",// a,1
            "28E9FA9E9D9F5E344D5A9E4BCF6509A7F39789F515AB8F92DDBCBD414D940E93",// b,2
            "FFFFFFFEFFFFFFFFFFFFFFFFFFFFFFFF7203DF6B21C6052B53BBF40939D54123",// n,3
            "32C4AE2C1F1981195F9904466A39C9948FE30BBFF2660BE1715A4589334C74C7",// gx,4
            "BC3736A2F4F6779C59BDCEE36B692153D0A9877CC62A474002DF32E52139F0A0" // gy,5
        };
        /// <summary>
        /// 椭圆曲线参数
        /// </summary>
        public string[] EccParam = CurveParameter;
        /// <summary>
        /// 椭圆曲线参数P
        /// </summary>
        public readonly BigInteger EccP;
        /// <summary>
        /// 椭圆曲线参数A
        /// </summary>
        public readonly BigInteger EccA;
        /// <summary>
        /// 椭圆曲线参数B
        /// </summary>
        public readonly BigInteger EccB;
        /// <summary>
        /// 椭圆曲线参数N
        /// </summary>
        public readonly BigInteger EccN;
        /// <summary>
        /// 椭圆曲线参数Gx
        /// </summary>
        public readonly BigInteger EccGx;
        /// <summary>
        /// 椭圆曲线参数Gy
        /// </summary>
        public readonly BigInteger EccGy;
        #endregion
        /// <summary>
        /// 椭圆曲线
        /// </summary>
        public readonly ECCurve EccCurve;
        /// <summary>
        /// 椭圆曲线的点G
        /// </summary>
        public readonly ECPoint EccPointG;
        /// <summary>
        /// 椭圆曲线 bc规范
        /// </summary>
        public readonly ECDomainParameters EccBcSpec;
        /// <summary>
        /// 椭圆曲线密钥对生成器
        /// </summary>
        public readonly ECKeyPairGenerator EccKeyPairGenerator;

        private SM2()
        {
            EccParam = CurveParameter;
            EccP = new BigInteger(EccParam[0], 16);
            EccA = new BigInteger(EccParam[1], 16);
            EccB = new BigInteger(EccParam[2], 16);
            EccN = new BigInteger(EccParam[3], 16);
            EccGx = new BigInteger(EccParam[4], 16);
            EccGy = new BigInteger(EccParam[5], 16);

            // 1. 创建曲线（FpCurve 会内部创建 FiniteField）
            EccCurve = new FpCurve(EccP, EccA, EccB);

            // 2. 使用 curve 创建基点坐标（✅ 正确方式）
            ECFieldElement ecc_gx_fieldelement = EccCurve.FromBigInteger(EccGx);
            ECFieldElement ecc_gy_fieldelement = EccCurve.FromBigInteger(EccGy);

            // 3. 创建基点 G
            //EccPointG = new FpPoint(EccCurve, ecc_gx_fieldelement, ecc_gy_fieldelement);
            EccPointG = EccCurve.CreatePoint(EccGx, EccGy);

            // 4. 创建域参数
            EccBcSpec = new ECDomainParameters(EccCurve, EccPointG, EccN);

            // 5. 初始化密钥生成器
            ECKeyGenerationParameters ecc_ecgenparam = new ECKeyGenerationParameters(EccBcSpec, new SecureRandom());
            EccKeyPairGenerator = new ECKeyPairGenerator();
            EccKeyPairGenerator.Init(ecc_ecgenparam);
        }

        /// <summary>
        /// 获取杂凑值H
        /// </summary>
        /// <param name="z">Z值</param>
        /// <param name="data">待签名消息</param>
        /// <returns></returns>
        public virtual byte[] Sm2GetH(byte[] z, byte[] data)
        {
            SM3Digest sm3 = new SM3Digest();
            //Z
            sm3.BlockUpdate(z, 0, z.Length);

            //待签名消息
            sm3.BlockUpdate(data, 0, data.Length);

            // H
            byte[] md = new byte[sm3.GetDigestSize()];
            sm3.DoFinal(md, 0);

            return md;
        }

        /// <summary>
        /// 获取Z值
        /// Z=SM3(ENTL∣∣userId∣∣a∣∣b∣∣gx∣∣gy ∣∣x∣∣y) 
        /// </summary>
        /// <param name="userId">签名方的用户身份标识</param>
        /// <param name="userKey">签名方公钥</param>
        /// <returns></returns>
        public virtual byte[] Sm2GetZ(byte[] userId, ECPoint userKey)
        {
            SM3Digest sm3 = new SM3Digest();
            byte[] p;
            // ENTL由2个字节标识的ID的比特长度 
            int len = userId.Length * 8;
            sm3.Update((byte)(len >> 8 & 0x00ff));
            sm3.Update((byte)(len & 0x00ff));

            // userId用户身份标识ID
            sm3.BlockUpdate(userId, 0, userId.Length);

            // a,b为系统曲线参数；
            p = EccA.ToByteArray();
            sm3.BlockUpdate(p, 0, p.Length);
            p = EccB.ToByteArray();
            sm3.BlockUpdate(p, 0, p.Length);
            //  gx、gy为基点
            p = EccGx.ToByteArray();
            sm3.BlockUpdate(p, 0, p.Length);
            p = EccGy.ToByteArray();
            sm3.BlockUpdate(p, 0, p.Length);

            // x,y用户的公钥的X和Y
            p = userKey.Normalize().XCoord.ToBigInteger().ToByteArray();
            sm3.BlockUpdate(p, 0, p.Length);
            p = userKey.Normalize().YCoord.ToBigInteger().ToByteArray();
            sm3.BlockUpdate(p, 0, p.Length);

            // Z
            byte[] md = new byte[sm3.GetDigestSize()];
            sm3.DoFinal(md, 0);

            return md;
        }
    }

    /// <summary>
    /// 密码计算
    /// </summary>
    public class Cipher
    {
        private int ct = 1;

        /// <summary>
        /// 椭圆曲线E上点P2
        /// </summary>
        private ECPoint p2;
        private SM3Digest sm3keybase;
        private SM3Digest sm3c3;


        private readonly byte[] key = new byte[32];
        private byte keyOff = 0;


        public Cipher()
        {
        }


        private void Reset()
        {
            sm3keybase = new SM3Digest();
            sm3c3 = new SM3Digest();


            byte[] p;

            p = p2.Normalize().XCoord.ToBigInteger().ToByteArray();
            sm3keybase.BlockUpdate(p, 0, p.Length);
            sm3c3.BlockUpdate(p, 0, p.Length);


            p = p2.Normalize().YCoord.ToBigInteger().ToByteArray();
            sm3keybase.BlockUpdate(p, 0, p.Length);


            ct = 1;
            NextKey();
        }


        private void NextKey()
        {
            SM3Digest sm3keycur = new SM3Digest(sm3keybase);
            sm3keycur.Update((byte)(ct >> 24 & 0x00ff));
            sm3keycur.Update((byte)(ct >> 16 & 0x00ff));
            sm3keycur.Update((byte)(ct >> 8 & 0x00ff));
            sm3keycur.Update((byte)(ct & 0x00ff));
            sm3keycur.DoFinal(key, 0);
            keyOff = 0;
            ct++;
        }


        public virtual ECPoint InitEnc(SM2 sm2, ECPoint userKey)
        {
            AsymmetricCipherKeyPair key = sm2.EccKeyPairGenerator.GenerateKeyPair();
            ECPrivateKeyParameters ecpriv = (ECPrivateKeyParameters)key.Private;
            ECPublicKeyParameters ecpub = (ECPublicKeyParameters)key.Public;
            BigInteger k = ecpriv.D;
            ECPoint c1 = ecpub.Q;


            p2 = userKey.Multiply(k);
            Reset();


            return c1;
        }


        public virtual void Encrypt(byte[] data)
        {
            //p2.Normalize();
            sm3c3.BlockUpdate(data, 0, data.Length);
            for (int i = 0; i < data.Length; i++)
            {
                if (keyOff == key.Length)
                    NextKey();


                data[i] ^= key[keyOff++];
            }
        }


        public virtual void InitDec(BigInteger userD, ECPoint c1)
        {
            p2 = c1.Multiply(userD);
            Reset();
        }


        public virtual void Decrypt(byte[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                if (keyOff == key.Length)
                    NextKey();


                data[i] ^= key[keyOff++];
            }
            sm3c3.BlockUpdate(data, 0, data.Length);
        }

        public virtual void Dofinal(byte[] c3)
        {
            byte[] p = p2.Normalize().YCoord.ToBigInteger().ToByteArray();
            sm3c3.BlockUpdate(p, 0, p.Length);
            sm3c3.DoFinal(c3, 0);
            Reset();
        }

    }
}
