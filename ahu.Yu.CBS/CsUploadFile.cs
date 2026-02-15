using Kingdee.BOS;
using Kingdee.BOS.WebApi.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
//using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ahu.YuYue.CBS
{
    [Kingdee.BOS.Util.HotUpdate]
    public class CsUploadFile
    {


        public string RunUpLoad1ExcelMain(Context pContext, K3CloudApiClient pK3CloudApiClient1,
       string pFormId, string pFId, string pFBillNo, string pFileURL, string pFileName
            , ref string pError, string pUploadSouce)
        {
            if (pFileURL == "")
            {
                pError = "电子回单，居然没有文件名";
                return "";
            }

            string strSQL = string.Format(@"
Select FCREATETIME
from [T_BAS_ATTACHMENT]
Where FBILLTYPE='WB_ReceiptBill'
and   FINTERID={0}", pFId);
            string strFCREATETIME = CsData.GetTopValue(pContext, pK3CloudApiClient1, strSQL);
            if (strFCREATETIME != "")
            {
                pError = string.Format("不需要同步：在{0}已同步过了。", strFCREATETIME);
                return "";
            }

            //获取拚接报文，用于再下一步，把图片下载到本机的。
            string strJson_DZHD = GetJsonUploadFileDZHD(K3FormId.strWB_ReceiptBill, pFId, pFBillNo, pFileURL, pFileName, ref pError, pUploadSouce);

            //再故意产生一个错误，并跟踪一下。
            //string strJson_DZHD = GetJsonUploadFileDZHD(pFormId, pFId, pFBillNo, pFileURL, pFileName, ref pError);
            if (pError != "")
                return pError;

            string strJSonResult = pK3CloudApiClient1.AttachmentUpload(strJson_DZHD);
            var jsonObject2 = JObject.Parse(strJSonResult);
            var responseStatus = jsonObject2["Result"]?["ResponseStatus"];
            bool isSuccess = responseStatus["IsSuccess"]?.Value<bool>() ?? true;
            if (!isSuccess)
            {
                pError = responseStatus["Errors"][0]["Message"]?.ToString();
                return "";

            }

                strSQL = string.Format(@"
--更新附件数
Update T_WB_RECEIPT Set F_PKUB_AttachmentCount=F_PKUB_AttachmentCount+1 Where FId={0}

Select FCREATETIME
from [T_BAS_ATTACHMENT]
Where FBILLTYPE='WB_ReceiptBill'
and   FINTERID={0}", pFId);
            strFCREATETIME = CsData.GetTopValue(pContext, pK3CloudApiClient1, strSQL);
            if (strFCREATETIME != "")
            {
                //不需要手工写附件信息。
                return strJSonResult;
            }

            //奇怪，有时，需要手工产生，有时，却不需要。2024/07/28 21:22

            // 解析JSON为JObject  
            JObject jsonObject = JObject.Parse(strJSonResult);

            // 访问嵌套的对象  
            JObject resultObject = (JObject)jsonObject["Result"];

            // 读取FileId的值  
            string strFileId = Convert.ToString(resultObject["FileId"]);

            strSQL = string.Format(@" Select top 1 FID  From T_BAS_ATTACHMENT Order by FID desc ");
            string strFID_New = CsData.GetTopValue(pContext, pK3CloudApiClient1, strSQL);
            long lngFID_AttachmentNew = Convert.ToInt64(strFID_New);
            lngFID_AttachmentNew++;

            string strExtension = "";
            int lastIndex = pFileName.LastIndexOf('.');
            if (lastIndex != -1)
            {
                //小数点，也要读出来呢。
                strExtension = pFileName.Substring(lastIndex);
            }

            strSQL = string.Format(@"

            INSERT INTO [dbo].[T_BAS_ATTACHMENT]
                       ([FID]
                       ,[FBILLTYPE]
                       ,[FINTERID]
                       ,[FATTACHMENTNAME]
                       ,[FBILLNO]
                       ,[FCREATETIME]
                       ,[FFILEID]
                       ,[FFILESTORAGE]
            		   ,FENTRYKEY,FENTRYINTERID,FEXTNAME
            )
                 VALUES
                       ({0},'WB_ReceiptBill','{1}','{2}'
            		   ,'{3}','{4}','{5}',1
            		   ,' ',-1,'{6}')", lngFID_AttachmentNew, pFId, pFileName, pFBillNo
           , System.DateTime.Now.ToString(), strFileId, strExtension);
            CsData.BobExecute(pContext, pK3CloudApiClient1, strSQL);

            return strJSonResult;
        }

        public string GetJsonUploadFileDZHD(
            string pFormId, string pFId, string pFBillNo, string pFileURL, string pFileName
            , ref string pError
            , string pUploadSouce)
        {
            //2024/7/13 20：32 周六 晴 雨
            //文件太大了吧 ，在本机测试没问题，服务器上测试，有问题。
            //Cannot write more bytes to the buffer than the configured maximum buffer size: 65536.
            //string strBase64 = MyReadByteFromURL(pFileURL, ref pError);
            string strBase64;
            try
            {
                strBase64 = ReadFileBytWeb(pFileURL);
            }
            catch (Exception e)
            {
                string strError = CsErrLog.GetExceptionInfo(e);
                if (strError.Contains("远程服务器返回错误: (403) 已禁止。"))
                {
                    pError = "远程服务器返回错误: (403) 已禁止。";
                    return "";

                }

                string strErrors = strError + Environment.NewLine + Environment.NewLine + "Cs2K3_BatchSave.DownloadFileAndRead:"
                                   + Environment.NewLine + Environment.NewLine + pFileURL;
                string strlogFile = ClsPublic.KingdeeLogPath + DateTime.Now.ToString("yyyy-MM-dd");
                ClsPublic.WriteLog(ClsPublic.KingdeeLogPath, strlogFile
                    , "错误日志:" + Environment.NewLine + strErrors);

                pError = strErrors;
                return "";
            }


            if (pError != "")
            {
                return "";
            }

            //注意，不需要包在data里，又不是用http,又不是外人，我是金蝶开发嘛 。
            var root = new
            {
                FileName = pFileName,
                FormId = pFormId,
                IsLast = true,
                InterId = pFId,
                BillNO = pFBillNo,
                AliasFileName = pUploadSouce,
                SendByte = strBase64
            };


            string strJSon = JsonConvert.SerializeObject(root);
            return strJSon;
        }

        private string MyReadByteFromURL(string pFileURL, ref string pError)
        {
            string strBase64 = "";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // 发送GET请求以获取文件的字节流  
                    HttpResponseMessage response = client.GetAsync(pFileURL, HttpCompletionOption.ResponseHeadersRead).GetAwaiter().GetResult();
                    response.EnsureSuccessStatusCode(); // 确保HTTP成功状态值  

                    // 读取响应内容作为字节数组  
                    byte[] fileBytes = response.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();

                    // 将字节数组转换为Base64字符串  
                    strBase64 = Convert.ToBase64String(fileBytes);

                }
            }
            catch (HttpRequestException e)
            {
                string strError = CsErrLog.GetExceptionInfo(e);
                string strErrors = strError + Environment.NewLine + Environment.NewLine + "Cs2K3_BatchSave.DownloadFileAndRead:"
                                   + Environment.NewLine + Environment.NewLine + pFileURL;
                string strlogFile = ClsPublic.KingdeeLogPath + DateTime.Now.ToString("yyyy-MM-dd");
                ClsPublic.WriteLog(ClsPublic.KingdeeLogPath, strlogFile
                    , "错误日志:" + Environment.NewLine + strErrors);

                pError = strErrors;
                return "";

            }

            return strBase64;

        }

        public string ReadFileBytWeb(string url)
        {

            WebClient client = new WebClient();
            byte[] data = client.DownloadData(url);

            string base64String = Convert.ToBase64String(data);
            return base64String;

        }




    }


}
