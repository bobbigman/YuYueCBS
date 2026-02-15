using Kingdee.BOS;
using Kingdee.BOS.Contracts;
using Kingdee.BOS.Core;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.Util;
using Kingdee.BOS.WebApi.Client;
using System;
using System.ComponentModel;

namespace ahu.YuYue.CBS
{

    [Kingdee.BOS.Util.HotUpdate]
    [Description("不要引用，从中间表同步到金蝶")]
    public class ClsSchedule2K3 : IScheduleService
    {

        public void Run(Context ctx, Schedule schedule)
        {

            Struct_K3LoginInfo Struct_K3LoginInfo1 = new Struct_K3LoginInfo();
            string strReturn = Middle2K3(ctx, null,ref Struct_K3LoginInfo1
                ,  "", 0, "", K3DatabaseMode.IntegrationK3);

        }

        public string Middle2K3(Context ctx, K3CloudApiClient pK3CloudApiClient,ref Struct_K3LoginInfo pStruct_K3LoginInfo
            , string pBillNo
            , int pintBillType, string pMiddleId, int p2DatabaseType
           )
        {
            try
            {
                string strReturns = "";

                if (pintBillType == 0)
                {
                    string strSQL = string.Format(@"
SELECT SyncBillTypeId
FROM tblSyncBillTypeCBS
where  Sync2K3=1
order by SyncBillTypeId");
                    DynamicObjectCollection doc1 = CsData.GetDynamicObjects(ctx, pK3CloudApiClient, strSQL);
                    foreach (DynamicObject do1 in doc1)
                    {
                            int intSyncBillTypeId = Convert.ToInt32(do1["SyncBillTypeId"]);

                            string strReturn1 = Only1BillType(ctx, pK3CloudApiClient, pStruct_K3LoginInfo
                                , pBillNo, intSyncBillTypeId, "", p2DatabaseType);


                            if (strReturn1 != "")
                            {
                                if (strReturns != "")
                                {
                                    strReturns += Environment.NewLine;
                                }

                                strReturns += strReturn1;
                            }

                        }
                    
                }

                else
                {
                    string strReturn1 = Only1BillType(ctx, pK3CloudApiClient, pStruct_K3LoginInfo
                        , pBillNo, pintBillType,  "", p2DatabaseType);
                }


                return strReturns;
            }
            catch (Exception ex)
            {
                throw new Exception(CsErrLog.GetExceptionWithLog(ex));
            }
        }


        public string Only1BillType(Context ctx, K3CloudApiClient pK3CloudApiClient
            , Struct_K3LoginInfo pStruct_K3LoginInfo
            , string pBillNo_Middle, int pintSyncBillType
            , string pMiddleId
            , int p2DatabaseType)
        {
            try
            {

                csMiddle2K3 csMiddle2Kingdee1 = new csMiddle2K3();

                csMiddle2Kingdee1.NeedCBSToken(true); 

                string strReturn = csMiddle2Kingdee1.Call2K3(ctx,ref pK3CloudApiClient,ref pStruct_K3LoginInfo
                      , pintSyncBillType
                      ,pBillNo_Middle, pMiddleId,  p2DatabaseType);
                return strReturn;

            }
            catch (Exception ex)
            {
                throw new Exception(CsErrLog.GetExceptionWithLog(ex));
            }
        }
    }
}


