
using Kingdee.BOS;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.Util;
using Kingdee.BOS.WebApi.Client;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ahu.YuYue.CBS 
{
    //从金蝶同步到浪潮用。
    public class CsDataCheckBeforeSync2K3
    {

        private K3CloudApiClient mK3CloudApiClient;
        private Context mContext;
        Struct_K3LoginInfo mStruct_K3LoginInfo1;

        string mstrGetDllLastWriteTime;
        string mstrFormID;
        string mstrMiddleTableName;

        public void SetInfo(Context pContext, K3CloudApiClient pK3CloudApiClient, Struct_K3LoginInfo pStruct_K3LoginInfo1
             , string pGetDllLastWriteTime, string pFormID, string pMiddleTableName
             )
        {
            mK3CloudApiClient = pK3CloudApiClient;
            mContext = pContext;
            mStruct_K3LoginInfo1 = pStruct_K3LoginInfo1;

            mstrGetDllLastWriteTime = pGetDllLastWriteTime;
            mstrFormID = pFormID;
            mstrMiddleTableName = pMiddleTableName;
        }

        private string GetBaseUnit(string pFNumber)
        {
            try
            {


                string strSQL = string.Format(@"/*dialect*/
declare @FUNITGROUPID int
declare @FISBASEUNIT int
declare @FNUMBER varchar(50)
Select @FNUMBER='{0}'
select Top 1 @FUNITGROUPID=FUNITGROUPID,@FISBASEUNIT=FISBASEUNIT
from t_bd_unit  with(nolock)
where FNUMBER=@FNUMBER

if @FISBASEUNIT=0
begin
    select  Top 1 @FNUMBER=FNUMBER
      from  t_bd_unit  with(nolock)
     where  FUNITGROUPID=@FUNITGROUPID
       and  FISBASEUNIT=1
end
Select @FNUMBER", pFNumber);
                string strReturn = CsData.GetTopValue(mContext, mK3CloudApiClient, strSQL);
                return strReturn;
            }
            catch (Exception ex)
            {
                throw new Exception(CsErrLog.GetExceptionWithLog(ex));
            }
        }
        public string GetValueAndCheck(string pReadMiddleValue, string pK3Caption, string pK3Field
            , ref string pError, bool pAllowNull, bool strRemoveLast0
            , string pMiddleID, string pFormId
            , string pCheckOrgId, string pCheckOrgNum)

        {
            pError = "";

            pK3Caption = pK3Caption.Trim();
            if (pReadMiddleValue == "")
            {
                //允许空值。
                if (pAllowNull == true)
                    return "";

                //不允许空值。
                //有时，是从审核里触发的,所以，要加上同步失败，这几个字。否则，还以为是审核操作，有异常了。

                //销售出库单，和其它的，兼容不了，所以，先标注掉。2023/1/21 19:57
                //pError = "同步失败：[" + pK3Caption + "] 没有输入。";
                //ClsPublic.UpdateMiddleTableSynchronizeError( mContext, mK3CloudApiClient1, mstrMiddleTableName
                //    ,  pMiddleID, pError, mstrGetDllLastWriteTime, pFormId);
                return "";
            }

            if (strRemoveLast0 == true)
            {
                pReadMiddleValue = ClsPublic.RemoveLast0(pReadMiddleValue);
                return pReadMiddleValue;
            }

            string strCheckK3TableBasic = "";
            string strk3FieldNumberCheck = "FNumber";
            switch (pK3Caption)
            {
                case "银行账号":
                    strCheckK3TableBasic = "T_CN_BANKACNT";
                    break;
                case "客户":
                    strCheckK3TableBasic = "T_BD_Customer";
                    break;
                case "供应商":
                    strCheckK3TableBasic = "t_bd_Supplier";
                    break;

                case "库存单位":
                case "基本单位":
                case "单位":
                    strCheckK3TableBasic = "t_bd_Unit";
                    break;

                case "仓库":
                case "调出仓库":
                case "调入仓库":
                    strCheckK3TableBasic = "t_bd_Stock";
                    break;
                case "领料部门":
                    strCheckK3TableBasic = "t_bd_Department";
                    break;
                case "表面色号":
                    strCheckK3TableBasic = "T_BAS_ASSISTANTDATAENTRY";
                    break;
            }

            if (strCheckK3TableBasic == "")
                return pReadMiddleValue;

            string strSQL = string.Format(@"
Select Top 1 FDocumentStatus,FFORBIDSTATUS
From  {0}
Where {1}='{2}'
and    FUseOrgId={3}
",  strCheckK3TableBasic, strk3FieldNumberCheck, pReadMiddleValue,pCheckOrgId);

            DynamicObjectCollection doc1= CsData.GetDynamicObjects(mContext, mK3CloudApiClient, strSQL);
            if (doc1.Count==0)
            {
                pError = pK3Caption + pReadMiddleValue + "， 在基础资料中不存在，组织="+ pCheckOrgNum;
                ClsPublic.UpdateMiddleTableSynchronizeError(mContext, mK3CloudApiClient
                    , mstrMiddleTableName, pMiddleID, pError, mstrGetDllLastWriteTime, pFormId);
                return "";
            }
            DynamicObject do1 = doc1[0];
            string strFDocumentStatus = Convert.ToString(do1["FDocumentStatus"]);
            string strFFORBIDSTATUS = Convert.ToString(do1["FFORBIDSTATUS"]);

            if (strFDocumentStatus.EqualsIgnoreCase("C")==false)
            {
                pError = pK3Caption + pReadMiddleValue + "， 在基础资料中没有审核，组织=" + pCheckOrgNum;
                ClsPublic.UpdateMiddleTableSynchronizeError(mContext, mK3CloudApiClient
                    , mstrMiddleTableName, pMiddleID, pError, mstrGetDllLastWriteTime, pFormId);
                return "";
            }

            if (strFFORBIDSTATUS.EqualsIgnoreCase("A") == false)
            {
                pError = pK3Caption + pReadMiddleValue + "， 在基础资料中被禁用了,组织=" + pCheckOrgNum;
                ClsPublic.UpdateMiddleTableSynchronizeError(mContext, mK3CloudApiClient
                    , mstrMiddleTableName, pMiddleID, pError, mstrGetDllLastWriteTime, pFormId);
                return "";
            }

            return pReadMiddleValue;

        }


        public string BasicCheck(string pBasicValue, string pBasicType, string pOrgNumber)

        {
            string strError = "";

            if (pBasicValue == "")
                return "";

            pBasicType = pBasicType.ToLower();
            string strCheckK3TableBasic = "";
            string strK3Caption = "";
           switch (pBasicType)
            {
                case "bd_supplier":
                    strCheckK3TableBasic = "t_bd_Supplier";
                    strK3Caption = "供应商";
                    break;
                case "bd_customer":
                    strCheckK3TableBasic = "t_BD_Customer";
                    strK3Caption = "客户";
                    break;

                case "bd_empinfo":
                    strCheckK3TableBasic = "T_HR_EMPINFO";
                    strK3Caption = "员工";
                    break;
                default:
                    throw new Exception("switch中，意外的 "+pBasicType + " CsDataCheckBeforeSync2K3.GetBasicCheckSQL");
            }

            string strSQL = string.Format(@"
Select Top 1 chk.FDocumentStatus,chk.FFORBIDSTATUS
From  {0} chk left outer join
       T_ORG_ORGANIZATIONS org on chk.FUseOrgId=org.FOrgId
Where chk.FNumber='{1}'
and   org.FNumber='{2}'
", strCheckK3TableBasic,  pBasicValue, pOrgNumber);

            DynamicObjectCollection doc1 = CsData.GetDynamicObjects(mContext, mK3CloudApiClient, strSQL);
            if (doc1.Count == 0)
            {
                strError = "组织[" + pOrgNumber+"]，"+ strK3Caption +"["+ pBasicValue + "]，不存在。";
                return strError;
            }

            DynamicObject do1 = doc1[0];
            string strFDocumentStatus = Convert.ToString(do1["FDocumentStatus"]);
            string strFFORBIDSTATUS = Convert.ToString(do1["FFORBIDSTATUS"]);

            if (strFDocumentStatus.EqualsIgnoreCase("C") == false)
            {
                strError = "组织[" + pOrgNumber + "]，" + strK3Caption  + "[" + pBasicValue + "]，需要审核。";
                return strError;
            }

            if (strFFORBIDSTATUS.EqualsIgnoreCase("A") == false)
            {
                strError = "组织[" + pOrgNumber + "]，" + strK3Caption + "[" + pBasicValue + "]，被禁用了。";
                return strError;
            }

            return "";

        }

    }
}
