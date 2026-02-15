using Kingdee.BOS.Core.DynamicForm.PlugIn;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ahu.YuYue.CBS 
{
    [Description("付款单，反审前校验")]
    [Kingdee.BOS.Util.HotUpdate]
    public class clsPayBill: AbstractOperationServicePlugIn
    {
        public override void OnPreparePropertys(PreparePropertysEventArgs e)
        {
            //e.FieldKeys.Add("FMaterialId");
        }

        public override void OnAddValidators(AddValidatorsEventArgs e)
        {
            //验证：负库存。
            ClsPayBillValidator ClsPayBillValidator1 = new ClsPayBillValidator();
            ClsPayBillValidator1.AlwaysValidate = true;
            ClsPayBillValidator1.EntityKey = "FBillHead";
            e.Validators.Add(ClsPayBillValidator1);
        }
    }
}
