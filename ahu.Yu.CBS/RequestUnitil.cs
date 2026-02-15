using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace RequestDemo
{
    public class RequestUnitil
    {
        public int HttpGet(string url, out string reslut)
        {
            reslut = "";
            try
            {
                HttpWebRequest wbRequest = (HttpWebRequest)WebRequest.Create(url);
                wbRequest.Proxy = null;
                wbRequest.Method = "GET";
                HttpWebResponse wbResponse = (HttpWebResponse)wbRequest.GetResponse();
                using (Stream responseStream = wbResponse.GetResponseStream())
                {
                    using (StreamReader sReader = new StreamReader(responseStream))
                    {
                        reslut = sReader.ReadToEnd();
                    }
                }
            }
            catch (Exception e)
            {
                reslut = e.Message;     //输出捕获到的异常，用OUT关键字输出
                return -1;              //出现异常，函数的返回值为-1
            }
            return 0;
        }


        public int HttpPost(string url, string sendData, List<RequestHeaders> requestHeadersList, out string reslut)
        {
            reslut = "";
            try
            {
                byte[] data = System.Text.Encoding.UTF8.GetBytes(sendData);
                HttpWebRequest wbRequest = (HttpWebRequest)WebRequest.Create(url);  // 制备web请求
                wbRequest.Proxy = null;     //现场测试注释掉也可以上传
                wbRequest.Method = "POST";
                wbRequest.ContentType = "application/json";
                wbRequest.ContentLength = data.Length;
                foreach (var requestHeaders in requestHeadersList)
                {
                    wbRequest.Headers.Add(requestHeaders.Key, requestHeaders.Value);
                }


                //#region //【1】获得请求流，OK
                //Stream newStream = wbRequest.GetRequestStream();
                //newStream.Write(data, 0, data.Length);
                //newStream.Close();//关闭流
                //newStream.Dispose();//释放流所占用的资源
                //#endregion

                #region //【2】将创建Stream流对象的过程写在using当中，会自动的帮助我们释放流所占用的资源。OK
                using (Stream wStream = wbRequest.GetRequestStream())         //using(){}作为语句，用于定义一个范围，在此范围的末尾将释放对象。
                {
                    wStream.Write(data, 0, data.Length);
                }
                #endregion

                //获取响应
                HttpWebResponse wbResponse = (HttpWebResponse)wbRequest.GetResponse();
                using (Stream responseStream = wbResponse.GetResponseStream())
                {
                    using (StreamReader sReader = new StreamReader(responseStream, Encoding.UTF8))      //using(){}作为语句，用于定义一个范围，在此范围的末尾将释放对象。
                    {
                        reslut = sReader.ReadToEnd();
                    }
                }
            }
            catch (Exception e)
            {
                reslut = e.Message;     //输出捕获到的异常，用OUT关键字输出
                if (reslut.Contains("服务器不可用"))
                    reslut +=Environment.NewLine+ url;

                return -1;              //出现异常，函数的返回值为-1
            }
            return 0;
        }


        public int HttpPostForBytes(string url, byte[] data, List<RequestHeaders> requestHeadersList, out byte[] reslut)
        {
            reslut = null;
            try
            {
                //byte[] data = System.Text.Encoding.UTF8.GetBytes(sendData);
                HttpWebRequest wbRequest = (HttpWebRequest)WebRequest.Create(url);  // 制备web请求
                wbRequest.Proxy = null;     //现场测试注释掉也可以上传
                wbRequest.Method = "POST";
                wbRequest.ContentType = "application/json";
                wbRequest.ContentLength = data.Length;

                System.Net.ServicePointManager.DefaultConnectionLimit = 50; //试了试，结果就解决超时的问题了。
                wbRequest.Timeout = 30 * 1000; // 30秒超时（默认100秒，可根据实际场景调整）
                wbRequest.ReadWriteTimeout = 30 * 1000; // 读写流超时，避免卡死前
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                wbRequest.KeepAlive = false;


                foreach (var requestHeaders in requestHeadersList)
                {
                    wbRequest.Headers.Add(requestHeaders.Key, requestHeaders.Value);
                }


                //#region //【1】获得请求流，OK
                //Stream newStream = wbRequest.GetRequestStream();
                //newStream.Write(data, 0, data.Length);
                //newStream.Close();//关闭流
                //newStream.Dispose();//释放流所占用的资源
                //#endregion

                #region //【2】将创建Stream流对象的过程写在using当中，会自动的帮助我们释放流所占用的资源。OK
                using (Stream wStream = wbRequest.GetRequestStream())         //using(){}作为语句，用于定义一个范围，在此范围的末尾将释放对象。
                {
                    wStream.Write(data, 0, data.Length);
                }
                #endregion

                //获取响应
                HttpWebResponse wbResponse = (HttpWebResponse)wbRequest.GetResponse();
                using (Stream responseStream = wbResponse.GetResponseStream())
                {
                    const int bufferLen = 4096;
                    byte[] buffer = new byte[bufferLen];
                    int count = 0;

                    MemoryStream stream = new MemoryStream();

                    while ((count = responseStream.Read(buffer, 0, bufferLen)) > 0)
                    {
                        stream.Write(buffer, 0, count);
                    }
                    reslut = stream.GetBuffer().Skip(0).Take(Convert.ToInt32(wbResponse.ContentLength)).ToArray();
                    //reslut = bytesTrimEnd(stream.GetBuffer());

                }
            }
            catch (Exception e)
            {
                //return -1;              //出现异常，函数的返回值为-1
                string strError=$"HTTP POST失败，URL：{url}，异常：{e.Message}，堆栈：{e.StackTrace}";
                throw new Exception(strError);
            }
            return 0;
        }

        /// <summary>
        /// 去除byte[]数组缓冲区内的尾部空白区;从末尾向前判断;
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public byte[] bytesTrimEnd(byte[] bytes)
        {
            List<byte> list = bytes.ToList();
            for (int i = bytes.Length - 1; i >= 0; i--)
            {
                if (bytes[i] == 0x00)
                {
                    list.RemoveAt(i);
                }
                else
                {
                    break;
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// 获取Unix时间戳
        /// </summary>
        /// <returns></returns>
        public string GetTimeStamp()
        {
            //返回 1589436601
            //long unitTimespan = (DateTime.UtcNow.Ticks - 621355968000000000) / 10000000;
            //return unitTimespan.ToString();

            //这种方式得到的时间戳更长一点,包含了小数点
            //TotalSeconds 属性 ： 获取以整秒数和秒的小数部分表示的当前 System.TimeSpan 结构的值
            //返回：158946540194438

            int hours = System.TimeZone.CurrentTimeZone.GetUtcOffset(System.DateTime.Now).Hours;
            TimeSpan ts = DateTime.Now.AddHours(-hours) - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return (ts.TotalMilliseconds).ToString();
        }
    }
}
