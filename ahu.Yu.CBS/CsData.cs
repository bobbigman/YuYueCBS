using Kingdee.BOS;
using Kingdee.BOS.App;
using Kingdee.BOS.App.Data;
using Kingdee.BOS.Core.DynamicForm;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.Util;
using Kingdee.BOS.WebApi.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ahu.YuYue.CBS 
{
    public static class CsData
    {


        public static int BobExecute(Context pContext, K3CloudApiClient pK3CloudApiClient, string pSQL)
        {

            //考虑一下，可以写到一个sql log里。近日期来。
            try
            {
                //pSQL = pSQL.Replace("'", "''");
                pSQL = pSQL.Replace("/*dialect*/", "");

                //晕死，*dialect* 要顶格。
                string strSQL = string.Format(@"/*dialect*/
BEGIN TRY  
    BEGIN TRANSACTION;  
    {0} 
    COMMIT TRANSACTION;  
END TRY  
BEGIN CATCH  
    ROLLBACK TRANSACTION;  
    DECLARE @ErrorMessage NVARCHAR(4000);  
    SET @ErrorMessage = ERROR_MESSAGE();  
    RAISERROR(@ErrorMessage, 16, 1);  
END CATCH;", pSQL);
                int intAffected = 0;
                if (pContext != null)
                {
                    intAffected = DBUtils.Execute(pContext, strSQL);
                }
                else if (pK3CloudApiClient != null)
                {
                    intAffected = BobExecute(pK3CloudApiClient, strSQL);
                }
                else
                    throw new Exception("异常，请退出系统，重试一次。如仍有错误，请洽程序设计员。ClsData.BobExecute,pContext=null,pK3CloudApiClient1=null");

                return intAffected;
            }
            catch (Exception ex)
            {
                string strError = CsErrLog.GetExceptionInfo(ex) + "*" + Environment.NewLine + pSQL;
                string strLogInfo = strError + Environment.NewLine + pSQL;
                string strlogFile = ClsPublic.KingdeeLogPath + DateTime.Now.ToString("yyyy-MM-dd");
                CsPublicOA.WriteLog(ClsPublic.KingdeeLogPath, strlogFile, strLogInfo);

                throw new Exception(strLogInfo);

            }

        }


        private static int BobExecute(K3CloudApiClient pK3CloudApiClient, string pSQL)
        {
            try
            {
                string strError = "";
                DbServiceTests DbServiceTests1 = new DbServiceTests();
                int intAffected = DbServiceTests1.Execute(pK3CloudApiClient, pSQL, ref strError);
                if (strError.IsNullOrEmptyOrWhiteSpace() == true)
                    return intAffected;
                else
                    throw new Exception(strError);
            }
            catch (Exception ex)
            {
                string strError = CsErrLog.GetExceptionInfo(ex) + "*" + Environment.NewLine + pSQL;
                string strLogInfo = strError + Environment.NewLine + pSQL;
                string strlogFile = ClsPublic.KingdeeLogPath + DateTime.Now.ToString("yyyy-MM-dd");
                CsPublicOA.WriteLog(ClsPublic.KingdeeLogPath, strlogFile, strLogInfo);

                throw new Exception(strLogInfo);
            }
        }


        public static string GetTopValue(Context pContext, K3CloudApiClient pK3CloudApiClient, string pSQL)
        {
            try
            {
                string strValue = "";
                if (pContext != null)
                {
                    strValue = DBUtils.ExecuteScalar<string>(pContext, pSQL, null);
                }
                else if (pK3CloudApiClient != null)
                {
                    strValue = GetTopValue(pK3CloudApiClient, pSQL);
                }
                else
                    throw new Exception("糟糕，GetTopValue 无法执行，pContext，pK3CloudApiClient 都是null");


                if (strValue == null)
                    return "";

                strValue = strValue.Trim();
                return strValue;
            }
            catch (Exception ex)
            {
                string strError = CsErrLog.GetExceptionInfo(ex) + "*" + Environment.NewLine + pSQL;
                string strLogInfo = strError + Environment.NewLine + pSQL;
                string strlogFile = ClsPublic.KingdeeLogPath + DateTime.Now.ToString("yyyy-MM-dd");
                CsPublicOA.WriteLog(ClsPublic.KingdeeLogPath, strlogFile, strLogInfo);

                throw new Exception(strLogInfo);
            }

        }


        private static string GetTopValue(K3CloudApiClient pK3CloudApiClient, string pSQL)
        {
            try
            {
                string strError = "";
                DbServiceTests DbServiceTests1 = new DbServiceTests();
                string strValue = DbServiceTests1.ExecuteScalar(pK3CloudApiClient, pSQL, ref strError);
                if (strError.IsNullOrEmptyOrWhiteSpace() == true)
                    return strValue;
                else
                    throw new Exception(strError);

                //SqlCommand SqlCommand1 = new SqlCommand(pSQL, pK3CloudApiClient);
                //string strValue = Convert.ToString(SqlCommand1.ExecuteScalar());
                //strValue = strValue.Trim();
                //return strValue;
            }
            catch (Exception ex)
            {
                string strError = CsErrLog.GetExceptionInfo(ex) + "*" + Environment.NewLine + pSQL;
                string strLogInfo = strError + Environment.NewLine + pSQL;
                string strlogFile = ClsPublic.KingdeeLogPath + DateTime.Now.ToString("yyyy-MM-dd");
                CsPublicOA.WriteLog(ClsPublic.KingdeeLogPath, strlogFile, strLogInfo);

                throw new Exception(strLogInfo);
            }

        }

        public static DataTable GetDataTable(Context pContext, K3CloudApiClient pK3CloudApiClient, string pSQL)
        {
            try
            {
                DataTable DataTable1;
                if (pContext != null)
                    DataTable1 = GetDataTable(pContext, pSQL);
                else if (pK3CloudApiClient != null)
                    DataTable1 = GetDataTable(pK3CloudApiClient, pSQL);
                else
                    throw new Exception("糟糕，GetDataTable无法执行，pContext，K3CloudApiClient1都是null");

                return DataTable1;
            }
            catch (Exception ex)
            {
                string strError = CsErrLog.GetExceptionInfo(ex) + "*" + Environment.NewLine + pSQL;
                string strLogInfo = strError + Environment.NewLine + pSQL;
                string strlogFile = ClsPublic.KingdeeLogPath + DateTime.Now.ToString("yyyy-MM-dd");
                CsPublicOA.WriteLog(ClsPublic.KingdeeLogPath, strlogFile, strLogInfo);

                throw new Exception(strLogInfo);
            }
        }

        public static DataTable GetDataTable(K3CloudApiClient pK3CloudApiClient, string pSQL)
        {
            try
            {
                string strError = "";
                DbServiceTests DbServiceTests1 = new DbServiceTests();
                DynamicObjectCollection doc1 = DbServiceTests1.GetDynamicObject(pK3CloudApiClient, pSQL, ref strError);
                DataTable dt1 = DbServiceTests1.GetDataTableByDynamicOjbect(doc1);
                if (strError.IsNullOrEmptyOrWhiteSpace() == true)
                    return dt1;
                else
                    throw new Exception(strError);
            }
            catch (Exception ex)
            {
                string strError = CsErrLog.GetExceptionInfo(ex) + "*" + Environment.NewLine + pSQL;
                string strLogInfo = strError + Environment.NewLine + pSQL;
                string strlogFile = ClsPublic.KingdeeLogPath + DateTime.Now.ToString("yyyy-MM-dd");
                CsPublicOA.WriteLog(ClsPublic.KingdeeLogPath, strlogFile, strLogInfo);

                throw new Exception(strLogInfo);
            }
        }



        public static DataTable GetDataTable(Context pContext, string pSQL)
        {
            try
            {
                pSQL = "/*dialect*/" + Environment.NewLine + pSQL;
                DataSet ds = DBUtils.ExecuteDataSet(pContext, pSQL);
                //DataSet ds = ServiceHelper.ExecuteDataSet(pContext, pSQL);

                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                string strError = CsErrLog.GetExceptionInfo(ex) + "*" + Environment.NewLine + pSQL;
                string strLogInfo = strError + Environment.NewLine + pSQL;
                string strlogFile = ClsPublic.KingdeeLogPath + DateTime.Now.ToString("yyyy-MM-dd");
                CsPublicOA.WriteLog(ClsPublic.KingdeeLogPath, strlogFile, strLogInfo);

                throw new Exception(strLogInfo);
            }
        }

        public static DynamicObjectCollection GetDynamicObjects(Context pContext, K3CloudApiClient pK3CloudApiClient, string pSQL)
        {
            try
            {
                DynamicObjectCollection doc1;
                if (pContext != null)
                    doc1 = GetDynamicObjects(pContext, pSQL);
                else if (pK3CloudApiClient != null)
                    doc1 = GetDynamicObjects(pK3CloudApiClient, pSQL);
                else
                    throw new Exception("糟糕，GetDynamicObjects  无法执行，pContext，pK3CloudApiClient 都是null");

                return doc1;
            }
            catch (Exception ex)
            {
                string strError = CsErrLog.GetExceptionInfo(ex) + "*" + Environment.NewLine + pSQL;
                string strLogInfo = strError + Environment.NewLine + pSQL;
                string strlogFile = ClsPublic.KingdeeLogPath + DateTime.Now.ToString("yyyy-MM-dd");
                CsPublicOA.WriteLog(ClsPublic.KingdeeLogPath, strlogFile, strLogInfo);

                throw new Exception(strLogInfo);
            }
        }

        public static DynamicObjectCollection GetDynamicObjects(
            K3CloudApiClient pK3CloudApiClient, string pSQL)
        {
            try
            {
                string strError = "";
                DbServiceTests DbServiceTests1 = new DbServiceTests();
                DynamicObjectCollection doc1 = DbServiceTests1.GetDynamicObject
                    (pK3CloudApiClient, pSQL, ref strError);

                if (strError.IsNullOrEmptyOrWhiteSpace() == false)
                    throw new Exception(strError);
                else
                    return doc1;
            }
            catch (Exception ex)
            {
                string strError = CsErrLog.GetExceptionInfo(ex) + "*" + Environment.NewLine + pSQL;
                string strLogInfo = strError + Environment.NewLine + pSQL;
                string strlogFile = ClsPublic.KingdeeLogPath + DateTime.Now.ToString("yyyy-MM-dd");
                CsPublicOA.WriteLog(ClsPublic.KingdeeLogPath, strlogFile, strLogInfo);

                throw new Exception(strLogInfo);
            }
        }

        public static DynamicObjectCollection GetDynamicObjects(Context pContext, string pSQL)
        {
            try
            {
                pSQL = "/*dialect*/" + pSQL;
                DynamicObjectCollection ds = DBUtils.ExecuteDynamicObject(pContext, pSQL);
                //DataSet ds = ServiceHelper.ExecuteDataSet(pContext, pSQL);

                return ds;
            }
            catch (Exception ex)
            {
                string strError = CsErrLog.GetExceptionInfo(ex) + "*" + Environment.NewLine + pSQL;
                string strLogInfo = strError + Environment.NewLine + pSQL;
                string strlogFile = ClsPublic.KingdeeLogPath + DateTime.Now.ToString("yyyy-MM-dd");
                CsPublicOA.WriteLog(ClsPublic.KingdeeLogPath, strlogFile, strLogInfo);

                throw new Exception(strLogInfo);
            }
        }


        public static string GetBillFormId(Context pContext, string pBillTypeId)
        {
            string strSQL = string.Format(@"
Select  FBillFormID
  From  T_BAS_BILLTYPE
WHERE   FBillTypeId='{0}'", pBillTypeId);
            string strFBillFormID = GetTopValue(pContext, null, strSQL);
            return strFBillFormID;
        }

  
        public static string GetJson2Kingdee(Context pContext, K3CloudApiClient pK3CloudApiClient, string pFormId, string pFMiddleID, string pstrJsonField)
        {
            try
            {


                string strSQL = string.Format(@"/*dialect*/
SELECT TOP 1
CreateTime, {0}
FROM  tbl2Kingdee with(nolock) 
WHERE   FormId='{1}'
AND     FMiddleID='{2}'
Order by ID Desc ", pstrJsonField, pFormId, pFMiddleID);

                DynamicObjectCollection doc1 = CsData.GetDynamicObjects(pContext, pK3CloudApiClient, strSQL);
                if (doc1.Count == 0)
                    return "";

                DynamicObject do1 = doc1[0];
                string strCreateTime = Convert.ToString(do1["CreateTime"]);
                string strReturn1 = "同步时间：" + strCreateTime + Environment.NewLine + Environment.NewLine;
                strReturn1 += "同步报文：" + Environment.NewLine;
                string strJson = Convert.ToString(do1[pstrJsonField]);

                if (strJson.Substring(0, 1) == "{")
                {
                    JObject JObject1 = JObject.Parse(strJson);
                    strJson = JObject1.ToString();
                }


                //没有效果，/r/n 还是存在。
                //string strDecodedJson = System.Web.HttpUtility.HtmlDecode(strJson);
                //注意，不要把\当成转义符，而当成普通字符。
                string strDecodedJson = strJson.Replace("\\r\\n", Environment.NewLine);

                strReturn1 += strDecodedJson;

                return strReturn1;
            }
            catch (Exception ex)
            {
                throw new Exception(CsErrLog.GetExceptionWithLog(ex));
            }

        }


        public static string GetJSon(DynamicObject[] pDataEntitys
            , Context pContext
            , string pFormId, string pFormOperation
             , bool pTranslate)
        {
            string strReturn = "";
            foreach (DynamicObject DO1 in pDataEntitys)
            {
                strReturn = GetJSon2(DO1, pContext, pFormId, pFormOperation, pTranslate);
                break;
            }
            return strReturn;
        }

        public static string GetJSon2(DynamicObject pDO1, Context pContext
            , string pFormId, string pFormOperation
            , bool pTranslate)
        {

            string strBillId = pDO1["Id"].ToString();
            string strFId = Convert.ToString(pDO1["Id"]);


            string strFBillNo;
            if (pDO1.Contains("Number"))
                strFBillNo = Convert.ToString(pDO1["Number"]);  //适用于部门。
            else if (pDO1.Contains("BillNo"))
                strFBillNo = Convert.ToString(pDO1["BillNo"]);  //适用于凭证。
            else
                throw new Exception("没法读到单据编号，请洽程序设计员。CsData.GetJSon2");

            string strReturn = CsPublicOA.GetJsonOA(pContext, null, pFormId, pFormOperation, strFId, strFBillNo, pTranslate);

            return strReturn;
        }

        public static void GetSyncBillTypeInfo(Context ctx, K3CloudApiClient pK3CloudApiClient
            , Struct_K3LoginInfo pStruct_K3LoginInfo
            , int pWDT2Where, ref string pStartDate, ref string pEndDate
            , int pintBillType
            , out string pFormText, out string pFormIdMid, out string pFormIdK3
            , out string pTableNameMidd, out string pTableNameK3, out string pOtherURL
            , out string pCleanProcedure
            , out string pDelDupicateProcedure
            , out string pOthereDateField, out string pOthereBillNoField
            , out string pPushRule

           )
        {
            try
            {
                string strFilter;
                if (pWDT2Where == WDT2.MiddleTable)
                    strFilter = " Sync2Middle=1  and isnull(OthersURL,'')<>''";
                else if (pWDT2Where == WDT2.OnlyCount)
                    strFilter = " Sync2Count=1  and isnull(OthersURL,'')<>'' ";
                else if (pWDT2Where == WDT2.K3)
                    strFilter = " Sync2K3=1";
                else
                    throw new Exception("在GetSyncBillTypeInfo里，未知同步行为：pWDT2Where=" + pWDT2Where);

                string strSQL = string.Format(@"
SELECT 
       SyncBillTypeText
      ,SyncBillTypeMidd
      ,SyncBillTypeK3
      ,TableNameMidd
      ,TableNameK3
      ,CleanProcedure,DelDupicateProcedure
      ,OthersURL
      ,GetLastDate,BeginTime,EndTime
      ,OthereDateField,OthereBillNoField
      ,PushRule
  FROM tblSyncBillTypeCBS
where   {0}
and   SyncBillTypeId={1}
order by SyncBillTypeId", strFilter, pintBillType);
                DynamicObjectCollection doc1 = CsData.GetDynamicObjects(ctx, pK3CloudApiClient, strSQL);
                if (doc1.Count != 1)
                    throw new Exception("在GetSyncBillTypeInfo里，取tblSyncBillTypeCBS时，行数为=" + doc1.Count);

                DynamicObject do1 = doc1[0];

                pFormText = Convert.ToString(do1["SyncBillTypeText"]);
                pFormIdMid = Convert.ToString(do1["SyncBillTypeMidd"]);
                pFormIdK3 = Convert.ToString(do1["SyncBillTypeK3"]);
                pTableNameMidd = Convert.ToString(do1["TableNameMidd"]);
                pTableNameK3 = Convert.ToString(do1["TableNameK3"]);
                pCleanProcedure = Convert.ToString(do1["CleanProcedure"]);
                pDelDupicateProcedure = Convert.ToString(do1["DelDupicateProcedure"]);
                pOtherURL = Convert.ToString(do1["OthersURL"]);
                pOthereDateField = Convert.ToString(do1["OthereDateField"]);
                pOthereBillNoField = Convert.ToString(do1["OthereBillNoField"]);
                pPushRule = Convert.ToString(do1["PushRule"]);

                string strGetLastDate = Convert.ToString(do1["GetLastDate"]);
                bool bolGetLastDate = false;
                if (strGetLastDate != "")
                {
                    bolGetLastDate = Convert.ToBoolean(strGetLastDate);
                }

                //先从单据类型里的参数读取。
                if (pStartDate == null || pStartDate == "")
                {
                    string strBeginTimeBill = Convert.ToString(do1["BeginTime"]);
                    if (strBeginTimeBill != "")
                    {
                        DateTime datBeginTimeBill = Convert.ToDateTime(strBeginTimeBill);
                        if (datBeginTimeBill.Year >= System.DateTime.Today.Year)
                            pStartDate = datBeginTimeBill.ToString("yyyy-MM-dd");
                        else if (pStruct_K3LoginInfo.SyncStartDate != "")
                        {
                            datBeginTimeBill = Convert.ToDateTime(pStruct_K3LoginInfo.SyncStartDate);
                            pStartDate = datBeginTimeBill.ToString("yyyy-MM-dd"); //然后，从公有的参数中读取。
                        }
                    }
                }

                if (pEndDate == null || pEndDate == "")
                {
                    string strEndTimeBill = Convert.ToString(do1["EndTime"]);
                    if (strEndTimeBill != "")
                    {
                        DateTime datEndTimeBill = Convert.ToDateTime(strEndTimeBill);
                        if (datEndTimeBill.Year >= 2024)
                            pEndDate = datEndTimeBill.ToString("yyyy-MM-dd");
                    }
                }


                if (pStartDate == "")
                {
                    throw new Exception("tblSynchroParaCBS中，没有配置，同步开始日期");
                }


                if (pEndDate == "" || bolGetLastDate == true)
                {
                    pEndDate = pStruct_K3LoginInfo.SyncEndDate;
                }


            }
            catch (Exception ex)
            {
                string strError = CsErrLog.GetExceptionInfo(ex);
                throw new Exception(strError);
            }
        }

    }
}
