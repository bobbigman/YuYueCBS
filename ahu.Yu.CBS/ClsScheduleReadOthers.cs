

using Kingdee.BOS;
using Kingdee.BOS.Contracts;
using Kingdee.BOS.Core;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.Util;
using Kingdee.BOS.WebApi.Client;
using System;
using System.ComponentModel;

namespace ahu.YuYue.CBS
{

    [Kingdee.BOS.Util.HotUpdate]

    public struct Struct_K3LoginInfo
    {
        public string ServerURL_K3;
        public string AcctID;
        public string Username;
        public string Password;
        public string OrganizationNum;
        public string OrganizationId;
        public string CurrID;
        public string SyncStartDate, SyncEndDate;
        public int SyncDateStep;
        public int BatchSaveCount;

        public string ServerURL_Others;
        public string CustomPrivateKey;
        public string PlatformPublicKey;
        public string OthersPageSize;
        public string Token;
        public string AppId, AppSecret;

        public string SyncWithLastError;

        //下面语句，有问题。
        //在执行计划里，手工点击时正常.
        //自动执行时，为-1
        //string strOrganizationId = ctx.CurrentOrganizationInfo.ID.ToString();

        public int Lcid;
        public bool LoginK3Ok;
        public string FormOperation;

        public TimeSpan iisResetTime;

    }


    [Kingdee.BOS.Util.HotUpdate]
    public class ClsScheduleReadOthers : IScheduleService
    {

        bool mbolRefreshPdf;
        public void RefreshPdf(bool pRefresh)
        {
            mbolRefreshPdf = pRefresh;
        }

        public void Run(Context ctx, Schedule schedule)
        {
            Struct_K3LoginInfo Struct_K3LoginInfo1 = new Struct_K3LoginInfo();
            string strReturns = ReadAnd2MiddleTableOrK3(ctx, null, ref Struct_K3LoginInfo1, WDT2.MiddleTable, "", "", "", 0, "", K3DatabaseMode.IntegrationK3, true);
            if (strReturns != "")
                throw new Exception(strReturns);

        }

        public string ReadAnd2MiddleTableOrK3(Context ctx, K3CloudApiClient pK3CloudApiClient
            , ref Struct_K3LoginInfo pStruct_K3LoginInfo
            , int pWDT2Where, string pStartDate, string pEndDate, string pBillNo
            , int pintBillType, string pMiddleId, int p2DatabaseType
             , bool pIsSchedule
           )
        {
            try
            {
                string strReturns = "";

                bool bolNeedLogInK3 = false;
                string strReturnType = "";
                int IntReturn = ClsPublic.LogIn(ref pStruct_K3LoginInfo, ctx
                    , ref pK3CloudApiClient, ref strReturnType, bolNeedLogInK3, p2DatabaseType);
                if (IntReturn != 1)
                    return strReturnType;

                ////同步前，删除交易明细中，对帐码为空的，或者，银行流水为空的。
                ///不能删除。比方说，一周前的记录，银行一直没有记帐码，那我就背锅了。
                ///改为：同步时，再删除吧。
                //string strSQL = "ahuDelBad_CashFlow_Midd";
                //CsData.BobExecute(ctx, pK3CloudApiClient, strSQL);

                string strError1 = "";
                string strToken = CsCallCBS.getToken(ctx, pK3CloudApiClient, p2DatabaseType, pStruct_K3LoginInfo, ref strError1);
                if (strError1 != "")
                    return "";

                if (pintBillType == 0)
                {
                    string strFilter;
                    if (pWDT2Where == WDT2.MiddleTable)
                        strFilter = " Sync2Middle=1";
                    else if (pWDT2Where == WDT2.OnlyCount)
                        strFilter = " Sync2Count=1";
                    else
                        return "未知同步行为。";

                    string strSQL = string.Format(@"
SELECT SyncBillTypeId
FROM   tblSyncBillTypeCBS
where   {0}
and isnull(OthersURL,'')<>''
order by SyncBillTypeId
", strFilter);
                    DynamicObjectCollection doc1 = CsData.GetDynamicObjects(ctx, pK3CloudApiClient, strSQL);
                    foreach (DynamicObject do1 in doc1)
                    {
                        int intSyncBillTypeId = Convert.ToInt32(do1["SyncBillTypeId"]);

                        string strReturn1 = Only1BillType(ctx, pK3CloudApiClient, ref pStruct_K3LoginInfo, pWDT2Where
                             , pStartDate, pEndDate, "0"
                             , pBillNo, intSyncBillTypeId, p2DatabaseType, strToken, pIsSchedule);

                        if (strReturn1 != "")
                        {
                            if (strReturns != "")
                            {
                                strReturns += Environment.NewLine;
                            }

                            strReturns += strReturn1;
                        }

                    }
                }

                else
                {
                    strReturns = Only1BillType(ctx, pK3CloudApiClient, ref pStruct_K3LoginInfo, pWDT2Where
                        , pStartDate, pEndDate, "0"
                        , pBillNo, pintBillType, p2DatabaseType, strToken, pIsSchedule);
                }


                return strReturns;
            }
            catch (Exception ex)
            {
                string strError = CsErrLog.GetExceptionInfo(ex);
                throw new Exception(strError);
            }
        }


        public string Only1BillType(Context ctx, K3CloudApiClient pK3CloudApiClient
            , ref Struct_K3LoginInfo pStruct_K3LoginInfo
            , int pWDT2Where, string pStartDate, string pEndDate
            , string pPageByExe
            , string pBillNo
            , int pintBillType
            , int pK3DatabaseType
            , string pToken, bool pIsSchedule)
        {
            try
            {
                ////2024/4/24 17:50 龙光
                ////从前台调试执行计划，取第三方系统的单据个数，出错了。因为ctx是null，pK3CloudApiClient也是null
                //Boolean bolNeedLogInK3 = false;
                //if (pWDT2Where == WDT2.K3 || pWDT2Where == WDT2.OnlyCount)
                //    bolNeedLogInK3 = true;

                //string strReturnType = "";
                //int IntReturn = ClsPublic.LogIn(ref pStruct_K3LoginInfo, ctx
                //    , ref pK3CloudApiClient, ref strReturnType, bolNeedLogInK3, pK3DatabaseType);
                //if (IntReturn != 1)
                //    return strReturnType;


                if (pintBillType==0)
                    return "";//不需要同步;

                if (pPageByExe == "")
                    pPageByExe = "0";

                CsReadWdt CsReadWdt1 = new CsReadWdt();

                string strReturn = "";
                CsReadWdt1.RefreshPdf(mbolRefreshPdf);

                strReturn = CsReadWdt1.ReadWDT_LoopPeriod(
                    ctx, pK3CloudApiClient, pStruct_K3LoginInfo, pBillNo
                    , pStartDate, pEndDate, pPageByExe
                    , pintBillType
                    , pWDT2Where, pK3DatabaseType
                    , pToken, pIsSchedule);

                return strReturn;


            }
            catch (Exception ex)
            {
                throw new Exception(CsErrLog.GetExceptionWithLog(ex));
            }
        }



        public void GetK3LoginInfo(Context pContext, K3CloudApiClient pK3CloudApiClient, ref Struct_K3LoginInfo pLoginInfo)
        {
            try
            {
                if (pLoginInfo.ServerURL_Others != null)
                    return;

                pLoginInfo.ServerURL_Others = GetK3SynchroPara(pContext, pK3CloudApiClient, "ServerURL_Others");
                pLoginInfo.ServerURL_K3 = GetK3SynchroPara(pContext, pK3CloudApiClient, "ServerURL_K3");

                pLoginInfo.AcctID = GetK3SynchroPara(pContext, pK3CloudApiClient, "AcctID");

                if (pContext != null && pLoginInfo.AcctID != pContext.DBId)
                    throw new Exception(pContext.DataCenterNumber + "|" + pContext.DBId + pContext + " 同步账套Id参数异常(" + pLoginInfo.AcctID + ")，请洽程序设计员，");

                pLoginInfo.Username = GetK3SynchroPara(pContext, pK3CloudApiClient, "Username");
                pLoginInfo.Password = GetK3SynchroPara(pContext, pK3CloudApiClient, "Password");
                pLoginInfo.OrganizationNum = GetK3SynchroPara(pContext, pK3CloudApiClient, "OrganizationNum");
                pLoginInfo.OrganizationId = GetK3SynchroPara(pContext, pK3CloudApiClient, "OrganizationId");
                pLoginInfo.CurrID = GetK3SynchroPara(pContext, pK3CloudApiClient, "CurrID");

                string strSyncBeforeDays = GetK3SynchroPara(pContext, pK3CloudApiClient, "SyncBeforeDays");
                if (strSyncBeforeDays == "")
                    strSyncBeforeDays = "7";

                int intBeforeDays = Convert.ToInt32(strSyncBeforeDays);
                intBeforeDays *= -1;
                string strSyncStartDate = DateTime.Now.AddDays(intBeforeDays).ToShortDateString();
                pLoginInfo.SyncStartDate = strSyncStartDate;

                string strSyncEndDays = GetK3SynchroPara(pContext, pK3CloudApiClient, "SyncEndDays");
                if (strSyncEndDays == "")
                    strSyncEndDays = "1";

                //SyncBeforeDaysEnd
                int intEndDays = Convert.ToInt32(strSyncEndDays);
                intEndDays *= -1;

                string strSyncEndDate = DateTime.Now.AddDays(intEndDays).ToShortDateString();
                pLoginInfo.SyncEndDate = strSyncEndDate;

                string strSyncDateStep = GetK3SynchroPara(pContext, pK3CloudApiClient, "SyncDateStep");
                if (strSyncDateStep == "") strSyncDateStep = "2";
                pLoginInfo.SyncDateStep = Convert.ToInt32(GetK3SynchroPara(pContext, pK3CloudApiClient, "SyncDateStep"));

                pLoginInfo.BatchSaveCount = Convert.ToInt32(GetK3SynchroPara(pContext, pK3CloudApiClient, "BatchSave"));

                pLoginInfo.Token = GetK3SynchroPara(pContext, pK3CloudApiClient, "Token");
                pLoginInfo.CustomPrivateKey = GetK3SynchroPara(pContext, pK3CloudApiClient, "CustomPrivateKey");
                pLoginInfo.PlatformPublicKey = GetK3SynchroPara(pContext, pK3CloudApiClient, "platformPublicKey");
                pLoginInfo.AppSecret = GetK3SynchroPara(pContext, pK3CloudApiClient, "AppSecret");
                pLoginInfo.AppId = GetK3SynchroPara(pContext, pK3CloudApiClient, "AppId");
                pLoginInfo.SyncWithLastError = GetK3SynchroPara(pContext, pK3CloudApiClient, "SyncWithLastError");

                string striisResetTime = GetK3SynchroPara(pContext, pK3CloudApiClient, "iisResetTime");
                TimeSpan.TryParse(striisResetTime, out TimeSpan iisResetTime1);
                pLoginInfo.iisResetTime = iisResetTime1;

                pLoginInfo.OthersPageSize = GetK3SynchroPara(pContext, pK3CloudApiClient, "WDTPageSize");

                pLoginInfo.iisResetTime = iisResetTime1;


                pLoginInfo.Lcid = 2052;

                //pLoginInfo.SubSystemId = "21";


            }
            catch (Exception ex)
            {

                //clsErrLog clsErrLog1 = new clsErrLog();
                //clsErrLog1.Exception2SQL(pContext, ex, mstrForm);
                string strError = CsErrLog.GetExceptionWithLog(ex);
                throw new Exception(strError);
            }

        }

        public string GetK3SynchroPara(Context pContext, K3CloudApiClient pK3CloudApiClient, string pFName)
        {

            string strSQL = string.Format(@"/*dialect*/
SELECT   TOP 1 FValue
FROM     tblSynchroParaCBS  with(nolock)
WHERE   (FName = '{0}') ", pFName);
            string strReturn = CsData.GetTopValue(pContext, pK3CloudApiClient, strSQL);
            return strReturn;

        }



    }







}

