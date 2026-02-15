using System.ComponentModel;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Core.DynamicForm.PlugIn;
using Kingdee.BOS;
using Kingdee.BOS.Core.DynamicForm;
using System;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.Util;
using Kingdee.BOS.WebApi.Client;
using System.Collections.Generic;

namespace ahu.YuYue.CBS
{
    [Description("尽量集成，各种操作")]
    [Kingdee.BOS.Util.HotUpdate]
    public class CsAllOperate : AbstractOperationServicePlugIn
    {
        Struct_K3LoginInfo mStruct_K3LoginInfo;
        Context mContext;

        public override void OnAddValidators(AddValidatorsEventArgs e)
        {
            base.OnAddValidators(e);

            //审核，禁用操作，也会调用同步。但是，同步失败，是不要提示的。
            mStruct_K3LoginInfo.FormOperation = this.FormOperation.Operation;

            //只有同步，才需要检验数据内容。并且，删除同步到浪潮,不需要校验的。
            if (this.FormOperation.Operation != "FSynchronBob")
            {
                return;
            }

            //稍后，调用AfterExecuteOperationTransaction，要用到 mContext。

            //审核插件，不调用这里，所以，不会出错，并退出。
            //string strFormId = this.BusinessInfo.GetForm().Id.ToLower();
            //CsK32OthersValidator CsK32OthersValidator1 = new CsK32OthersValidator();
            //CsK32OthersValidator1.AlwaysValidate = true;
            //CsK32OthersValidator1.SetInfo(mStruct_K3LoginInfo, strFormId, this.FormOperation.Operation);

            //CsK32OthersValidator1.EntityKey = "FBillHead";
            //e.Validators.Add(CsK32OthersValidator1);

        }
        public override void OnPreparePropertys(PreparePropertysEventArgs e)
        {
            base.OnPreparePropertys(e);
            e.FieldKeys.Add("FBillTypeID");

            //不加这一行，直接调拨单就取不到。当然，物语项目，只有这一个业务单据，要同步到旺店通。
            //还有一个是物料，用不到。
            e.FieldKeys.Add("FDate");


            //加了没有用。虽然赋值，填了错误信息，列表中，还是没法刷新。
            //可能要另外读取吧。
            //e.FieldKeys.Add("FSynchronError");

            //加了没用，物料，还是看不到。
            //但是，采购订单，不加，也看得到
            //e.FieldKeys.Add("FFormID");

        }

        public void SetContext(Context pContext)
        {
            mContext = pContext;

            Boolean bolNeedLogInK3 = false;

            string strReturnType = "";

            K3CloudApiClient K3CloudApiClient1 = new K3CloudApiClient();
            int IntReturn = ClsPublic.LogIn(ref mStruct_K3LoginInfo, mContext
                , ref K3CloudApiClient1, ref strReturnType, bolNeedLogInK3, K3DatabaseMode.IntegrationK3);

            if (IntReturn != 1)
                return;
        }

        public override void AfterExecuteOperationTransaction(AfterExecuteOperationTransaction e)
        {

            base.AfterExecuteOperationTransaction(e);

            string strOperation = this.FormOperation.Operation;
            string strFormId = this.BusinessInfo.GetForm().Id;


            if (mStruct_K3LoginInfo.AcctID.IsNullOrEmptyOrWhiteSpace() == false
                && mContext.DBId != mStruct_K3LoginInfo.AcctID)
            {
                throw new Exception("账套:" + mContext.DataCenterNumber + "(内码:" + mContext.DBId + ")，不需要同步！");
            }


            if (strOperation.EqualsIgnoreCase("FSynchronBob"))
            {
                Call2K3PlugIn(e);
                return;
            }






        }



        private void Call2K3PlugIn(AfterExecuteOperationTransaction afteE)
        {
            SetContext(this.Context);

            //不能传这个到同步的地方，要传下游的，比方说：生产入库单的。

            string strFormIdMidd = this.BusinessInfo.GetForm().Id;
            string strSQL = string.Format(@"
Select  top 1 SyncBillTypeId
  From   tblSyncBillTypeCBS 
 Where   SyncBillTypeMidd='{0}'", strFormIdMidd);
            string strSyncBillTypeId = CsData.GetTopValue(Context, null, strSQL);
            if (strSyncBillTypeId == "")
                throw new Exception("意外错误，请洽程序员计员，tblSyncBillTypeCBS，需要配置：" + strFormIdMidd);


            int intSyncBillTypeId = Convert.ToInt16(strSyncBillTypeId);
            string strFormOperation = this.FormOperation.Operation;
            mStruct_K3LoginInfo.FormOperation = strFormOperation;

            string strReturns = "";
            Kingdee.BOS.Core.DynamicForm.OperationResult OperationResults = (Kingdee.BOS.Core.DynamicForm.OperationResult)this.OperationResult;

            csMiddle2K3 CsMiddle2K3_Ahu = new csMiddle2K3();

            //K3CloudApiClient K3CloudApiClient_Middle = null;
            K3CloudApiClient K3CloudApiClient_SaveK3 = null;

            foreach (DynamicObject DO1 in afteE.DataEntitys)
            {
                string strBillId = DO1["Id"].ToString();

                string strBillNo;
                if (DO1.Contains("FBillNo"))
                    strBillNo = DO1["FBillNo"].ToString();
                else if (DO1.Contains("BillNo"))
                    strBillNo = DO1["BillNo"].ToString();
                else
                    throw new Exception("单据编号读取失败。");

                string strReturn1 = CsMiddle2K3_Ahu.Call2K3(Context
                    , ref K3CloudApiClient_SaveK3, ref mStruct_K3LoginInfo
                    , intSyncBillTypeId, strBillNo, strBillId, K3DatabaseMode.IntegrationK3);
                if (strReturn1 != null && strReturn1 != "")
                {
                    strReturns += strReturn1 + Environment.NewLine;
                }

            }

            if (strReturns != "")
            {
                //没作用
                //当前行，OperationResult中标了错，希望显示一下。
                OperationResults.IsSuccess = false;

                //这两个，有区别的。
                //KDBusinessException，会把 OperationResults 各行，设成同样的错误
                //但是，Exception，不显示 OperationResults ，只会以界面，返回结果。
                //throw new KDBusinessException("", strReturn);
                throw new Exception(strReturns);

            }

        }

    }
}
