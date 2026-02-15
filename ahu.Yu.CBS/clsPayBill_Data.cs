using Kingdee.BOS;
using Kingdee.BOS.WebApi.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ahu.YuYue.CBS 
{
    [Description("付款单，数据插件，不要调用")]
    [Kingdee.BOS.Util.HotUpdate]
    public static class clsPayBill_Data
    {
        public static string GetFBIZREFNOByFKD(Context pContext, K3CloudApiClient pK3CloudApiClient
            , string pFId, ref string pError,ref bool pNotCBS)
        {

            //FCbsPay，是没有空值的，只有0和1
            string strSQL = string.Format(@"
select Top 1 FCbsPay
from T_AP_PAYBILL 
where FId='{0}'", pFId);
            string strFCbsPay = CsData.GetTopValue(pContext, pK3CloudApiClient, strSQL);
            if (strFCbsPay == "")
            {
                pError = "没找到付款单。";
                return "";
            }
            if (strFCbsPay == "0")
            {
                pNotCBS=true;
                return "";
            }

            strSQL = string.Format(@"
select Top 1 FBIZREFNO
from T_AP_PAYBILL t1 join
T_AP_PAYBILLENTRY t2 on t1.FID = t2.FID
where t1.FId='{0}'
union all
select Top 1 FBIZREFNO_M
from T_AP_PAYBILL t1 join
    T_CN_PAYBILLMORERECENTRY t2 on t1.FID = t2.FID
where t1.FId='{0}'", pFId);

            string strFBIZREFNO = CsData.GetTopValue(pContext, pK3CloudApiClient, strSQL);
            if (strFBIZREFNO == "")
            {
                pError = "付款单没有业务参考号。";
                return "";
            }
            pError = "";
            return strFBIZREFNO;
        }



        public static bool CheckBeforeUnAudit(Context pContext, K3CloudApiClient pK3CloudApiClient
            , string strFID,ref string pError)
        {

            bool bolNotCbs = false;
            string strReferenceNum = GetFBIZREFNOByFKD(pContext, pK3CloudApiClient, strFID, ref pError, ref bolNotCbs);
            if (pError != "")
            {
                return false;
            }

            if (bolNotCbs == true)
            {
                pError = "不需要CBS支付";
                return true;
            }

            if (strReferenceNum == "")
            {
                pError = "付款单中，没有 [业务参考号]";
                return true;
            }

            string strBalanceUrl = "openapi/payment/openapi/v2/detail";
            var parameters3 = new Dictionary<string, object>
{
    { "referenceNum", strReferenceNum },
};

            string strJSon = JsonConvert.SerializeObject(parameters3);
            string strJsonResult = "";
            TestCBSDll TestCBSDll1 = new TestCBSDll();
            TestCBSDll1.CallCBS_Demo(strBalanceUrl, strJSon, ref strJsonResult, ref pError);
            if (pError!="")
            {
                return false;
            }

            JObject joReturn = JObject.Parse(strJsonResult);
            string strPayStatus = Convert.ToString(joReturn["data"][0]["payStatus"]);
            string strPayStatusChinese = "";
            bool bolOk = false;
            switch (strPayStatus)
            {
                case "":
                    bolOk = false;
                    strPayStatusChinese = "";
                    pError = "未取到银行支付状态";
                    break;
                case "a":
                    bolOk = false;
                    strPayStatusChinese = "待提交直连";
                    break;
                case "b":
                    bolOk = false;
                    strPayStatusChinese = "已提交直连";
                    break;
                case "c":
                    bolOk = false;
                    strPayStatusChinese = "银行已受理";
                    break;
                case "d":
                    bolOk = true;
                    strPayStatusChinese = "银行未受理";
                    break;
                case "e":
                    bolOk = false;
                    strPayStatusChinese = "可疑";
                    break;
                case "f":
                    bolOk = false;
                    strPayStatusChinese = "待人工确认";
                    break;
                case "g":
                    bolOk = false;
                    strPayStatusChinese = "支付成功";
                    break;
                case "h":
                    bolOk = true;
                    strPayStatusChinese = "支付失败";
                    break;
                case "j":
                    bolOk = true;
                    strPayStatusChinese = "退票";
                    break;
                case "k":
                    bolOk = true;
                    strPayStatusChinese = "取消支付"; 
                    break;
            }

            if (bolOk == false)
            {
                if (pError == "")
                {
                    pError = "支付状态：" + strPayStatusChinese;
                }

            }

            return bolOk;

        }
    }



}
