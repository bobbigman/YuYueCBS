using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
//using ahu.JuShuiTan.Util2;
using Kingdee.BOS;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.Util;
using Kingdee.BOS.WebApi.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ahu.YuYue.CBS
{

    [Description("别调用，是中间层啊。批量同步到金蝶")]

    //[Kingdee.BOS.Util.HotUpdate] //拿掉，就渝月项目来说，根本用不着，都不能调试。没有协同开发平台。
    public class csMiddle2K3
    {

        bool mbolNeedCBSToken;

        K3CloudApiClient mK3CloudApiClient;
        Struct_K3LoginInfo mStruct_K3LoginInfo;
        Context mContext;

        string mstrFBillNo_Call;
        string mstrFId_Call;

        string mstrTableName_Middle, mstrCleanProcedure, mstrDeleteDuplicateProcedure;
        string mstrFormId_Middle, mstrFormId_K3, mstrPushRuleId;

        DynamicObjectCollection mDocNodek3;

        DynamicObjectCollection mdocMiddleSource;
        //DynamicObject mdo1, mdo2;

        int mintDatabaseType;

        public void NeedCBSToken(bool pRefresh)
        {
            mbolNeedCBSToken = pRefresh;
        }

        public void SetBySchedule(Context pContext
            , ref K3CloudApiClient pK3CloudApiClient
            , ref Struct_K3LoginInfo pStruct_K3LoginInfo)
        {
            mContext = pContext;
            mK3CloudApiClient = pK3CloudApiClient;
            mStruct_K3LoginInfo = pStruct_K3LoginInfo;
        }

        public string Call2K3(Context pContext
            , ref K3CloudApiClient pK3CloudApiClient
            , ref Struct_K3LoginInfo pStruct_K3LoginInfo
            , int pBillTypeSeq
            , string pFNumber_Middle, string pFId_Middle
            , int p2DatabaseType)
        {

            //throw new Exception("同步到金蝶，暂时停止，不要执行。");

            mContext = pContext;
            mstrFBillNo_Call = pFNumber_Middle;
            mstrFId_Call = pFId_Middle;
            mintDatabaseType = p2DatabaseType;

            Boolean bolNeedLogInK3 = true;

            if (mstrFBillNo_Call == "" & mstrFId_Call != "")
            {
                return "意外错误，没传同步的单据编号，却传了内码。";
            }
            if (mstrFBillNo_Call != "" & mstrFId_Call == "")
            {
                return "意外错误，传了同步的单据编号，却没传内码。";
            }
            string strReturnType = "";

            //先来个登陆，读取中间表要用。

            //pK3CloudApiClient1 == null 增加于2026/1/16 7:22 周五 天熙 晴
            if (pStruct_K3LoginInfo.LoginK3Ok == false || pStruct_K3LoginInfo.AcctID == null || pK3CloudApiClient == null)
            {
                //有时，登陆了，参数却没有取得值。
                int IntReturn = ClsPublic.LogIn(ref pStruct_K3LoginInfo, pContext
                    , ref pK3CloudApiClient, ref strReturnType, bolNeedLogInK3, p2DatabaseType);
                if (IntReturn != 1)
                    return strReturnType;
            }

            mK3CloudApiClient = pK3CloudApiClient;
            mStruct_K3LoginInfo = pStruct_K3LoginInfo;

            //仅为兼容。
            string strStartDate = "", strEndDate = "";
            string strSyncBillTypeText, strTableNameK3;
            string strOthersURL, strOthereDateField, strOthereBillNoField;

            CsData.GetSyncBillTypeInfo(pContext, pK3CloudApiClient, pStruct_K3LoginInfo
                       , WDT2.K3, ref strStartDate, ref strEndDate, pBillTypeSeq
               , out strSyncBillTypeText, out mstrFormId_Middle, out mstrFormId_K3
               , out mstrTableName_Middle, out strTableNameK3, out strOthersURL
               , out mstrCleanProcedure, out mstrDeleteDuplicateProcedure
               , out strOthereDateField, out strOthereBillNoField, out mstrPushRuleId);

            //读出来的没有用，那是指中间表，读取报文什么字段的，也就是说，单据编号，从第三方什么字段读过来的。
            //mstrBillNoField_Middle = "FBillNo";

            //2025/1/28 8：14
            //用户在前台，点击的分布式调入，怎么同步呢？等一等，有这个可能吗？根本没有！！！！
            //所以，我配一下就行，对吧？
            //它会对什么有影响呢？批量同步同，我想不会有影响。需要测试一下。
            //用户点击按钮，同步，更不会有影响，因为不改，都用不了。

            if (mstrFormId_Middle == "")
                return "要同步的单据类型(" + mstrFormId_K3 + "，序号" + pBillTypeSeq + " )，暂不支持。";

            //表里定义为：tblSal_OutStock，这是因为，要从聚水潭接口，写到中间表。
            //现在呢，是要从中间表，同步到金蝶。
            //它们，是两个不同的中间表。

            string strError = GetMiddleSqlScript(p2DatabaseType);

            return strError;
        }

        private string GetMiddleSqlScript(int p2DatabaseType)
        {
            string strError1;

            //拿掉 top 1
            //以物料为例，加了Model, 下面，还有：SubHeadEntity，还要加，基本计量单位。

            //这个地方，写得不太规范。
            //采购入库单，有时要生成：直接调拨单。
            //但是，它的SQL,不要取直接调拨单，也不要取采购入库单，有它自己的。
            //注意：tblMatchK3呢，是用直接调拨单的。

            string strGetFormId = mstrFormId_Middle;


            string strSQL = string.Format(@"/*dialect*/
select ID,NodeText,BobSQL,ParentID,NodeIndex,AllowNull,Memo,IsJObject,MiddleTableName,NeedUpDateFields,k3TableName
from tblNodeCBS  with(nolock) 
where formid='{0}'
order by ID", strGetFormId);
            mDocNodek3 = CsData.GetDynamicObjects(mContext, mK3CloudApiClient, strSQL);
            if (mDocNodek3.Count == 0)
            {
                strError1 = "意外错误，请洽程序设计员。tblNodeK3中，还没有配置：" + mstrFormId_K3 + "  *";
                return strError1;

            }

            //同步前，做个清洗。
            string strFId_Clear = "0";

            if (mstrFId_Call != "")
                strFId_Clear = mstrFId_Call;

            ////不能改到读取后，马上执行。
            ////因为，基础资料问题修正好，也需要及时处理的。
            //string strCleanProcedure, strDeleteDuplicateProcedure;
            //if (mstrMiddleFormId == K3FormId.strWB_RecBankTradeDetail_Mid)
            //{
            //    strCleanProcedure = "ahuCleanJob_CashFlow_Midd";
            //    strDeleteDuplicateProcedure = "AhuDeleteDuplicateJob_CashFlow_Midd";
            //}
            //else if (mstrMiddleFormId == K3FormId.strWB_ReceiptBill_Mid)
            //{
            //    strCleanProcedure = "ahuCleanJob_Receipt_Midd";
            //    strDeleteDuplicateProcedure = "AhuDeleteDuplicateJob_ReceiptBill_Midd";
            //}
            //else if (mstrMiddleFormId == K3FormId.strWB_ReceiptBill_Attachment_Mid)
            //{

            //    strCleanProcedure = "ahuCleanJob_Receipt_Midd";
            //    strDeleteDuplicateProcedure = "AhuDeleteDuplicateJob_ReceiptBill_Midd";
            //}
            //else
            //    throw new Exception("GetMiddleTable里，" + mstrK3FormId + "没有设定清洗存储过程");

            strSQL = string.Format(@"exec {0} {1}  ", mstrCleanProcedure, strFId_Clear);

            CsData.BobExecute(mContext, mK3CloudApiClient, strSQL);

            if (strFId_Clear == "0")
            {
                strSQL = string.Format(@"exec {0}  ", mstrDeleteDuplicateProcedure);
                CsData.BobExecute(mContext, mK3CloudApiClient, strSQL);
            }


            //一次同步1千条试试，看花多少时间。
            //同步5000次，每次40条，可以吧？

            string strErrorLastChar = "";
            string strErrors = "";
            //for (int i = 1; i <= mStruct_K3LoginInfo.BatchSaveCount; i++)
            for (int i = 1; i <= 3000; i++)
            {

                Console.WriteLine("正在同步：" + mstrFormId_Middle + "第" + i + "/3000");

                strError1 = GetMiddleTable(ref i, p2DatabaseType);
                //如果是意外错误，不要继续处理了。停下来，排查问题。
                if (strError1.IsNullOrEmptyOrWhiteSpace() == false)
                {
                    strErrorLastChar = strError1.Substring(strError1.Length - 1, 1);
                    if (strErrorLastChar == "*")
                        return strError1;

                }

                if (strError1 != "")
                    strErrors = strErrors + strError1 + Environment.NewLine;

                //从同步工作台过滤来的，不用反复循环。
                if (mstrFId_Call != "")
                    return strErrors;

            }

            return strErrors;

        }

        private string AddSQLFilter_Header(string pSQL)
        {
            //以后，这里要改为参数
            //没必要，这是接口同步的思维，从接口取数，才需要的。
            //string strToday1 = System.DateTime.Now.AddDays(-1 * 10).ToString("yyyy -MM-dd");

            string strSQL = pSQL;

            if (mstrFId_Call.IsNullOrEmptyOrWhiteSpace() == false)
            {
                strSQL = strSQL + Environment.NewLine + string.Format(@" AND   s.FId={0}", mstrFId_Call);
                return strSQL;
            }

            if (mstrFBillNo_Call.IsNullOrEmptyOrWhiteSpace() == false)
            {
                strSQL = strSQL + Environment.NewLine + string.Format(@" AND   s.FBillNo='{0}'", mstrFBillNo_Call);
                return strSQL;
            }

            if (mstrFId_Call == "" || mstrFBillNo_Call == "0")
            {
                //判断附件的原因：不要判断是否成功，要判断，是否生成了附件。
                if (mstrFormId_Middle.EqualsIgnoreCase(K3FormId.strWB_ReceiptBill_Attachment_Mid) == false)
                {
                    //加回车符的原因：好死不死，直接调拨单，最后有注释。结果，卡死，因为条件也给注释掉了。
                    strSQL += Environment.NewLine + string.Format(@" AND   s.FSynchronStatusBob=0");

                }

            }

            strSQL += Environment.NewLine + " order by s.FId ";

            return strSQL;

        }


        //改动有点大，计划1月20号后删除掉。

        private string GetMiddleTable(ref int pRunTimes, int p2DatabaseType)
        {

            string strError = "";
            string strSQL;

            string strBobSQL = "";

            int intHeaderNodeIndex = 0;
            foreach (DynamicObject do1 in mDocNodek3)
            {
                int intThisNodeIndex = Convert.ToInt16(do1["NodeIndex"]);
                if (intThisNodeIndex == intHeaderNodeIndex)
                {
                    strBobSQL = Convert.ToString(do1["BobSql"]);
                    break;
                }
            }

            //这种情况，属于不需要处理，对吧？2022/12/23 20:03
            //报警要好一点，要不然，会以为同步成功了 2024/9/30 8:03
            if (strBobSQL == "")
            {
                //外面的次数，不要循环了。
                pRunTimes = 99999;
                return mstrFormId_Middle + " 还没有配置SQL *";
            }

            //2025/12/19 15:18 于龙光
            //这个好要拿掉，在外面做过了。

            //if (mstrFId_Call != "")
            //{
            //    //批量执行同步，会自动清洗
            //    //单个同步，在这里，也要洗一下。好象必要性不大，因为，每小时会同步一次吧？算了，先不管。
            //    //有必要性，总不能对用户说，你等1小时再做这份工作，对吧？

            //    string strCleanProcedure;
            //    if (mstrK3FormId == K3FormId.strSAL_SaleOrder)
            //        strCleanProcedure = "ahuPKUB_SAL_SaleOrder3_Special";
            //    else if (mstrK3FormId == K3FormId.strSal_OutStock)
            //        strCleanProcedure = "ahuPKUB_SAL_OutStock3_Special";
            //    else if (mstrK3FormId == K3FormId.strSAL_ReturnStock)
            //        strCleanProcedure = "ahuPKUB_SAL_ReturnStock3_Special";
            //    else
            //        throw new Exception("GetMiddleTable里，" + mstrK3FormId + "没有设定清洗存储过程");

            //    strSQL = string.Format(@"exec {0} {1}  ", strCleanProcedure, mstrFId_Call);

            //    CsData.BobExecuteWithoutTrans(mContext, mK3CloudApiClient, strSQL);

            //}

            strSQL = AddSQLFilter_Header(strBobSQL);

            //每次，取10个吧，太多，别撑坏了。
            string strBatchSave = string.Format(@"TOP ({0}) ", mStruct_K3LoginInfo.BatchSaveCount);
            //if (mstrK3FormId.EqualsIgnoreCase(K3FormId.strSal_OutStock))
            //{
            //    //when 5，timeout .
            //    strBatchSave = string.Format(@"TOP (50) ");
            //}

            strSQL = strSQL.Replace("TOP (500) ", strBatchSave);

            //这里，一定不要传context,因为，我想要打开中间表。2023/7/31
            DynamicObjectCollection docBillDad = CsData.GetDynamicObjects(mContext, mK3CloudApiClient, strSQL);

            if (docBillDad.Count == 0)
            {
                //外面的次数，不要循环了。
                pRunTimes = 99999;

                if (mstrFId_Call == "")
                    strError = "所有单据，都从中间表同步到金蝶了。" + mstrFormId_Middle;
                else
                {
                    strError = "没找到需要同步的单据。";
                }
                return strError;
            }

            mdocMiddleSource = docBillDad;

            if (mstrFId_Call != "")
            {
                DynamicObject doMiddleSource = mdocMiddleSource[0];
                string strFSYNCHRONSTATUSBOB = Convert.ToString(doMiddleSource["FSYNCHRONSTATUSBOB"]);
                if (strFSYNCHRONSTATUSBOB == "2" || strFSYNCHRONSTATUSBOB == "4")
                {
                    //这个功能，不太全，将就一下吧。
                    //以盘点单为例，这个错误提示，有个前提，先计算出单据类型。 因为，不知道单据类型，来源SQL ,取出来的，就是空。
                    //但是，往往基础资料错误时，是没法知道单据类型的,因为，都不会计算了。

                    //有错误，就不要硬顶着同步了。
                    //但是，这样不好吧？总要有改过自新的机会。2025/1/7 19:46
                    //问题：销售出库单同步出错后，就不会继续同步了。

                    //还是要提示。因为执行时，会自动清洗，这是校验的问题。
                    //比如说，采购入库单。 2025/1/8 10:10

                    string strFSYNCHRONERROR = Convert.ToString(doMiddleSource["FSYNCHRONERROR"]);
                    return strFSYNCHRONERROR;
                }
                else if (strFSYNCHRONSTATUSBOB == "1" && mstrFormId_Middle.EqualsIgnoreCase(K3FormId.strWB_ReceiptBill_Attachment_Mid) == false)
                {
                    //同步了，不要重复了。
                    string strFK3SYNCHRONIZETIME = Convert.ToString(doMiddleSource["FK3SYNCHRONIZETIME"]);
                    strError = "不需要同步，因为在：" + strFK3SYNCHRONIZETIME + " 就同步过了。";
                    return strError;
                }

            }



            strError = BeginSaveAfterGetDataTable(ref pRunTimes, p2DatabaseType);

            return strError;

        }
        private string BeginSaveAfterGetDataTable(ref int pRunTimes, int p2DatabaseType)
        {

            string strReturnType = "";

            if (mStruct_K3LoginInfo.LoginK3Ok == false || mStruct_K3LoginInfo.AcctID == null)
            {
                //有时，登陆了，参数却没有取得值。
                int IntReturn = ClsPublic.LogIn(ref mStruct_K3LoginInfo, mContext, ref mK3CloudApiClient
                    , ref strReturnType, true, mintDatabaseType);
                if (IntReturn != 1)
                    return strReturnType;

            }

            string strError = "";

            try
            {
                strError = Save2KingdeeByTableB(ref pRunTimes, p2DatabaseType);
            }

            catch (Exception ex)
            {
                strError = CsErrLog.GetExceptionWithLog(ex);

                //return strError;
                //2024/10/13  下面有问题，可能没地方记录。
                //不过，当初加了，应该有什么原因，先观察一下吧。
                ClsPublic.UpdateMiddleTableSynchronizeError(
                    mContext, mK3CloudApiClient, mstrTableName_Middle, mstrFId_Call, strError, mstrFormId_Middle);
            }
            finally
            {

            }
            return strError;
        }


        private string Save2KingdeeByTableB(ref int pRunTimes, int p2DatabaseType)
        {

            string strErrors = "";

            strErrors = Pushs(ref pRunTimes, p2DatabaseType);

            //不要退出，还要处理正常的。
            //return strError;

            //如果是意外错误，不要继续处理了。停下来，排查问题。
            if (strErrors.IsNullOrEmptyOrWhiteSpace() == false)
            {
                string strErrorLastChar2 = strErrors.Substring(strErrors.Length - 1, 1);
                if (strErrorLastChar2 == "*")
                    return strErrors;
            }

            return strErrors;

        }


        private string Pushs(ref int pRunTimes, int p2DatabaseType)
        {

            string strError = "";
            string strErrors = "";


            //string strK3TableName = CsPublic2.GetTableNameByFormId(mstrK3FormId);

            string strField_BillNoK3 = "FBillNO";

            int intTotal = mdocMiddleSource.Count;
            int intCureent = 0;


            string strToken = "";
            if (mbolNeedCBSToken)
            {
                strToken = CsCallCBS.getToken(mContext, mK3CloudApiClient, p2DatabaseType, mStruct_K3LoginInfo, ref strError);

            }


            if (strError != "")
                return strError;

            foreach (DynamicObject doMiddleDad in mdocMiddleSource)
            {
                intCureent++;

                if (intCureent >= 49)
                    Debug.Assert(true);

                TimeSpan currentTime = DateTime.Now.TimeOfDay;
                TimeSpan timeBefore = mStruct_K3LoginInfo.iisResetTime - TimeSpan.FromMinutes(10);
                TimeSpan timeAfter = mStruct_K3LoginInfo.iisResetTime + TimeSpan.FromMinutes(10);

                if (currentTime >= timeBefore && currentTime <= timeAfter)
                {
                    // 到了IIS 重启时间，就退出来算了。
                    //不退出来，我也干不了什么，context， 都不能用了。
                    return "不同步了，快到了回收IIS时间（" + mStruct_K3LoginInfo.iisResetTime + "），过20分钟再开干吧。*";
                }

                DateTime dtStart = DateTime.Now;
                string strFBillNo_Middle = Convert.ToString(doMiddleDad[strField_BillNoK3]).Trim();

                Console.WriteLine(dtStart + " 正在处理" + mstrTableName_Middle + "," + strFBillNo_Middle + "，第" + intCureent + "/" + intTotal);

                strFBillNo_Middle = strFBillNo_Middle.Replace("\n", "");
                string strFId_Middle = Convert.ToString(doMiddleDad["FId"]);
                string strFOrgId_Middle = Convert.ToString(doMiddleDad["FORGIDK3"]);

                if (strFId_Middle == "")
                    throw new Exception("太魔幻了，中间表Id，从SQL中取出来，值居然为空。");

                string strFK3Id = "";
                string strFileName_DZHD = "";
                string strFileURL_DZHD = "";
                string strFTransactiondate = "";
                string strFCheckCode = "";
                string strFBankSerialNumber = "";
                if (mstrFormId_K3.EqualsIgnoreCase(K3FormId.strWB_RecBankTradeDetail))
                {
                    string strFPushByAP_OtherPayable = Convert.ToString(doMiddleDad["FPushByAP_OtherPayable"]);
                }
                else if (mstrFormId_K3.EqualsIgnoreCase(K3FormId.strWB_ReceiptBill))
                {
                    strFK3Id = Convert.ToString(doMiddleDad["K3FId"]);
                    strFileName_DZHD = Convert.ToString(doMiddleDad["FbucketFileName"]);
                    strFileURL_DZHD = Convert.ToString(doMiddleDad["FbucketFileUrl"]);
                    strFTransactiondate = Convert.ToString(doMiddleDad["FTransactiondate"]);
                    strFCheckCode = Convert.ToString(doMiddleDad["FCheckCode"]);
                    strFBankSerialNumber = Convert.ToString(doMiddleDad["FBankSerialNumber"]);
                }


                if (strFBillNo_Middle == "")
                {
                    strError = "同步失败：单据编号为空。";
                    ClsPublic.UpdateMiddleTableSynchronizeError(mContext, mK3CloudApiClient, mstrTableName_Middle, strFId_Middle
                        , strError, mstrFormId_Middle);

                    if (strErrors != "")
                        strErrors += Environment.NewLine;

                    strErrors += strError;

                    continue;
                }


                if (mstrFormId_Middle.EqualsIgnoreCase(K3FormId.strWB_ReceiptBill_Attachment_Mid) &&
                    mstrFormId_Middle.EqualsIgnoreCase(K3FormId.strWB_ReceiptBill_Mid) &&
                    mstrFormId_Middle.EqualsIgnoreCase(K3FormId.strWB_RecBankTradeDetail_Mid) 
                    )
                    throw new Exception("请写清洗过程，判断是否同步到了下游单据。");

                //下推前，不需要在这里写代码来判断，要在存储过程里做清洗。2026/2/4 14:05 于龙光

                if (strFId_Middle == "")
                {
                    strError = "奇怪，strFId_Middle值为空";
                    strError += "；strFBillNo_Middle=" + strFBillNo_Middle;
                    strError += "；strFOrgId_Middle=" + strFOrgId_Middle;
                    strError += "；strFTransactiondate=" + strFTransactiondate;
                    strError += "；strFileName_DZHD=" + strFileName_DZHD;
                    strError += "；strFileURL_DZHD=" + strFileURL_DZHD;
                    strError += "；strToken=" + strToken;
                    throw new Exception(strError);
                }

                strError = Push1(strFId_Middle, strFK3Id, strFBillNo_Middle, strFOrgId_Middle, strFTransactiondate
                    , strFileName_DZHD, strFileURL_DZHD, strToken, strFCheckCode, strFBankSerialNumber);

                if (strError != "")
                {
                    if (strErrors != "")
                        strErrors += Environment.NewLine;

                    strErrors += strError;
                }



            }

            Thread.Sleep(100);
            return strErrors;

        }

        private string Push1(string pFId_Middle, string pFK3Id, string pFBillNo_Middle
                      , string pTargetOrgId
                      , string pFDate_DZHD
                      , string pFileName_DZHD
                      , string pFileURL_DZHD
                      , string pToken,string pCheckCode, string pBankSerialNumber

            )
        {

            string strError = "";

            //string strRuleId;
            string strNewIds = "";
            string strConvertInfo = "";
            string strSourceFormId = mstrFormId_Middle;
            string strSourceBillNo = pFBillNo_Middle;
            bool bolIsDraftWhenSaveFail = false;

            if (mstrFormId_Middle.EqualsIgnoreCase(K3FormId.strWB_ReceiptBill_Attachment_Mid) == false)

            {
                string strK3BillNo = pFBillNo_Middle;

                var JObjectTier_Bob = new
                {
                    Numbers = string.Format("{0}", strSourceBillNo),
                    RuleId = mstrPushRuleId,
                    TargetOrgId = pTargetOrgId,
                    IsDraftWhenSaveFail = bolIsDraftWhenSaveFail,
                };
                string strJSon = JsonConvert.SerializeObject(JObjectTier_Bob, Formatting.Indented);

                string strResultJson = mK3CloudApiClient.Push(strSourceFormId, strJSon);
                JObject joResultJson = JObject.Parse(strResultJson);

                var successEntitiesToken = joResultJson["Result"]?["ResponseStatus"]?["SuccessEntitys"];
                if (successEntitiesToken is JArray successEntities && successEntities.Count > 0)
                {
                    strNewIds = Convert.ToString(successEntities[0]["Id"] ?? string.Empty);
                }

                if (strNewIds == "")
                {

                    var root2 = JObject.Parse(strResultJson);
                    var errors = root2["Result"]?["ResponseStatus"]?["Errors"];

                    var errorMessages = errors
                        .Select(token => token["Message"]?.ToString()) // ← 这里 token 是 JToken
                        .Where(msg => !string.IsNullOrEmpty(msg))
                        .ToArray();

                    string combinedErrors1 = string.Join("；", errorMessages);

                    string combinedErrors2 = string.Join("；", combinedErrors1); // 使用中文分号
                    if (combinedErrors2 != "")
                        strError = strConvertInfo + combinedErrors2;



                    ClsPublic.UpdateMiddleTableSynchronizeError(mContext, mK3CloudApiClient
                , mstrTableName_Middle, pFId_Middle, strError, mstrFormId_K3);

                    return strError;
                }

                ClsPublic.UpdateMiddleTableSynchronizeOK(mContext, mK3CloudApiClient
            , mstrTableName_Middle, pFId_Middle, pFBillNo_Middle, strNewIds, false);


                //电子回单，如果审核了，无法写入附件,所以，要排除掉。
                //交易明细，不需要审核，是自动的。
                if (mstrFormId_Middle.EqualsIgnoreCase(K3FormId.strWB_ReceiptBill) == false &&
                    mstrFormId_Middle.EqualsIgnoreCase(K3FormId.strWB_ReceiptBill_Mid) == false  &&
                    mstrFormId_Middle.EqualsIgnoreCase(K3FormId.strWB_RecBankTradeDetail) == false)
                {
                    string strSaveFormId = mstrFormId_K3;

                    JObject JObjectRoot2 = new JObject();
                    JObjectRoot2.Add("Ids", strNewIds);
                    string strJSon2 = JObjectRoot2.ToString();

                    strResultJson = mK3CloudApiClient.Submit(strSaveFormId, strJSon2);

                    strResultJson = mK3CloudApiClient.Audit(strSaveFormId, strJSon2);

                }


                if (mstrFormId_K3.EqualsIgnoreCase(K3FormId.strWB_ReceiptBill) == false)
                    return "";

            }




            DateTime dtFDate_DZHD = Convert.ToDateTime(pFDate_DZHD);

            //同步7天7的电子回单，还需要刷新附件。
            if (dtFDate_DZHD.AddDays(7) <= DateTime.Today)
            {
                string strFDate_DZHD = dtFDate_DZHD.ToString("yyyy-MM-dd");
                //RefreshPdf(pFileURL_DZHD, strFDate_DZHD);
                RefreshPdf(pFileName_DZHD, strFDate_DZHD, ref strError, pToken);
                if (strError != "")
                    return strError;

                //还要重新读取附件，晕死。
                pFileURL_DZHD=ReadPdfUrl(pCheckCode, pBankSerialNumber, strFDate_DZHD, ref strError, pToken);

                if (strError != "")
                    return strError;

                //更新中间表
                string strSQL = string.Format(@"Update t_ReceiptBill_Midd set FBucketfileurl='{0}' Where FId={1}", pFileURL_DZHD, pFId_Middle);
                CsData.BobExecute(mContext,mK3CloudApiClient,strSQL);
            }

            CsUploadFile CsUploadFile1 = new CsUploadFile();

            string strResultJson_DZHD = "";
            try
            {
                strResultJson_DZHD = CsUploadFile1.RunUpLoad1ExcelMain(
                         mContext, mK3CloudApiClient, mstrFormId_K3, strNewIds, pFBillNo_Middle
                        , pFileURL_DZHD, pFileName_DZHD, ref strError, "从CBS下载");

            }
            catch (Exception ex1)
            {
                strError = ex1.Message;

                strError = "电子文件从cbs下载到金蝶失败了。" + strError;

                if (pFId_Middle == "")
                {
                    strError += Environment.NewLine + "糟糕，pMiddleID为空.电子回单号：" + pFBillNo_Middle;
                    return strError;
                }

                ClsPublic.UpdateMiddleTableSynchronizeError(mContext, mK3CloudApiClient
                    , mstrTableName_Middle, pFId_Middle, strError, mstrFormId_K3);
                return strError;

            }

            if (strError.Contains("已同步过了"))
            {
                ClsPublic.UpdateMiddleTableSynchronizeOKById(mContext, mK3CloudApiClient, mstrTableName_Middle, pFId_Middle, false);

                return strError;
            }

            if (strError != "")
            {
                if (strError.Contains("远程服务器返回错误: (403) 已禁止。"))
                {
                    strError = "单据同步成功，但pdf附件下载失败，只能下载7天内的文件。";
                }

                ClsPublic.UpdateMiddleTableSynchronizeError(mContext, mK3CloudApiClient, mstrTableName_Middle, pFId_Middle, strError, mstrFormId_K3);

                return strError;
            }



            Thread.Sleep(100);
            return "";

        }
        //public void RefreshPdf(Struct_K3LoginInfo pStruct_K3LoginInfo,  string pFile, string strFDate, ref string pError
        //     ,int p2DatabaseType)
        //{
        //    //DateTime FDate = Convert.ToDateTime(strFDate);
        //    //DateTime FDate1 = FDate.AddDays(-10);
        //    //DateTime FDate2 = FDate.AddDays(10);

        //    //string strstartDate = FDate1.ToString("yyyy-MM-dd");
        //    //string strendDate = FDate2.ToString("yyyy-MM-dd");


        //    var jsonObject = new
        //    {
        //        bucketFileNames = new[] { pFile },
        //        endDate = strFDate,
        //        startDate = strFDate,
        //    };

        //    string strJsonResult = "";
        //    string strRefreshUrl = "openapi/account/openapi/v1/electronic-bill/batch-refresh";
        //    string strJSon = jsonObject.ToString();
        //    strJSon = JsonConvert.SerializeObject(jsonObject);

        //    //测试不行。
        //    //TestCBSDll TestCBSDll1 = new TestCBSDll();
        //    //string strReturn2 = TestCBSDll1.CallCBS_Demo(strRefreshUrl, strJSon, ref strJsonResult, ref strError);

        //    CsCallCBS CsCallCBS1 = new CsCallCBS();

        //    string strToken = CsCallCBS.getToken(mContext,mK3CloudApiClient, p2DatabaseType, pStruct_K3LoginInfo, ref pError);
        //    if (pError != "")
        //        return ;

        //    CsCallCBS1.CallCBSWithToken(
        //        mStruct_K3LoginInfo, strRefreshUrl, strJSon, ref strJsonResult, ref pError, strToken);

        //}
        public void RefreshPdf(string pFile, string strFDate, ref string pError, string pToken)
        {
            //DateTime FDate = Convert.ToDateTime(strFDate);
            //DateTime FDate1 = FDate.AddDays(-10);
            //DateTime FDate2 = FDate.AddDays(10);

            //string strstartDate = FDate1.ToString("yyyy-MM-dd");
            //string strendDate = FDate2.ToString("yyyy-MM-dd");


            var jsonObject = new
            {
                bucketFileNames = new[] { pFile },
                endDate = strFDate,
                startDate = strFDate,
            };

            string strJsonResult = "";
            string strRefreshUrl = "openapi/account/openapi/v1/electronic-bill/batch-refresh";
            string strJSon = JsonConvert.SerializeObject(jsonObject);

            //测试不行。
            //TestCBSDll TestCBSDll1 = new TestCBSDll();
            //string strReturn2 = TestCBSDll1.CallCBS_Demo(strRefreshUrl, strJSon, ref strJsonResult, ref strError);

            CsCallCBS CsCallCBS1 = new CsCallCBS();
            CsCallCBS1.CallCBSWithToken(
                mStruct_K3LoginInfo, strRefreshUrl, strJSon, ref strJsonResult, ref pError, pToken);


            //调用子回单接口，读取新的文件地址。



        }


        public string ReadPdfUrl(string pCheckCode,string pBankSerialNumber, string strFDate, ref string pError, string pToken)
        {

            var jsonObject = new
            {
                startDate = strFDate,
                endDate = strFDate,
                currentPage = "0",
                pageSize = "10",
                checkCodeList = new[] { pCheckCode },
            };

            string strJsonResult = "";
            string strRefreshUrl = "openapi/account/openapi/v1/electronic-bill/query";
            string strJSon = JsonConvert.SerializeObject(jsonObject);

            CsCallCBS CsCallCBS1 = new CsCallCBS();
            CsCallCBS1.CallCBSWithToken(
                mStruct_K3LoginInfo, strRefreshUrl, strJSon, ref strJsonResult, ref pError, pToken);

            // 如果调用失败，直接返回空
            if (!string.IsNullOrEmpty(pError))
            {
                return "";
            }

            // 解析返回的 JSON
            var response = JsonConvert.DeserializeObject<System.Collections.Generic.Dictionary<string, object>>(strJsonResult);
            if (response == null || response["code"]?.ToString() != "0")
            {
                pError = response?["msg"]?.ToString() ?? "接口返回非成功状态";
                return "";
            }

            var data = response["data"] as JObject;
            var list = data?["list"] as JArray;

            if (list == null || !list.Any())
            {
                pError = "未查询到电子回单记录";
                return "";
            }

            // 遍历回单列表，查找 bankSerialNumber 匹配的记录
            foreach (var item in list)
            {
                var bankSerialNumber = item["bankSerialNumber"]?.ToString();
                if (string.Equals(bankSerialNumber, pBankSerialNumber, StringComparison.OrdinalIgnoreCase))
                {
                    string strBucketFileUrl = item["bucketFileUrl"]?.ToString() ?? "";
                    return strBucketFileUrl;
                }
            }

            pError = $"未找到匹配 bankSerialNumber: {pBankSerialNumber} 的回单记录";
            return "";

        }

    }

}


