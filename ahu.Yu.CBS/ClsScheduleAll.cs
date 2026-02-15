using Kingdee.BOS;
using Kingdee.BOS.Contracts;
using Kingdee.BOS.Core;
using Kingdee.BOS.Util;
using Kingdee.BOS.WebApi.Client;
using System;
using System.ComponentModel;

namespace ahu.YuYue.CBS
{
    public class ClsScheduleAll: IScheduleService
    {
        public void Run(Context ctx, Schedule schedule)
        {
            K3CloudApiClient pK3CloudApiClient = null;
            string strReturn1= MyScheduleAll(ctx, pK3CloudApiClient, K3DatabaseMode.IntegrationK3);
            if (strReturn1 != "")
            {
                strReturn1 = strReturn1.Replace("PKUB_CashFlow_Midd", "");
                strReturn1 = strReturn1.Replace("PKUB_CashFlow_Midd", "");
                strReturn1 = strReturn1.Replace("WB_ReceiptBill_Attachment_Midd", "");  //这个异常，还是AI 帮我发现的，惭愧。
                strReturn1 = strReturn1.Replace("所有单据，都从中间表同步到金蝶了。", "");
                strReturn1 = strReturn1.Replace("单据同步成功，但pdf附件下载失败，只能下载7天内的文件。", "");
                strReturn1 = strReturn1.Replace(Environment.NewLine+ Environment.NewLine, Environment.NewLine);

            }
            if (strReturn1 != "")
            {
                strReturn1 = strReturn1.Replace(Environment.NewLine ,"");
                if (strReturn1 != "")
                {
                    throw new Exception(strReturn1);
                }
            }
        }

        public string MyScheduleAll(Context ctx, K3CloudApiClient pK3CloudApiClient,int pDatabaseType)
        {

            string strReturns ="";
            Struct_K3LoginInfo Struct_K3LoginInfo1 = new Struct_K3LoginInfo();
            ClsScheduleReadOthers ClsScheduleReadOthers1 = new ClsScheduleReadOthers();
            strReturns = ClsScheduleReadOthers1.ReadAnd2MiddleTableOrK3(ctx, pK3CloudApiClient, ref Struct_K3LoginInfo1
                , WDT2.MiddleTable, "", "", "", 0, "", K3DatabaseMode.IntegrationK3, true);

            ClsSchedule2K3 ClsSchedule2K3_1 = new ClsSchedule2K3();
            string strReturn1 = ClsSchedule2K3_1.Middle2K3(ctx, pK3CloudApiClient, ref Struct_K3LoginInfo1
                , "", 0, "", pDatabaseType);
            if (strReturn1 != "")
            {
                if (strReturns != "")
                    strReturns += Environment.NewLine;

                strReturns += strReturn1;

            }
            return strReturns;
        }
    }
}
