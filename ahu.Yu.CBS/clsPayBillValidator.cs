using Kingdee.BOS;
using Kingdee.BOS.App.Data;
using Kingdee.BOS.Core;
using Kingdee.BOS.Core.Validation;
using Kingdee.BOS.Orm.DataEntity;
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

    [Description("校验，反审核付款前，判断支付状态")]
    [Kingdee.BOS.Util.HotUpdate]
    public class ClsPayBillValidator : AbstractValidator
    {
        public override void Validate(ExtendedDataEntity[] dataEntities, ValidateContext validateContext, Context ctx)
        {
            foreach (ExtendedDataEntity extendedDataEntity in dataEntities)
            {
                DynamicObject DO1 = extendedDataEntity.DataEntity;

                string strFID = Convert.ToString(DO1["Id"]);

                string strError = "";
                bool bolOk = clsPayBill_Data.CheckBeforeUnAudit(Context, null, strFID, ref strError);
                if (bolOk)
                    return;

                strError = string.Format("操作失败，" + strError);
                validateContext.AddError(DO1, new ValidationErrorInfo("", strFID, extendedDataEntity.DataEntityIndex, extendedDataEntity.RowIndex, "PayBill", strError, "", ErrorLevel.Error));


            }
        }



    }
}
