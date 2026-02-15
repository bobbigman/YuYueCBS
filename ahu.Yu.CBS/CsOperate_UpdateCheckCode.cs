using System.ComponentModel;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Core.DynamicForm.PlugIn;
using Kingdee.BOS;
using Kingdee.BOS.Core.DynamicForm;
using System;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.Util;
using Kingdee.BOS.WebApi.Client;
using System.Linq;
using System.Windows.Forms;

namespace ahu.YuYue.CBS
{
    [Description("交易明细，更新对帐码")]
    [Kingdee.BOS.Util.HotUpdate]

    public class CsOperate_UpdateCheckCode : AbstractOperationServicePlugIn
    {
        Struct_K3LoginInfo mStruct_K3LoginInfo;

        public override void OnPreparePropertys(PreparePropertysEventArgs e)
        {
            base.OnPreparePropertys(e);
            e.FieldKeys.Add("FTRANSDATE");
        }

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
        }

        public void SetContext(Context pContext)
        {


            Boolean bolNeedLogInK3 = false;

            string strReturnType = "";

            K3CloudApiClient K3CloudApiClient1 = new K3CloudApiClient();
            int IntReturn = ClsPublic.LogIn(ref mStruct_K3LoginInfo, pContext
                , ref K3CloudApiClient1, ref strReturnType, bolNeedLogInK3, K3DatabaseMode.IntegrationK3);


            if (IntReturn != 1)
                return;
        }

        public override void AfterExecuteOperationTransaction(AfterExecuteOperationTransaction e)
        {

            base.AfterExecuteOperationTransaction(e);

            string strOperation = this.FormOperation.Operation;

            if (strOperation.EqualsIgnoreCase("UpdateCheckCode") == false)
                return;

            if (mStruct_K3LoginInfo.AcctID.IsNullOrEmptyOrWhiteSpace() == false
                && Context.DBId != mStruct_K3LoginInfo.AcctID)
            {
                throw new Exception("测试账套，不需要同步！");
            }



            Call2K3PlugIn(e, false, strOperation);

        }

        private void Call2K3PlugIn(AfterExecuteOperationTransaction afteE, bool pErrorStop, string pOperation
           )
        {
            SetContext(this.Context);

            string strReturn1;
            string strReturns = "";
            Kingdee.BOS.Core.DynamicForm.OperationResult OperationResults = (Kingdee.BOS.Core.DynamicForm.OperationResult)this.OperationResult;

            bool bolNeedLogInK3 = false;
            int intK3DatabaseMode = K3DatabaseMode.IntegrationK3;
            string strReturnType = "";

            K3CloudApiClient K3CloudApiClient1 = new K3CloudApiClient();

            int IntReturn = ClsPublic.LogIn(ref mStruct_K3LoginInfo, Context
                , ref K3CloudApiClient1, ref strReturnType, bolNeedLogInK3, intK3DatabaseMode);
            if (IntReturn != 1)
                throw new Exception(strReturnType);

            string strError1 = "";
            string strToken = CsCallCBS.getToken(Context, K3CloudApiClient1, intK3DatabaseMode, mStruct_K3LoginInfo, ref strError1);
            if (strError1 != "")
                throw new Exception(strError1);

            ClsScheduleReadOthers ClsScheduleReadOthers1 = new ClsScheduleReadOthers();

            bool bolIsSchedule = false;
            foreach (DynamicObject DO1 in afteE.DataEntitys)
            {
                string strBillId = DO1["Id"].ToString();
                string strFDate = DO1["FTRANSDATE"].ToString();
                string strFBillNo = DO1["BillNo"].ToString();
                string strPage = "0";
                int intBillType = 1;
                CsFlow2Receipt CsFlow2Receipt1 = new CsFlow2Receipt();
                Struct_K3LoginInfo Struct_K3LoginInfo1 = new Struct_K3LoginInfo();

                strReturn1 = ClsScheduleReadOthers1.Only1BillType
                    (Context, null, ref Struct_K3LoginInfo1
                    , WDT2.UpdateCheckCode, strFDate, strFDate, strPage, strFBillNo, intBillType, K3DatabaseMode.IntegrationK3
                    , strToken, bolIsSchedule);

                if (strReturn1 != "")
                {
                    strReturns += Environment.NewLine + strReturn1 + Environment.NewLine;
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
