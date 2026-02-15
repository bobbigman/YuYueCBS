using Kingdee.BOS;
using Kingdee.BOS.App.Data;
using Kingdee.BOS.Core.Log;
//using Kingdee.BOS.ServiceFacade.KDServiceClient.Metadata;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.IO;
using System.Diagnostics;
using Kingdee.BOS.WebApi.Client;
using System.Data.OleDb;
using System.Reflection;
using Kingdee.BOS.Orm.DataEntity;
using System.Data.SqlClient;
using Kingdee.BOS.Orm;
using Kingdee.BOS.Util;
using Kingdee.BOS.Log;
using Kingdee.BOS.Contracts;
using Kingdee.BOS.Core.DynamicForm;
using Kingdee.BOS.Core.Metadata;
using Kingdee.BOS.Core.Interaction;
using Kingdee.BOS.App;
using System.Text.RegularExpressions;
using ahu.YuYue.CBS;

//using Kingdee.K3.SCM.App.Stock.ServicePlugIn;
//using Kingdee.K3.SCM.App;
//using Kingdee.K3.SCM.App.Core;



namespace ahu.YuYue.CBS
{
    [Kingdee.BOS.Util.HotUpdate]
    public struct K3FormId
    {

        public const string strWB_RecBankTradeDetail_Mid = "PKUB_CashFlow_Midd";
        public const string strWB_ReceiptBill_Mid = "PKUB_Receipt_Midd";
        public const string strWB_RecBankTradeDetail = "WB_RecBankTradeDetail";
        public const string strWB_ReceiptBill = "WB_ReceiptBill";
        public const string strWB_ReceiptBill_Attachment = "WB_ReceiptBill_Attachment";
        public const string strWB_ReceiptBill_Attachment_Mid = "WB_ReceiptBill_Attachment_Midd";

    }

    public struct WDT2
    {
        public const int OnlyCount = 1, K3 = 2, MiddleTable = 3, CheckEntryCount = 4, UpdateCheckCode = 5;
    }

    [Kingdee.BOS.Util.HotUpdate]

    public static class ClsPublic
    {

        //注意：不能部署到正式。之平正式，没有E盘。
        private static readonly string kingdeeLogPathC = @"c:\WorkSpace\kingdeeLog\";
        private static readonly string kingdeeLogPathD = @"d:\WorkSpace\kingdeeLog\";
        private static readonly string kingdeeLogPathE = @"e:\WorkSpace\kingdeeLog\";
        private static readonly string kingdeeLogPathF = @"f:\WorkSpace\kingdeeLog\";

        public static string KingdeeLogPath
        {
            get
            {
                // 定义要检查的盘符顺序
                string[] driveLetters = { "D", "E", "F", "C" };
                string[] logPaths = { kingdeeLogPathD, kingdeeLogPathE, kingdeeLogPathF, kingdeeLogPathC };

                for (int i = 0; i < driveLetters.Length; i++)
                {
                    string driveRoot = driveLetters[i] + @":\";
                    try
                    {
                        if (!Directory.Exists(driveRoot))
                            continue;

                        DriveInfo drive = new DriveInfo(driveRoot);
                        // 排除 CD/DVD 驱动器、网络驱动器、未准备好或只读的驱动器
                        if (drive.DriveType == DriveType.CDRom ||
                            drive.DriveType == DriveType.Network ||
                            !drive.IsReady ||
                            drive.IsReady && drive.DriveFormat == null) // 某些虚拟光驱可能 IsReady=true 但无格式
                        {
                            continue;
                        }

                        string logPath = logPaths[i];
                        try
                        {
                            if (!Directory.Exists(logPath))
                                Directory.CreateDirectory(logPath); // 如果没权限，这里会抛异常

                            return logPath;
                        }
                        catch (UnauthorizedAccessException)
                        {
                            continue; // 没权限，跳过
                        }
                        catch (IOException)
                        {
                            continue; // 磁盘错误等
                        }

                    }
                    catch (Exception)
                    {
                        // 忽略异常（如访问被拒绝等），继续下一个盘符
                        continue;
                    }
                }

                return ""; // 所有选项都不满足
            }
        }
        public static string GetxlsFileAndPath()
        {
            //读取本机的电脑名（也称为 COMPUTERNAME 环境变量）  
            string computerName = Environment.MachineName;

            if (computerName.EqualsIgnoreCase("green"))
            {
                return @"D:\Nut\我的坚果云\82 其他\05 诺飞\之平CBS接口\联调文档\电子回单附件导入\电子回单附件导入.xlsx";
            }
            else
            {
                return @"e:\电子回单附件导入\电子回单附件导入.xlsx";
            }
        }

        public static string GetxlsPathBackup()
        {
            //读取本机的电脑名（也称为 COMPUTERNAME 环境变量）  
            string computerName = Environment.MachineName;
            if (computerName.EqualsIgnoreCase("green"))
            {
                //一定要有个备份目录，不希望，所有月份的pdf，一直在同一个地方。
                return @"D:\Nut\我的坚果云\82 其他\05 诺飞\之平CBS接口\联调文档\电子回单附件导入备份\";
            }
            else
            {
                return @"e:\电子回单附件导入备份\";
            }
        }

        public static string ReadJObject(JObject jObject1, string pField, int pLength)
        {
            //return pField;

            string strValue = Convert.ToString(jObject1[pField]).Trim();
            strValue = strValue.Replace("\n", "").Trim();
            if (strValue == "")
                return "";

            strValue = strValue.Replace("'", "''");

            if (pField.EndsWith("date", StringComparison.OrdinalIgnoreCase))
            {
                strValue = CsPublicOA.Long2Date(strValue);        
            }

            if (strValue.Length >= pLength)

                //Debug.Assert(false);
                //throw new Exception("字段：" + pField + "长度为" + strValue.Length + " ，不支持。"); 
                strValue = strValue.Substring(0, pLength - 10) + "...略...";

            return strValue;
        }

        public static string CheckOpResultNew_Continue(string code, IOperationResult opResult, string pNotetype)
        {
            if (opResult.IsSuccess == true) return "";

            StringBuilder sb = new StringBuilder("");
            if (opResult.InteractionContext != null
                && opResult.InteractionContext.Option.GetInteractionFlag().Count > 0)
            {
                try
                {
                    // 有交互性提示

                    //取消原因：重复了。
                    //【100】更新库存出现异常情况，更新库存不成功！
                    //详情：出现负库存，库存更新不成功。 
                    //生产入库单 = SCRK00000036,序号 = 1，负库存 = -440,物料 = A01.001.0044,仓库 = 小料仓,生产日期 = 2021-03-02 00:00:00,批号 = 20210302 - 01
                    //sb.AppendLine(opResult.InteractionContext.SimpleMessage);

                    Kingdee.BOS.Orm.DataEntity.DynamicObject result = null;
                    opResult.InteractionContext.Option.TryGetVariableValue("STK_InvCheckResult", out result);
                    if (result != null && result["Entry"] != null)
                    {
                        DynamicObjectCollection DOC1 = result["Entry"] as DynamicObjectCollection;
                        if (DOC1.Count > 0)
                        {
                            sb.Append("详情：");
                            foreach (Kingdee.BOS.Orm.DataEntity.DynamicObject DO1 in DOC1)
                            {
                                string strQTY = DO1["Qty"].ToString();
                                if (strQTY.Contains(".")) strQTY = strQTY.TrimEnd('0').TrimEnd('.');

                                string strProduceDate = Convert.ToDateTime(DO1["ProduceDate"]).ToString("yyyy-MM-dd");
                                if (strProduceDate == "0001-01-01")
                                    strProduceDate = "";

                                if (strProduceDate != "")
                                    strProduceDate = "生产日期=" + strProduceDate + "，";

                                //如果没有批号，不要出现：批号=字样。
                                string strLotText = Convert.ToString(DO1["LotText"]);
                                if (strLotText != "")
                                    strLotText = "批号=" + strLotText + "，";

                                string strError2 = string.Format(@"{0}{1}{2}={3}，序号={4}，负库存={5}，物料={6}，仓库={7}，{8}{9}"
                                    , DO1["ErrMessage"].ToString(), Environment.NewLine, pNotetype, DO1["BillNo"].ToString(), DO1["EntrySeq"].ToString(), strQTY
                                    , DO1["MaterialNumber"].ToString(), DO1["StockName"].ToString(), strLotText, strProduceDate);
                                //sb.AppendLine($"{DO1["ErrMessage"].ToString()} {Environment.NewLine}{pNotetype}={DO1["BillNo"].ToString()}，序号={DO1["EntrySeq"].ToString()}，负库存={strQTY}，物料={DO1["MaterialNumber"].ToString()}，仓库={DO1["StockName"].ToString()}，批号={DO1["LotText"].ToString()}");
                                sb.AppendLine(strError2);
                            }
                        }
                    }
                }
                catch (Exception exp)
                {
                    string strError2 = CsErrLog.GetExceptionWithLog(exp);
                    return strError2;
                    //sb.Append(exp.Message + exp.StackTrace);
                    //Logger.Debug("BobWrite", exp.Message + exp.StackTrace);
                }
            }
            else
            {
                // 操作失败，拼接失败原因，然后抛出中断
                opResult.MergeValidateErrors();
                if (opResult.OperateResult == null)
                {
                    // 未知原因导致提交失败
                    sb.AppendLine("未知原因导致操作失败，请联系系统管理员！");
                }
                else
                {
                    sb.AppendLine("单据操作失败，失败原因：");
                    foreach (var operateResult in opResult.OperateResult)
                    {
                        sb.AppendLine(operateResult.Message);
                    }
                }
            }

            //可以加些：自定义的错误信息。
            //if (code == "保存" && pNotetype == "生产领料单" && sb.ToString().IndexOf("是必填项") > 0)
            //{
            //    throw new Exception("生产领料单产生失败。请检查库存。");
            //}

            //没有传真正的单据类型过来，pNotetype，不用算了。
            //string strError = string.Format(@"{0}{1}", pNotetype, code);
            string strError = string.Format(@"{0}失败{1}{2}", code, Environment.NewLine, sb.ToString());
            return strError;

        }

        //pMiddleNumber_Field, 淘宝的单据编号，来自同步工作台，传入的。
        //pFBillNO，金蝶自动产生的单据编号，要返回前端的。 这两个很象，不要弄混了哈。
        //在广亚，还真不是的。


        public static string GetSyncErrorInfo(JToken pJToken)
        {
            if (pJToken == null) return "";
            if (pJToken.Count() == 0) return "";
            string strCode = pJToken["code"].ToString();
            if (strCode == "0" || strCode == null) return "";

            //销售出库时，如下代码错。
            //string strMsg = pJToken["msg"].ToString();

            string strMsg = pJToken["message"].ToString();
            if (strMsg == "") return "";
            return strMsg;
        }
        public static JObject MyJObject(string key, string number)
        {
            JObject JObject1 = new JObject
            {
                { key, number }
            };
            return JObject1;
        }

        public static JObject MyJObject(JObject pJObject, string pK3Field, string pValue, bool pNumberField, bool pFAuxPropId)
        {

            if (pNumberField == true)
                pJObject.Add(pK3Field, ClsPublic.MyJObject("FNumber", pValue));
            else if (pFAuxPropId == true)
                MyJObject_FAuxPropId(pJObject, pK3Field, pValue);
            else
                pJObject.Add(pK3Field, pValue);

            return pJObject;
        }

        public static JObject MyJObject_FAuxPropId(JObject pJObject, string pK3Field, string pValue)
        {

            JObject jObject1 = new JObject();
            pJObject.Add("FAuxPropId", jObject1);
            jObject1.Add(pK3Field, ClsPublic.MyJObject("FNumber", pValue));

            return pJObject;
        }


        //pFNumber 从中间表取出来的单据编号
        //pFK3BillNO,金蝶单据编号，有可能是按金蝶规则产生的。

        // pFNumber_TBGZT，先拿掉。同步工作台，传过来的，有可能为空,太麻烦了，有时，我也不知道怎么传。
        public static string K3SQLLog(Context pContext, K3CloudApiClient pK3CloudApiClient,
            string pResultJson, string pJson,
            string pReadMiddleTable, string pSysncLogTable, string pGetDllLastWriteTime,
            string pFormId)
        {
            try
            {
                //从json里取。
                string strFMiddleId = "";

                // 调用Web API接口服务，保存采购订单
                int intOK = 0;
                string strK3BillID = "";
                string strFK3BillNO = ""; //要从json里解析。
                string strSQL = "";
                string strResult = "";
                string strErrors = "";

                //不这样,程序会报错呢.
                //必须要反回值,不能直接return ""
                if (pJson == "" || pResultJson == "")
                {
                    return "";
                }

                if (pResultJson.IndexOf("同步出错") > -1)
                    return pResultJson;

                //JObject JObjectResult = JObject.Parse(pResultJson.Replace("\\", ""));
                JObject JObjectResult = JObject.Parse(pResultJson);

                JToken JObjectResultK3 = JObjectResult["Result"]["ResponseStatus"];
                string strReturnCode = JObjectResultK3["IsSuccess"].ToString();

                string strResultJson = pResultJson.Replace("'", "''");
                string strK3DllVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString() + " | " + pGetDllLastWriteTime;

                if (strReturnCode == "True")
                {
                    if (pJson.IndexOf("Forbid") > -1)
                        strK3BillID = JObjectResult["Result"]["ResponseStatus"]["SuccessEntitys"][0]["Id"].ToString();
                    else
                        strK3BillID = JObjectResult["Result"]["Id"].ToString();

                    if (strK3BillID != "")
                    {

                        ClsPublic.UpdateMiddleTableSynchronizeOK(pContext, pK3CloudApiClient, pReadMiddleTable, strFMiddleId,  strFK3BillNO, strK3BillID, false);

                    }
                    strResult = "同步成功。";
                    intOK = 1;

                }
                else if (strReturnCode == "False")
                {

                    //出错：将截断字符串。
                    //原来，出错，供应商为空，重复了好多个。
                    //记录上次的错误。
                    string strError0 = "";
                    string strError1 = "";
                    for (int i = 0; i <= JObjectResultK3["Errors"].Count() - 1; i++)
                    {
                        strError1 = JObjectResultK3["Errors"][i]["Message"].ToString();
                        if (strError0 != strError1)
                        {
                            strError0 = strError1;

                            //把提示，改明确一点。
                            string strFind = "请先录入收款组织";
                            int i1 = strError1.IndexOf(strFind);
                            if (i1 > 0)
                                strError1 = "收款单新增时，选不到对应的：收款组织。";

                            strErrors = strErrors + Environment.NewLine + strError1;
                        }
                    }

                    if (strErrors.Length > 400)
                        strErrors = strErrors.Substring(0, 380) + "...省略上千字.....";

                    strSQL = string.Format(@"/*dialect*/ 
                       update    {0} set
                                   FSynchronStatusBob='2'
                                  ,FSynchronError=CONVERT(varchar(19), GETDATE(), 21)+'  {2}'
                                  ,K3SynchronizeTime=Getdate()
                                  ,K3Version='{3}'
                        where     FMiddleID='{1}'
                         ", pReadMiddleTable, strFMiddleId, strErrors, strK3DllVersion);
                    CsData.BobExecute(pContext, pK3CloudApiClient, strSQL);

                    strResult = "同步失败。" + Environment.NewLine + strErrors;
                    intOK = 0;
                }

                //如果不加， pJson.Replace("'","") ,执行会出错。

                string strResultSQLLog = strResultJson;
                if (strErrors != "")
                    strResultSQLLog = strErrors;

                if (strK3BillID == "")
                    strK3BillID = "Null";

                if (strFMiddleId.IsNullOrEmptyOrWhiteSpace() == true)
                    strFMiddleId = "Null";


                strSQL = string.Format(@"/*dialect*/ 
                INSERT INTO dbo.{0}
                           (FMiddleID, FSourceNO
                           ,[IsOk]
                           ,[ResultJson]
                           ,[Json],FormId)
                     VALUES
                ({1},'{2}',{3},'{4}','{5}','{6}')",
               pSysncLogTable, strFMiddleId, strFK3BillNO, intOK, strResultSQLLog, pJson.Replace("'", ""), pFormId);
                CsData.BobExecute(pContext, pK3CloudApiClient, strSQL);

                return strResult;

            }
            catch (Exception ex)
            {
                throw new Exception(CsErrLog.GetExceptionWithLog(ex));
            }
        }




        public static int LogIn(ref Struct_K3LoginInfo pStruct_K3LoginInfo,
    Context pContext, ref K3CloudApiClient pK3CloudApiClient1
            , ref string pResultType, bool pLoginK3
            , int pK3DatabaseType)
        {
            try
            {
                int intResultType = 1;  //默认成功。

                //2024/6/19 7:59  原来，是放在pStruct_K3LoginInfo前面，现在，改为放到后面了。
                //因为，绑定在金蝶中，取得pK3CloudApiClient1，要先了得金蝶网址，账号，密码这些。
                //if (pLoginK3 == true && pStruct_K3LoginInfo.LoginK3Ok == false && pRunMode != RunMode.UnKnow)

                //改回,放在前面了。2026/1/18 10：29
                if (pStruct_K3LoginInfo.ServerURL_K3 == null)
                {
                    ClsScheduleReadOthers ClsScheduleReadOthers1 = new ClsScheduleReadOthers();
                    ClsScheduleReadOthers1.GetK3LoginInfo(pContext, pK3CloudApiClient1, ref pStruct_K3LoginInfo);
                }

                //pK3CloudApiClient1 == null 增加于2026/1/16 7:22 周五 天熙 晴
                if (pK3CloudApiClient1==null ||(pLoginK3 == true && pStruct_K3LoginInfo.LoginK3Ok == false))
                {
                    //一般情况，这里不会运行。
                    //因为，从金蝶调集成调用，pRunMode=RunMode.UnKnow

                    //2024/7/7 7:36
                    //不，这里，还是要调用，即使在金蝶集成里，因为，要执行json,同步供应商到金蝶中。
                    pK3CloudApiClient1 = CsPublicOA.CreateK3CloudApiClient(ref pStruct_K3LoginInfo, pK3DatabaseType);
                    if (pK3CloudApiClient1 == null)
                    {
                        intResultType = 0;
                        pStruct_K3LoginInfo.LoginK3Ok = false;
                    }
                    else
                        pStruct_K3LoginInfo.LoginK3Ok = true;
                }

                //要注意顺序，2026/1/17 21：46 ，忘记了说这话的背景。改回去再说。

                //为什么要改回去，2026/1/18 10：29
                //因为，如果不执行下面这一步，在执行计划里，会报错，因为上面的登陆会失败了。
                //有时需要执行，比如：从交易明细的中间表，下推到金蝶应用。
                //if (pStruct_K3LoginInfo.ServerURL_K3 == null)
                //{
                //    ClsScheduleReadOthers ClsScheduleReadOthers1 = new ClsScheduleReadOthers();
                //    ClsScheduleReadOthers1.GetK3LoginInfo(pContext, pK3CloudApiClient1, ref pStruct_K3LoginInfo);
                //}

                if (pK3DatabaseType == K3DatabaseMode.IntegrationK3 && pContext.IsNullOrEmptyOrWhiteSpace() == false)
                {
                    //多此一举，赋值，为什么呢？我有context啊，就可以执行SQL.还用 K3CloudApiClient1,真是画蛇添足。
                    //不是的，基础资料验证，当初写的，写是调用接口。为什么不写的SQL呢？可能是公有云，觉得写不出SQL吧。
                    //如何测试呢？把PKUB_TBGZT，也部署到广亚？？
                    //但是这样，是否会效率很低？先别管这些了，以后，再来优化吧。

                    //2024/6/19  金蝶系统内，不要产生 pK3CloudApiClient1
                    //因为，我也不知道，连到哪一个服务器，真是鸡生蛋，蛋生鸡。
                    return 1; //默认成功


                    //int intRunMode = 0;
                    //if (pContext.DBId == "642bfc3f773db1")
                    //    intRunMode = RunMode.Normal;
                    //else
                    //    intRunMode = RunMode.Test;

                    //pK3CloudApiClient1 = CsPublicOA.CreateK3CloudApiClient(intRunMode);


                }


                return intResultType;

            }
            catch (Exception ex)
            {
                throw new Exception(CsErrLog.GetExceptionWithLog(ex));
            }
        }



        public static string RemoveLast0(string pInput)
        {

            if (pInput == "" || pInput == "0") return "0";

            string strReturn = pInput;

            while (strReturn.Contains(".") && strReturn.Substring(strReturn.Length - 1) == "0")
            {
                strReturn = strReturn.Substring(0, strReturn.Length - 1);
            }

            if (strReturn.Substring(strReturn.Length - 1) == ".")
                strReturn = strReturn.Substring(0, strReturn.Length - 1);

            return strReturn;

        }

        public static string GetSynchronizingFilter(string pSheet_NO)
        {

            string strFilter;
            if (pSheet_NO == "")
            {
                //批次执行，只执行：未同步的。 
                //上次死锁牺牲品呢，，算了，重来一次吧。

                //上次开始同步，24小时，还未做完的，也来一次。
                //DATEDIFF(hh, K3SynchronizeTime, getdate()) >= 24

                //更新库存不成功,要多试几次，等入库先同步，可能就行了。
                //DATEDIFF(hh,create_date,getdate()) between 1 and 12 ，加限制，如果库存不够，只试一天，就算了，别浪费资源。
                strFilter = string.Format(@" 
(     isnull(FSynchronStatusBob,0)='0' 
  or  FSynchronError LIKE '%死锁牺牲品%'
  or  FSynchronError LIKE '%正在中止线程%'
  or (FSynchronStatusBob='8' and   DATEDIFF(hh,K3SynchronizeTime,getdate())>=24)) 
  or (FSynchronError LIKE '%更新库存不成功%' and   DATEDIFF(hh,create_date,getdate()) between 1 and 12  )");

            }
            else
                strFilter = string.Format(@"        isnull(FSynchronStatusBob,0)<>'1'  
                                                AND  sheet_no='{0}' ", pSheet_NO);  //按单据编号同步，只要不成功，都可以都再次执行。


            return strFilter;

        }

        //收银汇总，取数据，不为1，也不为3.
        //为什么？
        //收银成功后，是否标为1？
        //如果中间表中，没有负数，标为1，
        //否则,标为3.
        public static string GetSynchronizingFilter_SY(string pSheet_NO)
        {

            string strFilter;
            if (pSheet_NO == "")
            {
                //批次执行，只执行：未同步的。 (为0时，表示未同步，为3呢，已同步到收银。)
                //上次死锁牺牲品呢，，算了，重来一次吧。
                //上次开始同步，24小时，还未做完的，也来一次。
                //销售出库，重新来，同步了好久。
                //如果遇到长假，当中没有同步，可能会执行好久？
                strFilter = string.Format(@"   (   isnull(FSynchronStatusBob,0)='0' 
                                               or  FSynchronError LIKE '%死锁牺牲品%'
                                               or (FSynchronStatusBob='8' and   DATEDIFF(hh,K3SynchronizeTime,getdate())>=24))");

            }
            else
                //收银批量，怎么办？
                strFilter = string.Format(@"    isnull(FSynchronStatusBob,0)<>'1' and  isnull(FSynchronStatusBob,0)<>'3'  
                                                AND  sheet_no='{0}' ", pSheet_NO);  //按单据编号同步，只要不成功，都可以都再次执行。


            return strFilter;

        }

        //收银退款。
        //问题来了。
        //退款单，标为2，是否要处理？
        //区别对待，批理，不处理；单独呢，要处理。
        public static string GetSynchronizingFilter_SY_Return(string pSheet_NO)
        {

            string strFilter;
            if (pSheet_NO == "")
            {
                //批次执行，只执行：未同步的。 
                //上次死锁牺牲品呢，，算了，重来一次吧。
                //上次开始同步，24小时，还未做完的，也来一次。
                //销售出库，重新来，同步了好久。
                //如果遇到长假，当中没有同步，可能会执行好久？

                //问题来了，标为2，是否要处理？
                //批量，当然不处理了，先手工判断吧。
                strFilter = string.Format(@"   (   isnull(FSynchronStatusBob,0)='3' 
                                               or  FSynchronError LIKE '%死锁牺牲品%'
                                               or (FSynchronStatusBob='8' and   DATEDIFF(hh,K3SynchronizeTime,getdate())>=24))");

            }
            else
                //共有0 默认，1成功 ，2出错，3收银，做了收款单，8 同步中。
                //标为2，也要处理。
                strFilter = string.Format(@"       ( FSynchronStatusBob='3' or  FSynchronStatusBob='2' or FSynchronStatusBob='8' )  
                                                    AND  sheet_no='{0}' ", pSheet_NO);  //按单据编号同步，只要不成功，都可以都再次执行。

            return strFilter;
        }

        public static void UpdateMiddleTableSynchronizeError(Context pContext, K3CloudApiClient pK3CloudApiClient
            , string pTable, string pMiddleID, string pFFSynchronError,  string pFormId
            )
        {
            try
            {
                UpdateMiddleTableSynchronizeError(pContext, pK3CloudApiClient, pTable, pMiddleID, pFFSynchronError,  pFormId, "");
            }
            catch (Exception ex)
            {
                throw new Exception(CsErrLog.GetExceptionWithLog(ex));
            }
        }

        public static void UpdateMiddleTableSynchronizeError(Context pContext, K3CloudApiClient pK3CloudApiClient
            , string pTable, string pMiddleID, string pFSynchronError,  string pFormId, string pFilter

            )
        {
            try
            {
                //没有Id,没法记录哇。

                //记录下，也无所谓嘛。2022/12/19 10:59
                //if (pMiddleID == "")
                //    return;

                string strFSynchronStatusBob = "2";

                //不要标为：同步错，因为不需要再次同步，这是没问题的。
                if (pFSynchronError.Contains("不需要同步"))
                {
                    strFSynchronStatusBob = "1";
                    pFSynchronError = "";
                }

                //算了，还是不标为1吧，标了后，没有看了。2024/9/16 7:44
                //if (pFFSynchronError.Contains("单据同步成功，但pdf附件下载失败，只能下载7天内的文件。"))
                //    strFSynchronStatusBob = "1";

                //如果是批量同步，出错了，退出算了。
                string strSQL = "";
                if (pMiddleID == "")
                {
                    string strError = "糟糕，pMiddleID为空。ClsPublic.UpdateMiddleTableSynchronizeError:"+System.DateTime.Now.ToString();
                    strError += Environment.NewLine + pFSynchronError;
                    strError += Environment.NewLine +"pFormId:"+ pFormId+ "Filter"+ pFilter;
                    throw new Exception(strError);
                }
                else
                {
                    strSQL = string.Format(@"/*dialect*/
Update {0} set
       FSynchronError='{1}',FSynchronStatusBob={3}
Where FID={2}", pTable, pFSynchronError, pMiddleID, strFSynchronStatusBob);


                    CsData.BobExecute(pContext, pK3CloudApiClient, strSQL);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(CsErrLog.GetExceptionWithLog(ex));
            }
        }


        public static void UpdateMiddleTableSynchronizeOK(Context pContext, K3CloudApiClient pK3CloudApiClient, string pTable
            , string pMiddleID,  string pFNumber, string pK3FId, bool pExitShowError)
        {
            try
            {
                if (pMiddleID == "")
                    UpdateMiddleTableSynchronizeOKByNum(pContext, pK3CloudApiClient, pTable,  pFNumber, pK3FId, pExitShowError);
                else
                    UpdateMiddleTableSynchronizeOKById(pContext, pK3CloudApiClient, pTable, pMiddleID,  pExitShowError);


            }
            catch (Exception ex)
            {
                throw new Exception(CsErrLog.GetExceptionWithLog(ex));
            }
        }

        public static void UpdateMiddleTableSynchronizeOKById(Context pContext, K3CloudApiClient pK3CloudApiClient
            , string pTable, string pMiddleID
            , bool pExitShowError)
        {
            try
            {
                string strError = "";

                //2022/12/23 取消，正常同步，也报这个，有点尴尬了。
                //如果用户，在同步工作台，选物料编号，在点击同步，这个提示，更好。
                if (pExitShowError == true)
                    strError = "之前，就同步过了。";

                string strSQL = string.Format(@"/*dialect*/
Update {0} set
       FSynchronError='{1}',FSynchronStatusBob=1
Where FId={2}", pTable, strError, pMiddleID);
                CsData.BobExecute(pContext, pK3CloudApiClient, strSQL);

            }
            catch (Exception ex)
            {
                throw new Exception(CsErrLog.GetExceptionWithLog(ex));
            }
        }

        public static void UpdateMiddleTableSynchronizeOKByNum(Context pContext, K3CloudApiClient pK3CloudApiClient
            , string pTable
            , string pFNumber, string pK3FId, bool pExitShowError)
        {
            try
            {

                //如果用户，是执行计划列表中，执行的呢？pMiddleID为空。
                //答：不用提示。
                //但是，如果没有标为：已同步的，我还是要标识下的。
                //2022/12/23 取消，正常同步，也报这个，有点尴尬了。
                //如果用户，在同步工作台，选物料编号，在点击同步，这个提示，更好。
                string strError = "NULL";

                //2022/12/25 8:17
                //pK3FId，有时传了，如调整用save,保存时。
                //pK3FId，有时没传。如判断是否存在。哎，这时，不用记录吧？算了，改为不记录，没意义。


                if (pExitShowError == true)
                    strError = "之前，就同步过了。";

                string strSQL = string.Format(@"/*dialect*/
Update {0} set
       FSynchronError={2},FSynchronStatusBob=1
Where FNumber='{1}'
and  isnull(FSynchronStatusBob,0)<>1", pTable, pFNumber, strError);

                CsData.BobExecute(pContext, pK3CloudApiClient, strSQL);

            }
            catch (Exception ex)
            {
                throw new Exception(CsErrLog.GetExceptionWithLog(ex));
            }
        }




        public static DateTime GetSyncEndDate()
        {
            DateTime dtYesterDay = DateTime.Now.AddDays(-1);
            return dtYesterDay;
        }


        public static string MillionSeconds2DateTime(long pSeconds)
        {
            TimeSpan TimeSpan_FDate = TimeSpan.FromMilliseconds(pSeconds);
            DateTime dt1 = new DateTime(TimeSpan_FDate.Ticks);
            dt1 = dt1.AddYears(1969);
            string strFDate = dt1.ToString("yyyy-MM-dd");
            return strFDate;
        }



        //public static bool CheckBasicNumAll(Context pContext, K3CloudApiClient pK3CloudApiClient
        //    , Struct_K3LoginInfo pStruct_K3LoginInfo1
        //    , string pFNumber_MES, ref String pError
        //    , string pMiddleID, string pGetDllLastWriteTime
        //    , string pCheckType, string pK3Table_Basic, string pMiddleTableName, bool pCheckByName, ref string pReturnNumber
        //    , string pFormID_Check, string pK3Field, string pName_Warehouse, int pWDT2Where
        //    , string pCheckOrgNum, string pCheckOrgId)
        //{
        //    try
        //    {
        //        pReturnNumber = "";

        //        if (pFNumber_MES == "")
        //        {
        //            pError = string.Format("ERP单据中，{0}为空", pCheckType);
        //            //外面处理了。
        //            //ClsPublic.UpdateMiddleTableSynchronizeError(pK3CloudApiClient1, pMiddleTableName, pMiddleID, pError, pGetDllLastWriteTime);
        //            return false;
        //        }

        //        bool bolReturn = CheckBasicNumAll(pContext, pK3CloudApiClient
        //            , pFNumber_MES, pMiddleTableName, ref pError, pMiddleID, pGetDllLastWriteTime
        //            , pCheckType, pFormID_Check, true, ref pReturnNumber);

        //        pReturnNumber = pFNumber_MES;
        //        return true;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(CsErrLog.GetExceptionWithLog(ex));
        //    }
        //}


        //public static string GetFormIdBySeq(string pSyncBillType, ref string pMiddleTable)
        //{
        //    string strFormId = "";

        //    switch (pSyncBillType)
        //    {
        //        case "1":
        //            strFormId = K3FormId.strWB_RecBankTradeDetail_Mid;
        //            pMiddleTable = "t_CashFlow_Midd";
        //            break;
        //        case "2":
        //            strFormId = K3FormId.strWB_ReceiptBill_Mid;
        //            pMiddleTable = "t_ReceiptBill_Midd";
        //            break;
        //        case "3":
        //            strFormId = K3FormId.strWB_ReceiptBill_Attachment_Mid;
        //            pMiddleTable = "t_ReceiptBill_Midd";
        //            break;

        //        default:
        //            throw new Exception("case " + pSyncBillType + "，在ClsPublic.GetFormIdBySeq里，没有写。");
        //    }

        //    //下面，还是简单粗暴一点好,下面算了那么多，觉得太复杂了。
        //    return strFormId;

        //}


        //public static string GetFormIdBySeq(Context pContext, K3CloudApiClient pK3CloudApiClient, string pSyncBillType
        //    , ref string pFormChinese, ref string pStartTime, ref string pEndTime, ref string pPage
        //    , string pSyncWithLastError
        //    , string pSyncStartDate
        //    )
        //{


        //    if (pStartTime == "")
        //        pStartTime = pSyncStartDate;

        //    //先给一下默认值。2024/1/26 10:40
        //    //上线后，我不希望，每次都从初始日期开始，调用接口。
        //    if (pStartTime == "")
        //    {
        //        pStartTime = System.DateTime.Today.AddDays(-6).ToString("yyyy-MM-dd");
        //        pEndTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
        //    }

        //    //下面，还是简单粗暴一点好,分析历史记录，太复杂了。
        //    return strFormId;

        //}

        //public static string GetFormIdBySeq(Context pContext, K3CloudApiClient pK3CloudApiClient, string pintSyncFormType
        //    , ref string pFormChinese, Struct_K3LoginInfo pStruct_K3LoginInfo)
        //{
        //    string strFormId = "";

        //    //仅为兼容。这个语法有点怪，为什么只为兼容？
        //    //好吧，是同步工作台调用的，所以，这样就明白了。
        //    string strStartTime = "", strEndTime = "", strPage = ""; ;

        //    strFormId = GetFormIdBySeq(pContext, pK3CloudApiClient, pintSyncFormType
        //        , ref pFormChinese, ref strStartTime, ref strEndTime, ref strPage
        //        , pStruct_K3LoginInfo.SyncWithLastError
        //         , pStruct_K3LoginInfo.SyncStartDate);
        //    return strFormId;
        //}

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

        public static void WriteWDTLogWithOutCheckFile(string pPath, string pFileName, string pMesssage)
        {
            if (!System.IO.Directory.Exists(pPath))
            {
                System.IO.Directory.CreateDirectory(pPath);
            }

            string strlogFile = System.IO.Path.Combine(pPath, pFileName + ".txt");
            string strlogFile2 = strlogFile.Replace(":", "_");
            //糟糕，盘符从 D: 变成了 d_
            strlogFile2 = strlogFile2.Substring(0, 1) + ":" + strlogFile2.Substring(2);

            strlogFile2 = strlogFile2.Replace(",", "-");

            StreamWriter swLogFile = new StreamWriter(strlogFile2, true, Encoding.Unicode);
            swLogFile.WriteLine(Environment.NewLine + Environment.NewLine + new String('*', 50)
                + Environment.NewLine + "日志写入时间2：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                + Environment.NewLine + pMesssage);
            swLogFile.Close();
            swLogFile.Dispose();

        }
        public static void WriteWDTLog(string pPath, string pFileName, string pMesssage)
        {
            if (!System.IO.Directory.Exists(pPath))
            {
                System.IO.Directory.CreateDirectory(pPath);
            }

            string strlogFile = System.IO.Path.Combine(pPath, pFileName + ".txt");
            string strlogFile2 = strlogFile.Replace(":", "_");
            //糟糕，盘符从 D: 变成了 d_
            strlogFile2 = strlogFile2.Substring(0, 1) + ":" + strlogFile2.Substring(2);

            strlogFile2 = strlogFile2.Replace(",", "-");

            //2023/3/5 7:44 读到的值，总是 false
            //if (System.IO.File.Exists(logFile))
            //{
            //    return;
            //}

            try
            {
                Stream s = File.OpenRead(strlogFile2);
                s.Close();
                s.Dispose();
            }
            catch
            {
                StreamWriter swLogFile = new StreamWriter(strlogFile2, true, Encoding.Unicode);
                swLogFile.WriteLine(Environment.NewLine + Environment.NewLine + new String('*', 50)
                    + Environment.NewLine + "日志写入时间3：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    + Environment.NewLine + pMesssage);
                swLogFile.Close();
                swLogFile.Dispose();
            }

        }





        //public static bool CheckBasicNumAll(Context pContext, K3CloudApiClient pK3CloudApiClient
        //    , string pFNumber_MES, String pTable, ref String pError, string pMiddleID, string pGetDllLastWriteTime
        //    , string pCheckType, string pFormID, bool pCheckByName, ref string pReturnNumber)
        //{
        //    try
        //    {
        //        pReturnNumber = "";

        //        if (pFNumber_MES == "")
        //        {
        //            pError = string.Format("MES单据中，{0}为空", pCheckType);
        //            UpdateMiddleTableSynchronizeError(pContext, pK3CloudApiClient, pTable, pMiddleID, pError, pGetDllLastWriteTime, pFormID);
        //            return false;
        //        }

        //        bool bolExisit = ClsSeek.CheckBasicNumber(pK3CloudApiClient, pFNumber_MES, pFormID, false);

        //        if (bolExisit == false)
        //        {
        //            string strSearchByName = pFNumber_MES;
        //            pFNumber_MES = ""; //都不存在，清空，便于一下步来判断。

        //            if (pCheckByName == false)
        //            {
        //                pFNumber_MES = ClsSeek.GetBasicNumberByName(pK3CloudApiClient, strSearchByName, pFormID);
        //            }

        //            //依据名称，也没找到编号。
        //            //给了两次机会，都不行，只能报错了。
        //            if (pFNumber_MES == "")
        //            {
        //                pError = string.Format("在基础资料{0}中，找不到：编码 {1}", pCheckType, pFNumber_MES);
        //                ClsPublic.UpdateMiddleTableSynchronizeError(pContext, pK3CloudApiClient, pTable, pMiddleID, pError, pGetDllLastWriteTime, pFormID);
        //                return false;
        //            }
        //        }


        //        bolExisit = ClsSeek.CheckBasicNumber(pK3CloudApiClient, pFNumber_MES, pFormID, true);
        //        if (bolExisit == false)
        //        {
        //            pError = string.Format("{0}编码 {1} ,没有审核。", pCheckType, pFNumber_MES);
        //            ClsPublic.UpdateMiddleTableSynchronizeError(pContext, pK3CloudApiClient, pTable, pMiddleID, pError, pGetDllLastWriteTime, pFormID);
        //            return false;
        //        }

        //        pReturnNumber = pFNumber_MES;

        //        return true;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public static void AddError2OperationResult(string pError, OperationResult pOperationResults, int intIndex, string pOperate)
        {
            OperateResult OperateResult1;
            if (pOperationResults.OperateResult.Count <= intIndex)
            {
                OperateResult1 = new OperateResult();
                pOperationResults.OperateResult.Add(OperateResult1);
            }

            OperateResult1 = pOperationResults.OperateResult[intIndex];

            OperateResult1.Name = pOperate;
            if (pError == "")
            {
                OperateResult1.Message = pOperate + "成功";
                OperateResult1.MessageType = MessageType.Normal;
                OperateResult1.SuccessStatus = true;
            }
            else
            {
                OperateResult1.Message = pError;
                OperateResult1.MessageType = MessageType.Warning;
                OperateResult1.SuccessStatus = false;
            }

            //希望在csList中，能刷新
            //验证没戏。List中，还是不能刷新。跟踪时，dyanmicObject中，没有值。
            //pDO["FSynchronError"] = pError;  
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

    } //ClsPublic

}
