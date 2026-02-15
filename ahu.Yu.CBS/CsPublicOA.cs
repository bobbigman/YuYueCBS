using Kingdee.BOS;
using Kingdee.BOS.App.Data;
using Kingdee.BOS.Core.DynamicForm;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.Util;
using Kingdee.BOS.WebApi.Client;
//using Kingdee.BOS.WebApi.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ahu.YuYue.CBS
{
    [Kingdee.BOS.Util.HotUpdate]
    public class CsPublicOA
    {

        //public const string KingdeeLogPath = @"c:\WorkSpace\kingdeeLog\";

        public static void WriteLog(string pPath, string pFileName, string pMesssage)

        {
            if (!System.IO.Directory.Exists(pPath))
            {
                System.IO.Directory.CreateDirectory(pPath);
            }
            string logFile = System.IO.Path.Combine(pPath, pFileName + ".txt");
            StreamWriter swLogFile = new StreamWriter(logFile, true, Encoding.Unicode);
            swLogFile.WriteLine(Environment.NewLine + Environment.NewLine + new String('*', 50)
                + Environment.NewLine + "日志写入时间1：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                + Environment.NewLine + pMesssage);
            swLogFile.Close();
            swLogFile.Dispose();
        }

        public static K3CloudApiClient CreateK3CloudApiClient(ref Struct_K3LoginInfo pStruct_K3LoginInfo, int pK3DatabaseType)

        {
            string strK3Url = "";
            string strK3DbId = "";
            string strUserName = "";
            string strPassword = "";

            switch (pK3DatabaseType)
            {
                case K3DatabaseMode.Design:
                    //strK3Url = struct_K3LoginDesign.ServerURL;
                    //strK3DbId = struct_K3LoginDesign.AcctID;
                    //strUserName = struct_K3LoginDesign.Username;
                    //strPassword = struct_K3LoginDesign.Password;
                    return null;
                //break;
                case K3DatabaseMode.Test:
                    strK3Url = struct_K3LoginTest.ServerURL;
                    strK3DbId = struct_K3LoginTest.AcctID;
                    strUserName = struct_K3LoginTest.Username;
                    strPassword = struct_K3LoginTest.Password;

                    break;

                case K3DatabaseMode.Normal:
                    //if (Environment.MachineName.EqualsIgnoreCase("green") || Environment.MachineName.EqualsIgnoreCase("GREENOLD"))
                    //    strK3Url = struct_K3LoginNormal.ServerURL_Bob;
                    //else
                    //    strK3Url = struct_K3LoginNormal.ServerURL;

                    strK3Url = struct_K3LoginNormal.ServerURL;
                    strK3DbId = struct_K3LoginNormal.AcctID;
                    strUserName = struct_K3LoginNormal.Username;
                    strPassword = struct_K3LoginNormal.Password;

                    break;

                case K3DatabaseMode.IntegrationK3:  //表示，集成在金蝶系统里。
                    strK3Url = pStruct_K3LoginInfo.ServerURL_K3;
                    strK3DbId = pStruct_K3LoginInfo.AcctID;
                    strUserName = pStruct_K3LoginInfo.Username;
                    strPassword = pStruct_K3LoginInfo.Password;
                    break;

            }

            K3CloudApiClient mK3CloudApiClient = new K3CloudApiClient(strK3Url);
            try
            {
                //更改原因：登陆失败，希望显示错误信息。
                //bolLoginResult = mK3CloudApiClient.Login(strK3DbId, strUserName, strPassword, 2052);
                string strLoginResult = mK3CloudApiClient.ValidateLogin(strK3DbId, strUserName, strPassword, 2052);
                JObject joLoginResult = JObject.Parse(strLoginResult);
                int intLoginResultType = joLoginResult["LoginResultType"].Value<int>();

                if (intLoginResultType == 1)
                {
                    pStruct_K3LoginInfo.LoginK3Ok = true;
                    return mK3CloudApiClient;
                }
                else
                {
                    string strMessage = JObject.Parse(strLoginResult)["Message"].Value<string>()+Environment.NewLine+ "DbId:" + strK3DbId+ ";UserName:"+ strUserName+ "Password:"+ strPassword ;
                    throw new Exception(strMessage);
                }
            }
            catch (Exception ex)
            {
                string strError = "登陆失败:" + ex.Message + Environment.NewLine +"Url:"+ strK3Url+";AcctId:"+ strK3DbId+";UserName:" + strUserName+ ";Password"+ strPassword+Environment.NewLine+ Environment.NewLine;
                throw new Exception(strError);

                //throw new Exception(CsErrLog.GetExceptionWithLog(ex));
            }

        }


        public static void AddOperateResult(IOperationResult pIOperationResults, string strBillId, string strNumber, string strMessage)
        {
            try
            {
                ;

                Kingdee.BOS.Core.DynamicForm.OperateResult OperateResult1;


                if (pIOperationResults.OperateResult.Count == 0)
                {
                    OperateResult1 = new OperateResult();
                    pIOperationResults.OperateResult.Add(OperateResult1);

                }

                OperateResult1 = pIOperationResults.OperateResult[0];

                if (OperateResult1.PKValue == null)
                {
                    //AfterExecuteOperationTransaction,实际上不需要的，
                    //只为兼容：BeforeExecuteOperationTransaction
                    OperateResult1.PKValue = strBillId;
                    OperateResult1.Number = strNumber;
                }

                OperateResult1.SuccessStatus = false;
                OperateResult1.Name = "同步报文";
                OperateResult1.Message = strMessage;
                OperateResult1.MessageType = Kingdee.BOS.Core.DynamicForm.MessageType.Normal;
            }

            catch (Exception ex)
            {
                throw new Exception(CsErrLog.GetExceptionWithLog(ex));
            }

        }

        public static void AddError2OperationResult(string pError, OperationResult pOperationResults, int intIndex)
        {
            if (pError.IsNullOrEmptyOrWhiteSpace() == true)
                return;

            OperateResult OperateResult1;
            if (pOperationResults.OperateResult.Count <= intIndex)
            {
                OperateResult1 = new OperateResult();
                pOperationResults.OperateResult.Add(OperateResult1);
            }

            OperateResult1 = pOperationResults.OperateResult[intIndex];

            OperateResult1.Name = "同步";
            OperateResult1.Message = pError;
            OperateResult1.MessageType = MessageType.Warning;
            OperateResult1.SuccessStatus = (pError == "");

            //希望在csList中，能刷新
            //验证没戏。List中，还是不能刷新。跟踪时，dyanmicObject中，没有值。
            //pDO["FSynchronError"] = pError;  
        }

        public static void Mark_FSynchronStatus2OtherSystem(Context pContext, K3CloudApiClient pK3CloudApiClient1
            , string pTable, ref string pError, string pFID_FieldName
, string pFID_Value, string pIDLC, string pOperateType)
        {
            try
            {

                if (pError == "")
                    Mark_FSynchronStatusBob_OK(pContext, pK3CloudApiClient1, pTable
                        , ref pError, pFID_FieldName, pFID_Value, pIDLC, pOperateType);
                else
                    Mark_FSynchronStatusBob_Error(pContext, pK3CloudApiClient1, pTable, ref pError, pFID_FieldName, pFID_Value);
            }
            catch (Exception ex)
            {
                throw new Exception(CsErrLog.GetExceptionWithLog(ex));
            }
        }

        public static void Mark_FSynchronStatusBob_OK(Context pContext, K3CloudApiClient pK3CloudApiClient1
    , string pTable, ref string pError, string pFID_FieldName
, string pFID_Value, string pIDLC, string pOperateType)
        {
            try
            {
                string strFieldError = "FSynchronError";
                string strFieldStatusBob = "FSynchronStatusBob";

                string strOkResult = "1";

                //部门删除，要改为未同步。
                if (pOperateType.EqualsIgnoreCase("delete"))
                    strOkResult = "0";

                string strSQL = string.Format(@"/*dialect*/
Update {0} set
       {3}='',{4}={6},FIDLC2='{5}'
Where {1}='{2}'", pTable, pFID_FieldName, pFID_Value, strFieldError, strFieldStatusBob, pIDLC, strOkResult);

                int intAffected = CsData.BobExecute(pContext, pK3CloudApiClient1, strSQL);

            }
            catch (Exception ex)
            {
                throw new Exception(CsErrLog.GetExceptionWithLog(ex));
            }
        }

        public static void Mark_FSynchronStatusBob_Error(Context pContext, K3CloudApiClient pK3CloudApiClient1, string pTable, ref string pError, string pFID_FieldName
            , string pFID_Value)
        {

            string strFieldError = "FSynchronError";
            string strFieldStatusBob = "FSynchronStatusBob";

            try
            {
                pError = pError.Replace("'", "");
                string strSQL = string.Format(@"/*dialect*/
Update {0} set
       {4}='{1}',{5}=2
Where {2}='{3}'", pTable, pError, pFID_FieldName, pFID_Value, strFieldError, strFieldStatusBob);

                int intAffected = CsData.BobExecute(pContext, pK3CloudApiClient1, strSQL);


            }
            catch (Exception ex)
            {
                throw new Exception(CsErrLog.GetExceptionWithLog(ex));
            }
        }

        public static string GetBillTypeId(DynamicObject pDO)
        {
            string strV1 = "";

            string strFindKey = "BillType";

            for (int i = 1; i <= 4; i++)
            {
                switch (i)
                {
                    case 1:
                        strFindKey = "BillType";
                        break;
                    case 2:
                        strFindKey = "BillTypeId";
                        break;
                    case 3:
                        strFindKey = "FBillType";
                        break;
                    case 4:
                        strFindKey = "FBillTypeId";
                        break;
                }
                if (pDO.Contains(strFindKey) == true)
                {
                    DynamicObject doBillTypeId = (DynamicObject)pDO[strFindKey];
                    strV1 = Convert.ToString(doBillTypeId["Id"]);
                    return strV1;
                }

            }
            return strV1;
        }


        public static string MyWebRequest(string pFullURL, string pJson)
        {

            Uri uri = new Uri(pFullURL);
            WebRequest webRequest1 = WebRequest.Create(uri);
            HttpWebRequest HttpWebRequest1 = (HttpWebRequest)webRequest1;
            HttpWebRequest1.Method = "POST";
            HttpWebRequest1.ContentType = "text/xml";

            //HttpWebRequest1.Headers.Add("SOAPAction: " + pSOAPAction);  //加了这个就报错。

            byte[] postBytes = Encoding.UTF8.GetBytes(pJson);
            HttpWebRequest1.ContentLength = postBytes.Length;
            HttpWebRequest1.Credentials = CredentialCache.DefaultCredentials;

            string strReturn = "";
            try
            {
                using (Stream requestStream = webRequest1.GetRequestStream())
                {
                    byte[] paramBytes = Encoding.UTF8.GetBytes(pJson);
                    requestStream.Write(paramBytes, 0, paramBytes.Length);
                }
                //响应
                HttpWebResponse HttpWebResponse1 = (HttpWebResponse)webRequest1.GetResponse();

                //WebResponse webResponse = webRequest1.GetResponse();
                using (StreamReader myStreamReader = new StreamReader(HttpWebResponse1.GetResponseStream(), Encoding.UTF8))
                {
                    strReturn = myStreamReader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                strReturn = CsErrLog.GetExceptionWithLog(ex);

                //显示重复了。
                //strReturn = strReturn + Environment.NewLine + pURL_Soap;

            }
            return strReturn;
        }


        public static void JsonLog_Bob(Context pContext, K3CloudApiClient pK3CloudApiClient, string pFID, string pFNumber, string pFDate
     , string pResultJson, string pJson, string pObjectTypeId, string pURL
            , string pForbidStatus, int pOk
            )
        {
            try
            {
                // 调用Web API接口服务，保存采购订单
                string strSQL = "";

                if (pJson == "" || pResultJson == "")
                    return;

                string strResultJson = pResultJson.Replace("'", "''");

                //为什么要省略？没必要。2024/1/12 8:44
                //if (strResultJson.Length >= 650)
                //    strResultJson = strResultJson.Substring(0, 250) + "...省略n个字符...";

                string strK3DllVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

                //FID要用引号括起来。
                strSQL = string.Format(@"/*dialect*/ 
                INSERT INTO dbo.tbl2Others
                           (FBillType,[FID]
                           ,[FBillNO],FBillDate
                           ,[IsOk]
                           ,[ResultJson]
                           ,[Json],DllVersion,ForbidStatus,URL
                          )
                     VALUES
                ('{0}','{1}','{2}','{3}',{4},'{5}','{6}','{7}','{8}','{9}')",
               pObjectTypeId, pFID, pFNumber, pFDate, pOk
               , strResultJson, pJson.Replace("'", "")
               , strK3DllVersion, pForbidStatus, pURL
               );
                CsData.BobExecute(pContext, pK3CloudApiClient, strSQL);

            }
            catch (Exception ex)
            {
                throw new Exception(CsErrLog.GetExceptionWithLog(ex));

            }
        }


        //没有日期，适用于基础资料同步。
        public static string SQLLog_Bob(Context pContext, K3CloudApiClient pK3CloudApiClient, string pFID, string pFNumber
            , string pResultJson, string pJson, string pObjectTypeId, string pForbidStatus, string pURL
           , string pOperateType)
        {
            string strReturn = SQLLog_Bob(pContext, pK3CloudApiClient, pFID, pFNumber, ""
                , pResultJson, pJson, pObjectTypeId, false, pURL, pForbidStatus
                , pOperateType
                );
            return strReturn;
        }

        //有日期，目的：要适用于单据同步。
        //注：不是取消，也不是禁用。
        //假设单据，也有禁用接口，仅为兼容。
        public static string SQLLog_Bob(Context pContext, K3CloudApiClient pK3CloudApiClient, string pFID, string pFNumber, string pFDate
            , string pResultJson, string pJson, string pObjectTypeId, string pForbidStatus, string pURL
         , string pOperateType)
        {
            string strReturn = "";
            strReturn = SQLLog_Bob(pContext, pK3CloudApiClient, pFID, pFNumber, pFDate
                , pResultJson, pJson, pObjectTypeId, false, pURL, pForbidStatus
                , pOperateType
              );
            return strReturn;
        }

        //总的吧，除了同步，还能适用于：同步取消接口，基础资料禁用接口
        public static string SQLLog_Bob(Context pContext, K3CloudApiClient pK3CloudApiClient
            , string pFID, string pFNumber, string pFDate
            , string pResultJson, string pJson
            , string pObjectTypeId, bool pIsCancel, string pURL, string pForbidStatus
            , string pOperateType
          )
        {
            string strTable = "";
            try
            {
                // 调用Web API接口服务，保存采购订单
                int intOK = 0;
                string strResult = "";
                string strError = "";
                string strFieldId = "";
                if (pJson == "" || pResultJson == "")
                {
                    return "";
                }

                if (pResultJson.IndexOf("检测到此数据已经重复请求3次以上") > -1)
                    return "同步失败。" + Environment.NewLine + pResultJson;

                string strJSon_STATUS = "code";
                string strJSon_Error = "msg";

                switch (pObjectTypeId)
                {
                    //全部要改。

                    //case K3FormId.strCN_BANKACNT:
                    //    strFieldId = "FBANKACNTID";
                    //    strTable = "t_CN_BANKACNT";
                    //    break;

                    case K3FormId.strWB_RecBankTradeDetail:
                        strFieldId = "FBANKACNTID";
                        strTable = "t_CN_BANKACNT";
                        break;
                    case K3FormId.strWB_ReceiptBill:

                    default:
                        throw new Exception("case中，没有：" + pObjectTypeId + ",CsPublicOA.SQLLog_Bob");
                        //strFieldId = "FID";
                        //strTable = "T_" + pObjectTypeId;
                        //break;


                }

                JObject JObjectResult = JObject.Parse(pResultJson);

                string strReturnCode = JObjectResult[strJSon_STATUS].ToString();
                string strIDLC = "";  //浪潮内码，要返写到金蝶凭证中，如果新增成功的话。

                if (strReturnCode == "1")
                {
                    strError = JObjectResult[strJSon_Error].ToString();
                    if (strError.Length > 200)
                    {
                        strError = strError.Substring(0, 200) + "...后面省略...";
                    }
                }
                else if (strReturnCode == "0")
                {

                    strError = ""; //成功了，肯定要清空。
                    strIDLC = JObjectResult["msg"].ToString(); //取全部了，不要分开。
                    //string strMsg = JObjectResult["msg"].ToString();
                    //int intHashIndex = strMsg.IndexOf('#');
                    //// 确保找到了#，如果找不到，就使用空，不取内码呗。  
                    //if (intHashIndex >= 0)
                    //{
                    //    // 提取#之前的字符串  
                    //    strIDLC = strMsg.Substring(0, intHashIndex);
                    //}
                }


                //成功失败，都在这里。
                CsPublicOA.Mark_FSynchronStatus2OtherSystem(pContext, pK3CloudApiClient, strTable
                    , ref strError, strFieldId, pFID, strIDLC, pOperateType);
                if (strReturnCode != "0" && strReturnCode != "200")
                {
                    strResult = strError + Environment.NewLine + "网址： " + pURL + Environment.NewLine + "同步报文：" + Environment.NewLine + pJson;
                }

                if (strReturnCode == "0" || strReturnCode == "200" || strError.EqualsIgnoreCase("浪潮已执行到状态:归档"))
                {
                    intOK = 1;
                }
                else
                    intOK = 0;

                JsonLog_Bob(pContext, pK3CloudApiClient, pFID, pFNumber
                    , pFDate, pResultJson, pJson, pObjectTypeId, pURL, pForbidStatus, intOK
                    );

                return strResult;

            }
            catch (Exception ex)
            {
                throw new Exception(CsErrLog.GetExceptionWithLog(ex));

            }
        }

        public static string Long2Date(string pFDate)
        {
            // 将时间戳转换为DateTimeOffset，然后获取UTC或本地时间  
            string strFDate = pFDate;
            if (strFDate.IndexOf("/") > -1)
                return strFDate;

            if (strFDate.IndexOf("-") > -1)
                return strFDate;

            long longFDate = Convert.ToInt64(strFDate);
            if (longFDate > 0)
            {
                //2024/9/19 9:49 1717197847000毫秒，误差为几小时。本来为2024/6/1,算出来，是2024/5/31

                //DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(longFDate);
                //DateTime dt2 = dateTimeOffset.UtcDateTime; // 或者使用 dateTimeOffset.DateTime 获取本地时间  
                //strFDate = dt2.ToString("yyyy-MM-dd");

                // Unix纪元时间（1970年1月1日 UTC）  
                DateTimeOffset unixEpoch = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

                // 将时间戳转换为DateTimeOffset  
                DateTimeOffset dateTimeOffset = unixEpoch.AddMilliseconds(longFDate);

                // 转换为北京时间（UTC+8）  
                DateTimeOffset dtBeiJin = dateTimeOffset.ToOffset(TimeSpan.FromHours(8));

                //// 格式化输出北京时间  
                return dtBeiJin.DateTime.ToString("yyyy-MM-dd");

            }
            return strFDate;
        }

        public static string GetJsonOA(Context pContext, K3CloudApiClient pK3CloudApiClient1
, string pFormID, string pFormOperation
, string pFId, string pBillNo, bool pTranslate
)
        {
            string strReturn = "";
            string strJson;
            string strResultJson;
            string strCreateTime;
            string strURL;

            string strFilerByBill;
            if (pFId.IsNullOrEmptyOrWhiteSpace() == true)
                strFilerByBill = " FBillNo='" + pBillNo + "'";
            else
                strFilerByBill = " FId='" + pFId + "'";

            string strSQL = string.Format(@"/*dialect*/ 
 SELECT  Top 4 JSon, ResultJson,CreateTime,URL
   FROM tbl2Others  with(nolock) 
 WHERE  {0}
  and   FBillType='{1}'
  Order by ToOthersID desc
", strFilerByBill, pFormID);
            DataTable dt1 = CsData.GetDataTable(pContext, pK3CloudApiClient1, strSQL);

            //怪事，判断不准。
            //if (ObjectUtils.IsNullOrEmpty(dt1) == true || dt1.Rows.Count == 0) 
            if (dt1 == null || dt1.Rows.Count == 0)
                return "";

            //为什么不用SQL过滤呢，like %%?答：会卡着。
            //为什么要过滤NeedUpDateFields呢？答：我查报文，只想找保存的，不想查提交，审核的
            //这也是我为什么要循环的原因。
            foreach (DataRow dr1 in dt1.Rows)
            {
                strCreateTime = Convert.ToString(dr1["CreateTime"]);
                strJson = Convert.ToString(dr1[0]);
                strResultJson = Convert.ToString(dr1[1]);
                strURL = Convert.ToString(dr1["URL"]);

                //查报文，只想找保存的，不想查提交，审核的
                if (strJson.Length < 30 && strJson != "同步取消")
                    continue;

                if (strJson == "同步取消")
                    strJson = "同步取消操作，没有同步报文。";

                if (strReturn.IsNullOrEmptyOrWhiteSpace() == false)
                    strReturn += Environment.NewLine + Environment.NewLine + "****************************继续找报文记录*******************************" + Environment.NewLine;

                strReturn += "同步时间:" + strCreateTime + Environment.NewLine;
                strReturn += strURL + Environment.NewLine;
                strReturn += "同步执行后返回的信息" + Environment.NewLine + strResultJson + Environment.NewLine + Environment.NewLine;

                if (pTranslate == true && strJson.IndexOf("同步取消") == -1)
                {
                    strJson = TranlsateEnglishJsonOA_All(pContext, pK3CloudApiClient1, strJson, pFormID, pFormOperation);
                    strReturn += "同步内容(和报文格式上是不同的)" + Environment.NewLine + strJson;
                }
                else
                {
                    strReturn += "同步报文:" + Environment.NewLine + strJson;
                }

            }

            return strReturn;


        }

        private static string TranlsateEnglishJsonOA_All(Context pContext, K3CloudApiClient pK3CloudApiClient1
            , string pJson, string pFormId, string pFormOperation)
        {

            string strReturn;
            string strSQL = string.Format(@"
Select  MatchField,K3Caption
  from  tblMatch
 Where  FormId='{0}'
", pFormId);
            DataTable dt1 = CsData.GetDataTable(pContext, pK3CloudApiClient1, strSQL);
            dt1.DefaultView.Sort = "MatchField";

            StringBuilder sb1 = new StringBuilder();

            TranlsateEnglishJsonOAPart_Master(pJson, dt1, sb1, pFormId, "");

            strReturn = sb1.ToString();

            return strReturn;
        }

        private static void TranlsateEnglishJsonOAPart_Master(
            string pJson, DataTable pDataTable2Match, StringBuilder pStringBuilder, string pFormId
            , String pNodeKey
           )
        {


            JObject joLog = JObject.Parse(pJson);
            JObject JObject1;

            //部门，没有那么多层。
            if (pNodeKey.IsNullOrEmptyOrWhiteSpace() == true)
                JObject1 = joLog as JObject;
            else
                JObject1 = joLog[pNodeKey][0] as JObject;//joLog["data"]是jarray

            pDataTable2Match.DefaultView.Sort = "MatchField";

            foreach (JProperty jProperty1 in JObject1.Properties())
            {
                string strName = jProperty1.Name;
                string strValue = jProperty1.Value.ToString();

                TranlsateEnglishJsonByFieldTypeOA(pDataTable2Match, jProperty1, pStringBuilder);
            }

            return;
        }

        private static void TranlsateEnglishJsonOAPart_Detail(
            string pJson, DataTable pDataTable2Match, StringBuilder pStringBuilder
            , string pNodeKey)
        {

            JObject joLog = JObject.Parse(pJson);

            pDataTable2Match.DefaultView.Sort = "MatchField";

            JArray JArrayDetaiil = new JArray();
            //晕死，明细行不能直接转为json，还要来个循环。2023/10/6 17:54
            foreach (JProperty jProperty1 in joLog.Properties())
            {
                string strName = jProperty1.Name;
                string strValue = jProperty1.Value.ToString();

                if (strName != pNodeKey)
                    continue;

                JArrayDetaiil = JArray.Parse(strValue);
                break;
            }

            int intRow = 0;
            pStringBuilder.AppendLine();
            foreach (JToken jToken2 in JArrayDetaiil)
            {
                intRow++;
                pStringBuilder.AppendLine("明细第" + intRow + "行开始*****");
                JObject JObject2 = jToken2 as JObject;
                foreach (JProperty property2 in JObject2.Properties())
                {
                    TranlsateEnglishJsonByFieldTypeOA(pDataTable2Match, property2, pStringBuilder);
                }
                pStringBuilder.AppendLine("明细第" + intRow + "行结束******");
                pStringBuilder.AppendLine();
            }



            return;
        }
        private static void TranlsateEnglishJsonByFieldTypeOA(DataTable dt1, JProperty pProperty1, StringBuilder sb1)
        {
            string strName = pProperty1.Name;
            string strValue = pProperty1.Value.ToString();
            string strK3Caption = GetValueFromTableOA(dt1, strName);

            string strItem = strK3Caption + "|" + strName + " = " + strValue;
            sb1.AppendLine(strItem);

        }

        private static string GetValueFromTableOA(DataTable dt1, string pValue)
        {
            int intFound = dt1.DefaultView.Find(pValue); //获得行的位置
            if (intFound == -1)
                return "";

            string strReturn = Convert.ToString(dt1.DefaultView[intFound]["K3Caption"]);

            return strReturn;
        }


        public static string GetFormIdByBillType(string pstrFPOSBillType)
        {
            string strFormId = "";

            switch (pstrFPOSBillType)
            {
                case "接收银行交易明细":
                    strFormId = K3FormId.strWB_RecBankTradeDetail;
                    break;
                case "电子回单":
                    strFormId = K3FormId.strWB_ReceiptBill;
                    break;




                default: /* 不可选的 */
                    string strBadInfo = "糟糕，swithc选项不存在。[" + pstrFPOSBillType + "]" + Environment.NewLine + "ClsPosTBGZT.GetPOSTDatas";
                    throw new Exception(strBadInfo);
            }

            return strFormId;

        }

    }
}
