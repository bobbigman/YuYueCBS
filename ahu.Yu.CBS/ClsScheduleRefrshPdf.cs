using Kingdee.BOS;
using Kingdee.BOS.Contracts;
using Kingdee.BOS.Core;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.Util;
using Kingdee.BOS.WebApi.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ahu.YuYue.CBS
{
    public class ClsScheduleRefrshPdf : IScheduleService
    {
        public void Run(Context ctx, Schedule schedule)
        {
            K3CloudApiClient pK3CloudApiClient = null;
            Struct_K3LoginInfo Struct_K3LoginInfo1= new Struct_K3LoginInfo ();
            string strReturn1 = MySchedule(ctx, pK3CloudApiClient,ref Struct_K3LoginInfo1, K3DatabaseMode.IntegrationK3);

            if (strReturn1 != "")
            {
                throw new Exception(strReturn1);
            }
        }

        public string MySchedule(Context pContext, K3CloudApiClient pK3CloudApiClient
            , ref Struct_K3LoginInfo pStruct_K3LoginInfo,int p2DatabaseType)
        {

            string strReturns = "";

            string strReturnType = "";
            bool bolNeedLogInK3 = false;

            if (pStruct_K3LoginInfo.LoginK3Ok == false || pStruct_K3LoginInfo.AcctID == null || pK3CloudApiClient == null)
            {
                //有时，登陆了，参数却没有取得值。
                int IntReturn = ClsPublic.LogIn(ref pStruct_K3LoginInfo, pContext
                    , ref pK3CloudApiClient, ref strReturnType, bolNeedLogInK3, p2DatabaseType);
                if (IntReturn != 1)
                    return strReturnType;
            }

            string strSQL = string.Format(@"/*dialect*/
SELECT rec.FId, rec.fdate 
       ,s.FbucketFileName
FROM  t_ReceiptBill_Midd  s  inner join
	  T_WB_RECEIPT rec on s.FBILLNO=rec.FBILLNO 
Where rec.F_PKUB_ATTACHMENTCOUNT=0
And  isnull(FRefreshFileDatetime,'2000-1-1')<dateadd(d,-5,getdate())    --5天前刷新的不算数，要重新刷一下了。
");

            DynamicObjectCollection doc1 = CsData.GetDynamicObjects(pContext, pK3CloudApiClient, strSQL);

            CsCallCBS CsCallCBS1 = new CsCallCBS();

            string strError1 = "";
            string strToken = CsCallCBS.getToken(pContext, pK3CloudApiClient, p2DatabaseType,pStruct_K3LoginInfo, ref strError1);
            if (strError1 != "")
                return "";

            csMiddle2K3 csMiddle2K3_1 = new csMiddle2K3();
            csMiddle2K3_1.SetBySchedule(pContext,ref pK3CloudApiClient,ref pStruct_K3LoginInfo);

            List<string> fIdList = new List<string>();

            string strSQL0 = string.Format(@"Update t_ReceiptBill_Midd set FRefreshFileDatetime=getdate() ");
            foreach (DynamicObject do1 in doc1)
            {
                DateTime dtFDate_DZHD = Convert.ToDateTime(do1["fdate"]);
                string strFDate_DZHD = dtFDate_DZHD.ToString("yyyy-MM-dd");
                string strFileName_DZHD = Convert.ToString("FbucketFileName");
                string strFId = Convert.ToString(do1["FId"]);
                //RefreshPdf(pFileURL_DZHD, strFDate_DZHD);
                csMiddle2K3_1.RefreshPdf(strFileName_DZHD, strFDate_DZHD,ref strError1, strToken);
                if (strError1 != "")
                {
                    if (strReturns != "")
                        strReturns += Environment.NewLine;

                    strReturns += strError1;
                }
                else
                {
                    fIdList.Add(strFId); // 收集成功处理的 FId
                }

                if (fIdList.Count > 50)
                {
                    string strFIds = string.Join(",", fIdList); 
                    strSQL = strSQL0 + string.Format(" where FId in ({0})", strFIds);
                    CsData.BobExecute(pContext,  pK3CloudApiClient, strSQL);
                    fIdList.Clear();

                }
            }

            if (fIdList.Count > 0)
            {
                string strFIds = string.Join(",", fIdList);
                strSQL = strSQL0 + string.Format(" where FId in ({0})", strFIds);
                CsData.BobExecute(pContext, pK3CloudApiClient, strSQL);

            }

            return strReturns;
        }
    }
}
