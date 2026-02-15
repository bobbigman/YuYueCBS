using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ahu.YuYue.CBS 
{
    public class ClsMESHttp
    {
        [Kingdee.BOS.Util.HotUpdate]

        int mint;
        public string LookUp(string pURL, string pJson)
        {
            string strReturn = "";
            string strURL = pURL;

            try
            {
                HttpWebRequest HttpWebRequest1 = (HttpWebRequest)WebRequest.Create(pURL);
                HttpWebRequest1.Method = "POST";
                HttpWebRequest1.ContentType = "application/json";

                // 添加头部信息  
                HttpWebRequest1.Headers.Add("Accept-Language", "zh-CHS");
                HttpWebRequest1.Headers.Add("X-ECC-Current-Tenant", "10000");
                HttpWebRequest1.Headers.Add("Authorization", "Bearer c4ad2142-bf90-ea17-ad83-9d3be0a5a97b"); //更正

                // 将 JSON 数据写入请求流  
                byte[] byteArray = Encoding.UTF8.GetBytes(pJson);
                Stream dataStream = HttpWebRequest1.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                //HttpWebRequest1.Timeout = 300000;

                HttpWebResponse response = (HttpWebResponse)HttpWebRequest1.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                strReturn = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

                string strFind = "timeout";
                string strFind2 = "Read timed out";
                if (strReturn.IndexOf(strFind) != -1 || strReturn.IndexOf(strFind2) != -1)
                {
                    //允许timeout 3次，够意思吧？

                    if (mint > 3)
                        return strFind;

                    mint = mint + 1;
                    strReturn = LookUp(strURL, pJson);

                }

            }

            catch (Exception ex)
            {
                //允许试错3次，够意思吧？
                //if (mint > 3)
                    throw ex;

                //mint = mint + 1;
                //strReturn = LookUp(strURL);

            }
            return strReturn;
        }




    }
}