using Kingdee.BOS;
using Kingdee.BOS.Util;
using Kingdee.BOS.WebApi.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace ahu.YuYue.CBS
{
    [Description("通用模块，同步到金蝶,新写的，CBS业务，通用各单据哦。")]
    [Kingdee.BOS.Util.HotUpdate]
    public class CsReadWdt
    {  
        //测试提交git.
        K3CloudApiClient mK3CloudApiClient;
        Struct_K3LoginInfo mStruct_K3LoginInfo1;

        string mstrFormIdMid, mstrFormIdK3, mstrTableNameMidd, mstrTableNameK3;
        string mstrOtherURL, mstrCleanProcedure, mstrDelDupicateProcedure, mstrOthereDateField, mstrOthereBillNoField;

        private Context mContext1;


        bool mbolRefreshPdf;
        public void RefreshPdf(bool pRefresh)
        {
            mbolRefreshPdf = pRefresh;
        }

        public string ReadWDT_LoopPeriod(Context pContext, K3CloudApiClient pK3CloudApiClient1
            , Struct_K3LoginInfo pStruct_K3LoginInfo1
            , string pBillNo
            , string pStartDate, string pEndDate
            , string pStartPage
            , int pIntBillType
            , int pWDT2Where
            , int pK3DatabaseType
            , string pToken, bool pIsSchedule)
        {
            try
            {
                mContext1 = pContext;
                mK3CloudApiClient = pK3CloudApiClient1;
                mStruct_K3LoginInfo1 = pStruct_K3LoginInfo1;

                int intPage1 = 0, intPage2 = 99999;

                if (pStartPage != "")
                {
                    int intPage_Parame = Convert.ToInt32(pStartPage);
                    intPage1 = intPage_Parame;
                }

                pEndDate = pEndDate.Replace("00:00:00", "23:59:59");

                string strSyncBillTypeText;
                string strPushRuldId; //仅为兼容
                CsData.GetSyncBillTypeInfo(pContext, mK3CloudApiClient, mStruct_K3LoginInfo1
                           , pWDT2Where, ref pStartDate, ref pEndDate, pIntBillType
                   , out strSyncBillTypeText, out mstrFormIdMid, out mstrFormIdK3
                   , out mstrTableNameMidd, out mstrTableNameK3, out mstrOtherURL
                   , out mstrCleanProcedure, out mstrDelDupicateProcedure
                   , out mstrOthereDateField, out mstrOthereBillNoField, out strPushRuldId);


                string strError = "";
                string strErrors = "";

                //有时，根据单号查询，这两个，也要赋值。不然，下面引用时，会报错：使用了未赋值的变量。
                DateTime dtEndDate;
                DateTime dtStartDate;

                //这里不能省，因为，本方法要用到。虽然，和上面重复了。
                Boolean bolNeedLogInK3 = false;
                if (pWDT2Where == WDT2.K3 || pWDT2Where == WDT2.OnlyCount)
                    bolNeedLogInK3 = true;

                string strReturnType = "";
                int IntReturn = ClsPublic.LogIn(ref mStruct_K3LoginInfo1, pContext
                    , ref mK3CloudApiClient, ref strReturnType, bolNeedLogInK3, pK3DatabaseType);
                if (IntReturn != 1)
                    return strReturnType;

                //客户，不需要传日期，仅为兼容。所以，预设一个。但是，不能设为今天，否则，不能进放循环。
                //先给一个默认值。
                dtStartDate = Convert.ToDateTime(pStartDate);

                //大多数时，不要用默认值。
                if (pStartDate.IsNullOrEmptyOrWhiteSpace() == false)
                    dtStartDate = Convert.ToDateTime(pStartDate);

                dtEndDate = DateTime.Now;
                if (pEndDate.IsNullOrEmptyOrWhiteSpace() == false)
                    dtEndDate = Convert.ToDateTime(pEndDate);

                // 计算两个日期之间的时间间隔  
                TimeSpan duration = dtEndDate.Subtract(dtStartDate);

                // 获取时间间隔中的天数  
                int intDaysBetween = duration.Days;

                //if (intDaysBetween >= 20)
                //    return "程序员限定了：日期范围，不能超过20天。原因：不要耗费资源。";


                if (pBillNo != "")
                {
                    intPage1 = 1;   //有传单据编号，只查第1页就行。
                    strError = ReadWDT_1Period(pBillNo, dtStartDate, dtEndDate
                        , ref intPage1, pWDT2Where
                         , pToken);
                    return strError;
                }

                //避免死锁。
                if (pIsSchedule == true && pWDT2Where == WDT2.MiddleTable)
                {
                    string strSQL2 = string.Format(@"
SELECT TOP 1  FCREATEDATE
FROM {0} 
order by FId desc
", mstrTableNameMidd);
                    string strFCREATEDATE = CsData.GetTopValue(pContext, pK3CloudApiClient1, strSQL2);

                    DateTime createdDate;
                    if (DateTime.TryParse(strFCREATEDATE, out createdDate))
                    {
                        // 获取当前时间
                        DateTime now = DateTime.Now;

                        // 计算两个时间点之间的差距
                        TimeSpan diff2 = now - createdDate;

                        // 判断如果相差在10分钟以内，则返回错误信息
                        //比如说，1分钟这前，还有在同步，就退出。
                        if (diff2.TotalMinutes <= 10)
                        {
                            return "侦测到同步在其它线程运行，退出，避免死锁。";
                        }
                    }

                }

                while (dtStartDate <= dtEndDate)
                {
                    DateTime dtEndDate1 = GetFilterEndDateTime(dtStartDate, dtEndDate);

                    //仅为兼容，其它同步接口，要分页，要限定，日期查询范围。
                    for (int intPage = intPage1; intPage <= intPage2; intPage++)
                    {
                        //一定要加下面的判断。否则，会出错：正在中止线程。
                        //如果iisreset 重启了，context不能再调用了。
                        TimeSpan currentTime = DateTime.Now.TimeOfDay;
                        TimeSpan restartTime = pStruct_K3LoginInfo1.iisResetTime;
                        //奇怪，没有退出来，难道是时间不够？
                        TimeSpan timeBefore = restartTime - TimeSpan.FromMinutes(10);
                        TimeSpan timeAfter = restartTime + TimeSpan.FromMinutes(10);

                        if (currentTime >= timeBefore && currentTime <= timeAfter)
                        {
                            // 到了IIS 重启时间，就退出来算了。
                            //不退出来，我也干不了什么，context， 都不能用了。
                            //等再长时间，也是白等
                            return "不同步了，快到了回收IIS时间（" + pStruct_K3LoginInfo1.iisResetTime + "），过20分钟再开干吧。*";
                        }


                        strError = ReadWDT_1Period(pBillNo, dtStartDate, dtEndDate1, ref intPage, pWDT2Where, pToken);

                        if (strError != "")
                        {
                            if (strErrors != "")
                                strErrors += Environment.NewLine;

                            strErrors += strError;

                            if (strError.Substring(strError.Length - 1) == "*")
                                return strErrors;
                        }

                        //晕死，bug,跳页了。2024/7/16 For 中，有加一页的。
                        //intPage += 1;
                    }

                    //其实没必要，上面判断过了。不过，放在这里，也没坏处。就当个提醒。
                    if (pBillNo != "")
                        break;

                    dtStartDate = GetFilterStartDateTime(dtEndDate1);


                }

                //希望在这里写，能兼顾到：销售出库，和销售退货。 2023/3/19
                if (strErrors != "")
                {
                    string strLogInfo = mstrFormIdK3 + Environment.NewLine + strErrors;
                    string strlogFile = ClsPublic.KingdeeLogPath + DateTime.Now.ToString("yyyy-MM-dd");
                    ClsPublic.WriteLog(ClsPublic.KingdeeLogPath, strlogFile, strLogInfo);
                }

                string strFId_MiddleClean = "0";
                string strSQL = string.Format(@"exec {0} {1}  ", mstrCleanProcedure, strFId_MiddleClean);
                strSQL += Environment.NewLine + string.Format(@"exec {0}  ", mstrDelDupicateProcedure);
                CsData.BobExecute(mContext1, mK3CloudApiClient, strSQL);

                //2024/2/24 18:06
                //改为成功，为什么呢？我在前台调试时，总要显示点东西。
                //什么前台？ frmEPRTest,点按钮：取单据统计(模拟执行计划)
                //还要，strFormId也是这次增加的。增加原因，我希望返回：各单据类型的同步情况。
                if (strErrors == "" && mContext1 == null)
                {
                    strErrors = "执行成功。";
                    return mstrFormIdK3 + Environment.NewLine + strErrors;
                }

                return strErrors;
            }
            catch (Exception ex)
            {
                //clsErrLog clsErrLog1 = new clsErrLog();
                //clsErrLog1.Exception2SQL(this.mContext1, ex, mstrMyClass);
                return CsErrLog.GetExceptionWithLog(ex);
            }
            finally
            {

            }
        }


        public string ReadWDT_1Period(string pFNumber, DateTime pStartDate, DateTime pEndDate, ref int pPageNo
              , int pWDT2Where, string pToken)
        {

            string strFilterTime = "";

            try
            {
                string strStartDate1 = pStartDate.ToString("yyyy-MM-dd");
                string strEndDate1 = pEndDate.ToString("yyyy-MM-dd");

                //CBS不支持：时分秒格式。
                //string strEndDate1 = pEndDate.AddDays(1).AddSeconds(-2).ToString("yyyy-MM-dd HH:mm:ss");

                //粮库不是公有云，能记录啊。
                string strDirectory_StartDate = pStartDate.ToString("yyyyMMdd");

                strFilterTime = string.Format("日期范围从{0}到{1},页数{2}", strStartDate1, strEndDate1, pPageNo);
                if (pFNumber.IsNullOrEmptyOrWhiteSpace() == false)
                {
                    strFilterTime = "按编号" + pFNumber + "过滤。";

                    //CBS中，不能拿掉。2024/7/10 7:07
                    //能用到。
                    //strStartDate1 = "";
                    //strEndDate1 = "";
                }

                if (pWDT2Where == WDT2.OnlyCount)
                {
                    //取当前单据总数前，检查一下，如果在日期范围内，如果取过了，就不取了。
                    string strSQL = string.Format(@"/*dialect*/
Select Top 1 WDTCount 
From [tblReadWDTCount] with (nolock)
Where FormId='{0}'
AND   SyncURL='{1}'", mstrFormIdK3, strFilterTime);
                    string strValue = CsData.GetTopValue(mContext1, mK3CloudApiClient, strSQL);

                    //拿掉 && strValue != "0"，不希望重复。 2023/3/16 9:12
                    //为什么拿掉呢？ 发现计算错了。2024/1/27 8:45 于幸福小区
                    if (strValue != "" && strValue != "0")
                    {
                        //该日期范围，已有值。
                        pPageNo = 9999999;
                        return "";
                    }
                }

                string strError = "";
                string strSyncInfo = "";
                JToken JToken_WDT = GetDataFromWDT(strStartDate1, strEndDate1, pFNumber
                                 , ref strError, pPageNo, ref strSyncInfo
                                 , pToken);
                if (strError != "")
                {
                    pPageNo = 9999999;
                    strError += Environment.NewLine + "同步时间:" + DateTime.Now + Environment.NewLine + strSyncInfo + "*";
                    return strError;
                }

                //记录下报文，注意，只记录第一次。
                //以日期，做为目录，详细过滤条件，strFilterTime，作为文件名
                //D:\WorkSpace\kingdeeLog

                //出错了，就不要跑了，要不然，循环，也是浪费资源，对吧？

                string strFilterTimeLog = strFilterTime.Replace(",", "，第 ");

                if (pFNumber.IsNullOrEmptyOrWhiteSpace() == false)
                    strFilterTimeLog += " 页";

                string strWDTLog = "同步条件：" + strFilterTimeLog + Environment.NewLine + new String('*', 50) + Environment.NewLine;

                if (JToken_WDT.ToString().IndexOf("data") == -1)
                {
                    //异常来了，别死循环了。
                    strError = JToken_WDT.ToString() + Environment.NewLine + "同步时间:" + DateTime.Now + Environment.NewLine + strSyncInfo + "*";
                    pPageNo = 9999999;
                    return strError;
                }

                JToken JToken_Data = JToken_WDT["data"];
                string strTotal_count = Convert.ToString(JToken_Data["total"]);

                //只适用于CBS接口，其它的，记得要改。********************************************
                string strPageNumReadFromOthers = Convert.ToString(JToken_Data["pageNum"]);
                int intPageNumReadFromOthers = Convert.ToInt16(strPageNumReadFromOthers);
                if (pPageNo > intPageNumReadFromOthers)
                {
                    //读不出来啦，别死循环了。
                    pPageNo = 9999999;
                    return strError;
                }


                //如果是公有云，记得拿掉，不要调用。
                //调试完，就要记得拿掉。
                string strPath = ClsPublic.KingdeeLogPath + mstrFormIdK3 + strDirectory_StartDate;

                strWDTLog = "日志时间：" + DateTime.Now.ToString() + Environment.NewLine + mstrFormIdK3 + "：" + strFilterTime;

                strWDTLog += "返回数=" + strTotal_count;
                strWDTLog += "返回数=" + JToken_WDT.ToString();

                if (strError != "")
                    strWDTLog += strError;

                strWDTLog += JToken_WDT.ToString();
                string strlogFile = strFilterTime;

                //2024/9/15 20:39 周日 多云 于幸福小区 骑单车回来
                //先停下来吧，别把人硬盘弄爆了。只是调试时，用一下而已。
                //记得要停下来。这次，联调电子回单，暂时记一下哈。！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！
                //ClsPublic.WriteWDTLogWithOutCheckFile(strPath, strlogFile, strWDTLog);

                if (strError != "")
                {
                    pPageNo = 9999999;
                    return strError;
                }

                if (JToken_Data.IsNullOrEmptyOrWhiteSpace() || JToken_Data.HasValues == false || strTotal_count == "0")
                {
                    //没有数据，没必要继续执行了吗？2023/01/22 16:53
                    pPageNo = 9999999;

                    if (pFNumber != "")
                    {
                        strError = "同步" + mstrFormIdK3 + "时，接口取不到值，编号=" + pFNumber + ".";
                        strError += Environment.NewLine + DateTime.Now + Environment.NewLine + strSyncInfo + "*";
                        return strError;
                    }

                    if (pPageNo != 0)
                    {
                        //分页参数错误，不用返回错误。
                        //但是，为什么要返回呢，2023/11/23 9:43 
                        //读不到数据，就要返回，是吧？ 有道理。不过，同步到K3可以，但是，单据数量统计，还是计录一下更好。
                        //return "";

                        strError = "";

                    }

                    return strError;

                }


                switch (pWDT2Where)
                {
                    case WDT2.MiddleTable:
                        //如果电商，要加上判断：当前页有多少行，中间表有多少行。
                        //如果同步完了，就不要写了，节约时间。
                        //但是，银行就算了。撑死也没多少单据

                        Post2MiddleTable(JToken_Data, pStartDate.ToString("yyyy-MM-dd"));

                        break;

                    case WDT2.UpdateCheckCode:
                        if (pFNumber == "")
                        {
                            pPageNo = 9999999;
                            return "意外错误，请洽程序设计员。要更新对帐码，但是，没有传单据编号。";
                        }
                        if (strTotal_count != "1")
                        {
                            pPageNo = 9999999;
                            return "意外错误，请洽程序设计员。要更新对帐码，但是，当前取到的记录为=" + strTotal_count;
                        }

                        JArray JarrWDT = (JArray)JToken_Data["list"];
                        JObject JObject_Head_Mes = JarrWDT[0] as JObject;
                        string strBankSerialNumber = Convert.ToString(JObject_Head_Mes["bankSerialNumber"]);
                        string strCheckCode = Convert.ToString(JObject_Head_Mes["checkCode"]);

                        if (strCheckCode.IsNullOrEmptyOrWhiteSpace())
                        {
                            pPageNo = 9999999;
                            return "对帐码为空";
                        }
                        if (strBankSerialNumber.IsNullOrEmptyOrWhiteSpace())
                        {
                            pPageNo = 9999999;
                            return "银行流水号为空";
                        }
                        string strFReceiptNo = strCheckCode + "_" + strBankSerialNumber;

                        strError = UpdateCheckCode(pFNumber, strFReceiptNo, strCheckCode);


                        break;

                    default:
                        throw new Exception("意外错误，请退出系统，重试一遍。如果问题仍然存在，请洽程序设计员。");
                }

                if (strError.IsNullOrEmptyOrWhiteSpace() == false)
                    strError = strFilterTime + Environment.NewLine + strError;

                return strError;
            }
            catch (Exception ex)
            {
                //clsErrLog clsErrLog1 = new clsErrLog();
                //clsErrLog1.Exception2SQL(this.mContext1, ex, mstrMyClass);
                string strError = strFilterTime + Environment.NewLine + CsErrLog.GetExceptionWithLog(ex);
                return strError;
            }
            finally
            {

            }
        }



        public JToken GetDataFromWDT(string pStartDate, string pEndDate, string pFBillNo, ref string pError
            , int pPageNo, ref string pSycnInfo, string pToken)
        {
            try
            {
                //2024/7/7 16:03
                //注意：费了很大的劲，传过来日期，这里没起作用。

                //赋值，只是为了返回不报错。
                DateTime dtStartDate = DateTime.Now;
                string strCode = "", strMessage = "";
                pError = "";

                //如果不按编号，而是日期范围来查，就要检查一下，起始范围，不能找到今天。
                if (pFBillNo == "")
                {
                    string strToday = System.DateTime.Today.ToString("yyyy-MM-dd");
                    DateTime dtCheck = Convert.ToDateTime(pStartDate);
                    string strCheck = dtCheck.ToString("yyyy-MM-dd");

                }

                string strJson;

                switch (mstrFormIdK3)
                {

                    case K3FormId.strWB_RecBankTradeDetail:

                        //注意顺序。2024/7/10 9:54
                        var parameters = new Dictionary<string, string>
{
    { "startDate", pStartDate },
    { "endDate", pEndDate },
    { "dateType", "0" },
    { "currentPage", pPageNo.ToString()},
    { "pageSize", mStruct_K3LoginInfo1.OthersPageSize }
};
                        if (string.IsNullOrWhiteSpace(pFBillNo) == false)
                            parameters.Add("transactionSerialNumber", pFBillNo);

                        strJson = JsonConvert.SerializeObject(parameters);

                        break;

                    case K3FormId.strWB_ReceiptBill:
                    case K3FormId.strWB_ReceiptBill_Attachment:
                        var JsonParameters = new Dictionary<string, object>
                        {
                            { "startDate", pStartDate },
                            { "endDate", pEndDate },
                            { "currentPage", pPageNo.ToString() },
                            { "pageSize", mStruct_K3LoginInfo1.OthersPageSize}
                        };

                        if (string.IsNullOrWhiteSpace(pFBillNo) == false)
                            JsonParameters.Add("checkCodeList", new List<string> { pFBillNo });

                        strJson = JsonConvert.SerializeObject(JsonParameters);

                        break;

                    default:
                        throw new Exception("switch中没有case：" + mstrFormIdK3 + "，CsReadWdt.GetDataFromWDT");
                }


                string strFullUrl = mStruct_K3LoginInfo1.ServerURL_Others + mstrOtherURL;

                int retryCount = 0; // 重试计数器  
                const int maxRetries = 1; // 最大重试次数  
                while (retryCount <= maxRetries)
                {
                    CsCallCBS CsCallCBS1 = new CsCallCBS();
                    string strResult = "";

                    string strReturn = "";

                    pSycnInfo = "同步网址：" + strFullUrl + Environment.NewLine + "同步报文" + strJson;

                    CsCallCBS1.CallCBSWithToken(mStruct_K3LoginInfo1, mstrOtherURL, strJson, ref strResult, ref pError, pToken);

                    if (pError != "")
                        return "";

                    if (strResult == "")
                    {
                        retryCount++;
                        continue;
                    }

                    try
                    {
                        JToken JToken_WDT = JObject.Parse(strResult);

                        strCode = JToken_WDT["code"].ToString();
                        strMessage = JToken_WDT["msg"].ToString();

                        if (strCode != "0")
                        {
                            //2023/3/13 9：28 同步销售退货单，没有进展。原来，同步出错了：超过每分钟最大频率。
                            pError = "网址：" + strFullUrl + Environment.NewLine + Environment.NewLine + "Code=" + strCode + "，Message=" + strMessage + " *";
                            return null;
                        }
                        return JToken_WDT;
                    }
                    catch (Exception ex)
                    {
                        retryCount++;

                        if (retryCount > maxRetries)
                        {
                            if (strResult == "")
                                strResult = "空";

                            pError = "读取CBS错，返回信息：" + strResult + "，共执行 " + retryCount + " 次。" + Environment.NewLine + Environment.NewLine + "*"; // 设置错误信息  
                            break;
                        }

                        Thread.Sleep(4000); // 等待3秒  
                    }

                }  // 如果需要重试，则继续循环 
                return null;
            }

            catch (Exception ex)
            {
                throw new Exception(CsErrLog.GetExceptionWithLog(ex));
            }
        }



        private DateTime GetFilterStartDateTime(DateTime dtPreviousEndDate)
        {


            DateTime dtStartDate = dtPreviousEndDate.AddDays(1);

            return dtStartDate;

        }
        private DateTime GetFilterEndDateTime(DateTime dtStartDate, DateTime dtLastDate)
        {

            DateTime dtEndDate1 = dtStartDate;
            if (dtEndDate1 > dtLastDate)
                dtEndDate1 = dtLastDate;

            return dtEndDate1;

        }

        private void Post2MiddleTable(JToken JToken_WDT
            , string pFDate)
        {
            try
            {
                CsOthers2MiddleTable CsCBS2MiddleTable1 = new CsOthers2MiddleTable();
                CsCBS2MiddleTable1.Call2MiddleTable(mContext1, mK3CloudApiClient
                    , mStruct_K3LoginInfo1, JToken_WDT, true
                    , mstrFormIdMid, mstrTableNameMidd
                    , mstrOthereDateField, mstrOthereBillNoField
                    , pFDate);

                return; ;
            }
            catch (Exception ex)
            {
                //clsErrLog clsErrLog1 = new clsErrLog();
                //clsErrLog1.Exception2SQL(this.mContext1, ex, mstrMyClass);
                throw new Exception(CsErrLog.GetExceptionWithLog(ex));
            }
        }

        private string UpdateCheckCode(string pBillNo, string FReceiptNo, string pCheckCode)
        {

            string strSQL = string.Format(@"/*dialect*/
Select top 1 FReceiptNo 
From T_CN_BANKCASHFLOW
Where FBillNo='{0}'", pBillNo);
            string strFReceiptNo = CsData.GetTopValue(mContext1, mK3CloudApiClient, strSQL);
            if (strFReceiptNo == FReceiptNo)
            {
                return "不需要更新，对帐码是一致的。";
            }

            /*--电子回单中间表清洗：
--FReceiptNo, 电子回单关联标记,FcheckCode+'_'+FbankSerialNumber

--电子回单，单据转换字段映射，
--FReceiptNo,电子回单编号：单据编号，电子回单编号（FBillNo，不是拚接的。），没问题,，虽然有点怪。
--FDetailNo，关联标记：电子回单关联标记（FReceiptNo，拚接的），也没问题，虽然有点怪。*/

            strSQL = string.Format(@"/*dialect*/
Update t_CashFlow_Midd  Set FReceiptNo='{0}',FcheckCode='{2}'
Where FBillNo='{1}'

Update T_CN_BANKCASHFLOW  Set FReceiptNo='{0}'
Where FBillNo='{1}'

Update T_WB_RECEIPT Set FDetailNo='{0}',FBillNo='{0}'
Where FDetailNo='{1}'  --拚接的

update t_ReceiptBill_Midd set FreceiptNo='{0}',FCheckCode='{2}'
where FReceiptNo='{1}'
", FReceiptNo, pBillNo, pCheckCode);

            int intAffected = CsData.BobExecute(mContext1, mK3CloudApiClient, strSQL);

            return "";
        }
    }
}

