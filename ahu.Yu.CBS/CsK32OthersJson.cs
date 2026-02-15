
using System.ComponentModel;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Core.DynamicForm.PlugIn;
using Kingdee.BOS;
using Kingdee.BOS.Core.DynamicForm;
using Kingdee.BOS.Orm.DataEntity;
using System;
using Kingdee.BOS.Core;

namespace ahu.YuYue.CBS 

{
    [Description("获取同步报文")]
    [Kingdee.BOS.Util.HotUpdate]

    public class CsK32OthersJson : AbstractOperationServicePlugIn
    {

        public override void OnPreparePropertys(PreparePropertysEventArgs e)
        {
            base.OnPreparePropertys(e);
            e.FieldKeys.Add("FBillTypeID");
            e.FieldKeys.Add("FBillType");  //字段取名，有点奇葩啊，eng_bom中。

        }


        public override void AfterExecuteOperationTransaction(AfterExecuteOperationTransaction e)
        {

            string strFormOperation = this.FormOperation.Operation;
            string strFormId = BusinessInfo.GetForm().Id; //业务对象类型
            string strReturn = "";

            bool bolTranslae = (strFormOperation == "GetJsonTranslate");
            strReturn = CsData.GetJSon(e.DataEntitys,  this.Context, strFormId
                , strFormOperation,  bolTranslae);

            //取报文，调试时，这里出错，不用慌。反正是显示给用户看。
            if (strReturn == "")
                strReturn = "没找到同步报文";

            throw new Exception(strReturn);

        }

        //避免用BeforeExecuteOperationTransaction
        //它只能用：e.SelectedRows，和AfterExecuteOperationTransaction不兼容。


    }
}

