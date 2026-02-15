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
using System.Text;
using Kingdee.BOS.Orm;
using Kingdee.BOS.Contracts;
using Kingdee.BOS.App;
using Kingdee.BOS.Core.Metadata;
using Kingdee.BOS.Core.Validation;
using Newtonsoft.Json;
using System.Linq;

namespace ahu.YuYue.CBS
{
    [Kingdee.BOS.Util.HotUpdate]
    public class CsFlow2Receipt
    {
        K3CloudApiClient mK3CloudApiClient1;
        Struct_K3LoginInfo mstruct_K3LoginInfo;
        Context mContext;
        string mstrFormId;
        string mstrFId;

        public void SetCloudApiClient(K3CloudApiClient pK3CloudApiClient1)
        {
            mK3CloudApiClient1 = pK3CloudApiClient1;
        }

        public string Call2KingdeeMain(Context pContext, K3CloudApiClient pK3CloudApiClient
           , Struct_K3LoginInfo pStruct_K3LoginInfo, string pFormId
           , string pFId, int pintRunMode)
        {

            mContext = pContext;
            mK3CloudApiClient1 = pK3CloudApiClient;

            mstrFormId = pFormId;


            mContext = pContext;
            mstruct_K3LoginInfo = pStruct_K3LoginInfo;

            mstrFId = pFId;

            string strError = TBGZT2Kingdee_b(pintRunMode);
            return strError;
        }

        private string TBGZT2Kingdee_b(int pintRunMode)
        {
            bool bolNeedLoginK3 = true;
            string strReturnType = "";
            int IntReturn = ClsPublic.LogIn(ref mstruct_K3LoginInfo, mContext, ref mK3CloudApiClient1, ref strReturnType, bolNeedLoginK3, pintRunMode);
            if (IntReturn != 1)
                return strReturnType;

            try
            {
                string strError1 = Save2Kingdee();
                return strError1;
            }

            catch (Exception ex)
            {
                throw new Exception(CsErrLog.GetExceptionWithLog(ex));
            }
        }


        private string Save2Kingdee()
        {

            string strError = "";

            if (mstrFId == "")
            {
                strError = string.Format(@" 不支持批量导入。");
                return strError;
            }

            string strSQL = string.Format(@"
select TOP 1 FReceiptNo
from   T_CN_BANKCASHFLOW 
Where  FId='{0}'  ", mstrFId);
            string strFReceiptNo = CsData.GetTopValue(mContext, mK3CloudApiClient1, strSQL);

            if (strFReceiptNo == "")
            {
                strError = string.Format(@" 没有对账码或银行流水号，无法生成电子回单");
                return strError;
            }

            strSQL = string.Format(@"
Select top 1 1 
From   T_CN_BANKCASHFLOW
where  FReceiptNo='{0}'
and    FID<>{1}", strFReceiptNo, mstrFId);
            string strExist = CsData.GetTopValue(mContext, mK3CloudApiClient1, strSQL);
            if (strExist == "1")
            {
                strError = string.Format(@" 电子回单关联标记重复【银行对帐码+银行流水号】， {0}", strFReceiptNo);
                return strError;
            }


            strSQL = string.Format(@"
select TOP 1 rec.FBillNo
from   T_CN_BANKCASHFLOW flo inner join 
       T_WB_RECEIPT rec on flo.FreceiptNo =rec.FDetailNo
Where  flo.FId='{0}'
And    flo.FReceiptNo<>''  ", mstrFId);
            string strFBillNo_ReceiptBill = CsData.GetTopValue(mContext, mK3CloudApiClient1, strSQL);
            if (strFBillNo_ReceiptBill != "")
            {
                strError = string.Format(@"已生成电子回单，单据号： {0}", strFBillNo_ReceiptBill);
                return strError;
            }
            strSQL = string.Format(@"
Select top 1 FDate from tblReadMes2 
where formid = 'WB_ReceiptBill'
and fnumber='{0}' ", strFBillNo_ReceiptBill);
            string strFDate = CsData.GetTopValue(mContext, mK3CloudApiClient1, strSQL);


            strSQL = string.Format(@"
select TOP 1 FBillNo 
, org.FNUMBER FPAYORGID
,FTRANSDATE --
,FRecBillNo AS FSRCBILLNO 
,FBUSIREFENO 
,acnt.FNUMBER as FACCOUNTID  
,ban.FNUMBER AS FBankId  
,FBANKACNTNAME as FACCOUNTNAME    
,cur.FNUMBER as FCURRENCYID  
,FDEBITAMOUNT
,FCREDITAMOUNT 
,FDEBITAMOUNT+FCREDITAMOUNT as  FAmount 
,FEXPLANATION
,FOppOpenBankName AS  FOPPOSITEBANKNAME 
,FOppBankAcntNo as FOPPOSITEBANKACNTNO 
,FOppBankAcntName as FOPPOSITECCOUNTNAME 

,FBNKSEQNO  as FFlowNo 
,FreceiptNo as FDetailNo
,FRecBillType  as FSRCFormId     
,bil.FNUMBER as FSRCBILLTYPEID    
,FKDRETFLAG 
--,FReceiptInfo  拿掉
,FEXPLANATION as  FRemark 
from T_CN_BANKCASHFLOW flo inner join 
     T_BAS_BILLTYPE bil on flo.FBUSINESSTYPE=bil.FBILLTYPEID left outer join
	 T_BD_CURRENCY cur on flo.FCURRENCYID=cur.FCURRENCYID   left outer join
	 T_ORG_ORGANIZATIONS org on flo.FPAYORGID=org.FORGID left outer join
	 T_CN_BANKACNT acnt on flo.FACCOUNTID=acnt.FBankAcntId   left outer join
	 T_BD_BANK ban on acnt.FBANKID=ban.FBANKID
where flo.fid={0}", mstrFId);
            DynamicObjectCollection docH = CsData.GetDynamicObjects(mContext, mK3CloudApiClient1, strSQL);
            string strFBillNo_Middle = Convert.ToString(docH[0]["FBillNo"]);

            //尽量从电子回单中读取日期，因为单个同步时，要传入自己的日期。
            if (strFDate == "")
                strFDate = Convert.ToString(docH[0]["FTRANSDATE"]);

            var jsonObject = new
            {
                // FormId = "PRD_INSTOCK",   用mK3CloudApiClient1调试，不需要这样的。
                // data = new
                // {
                IsVerifyBaseDataField = "true",
                ValidateFlag = "true",
                NumberSearch = "true",
                IsAutoAdjustField = "false",
                InterationFlags = "",
                IgnoreInterationFlag = "true",
                //Model =  new  用下面的语法，想改为数组
                Model = docH.AsEnumerable().Select(doHead => new
                {
                    FBillNo = Convert.ToString(doHead["FDetailNo"]),
                    FPAYORGID = new { FNumber = Convert.ToString(doHead["FPAYORGID"]) },
                    FDate = strFDate,
                    FACCOUNTID = new { FNumber = Convert.ToString(doHead["FACCOUNTID"]) },
                    FBankId = new { FNumber = Convert.ToString(doHead["FBankId"]) },
                    FCURRENCYID = new { FNumber = Convert.ToString(doHead["FCURRENCYID"]) },
                    FCREDITAMOUNT = Convert.ToString(doHead["FCREDITAMOUNT"]),
                    FDEBITAMOUNT = Convert.ToString(doHead["FDEBITAMOUNT"]),
                    FAmount = Convert.ToString(doHead["FAmount"]),
                    FOPPOSITEBANKNAME = Convert.ToString(doHead["FOPPOSITEBANKNAME"]),
                    FOPPOSITEBANKACNTNO = Convert.ToString(doHead["FOPPOSITEBANKACNTNO"]),
                    FOPPOSITECCOUNTNAME = Convert.ToString(doHead["FOPPOSITECCOUNTNAME"]),
                    FFlowNo = Convert.ToString(doHead["FFlowNo"]),
                    FDetailNo = Convert.ToString(doHead["FDetailNo"]),
                    FSRCBILLNO = Convert.ToString(doHead["FSRCBILLNO"]),
                    FBUSIREFENO = Convert.ToString(doHead["FBUSIREFENO"]),
                    FRemark = Convert.ToString(doHead["FRemark"]),
                    FSRCFormId = Convert.ToString(doHead["FSRCFormId"]),
                    FSRCBILLTYPEID = new { FNumber = Convert.ToString(doHead["FSRCBILLTYPEID"]) },
                }).ToList()
            };

            // 转换为JSON字符串  
            string strJson = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObject, Formatting.Indented);

            try
            {
                strError = BatchSaveAudit(mK3CloudApiClient1, mstrFormId, strJson);
            }

            catch (Exception ex)
            {
                //注意，不要用这个，不要抛出错误，为什么呢？留到外面判断。2023/11/20 6:30.当然，也不是绝对，以后再调整吧。
                //strError +=  throw new Exception(CsErrLog.GetExceptionWithLog(ex));

                strError += CsErrLog.GetExceptionWithLog(ex);


                return strError;

                //throw new Exception(clsErrLog.GetException(ex));
            }

            return strError;

        }

        private string BatchSaveAudit(K3CloudApiClient pK3CloudApiClient1, string pObjectTypeId, string pSaveJSon
         )

        {

            string strSaveResultJson = pK3CloudApiClient1.BatchSave(pObjectTypeId, pSaveJSon);

            string strResult = K3SQLLog(strSaveResultJson);

            //晕死，这里，什么动作也没有做，至少，要保存报文，以及报文返回结果的。
            //连出错了，也没有保存。



            return strResult;
        }

        private string K3SQLLog(
             string pResultJson
             )
        {
            string strErrorMessage1 = "", strErrorMessages = "";
            try
            {
                //int b = 0;
                //int intC = 1 / ;

                JObject joResult = JObject.Parse(pResultJson);
                JArray JArrayError = (JArray)joResult["Result"]["ResponseStatus"]["Errors"];

                //同步失败的，在中间表，做个标记。
                StringBuilder sb1 = new StringBuilder();

                foreach (JObject jObject1 in JArrayError)
                {
                    strErrorMessage1 = jObject1["Message"].ToString();

                    //严重错误，不要往下同步了。错误，也没法更新到中间表。因为返回的报文信息中，读不到单据编号。
                    if (pResultJson.ToLower().IndexOf("number") == -1 && pResultJson.ToLower().IndexOf("billno") == -1)
                        return strErrorMessage1 + "*";

                    if (strErrorMessage1.Length > 200)
                        strErrorMessage1 = strErrorMessage1.Substring(0, 200) + "...太多，省略...";

                    strErrorMessages = strErrorMessages + strErrorMessage1 + Environment.NewLine;
                }
            }
            catch (Exception ex)
            {
                return CsErrLog.GetExceptionWithLog(ex);

            }

            return strErrorMessages;



        }
        private string GetReturnBillNo(string pField_BillNo_Success, JObject jObject1)
        {
            string strjObject1 = jObject1.ToString();
            string strBillNO = "";
            string strCheckField = "Number";

            strBillNO = GetValueFromJson(strCheckField, jObject1);
            if (strBillNO != "")
                return strBillNO;


            strCheckField = "FNumber";
            strBillNO = GetValueFromJson(strCheckField, jObject1);
            if (strBillNO != "")
                return strBillNO;

            //FBillNo,要放在最后面来找。否则，下面返回信息，会出错。2023/9/12 10:55
            //"FieldName": "FBillNo","Message": "违反字段唯一性要求：编码唯一。[CL.YCT230816001]在当前系统中已经被使用。"
            strBillNO = GetValueFromJson(pField_BillNo_Success, jObject1);
            if (strBillNO != "")
                return strBillNO;

            return "";
        }
        private string GetValueFromJson(string pField, JObject jObject1)
        {
            string strjObject1 = jObject1.ToString();
            string strFind = string.Format("\"{0}\"", pField);
            string strReturn = "";

            //放在外面判断吧，这里不好返回错误信息。
            //if (strjObject1.IndexOf("违反字段唯一性要求：编码唯一") != -1)
            //    return "";

            if (strjObject1.IndexOf(strFind) == -1)
                return "";

            strReturn = jObject1[pField].ToString();
            return strReturn;
        }
    }

}


