using ahu.YuYue.CBS;
using Kingdee.BOS;
using Kingdee.BOS.App;
using Kingdee.BOS.Contracts;
using Kingdee.BOS.Core.Metadata.ConvertElement;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.Util;
using Kingdee.BOS.WebApi.Client;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;

using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ahu.YuYue.CBS 
{
    public class CsPublic2
    {

        public static ConvertRuleElement GetDefaultConvertRule(Context ctx, string srcFormId, string destFormId, string ruleKey)
        {
            IMetaDataService service = ServiceHelper.GetService<IMetaDataService>();
            List<ConvertRuleElement> convertRules = service.GetConvertRules(ctx, srcFormId, destFormId);
            ConvertRuleElement result;
            if (!ObjectUtils.IsNullOrEmptyOrWhiteSpace(ruleKey))
            {
                result = convertRules.FirstOrDefault((ConvertRuleElement t) => StringUtils.EqualsIgnoreCase(t.Key, ruleKey) || StringUtils.EqualsIgnoreCase(t.Id, ruleKey));
            }
            else
            {
                result = convertRules.FirstOrDefault((ConvertRuleElement p) => p.IsDefault);
            }
            return result;
        }
        public static string FormatAccountCode(string pAccountCode)
        {
            if (string.IsNullOrEmpty(pAccountCode))
            {
                throw new ArgumentException("科目编号，不能为空。");
            }

            // 如果会计科目长度小于或等于4，则无需格式化，直接返回原字符串  
            if (pAccountCode.Length <= 4)
            {
                return pAccountCode;
            }

            //判断，是否加了。
            int intFind = pAccountCode.IndexOf(".");
            if (intFind != -1)
                return pAccountCode;

            StringBuilder formattedCode = new StringBuilder();
            formattedCode.Append(pAccountCode.Substring(0, 4)); // 先添加前4位  

            // 从第5位开始，每2位添加一个点，除非剩余字符不足2位  
            for (int i = 4; i < pAccountCode.Length; i += 2)
            {
                // 如果剩余字符不足2位，则只取剩余的部分  
                if (i + 2 > pAccountCode.Length)
                {
                    formattedCode.Append('.');
                    formattedCode.Append(pAccountCode.Substring(i));
                    break;
                }

                formattedCode.Append('.');
                formattedCode.Append(pAccountCode.Substring(i, 2));
            }

            return formattedCode.ToString();
        }
        public static int GetBillTypeID(string pBillType)
        {
            int intBillType = 0;
            switch (pBillType)
            {
                case "接收银行交易明细":
                    intBillType = 1;
                    break;
                case "电子回单":
                    intBillType = 2;
                    break;
                case "电子回单附件":
                    intBillType = 3;
                    break;
                default:
                    //MessageBox.Show("请输入单据类型。");
                    //intBillType = 0;
                    //break;
                    throw new Exception("意外错误，请洽程序设计员。case中，没有 pBillType "+ pBillType);
            }

            return intBillType;

        }
        public static void HaveaRest()
        {
            for (int i = 0; i <= 8; i++)
            { Application.DoEvents(); }
        }

        public static void RadioButtonChecked(RadioButton pRB)
        {
            pRB.Checked = true;
            RadioButtonCheckedColor(pRB);
        }

        public static void RadioButtonCheckedColor(RadioButton pRB)
        {
            pRB.BackColor = Color.Blue;
            pRB.ForeColor = Color.Yellow;
        }

        public static void RadioButtonNotChecked(RadioButton pRB)
        {
            pRB.Checked = false;
            RadioButtonNotCheckedColor(pRB);
        }
        public static void RadioButtonNotCheckedColor(RadioButton pRB)
        {
            pRB.BackColor = System.Drawing.SystemColors.Control;
            pRB.ForeColor = System.Drawing.SystemColors.ControlText;
        }

        public static string GetTableNameByFormId(string pFormId)
        {
            string strK3TableName = "";
            switch (pFormId)
            {
                case K3FormId.strWB_RecBankTradeDetail:
                    strK3TableName = "T_CN_BANKCASHFLOW";
                    break;
                case K3FormId.strWB_ReceiptBill:
                    strK3TableName = "T_WB_RECEIPT";
                    break;
                case K3FormId.strWB_ReceiptBill_Attachment:
                    strK3TableName = "T_WB_RECEIPT";
                    break;
                default:
                    throw new Exception("case " + pFormId + "，CsPublic2.GetTableNameByFormId，没有写。");

            }
            return strK3TableName;

        }


//        public static string TestKingdee2Kingdee(K3CloudApiClient pK3CloudApiClient1
//  , string pBillType, string pBillNo, bool pIsDelete)
//        {

//            string strReturn;
//            bool bolSeekSynchroFlagNot = true; //出错了，就不再同步了，别浪费资源，要增量同步。
//            string strTable;
//            string strFormID;
//            string strSQL;
//            string strFBillNo_Field;
//            string strFId_Field;
//            string strOperateType = "";
//            string strFId_主账簿 = "";
//            string strFId_副账簿 = "";
//            switch (pBillType)
//            {

//                case "凭证":
//                    strFormID = K3FormId.strGL_VOUCHER;
//                    strTable = "t_GL_VOUCHER";
//                    strFBillNo_Field = "FBillNo";
//                    strFId_Field = "FVOUCHERID";

//                    if (pIsDelete == true)
//                        strOperateType = "delete";
//                    else if (pBillNo == "")
//                        //批量同步新增
//                        strOperateType = "new";
//                    else
//                    {
//                        strSQL = string.Format(@"
//select Top 1  FVourcherID
//from T_GL_VOUCHER
//Where FBillNo='{0}'", pBillNo);
//                        strFId_主账簿 = CsData.GetTopValue(null, pK3CloudApiClient1, strSQL);
//                        if (strFId_主账簿 == "")
//                            return "意外的错误，金蝶数据库中，找不到单据编号: " + pBillNo + "  CsPublic2.TestKingdee2Kingdee";


//                        strSQL = string.Format(@"
//select Top 1 FVourcherID  
//from T_GL_VOUCHER
//Where FVourcherID_FSource='{0}'", strFId_主账簿);
//                        strFId_副账簿 = CsData.GetTopValue(null, pK3CloudApiClient1, strSQL);

//                        if (strFId_副账簿.IsNullOrEmptyOrWhiteSpace() == true)
//                            strOperateType = "new";
//                        else
//                            strOperateType = "edit";
//                    }

//                    if (pIsDelete)
//                        strOperateType = "delete";

//                    CsKingdee2Kingdee CsKingdee2Kingdee1 = new CsKingdee2Kingdee();
//                    CsKingdee2Kingdee1.MySetParaByConnection(strFormID, strFId_Field, strFBillNo_Field, strTable, pK3CloudApiClient1);
//                    strReturn = CsKingdee2Kingdee1.Actions_Main1_FZB(strFId_主账簿, strFId_副账簿, pBillNo, bolSeekSynchroFlagNot, strOperateType);



//                    break;


//                default:
//                    throw new Exception("意外错误，case中 BillType=" + pBillType + "     Bob.GrainDepot.RunExe.TestSyncOA");

//            }



//            return strReturn;
//        }

    }
}
