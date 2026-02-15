using System;
using System.ComponentModel;

using Newtonsoft.Json.Linq;
using Kingdee.BOS;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.App.Data;
using Kingdee.BOS.Core.DynamicForm.PlugIn;
using System.Diagnostics;
using Kingdee.BOS.Util;
using Kingdee.BOS.Core.DynamicForm;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Collections.Generic;
using Kingdee.BOS.WebApi.Client;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using ahu.YuYue.CBS;
using static ahu.YuYue.CBS.CacheHelper;
using System.Linq;

namespace ahu.YuYue.CBS
{

    [Description("银行接收交易明细，从CBS，写到中间表")]

    [Kingdee.BOS.Util.HotUpdate]
    public class CsOthers2MiddleTable
    {
        Context mContext;

        private K3CloudApiClient mK3CloudApiClient1;
        private Context mContext1;
        Struct_K3LoginInfo mStruct_K3LoginInfo;

        //2022/12/17 15:50
        //DataTable mdtNode;
        JToken mJToken_WDT;

        DynamicObjectCollection mdocBillNo, mdocMatchFields;

        public void Call2MiddleTable(Context pContext, K3CloudApiClient pK3CloudApiClient1
            , Struct_K3LoginInfo pStruct_K3LoginInfo
            , JToken pJToken_WDT, bool pNeedCheckIfExists
            , string pFormIdMid
            , string pTableNameMidd
            , string pOthereDateField, string pOthereBillNoField
            , string pDate)
        {
            mContext1 = pContext;
            mK3CloudApiClient1 = pK3CloudApiClient1;
            mStruct_K3LoginInfo = pStruct_K3LoginInfo;
            mJToken_WDT = pJToken_WDT;
            mContext = pContext;
            try
            {

                BuildCache(pFormIdMid, pNeedCheckIfExists, pDate, pTableNameMidd, pOthereDateField);

                Save2MidTable(pNeedCheckIfExists, pFormIdMid, pTableNameMidd, pOthereBillNoField);
            }
            catch (Exception ex)
            {
                throw new Exception(CsErrLog.GetExceptionWithLog(ex));
            }
        }

        private void Save2MidTable(bool pNeedCheckIfExists,string pFormIdMidd, string pTableNameMidd, string pOthereBillNoField)
        {
            string  strMiddleTable_Z="Z"+ pTableNameMidd.Substring(1);

            int intBillNoLength=50;
            if (pFormIdMidd.EqualsIgnoreCase(K3FormId.strWB_ReceiptBill_Mid))
            {
                intBillNoLength = 500;
            }
            else if (pFormIdMidd.EqualsIgnoreCase(K3FormId.strWB_ReceiptBill_Attachment_Mid))
            {
                //电子回单附件，不需要同步到中间表。
                return;
  
            }

            string strFields = "";
            foreach (DynamicObject doField1 in mdocMatchFields)
            {
                string strField1 = Convert.ToString(doField1["FieldK3"]);
                if (strField1=="")
                {
                    strField1 = "F" + Convert.ToString(doField1["FieldMatch"]);
                    strField1 = strField1.Replace("\r\n", "");
                }
                strFields += strField1 + ",";
            }
            strFields = strFields.TrimEnd(',');

            StringBuilder sbCheckEntryCount = new StringBuilder();
            StringBuilder sbh = new StringBuilder();
            StringBuilder sbDelete = new StringBuilder();

            JToken jTokenList = mJToken_WDT["list"];

            int intCount = jTokenList.Count();



            long lngFID_Add = GetId(strMiddleTable_Z, intCount);



            string strSQL_Insert_Fields = string.Format(@"
INSERT INTO[dbo].{1}
  (FID, FBillNo,{0}) Values", strFields, pTableNameMidd);

            sbh.AppendLine(strSQL_Insert_Fields);

            bool bolHadData = false;
            int intSaveBill = 0;

            string strSQL;
            foreach (JObject joDad in jTokenList)
            {
                //电子回单，和交易明细，是不同的.............................................
                string strFBillNo = ClsPublic.ReadJObject(joDad, pOthereBillNoField, intBillNoLength);
                strFBillNo = strFBillNo.Trim();

                if (pNeedCheckIfExists == true)
                {

                    if (pFormIdMidd.EqualsIgnoreCase(K3FormId.strWB_RecBankTradeDetail_Mid))
                    {
                        //先删除历史记录中，没有对帐码和银行流水号
                        strSQL = string.Format(@"/*dialect*/
delete
  from  {1} 
where  FSYNCHRONSTATUSBOB=0
and    (FcheckCode=''  or  FbankSerialNumber='')
and     FBILLNO='{0}'", strFBillNo, pTableNameMidd);
                        sbDelete.AppendLine(strSQL);
                    }

                    string strError = CheckExist(strFBillNo);

                    if (strError != "")
                    {
                        //退出 docTables 子项循环，而不是所有单据，老胡我可是亲自确认了的。2025/2/8 19：41
                        continue;
                    }


                }

                bolHadData = true;

                //只取max会有问题的，全部重复。
                //因为批量保存，取第2项时，第1项，还没有保存呢。
                //string strFId = GetNewFId(mContext, mK3CloudApiClient1, strTable, strTable_Z);
                string strValues =string.Format(@"({0},'{1}',",lngFID_Add, strFBillNo);

                foreach (DynamicObject doField1 in mdocMatchFields)
                {
                    int intLength = Convert.ToInt16(doField1["Length"]);
                    string strField1 = Convert.ToString(doField1["FieldMatch"]);
                    strField1 = strField1.Replace("\r\n","").Trim();
                    //strField1 = strField1.Substring(1).Trim(); //以前是F开头的，我拿掉了。
                    string strValue1 = ClsPublic.ReadJObject(joDad, strField1, intLength);
                    strValues += "'"+strValue1 + "',";
                }
                strValues = strValues.TrimEnd(',');
                strValues += "),";
                sbh.AppendLine(strValues);

                intSaveBill++;

                //每30个单据写一次？不是的，有参数呢。
                //参考保存到金蝶
                //不能用这个参数。这是从接口，直接同步到金蝶的。如果用它，就乱套了，另外设定一个吧。
                //if (intSaveBill >= mStruct_K3LoginInfo.BatchSaveCount)
                if (intSaveBill >= 30)
                {
                    strSQL = sbDelete.ToString();
                    CsData.BobExecute(mContext, mK3CloudApiClient1, strSQL);
                    sbDelete.Clear();

                    string strSQLh1 = sbh.ToString();
                    strSQLh1 = strSQLh1.Substring(0, strSQLh1.Length - 3);
                    CsData.BobExecute(mContext, mK3CloudApiClient1, strSQLh1);

                    sbh.Clear();
                    sbh.AppendLine(strSQL_Insert_Fields);

                    bolHadData = false;
                    intSaveBill = 0;
                }

                lngFID_Add++;

            }

            if (bolHadData == false)
                return;

            strSQL = sbDelete.ToString();
            CsData.BobExecute(mContext, mK3CloudApiClient1, strSQL);

            string strSQLh2 = sbh.ToString();
            ////这个语法不行，后面，还有一个回车符。            
            ////strSQLh = strSQLh.TrimEnd(',');
            //string strSQLh = sbh.ToString();
            strSQLh2 = strSQLh2.Substring(0, strSQLh2.Length - 3);

            try
            {
                CsData.BobExecute(mContext, mK3CloudApiClient1, strSQLh2);
            }
            catch (Exception ex)
            {
                throw new Exception(CsErrLog.GetExceptionWithLog(ex));
            }

        }


        public void BuildCache(string pFormIdMid, bool pCheckExist, string pBillDate
            , string pMiddleTableName, string pOthereDateField)

        {

            StringBuilder Sb1 = new StringBuilder();
            string strSQL = string.Format(@"/*dialect*/
DECLARE @NewFId BigInt 
DECLARE @NewEntryId BigInt 
");
            Sb1.AppendLine(strSQL);

            // 构造唯一的缓存键,这样，不用每调用一次接口，从SQL 交互一次。
            //在不同日期间循环时，注明失效。
            //不希望，每次一定要重新拉取吧，才能避免重复。
            string strCacheKeyDocBillNo = $"ImportData_FBillNo_{pMiddleTableName}_{pBillDate}";
            string strCacheKeytblMatch = "ImportDatatblMatch" + pMiddleTableName;

            if (CacheHelper.CacheContext.UpdateCache)
            {
                // 清除相关缓存
                CacheHelper.ClearCacheForDocument(strCacheKeyDocBillNo);
                CacheHelper.ClearCacheForDocument(strCacheKeytblMatch);
                CacheContext.UpdateCache = false; // 重置标志位，要放到最后面。
            }

            // 构造唯一的缓存键

            if (CacheHelper.CacheContext.UpdateCache)
            {
                // 清除相关缓存
                //CacheHelper.ClearCacheForDocument(strCacheKeyDocBillType);
                CacheContext.UpdateCache = false; // 重置标志位
            }
            //删除单据后，这里不能同步。
            //难道要加一个命令，清除缓存？
            // 尝试从静态类中获取缓存的结果
            mdocBillNo = CacheHelper.GetMdocBillNo(strCacheKeyDocBillNo);
            // 如果没有找到，则执行查询并缓存结果
            if ((mdocBillNo == null || mdocBillNo.Count==0) && pCheckExist == true)
            {
                //***注意，这里可能有坑***
                //日期字段如果包含时分秒，要用cast({2} as date)
                strSQL = String.Format(@"
SELECT FBillNO,{2},FCreateDate
FROM {0}
WHERE {2}='{1}'", pMiddleTableName, pBillDate, pOthereDateField);
                mdocBillNo = CsData.GetDynamicObjects(mContext1, mK3CloudApiClient1, strSQL);

                // 将结果保存到静态类中
                CacheHelper.SetMdocBillNo(strCacheKeyDocBillNo, mdocBillNo);
            }

            // 尝试从静态类中获取缓存的结果
            mdocMatchFields = CacheHelper.GetMdocBillNo(strCacheKeytblMatch);
            if (mdocMatchFields == null)
            {
                strSQL = String.Format(@"
SELECT [FormID]
      ,[Caption]
      ,[FieldMatch],FieldK3
      ,[NodeText]
      ,Length
  FROM  tblMiddMatchCBS  with (nolock)
  Where FormId='{0}'
   AND  IsNull(IsForbid,0)=0", pFormIdMid);

                mdocMatchFields = CsData.GetDynamicObjects(mContext1, mK3CloudApiClient1, strSQL);
                if (mdocMatchFields.Count == 0)
                {
                    throw new Exception("请洽程序员计员，tblMiddMatchMT 没有设定：" + pFormIdMid);
                }
                CacheHelper.SetMdocBillNo(strCacheKeytblMatch, mdocMatchFields);
            }

        }

        private string CheckExist(string pBillNo)
        {

            if (mdocBillNo == null || mdocBillNo.Count == 0)
            {
                return "";
            }

            pBillNo = pBillNo.Trim();


            // 3. LINQ FirstOrDefault 短路查询，找到即终止，底层迭代器优化
            var existBill = mdocBillNo.FirstOrDefault(doObj =>
                string.Equals(Convert.ToString(doObj["FBillNo"])?.Trim(), pBillNo, StringComparison.Ordinal)
            );

            // 匹配到则返回原业务的提示语，未匹配则返回空
            return existBill == null ? "" : $"在 {Convert.ToString(existBill["FCreateDate"]) ?? ""} 时已经产生了。";



        }

        private string FindValue(DynamicObjectCollection doc1, string pSeekField, string pFindText, string pReturnField)
        {
            foreach (DynamicObject do1 in doc1)
            {
                string seekValue = Convert.ToString(do1[pSeekField]);
                if (seekValue == pFindText)
                {
                    return Convert.ToString(do1[pReturnField]);
                }
            }

            // 如果没有找到匹配项，返回空字符串
            return "";
        }

        private long GetId(string strMiddleTable_Z, int pRowCount)
        {
            List<string> lstSQL = new List<string>();

            string strSQL = string.Format(@"/*dialect*/
DECLARE @NewFId BigInt 
insert into {0}(Column1) values (0)
select @NewFId  = SCOPE_IDENTITY();
select @NewFId  --这一行不能漏，否则，取出值为空字符串。
", strMiddleTable_Z);
            string strFId_Add = CsData.GetTopValue(mContext, mK3CloudApiClient1, strSQL);
            if (strFId_Add == "")
                strFId_Add = "0";

            long lngF_Add = Convert.ToInt64(strFId_Add);

            //把ID 先占住,不希望重复。
            strSQL = string.Format(@"
insert into {0}(Column1) values(0)
", strMiddleTable_Z);
            for (int i = 0; i <= pRowCount + 2; i++)
            {
                lstSQL.Add(strSQL);
            }
            strSQL = string.Join("", lstSQL);
            CsData.BobExecute(mContext, mK3CloudApiClient1, strSQL);

            return lngF_Add;
        }
    }
}


