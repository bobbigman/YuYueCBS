using Kingdee.BOS;
using Kingdee.BOS.Contracts;
using Kingdee.BOS.Core;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.Util;
using Kingdee.BOS.WebApi.Client;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using static System.Windows.Forms.AxHost;

namespace ahu.YuYue.CBS
{
    [Description("系统优化")]
    [Kingdee.BOS.Util.HotUpdate]
    public class ClsScheduleOptimization : IScheduleService
    {
        public void Run(Context ctx, Schedule schedule)
        {

            MyOptimization(ctx, null, K3DatabaseMode.IntegrationK3);
        }


        public string MyOptimization(Context pContext, K3CloudApiClient pK3CloudApiClient, int pIntRunServerType)
        {

            string strReturns = "";
            bool bolIsSchedule = true;

            Struct_K3LoginInfo Struct_K3LoginInfo1 = new Struct_K3LoginInfo();

            //第一步，同步前一个月的,从CBS到中间表。
            ClsScheduleReadOthers ClsScheduleReadOthers1 = new ClsScheduleReadOthers();
            string strStartDate = System.DateTime.Today.AddDays(-40).ToString("yyyy-MM-dd");
            string strEndDate = System.DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
            string strGetDllLastWriteTime = System.IO.File.GetLastWriteTime(this.GetType().Assembly.Location).ToString();
            string strReturn1 = ClsScheduleReadOthers1.ReadAnd2MiddleTableOrK3(pContext, pK3CloudApiClient,ref Struct_K3LoginInfo1
                , WDT2.MiddleTable, strStartDate, strEndDate, "", 0, "", pIntRunServerType,  bolIsSchedule);
            strReturns = "同步单据（交易明细，电子回单，附件)，返回信息为：" + Environment.NewLine + strReturn1;

//             感觉是画蛇添足。虽然，要不了多少时间。
//            //第二步，交易明细，没有电子回单的，同步到中间表
//            string strSQL = string.Format(@"
//Select  freceiptno,ftransdate 
//from T_CN_BANKCASHFLOW
//where  freceiptcount=0
//and ftransdate>'{0}'
//and freceiptno like 'dzm%'
//", strStartDate);
//            DynamicObjectCollection doc1 = CsData.GetDynamicObjects(pContext, pK3CloudApiClient, strSQL);
//            foreach (dynamic do1 in doc1)
//            {
//                string strFreceiptno = Convert.ToString(do1["freceiptno"]);
//                string strCheckCode = ExtractBeforeUnderscore(strFreceiptno);
//                if (strCheckCode == "")
//                    continue;

//                //把日期范围放大一些。
//                //同步电子回单。
//                string strFtransdate = Convert.ToString(do1["ftransdate"]);
//                DateTime dt1 = Convert.ToDateTime(strFtransdate);
//                string strStartDate2 = dt1.AddDays(-10).ToString("yyyy-MM-dd");
//                string strEndDate2 = dt1.AddDays(10).ToString("yyyy-MM-dd");
//                strReturn1 = ClsScheduleReadOthers1.ReadAnd2MiddleTableOrK3(pContext, pK3CloudApiClient, ref Struct_K3LoginInfo1
//                    , WDT2.MiddleTable, strStartDate2, strEndDate2, strCheckCode, 2, "", pIntRunServerType, false, true);
//            }


            //第二步，从中间表同步到金蝶。
            ClsSchedule2K3 ClsSchedule2K3Bob = new ClsSchedule2K3();
            strReturn1 = ClsSchedule2K3Bob.Middle2K3(pContext, pK3CloudApiClient
                , ref Struct_K3LoginInfo1,"",0,"", pIntRunServerType);
            if (strReturn1!="")
            {
                if (strReturns != "")
                    strReturns += Environment.NewLine;

                strReturns += strReturn1;

            }

//            感觉不需要了，直接从中间表同步，更简洁。
//            //第四步，找出电子回单中没有附件的，开始同步。
//            strSQL = string.Format(@"
//Select fdetailno, FDate
//from T_WB_RECEIPT  
//WHERE F_PKUB_ATTACHMENTCOUNT=0
//AND FDATE>='{0}'  --往前面一点，因为24/1/1的附件，没法同步
//AND fdetailno like 'DZM%'
//", strStartDate);
//            DynamicObjectCollection doc2 = CsData.GetDynamicObjects(pContext, pK3CloudApiClient, strSQL);

//            //想个法子，先刷新，但是，不想传参数。
//            foreach (dynamic do2 in doc2)
//            {
//                string strfdetailno = Convert.ToString(do2["fdetailno"]);
//                string strCheckCode = ExtractBeforeUnderscore(strfdetailno);
//                if (strCheckCode == "")
//                    continue;

//                //把日期范围放大一些。
//                //同步电子回单。
//                string strFdate = Convert.ToString(do2["FDate"]);
//                DateTime dt1 = Convert.ToDateTime(strFdate);
//                string strStartDate1 = dt1.AddDays(-10).ToString("yyyy-MM-dd");
//                string strEndDate1 = dt1.AddDays(10).ToString("yyyy-MM-dd");

//                ClsScheduleReadOthers1.RefreshPdf(true);
//                strReturn1 = ClsScheduleReadOthers1.ReadAnd2MiddleTableOrK3(pContext, pK3CloudApiClient,ref Struct_K3LoginInfo1
//                    , WDT2.K3, strStartDate1, strEndDate1, strCheckCode, 3, "", pIntRunServerType, false, true);
//            }


            return strReturns;

        }


        private string ExtractBeforeUnderscore(string s)
        {
            int index = s.IndexOf('_');
            if (index != -1)
            {
                return s.Substring(0, index);
            }
            else
            {
                return "";
            }
        }

    }







}

