using System;
using System.ComponentModel;
using Kingdee.BOS;
using Kingdee.BOS.App.Data;
using Kingdee.BOS.Core;
using Kingdee.BOS.Core.Validation;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.Util;



namespace B.ZP
{
    [Description("通用校验,同步到其它平台时，才会调用，这里平台，是浪潮")]
    [Kingdee.BOS.Util.HotUpdate]
    public class CsK32OthersValidator : AbstractValidator
    {

        Struct_K3LoginInfo mStruct_K3LoginInfo;
        string mstrField_ID, mstrCheckOrgField, mstrFormId;


        public override void Validate(ExtendedDataEntity[] dataEntities, ValidateContext validateContext, Context ctx)
        {

            if (dataEntities == null) return;

            foreach (ExtendedDataEntity extendedDataEntity in dataEntities)
            {
                DynamicObject DO1 = extendedDataEntity.DataEntity;

                string strDynamicObjectTypeName = DO1.DynamicObjectType.Name;

                string strBillTypeId = CsPublicOA.GetBillTypeId(DO1);

                string strK3Table = "";  //仅为兼容。
                string strK3Number_Field = "";  //仅为兼容。
                string strURL = "";  //仅为兼容。
                CsData.GetK3BillInfo(Context, null, strDynamicObjectTypeName, strBillTypeId
    , out strK3Table, ref mstrFormId, out mstrField_ID
    , out strK3Number_Field, out mstrCheckOrgField, out strURL, "");

                string strFId = Convert.ToString(DO1["Id"]);

                DynamicObject Do_CheckOrgId = (DynamicObject)DO1[mstrCheckOrgField];
                string strFOrgId = Convert.ToString(Do_CheckOrgId["Id"]);

                string strError = "";

                if (strFOrgId != mStruct_K3LoginInfo.OrganizationId
                    && mStruct_K3LoginInfo.OrganizationId.IsNullOrEmptyOrWhiteSpace() == false)
                {
                    strError = string.Format(@"不需要同步。同步条件：组织编号= {0}", mStruct_K3LoginInfo.OrganizationNum);
                    validateContext.AddError(DO1, new ValidationErrorInfo(mstrField_ID, strFId, extendedDataEntity.DataEntityIndex, extendedDataEntity.RowIndex, "csk32OthersSystemValidator.Validate", strError, "", ErrorLevel.Error));
                    return;
                }
            }
        }


        public string CheckPur_PurchaseOrder(string strFID, string pBillTypeName)
        {

            if (pBillTypeName == "标准采购订单" || pBillTypeName == "标准委外订单")
                return "";

            string strError = string.Format(@"[{0}] 不需要同步。同步条件:标准采购订单,或：标准委外订单", pBillTypeName);

            return strError;

        }

        public string CheckSal_DeliveryNoticeMoreSaleOrder(string strFID)
        {
            string strSQL = string.Format(@"
select  distinct FSrcBillNo
from    T_SAL_DELIVERYNOTICEENTRY 
where  fid={0}
 and   FSrcBillNo<>'' ", strFID);

            DynamicObjectCollection doc1 = DBUtils.ExecuteDynamicObject(Context, strSQL);

            if (doc1.Count <= 1) return "";

            string strFSrcBillNo = "";
            foreach (DynamicObject do1 in doc1)
            {
                strFSrcBillNo = strFSrcBillNo + do1[0].ToString() + ",";
            }
            strFSrcBillNo = strFSrcBillNo.Substring(0, strFSrcBillNo.Length - 1);
            string strError = string.Format(@"不可以同步。MES同步条件：一张发货通知单，只允许一个销售订单。{0} 但现在，有多个销售订单：", Environment.NewLine, strFSrcBillNo);

            return strError;

        }





    }
}
