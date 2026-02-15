using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using CryptoHelper;
using RequestDemo;
using Newtonsoft.Json;
using System.Net;
using Kingdee.BOS;
using Kingdee.BOS.WebApi.Client;

namespace ahu.YuYue.CBS 
{
    public class CsCallCBS
    {


        //下面，不要替换，会报错：交互异常。2024/7/3 11:15
        /// </summary>
        /// <param name="args"></param>
        //
        //static RequestUnitil requestUnitil = new RequestUnitil();
        //static string domainName = "http://cbs8-openapi-reprd.csuat.cmburl.cn";

        ////企业ERP系统通过招行CBS8发起银行支付指令
        //static string balanceUrl = "/openapi/payment/openapi/v1/payment-apply-common";

        ////// 客户私钥，请替换实际客户私钥
        //const string customPrivateKey = @"c6c45e87654fdfa5fa6751a8f531b42d1037b3dbaeca0828d3ef78cb20bcd51b";
        //// 平台公钥，请替换实际平台公钥
        //const string platformPublicKey = @"049B99962CC36BA537010FD962375A87CEC1B7F9B0B277E9419547BEB9510222FE04B215DE73BB2B2D4F44C77D53E4BDBB443D4393D7CE91E134451B4D63CF675E";

        static RequestUnitil mRequestUnitil = new RequestUnitil();

        //public string MainBak(Struct_K3LoginInfo pStruct_K3LoginInfo,string pPartURL, string pJson,ref string pJsonResult
        //   ,ref string pError)
        //{
        //    //string strFullUrl = pStruct_K3LoginInfo.ServerURL_Others + pStruct_K3LoginInfo.Token;
        //    string strToken = CsCallCBS.getToken(pStruct_K3LoginInfo,ref pError);
        //    if (pError != "")
        //        return "";

        //    StringBuilder sb1 = new StringBuilder();

        //    string strFullUrl = pStruct_K3LoginInfo.ServerURL_Others + pPartURL;
        //    CsCallCBS.setupRequest(sb1, strToken, pJson, strFullUrl
        //        , pStruct_K3LoginInfo.PlatformPublicKey
        //        , pStruct_K3LoginInfo.CustomPrivateKey
        //        ,ref pJsonResult);

        //    //Only for test
        //    //CsCallCBS.setupRequest(sb1, strToken, pJson, strFullUrl, platformPublicKey,
        //    //   customPrivateKey);

        //    return sb1.ToString();
        //}

        public void CallCBSWithToken(Struct_K3LoginInfo pStruct_K3LoginInfo, string pPartURL, string pJson, ref string pJsonResult
   , ref string pError,string pToken)
        {

            string strFullUrl = pStruct_K3LoginInfo.ServerURL_Others + pPartURL;

            pError = CsCallCBS.setupRequest(pToken,pJson, strFullUrl
                , pStruct_K3LoginInfo.PlatformPublicKey
                , pStruct_K3LoginInfo.CustomPrivateKey
                , ref pJsonResult);

            //Only for test
            //CsCallCBS.setupRequest(sb1, strToken, pJson, strFullUrl, platformPublicKey,
            //   customPrivateKey);

            return ;
        }

        public static string getToken( Context pContext, K3CloudApiClient pK3CloudApiClient,int p2DatabaseType
                 , Struct_K3LoginInfo pStruct_K3LoginInfo,ref string pError)
        {
            //string requestBodyForGetToken = "{\"app_id\":\"Y9Gc0iyE\",\"app_secret\":\"fa22cb55c8db924b51dcba8bb442f139fa6b4b55\",\"grant_type\":\"client_credentials\"}";
            // 创建一个字典  

            if (pStruct_K3LoginInfo.LoginK3Ok == false || pStruct_K3LoginInfo.AcctID == null || pK3CloudApiClient == null)
            {

                string strReturnType="";
                bool bolNeedLogInK3 = false;
                int IntReturn = ClsPublic.LogIn(ref pStruct_K3LoginInfo, pContext
                    , ref pK3CloudApiClient, ref strReturnType, bolNeedLogInK3, p2DatabaseType);
                if (IntReturn != 1)
                    throw new Exception(strReturnType);
            }

            Dictionary<string, string> dict = new Dictionary<string, string>
        {
            { "app_id", pStruct_K3LoginInfo.AppId },
            { "app_secret", pStruct_K3LoginInfo.AppSecret },
            { "grant_type", "client_credentials" }
        };
            // 将字典转换为JSON字符串  
            string requestBodyForGetToken = JsonConvert.SerializeObject(dict);

            //requestBodyForGetToken = "{\"app_id\":\"YUduXtUm\",\"app_secret\":\"95dcab45c47455c1912dc584f95ffe94378e56f6\",\"grant_type\":\"client_credentials\"}";


            string outString = string.Empty;
            string strFullName = pStruct_K3LoginInfo.ServerURL_Others + pStruct_K3LoginInfo.Token;
            mRequestUnitil.HttpPost(strFullName, requestBodyForGetToken, new List<RequestHeaders>(), out outString);
            if (outString.IndexOf("远程服务器")>-1  || outString.IndexOf("data")==-1)
            {
                pError = outString+"*";
                return "";
            }

            JObject jobject = JObject.Parse(outString);
            return jobject["data"]["token"].ToString();
        }

        //2024/7/15 记得删除。
        //不能删除，调试时要用。2024/8/22
        public static string GetTokenBak(string fullUrl, ref string errorMessage
                                     , string appId, string appSecret)
        {
            // 使用参数构建请求体，替代硬编码的app_id和app_secret
            string requestBodyForGetToken = $"{{\"app_id\":\"{appId}\",\"app_secret\":\"{appSecret}\",\"grant_type\":\"client_credentials\"}}";
            string responseContent = string.Empty;

            // 调用HTTP工具类发送请求
            mRequestUnitil.HttpPost(fullUrl, requestBodyForGetToken, new List<RequestHeaders>(), out responseContent);

            // 检查是否包含远程服务器错误信息
            if (responseContent.IndexOf("远程服务器") > -1)
            {
                errorMessage = responseContent + "*";
                return string.Empty;
            }

            // 解析响应并返回token
            JObject responseJson = JObject.Parse(responseContent);
            return responseJson["data"]["token"]?.ToString() ?? string.Empty;
        }


        public static string setupRequest(string pToken, string pJson
            , string pFullUrl, string pPlatformPublicKey, string pCustomPrivateKey
            ,ref string pJsonResut
            )
        {

            StringBuilder sbLog_Temp=new StringBuilder();

            // 时间戳精确到毫秒，考虑时区，需与服务器时间保持一致
            string strTimeStamp = mRequestUnitil.GetTimeStamp().Split('.')[0];


            // 签名对象
            string signData = pJson + "&timestamp=" + strTimeStamp;

            // 使用C1C3C2方式
            var sm2CryptoUtil = new SM2CryptoUtil(pPlatformPublicKey, pCustomPrivateKey, CryptoHelper.SM2CryptoUtil.Mode.C1C3C2);

            // 报文使用UTF8编码，进行加密
            byte[] encryptedData = sm2CryptoUtil.Encrypt(Encoding.UTF8.GetBytes(pJson));

            List<RequestHeaders> listRequestHeaders = new List<RequestHeaders>();

            string strId = "1234567812345678"; //签名时的随机数
            string strHeaderSIGN = Convert.ToBase64String(sm2CryptoUtil.Sign(Encoding.UTF8.GetBytes(signData), Encoding.UTF8.GetBytes(strId)));
            listRequestHeaders.Add(new RequestHeaders { Key = "X-MBCLOUD-API-SIGN", Value = strHeaderSIGN });

            // 携带此字段时，无需对报文进行加解密
            //requestHeadersList.Add(new RequestHeaders { Key = "X-MBCLOUD-ENCRYPTION-ENABLED", Value = "false" });

            listRequestHeaders.Add(new RequestHeaders { Key = "X-MBCLOUD-TIMESTAMP", Value = strTimeStamp });
            listRequestHeaders.Add(new RequestHeaders { Key = "Authorization", Value = "Bearer " + pToken });

            byte[] reslut=null;

            //int ret = mRequestUnitil.HttpPostForBytes(pFullUrl, encryptedData, listRequestHeaders, out reslut);

            int ret = 0;

            try
            {
                // 第一次尝试
                ret  = mRequestUnitil.HttpPostForBytes(pFullUrl, encryptedData, listRequestHeaders, out reslut);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("操作超时"))
                {
                    sbLog_Temp.AppendLine("首次请求超时，等待1秒后重试...");
                    System.Threading.Thread.Sleep(1000);

                    try
                    {
                        // 重试一次
                        ret = mRequestUnitil.HttpPostForBytes(pFullUrl, encryptedData, listRequestHeaders, out reslut);
                    }
                    catch (Exception ex2)
                    {
                        // 重试失败，记录最终错误
                        string strError = ex2.Message;
                        return strError;
                    }
                }
                else
                {
                    // 非超时异常，不重试
                    string strError = ex.Message;
                    return strError ;
                }
            }



            if (ret != 0)
            {
                sbLog_Temp.AppendLine("交互异常");
                //Console.WriteLine("交互异常");
                //Console.ReadKey();

                return "";
            }

            string strReslutStr = string.Empty;

            try
            {
                byte[] reslutByte = sm2CryptoUtil.Decrypt(reslut);

                strReslutStr = Encoding.UTF8.GetString(reslutByte);
            }
            catch (Exception)
            {
                //如果网关异常，会导致数据没加密，所以会解密异常
                //所以可以不用解密，直接转字符串即可
                //也可自行处理异常
                strReslutStr = Encoding.UTF8.GetString(reslut);
            }

            sbLog_Temp.AppendLine("请求网址");
            sbLog_Temp.AppendLine(pFullUrl);

            //sb1.AppendLine("");
            //sb1.AppendLine("请求头信息");
            //foreach (var v1 in listRequestHeaders)
            //{
            //    sb1.AppendLine(v1.Key + " = " + v1.Value);
            //}

            string str1Char = pJson.Substring(0, 1);
            if (str1Char == "[")
            {
                JArray jaRequestData = JArray.Parse(pJson);
                pJson = jaRequestData.ToString();
            }
            else if (str1Char == "{")
            {
                JObject joRequestData = JObject.Parse(pJson);
                pJson = joRequestData.ToString();
            }

            sbLog_Temp.AppendLine("");
            sbLog_Temp.AppendLine("请求报文");
            sbLog_Temp.AppendLine(pJson);

            sbLog_Temp.AppendLine("");
            sbLog_Temp.AppendLine("CBS报文返回结果");
            JObject joReslut = (JObject)JsonConvert.DeserializeObject(strReslutStr);

            pJsonResut = joReslut.ToString();
            sbLog_Temp.AppendLine(pJsonResut);

            sbLog_Temp.AppendLine("");
            sbLog_Temp.AppendLine("老胡我开始分析结果了哈....");

            if (pFullUrl.IndexOf("query") >-1)
            {
                string strMsg = Convert.ToString(joReslut["msg"]);
                string strData = Convert.ToString(joReslut["data"]);
                if (strData=="")
                {
                    sbLog_Temp.AppendLine("分析中断，因为data为空。");
                  

                }
                else
                {
                    JObject joRoot = joReslut.Value<JObject>("data");
                    int intTotal = (int)joRoot["total"];

                    if (strMsg == "ok")
                    {
                        sbLog_Temp.AppendLine("查询成功，共取到：" + intTotal + " 单据。");
                    }
                    else
                    {
                        sbLog_Temp.AppendLine("查询失败，" + strMsg);
                    }
                }



            }
            else if (pFullUrl.IndexOf("payment-apply-common")>-1)
            {
                JArray rootJA = joReslut.Value<JArray>("data");
                string errorMsg = "";
                foreach (JObject itemJ in rootJA)
                {
                    errorMsg = (string)itemJ["errorMsg"];
                    if ((Boolean)itemJ["successed"])
                    {
                        sbLog_Temp.AppendLine("支付成功");
                        //Console.WriteLine("支付成功", "支付成功");
                    }
                    else
                    {
                        sbLog_Temp.AppendLine("支付失败, 单据编号 " + "ceshi001" + ":" + errorMsg);
                        //Console.WriteLine("支付失败", "单据编号 " + "ceshi001" + ":" + errorMsg);
                    }
                }
            }


            return "";


            //sb1.AppendLine("");

            //foreach (var e in reslutStrjs)
            //{
            //    sb1.AppendLine(e.ToString());
            //    //Console.WriteLine(e);
            //}

        }


    }
}
