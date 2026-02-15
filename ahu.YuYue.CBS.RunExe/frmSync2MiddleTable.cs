using ahu.YuYue.CBS;
using Kingdee.BOS;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.Util;
using Kingdee.BOS.WebApi.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ahu.YuYue.CBS.RunExe
{
    public partial class frmSync2MiddleTable : Form
    {
        //SqlConnection mSqlConnectionK3;
        K3CloudApiClient mK3CloudApiClient;
        bool mbolRun = false;
        StringBuilder msb1 = new StringBuilder();
        public frmSync2MiddleTable()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //txtStartTime.Text = System.DateTime.Today.AddDays(-5).ToString("yyyy-MM-dd");
            //txtEndDate.Text = System.DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");

        }



        private void btnReadOthers_Click(object sender, EventArgs e)
        {
            rtbResult.Clear();

            StringBuilder sb1 = new StringBuilder();
            sb1.AppendLine(System.DateTime.Now.ToString());
            sb1.AppendLine("资料处理中，请稍后....");

            rtbResult.Text = sb1.ToString();

            CsPublic2.HaveaRest();

            //企业ERP系统通过招行CBS8发起银行支付指令
            string strBalanceUrl = "/openapi/payment/openapi/v1/payment-apply-common";

            string strDate1 = txtStartTime.Text;
            string strDate2 = txtEndDate.Text;
            string strPage = txtPage.Text;
            string strBillType = cboBillType.Text;
            string strBillNo = txtBillNo.Text;


            string strJSon = "";
            switch (strBillType)
            {
                case "接收银行交易明细":
                    strBalanceUrl = "openapi/account/openapi/v1/transaction-detail/query";
                    string strBankSerialNumber = txtBankSerialNumber.Text;

                    //Dictionary，这个方法能通用，还能处理数组哦。
                    //不过，这个方法，只适合测试时用，太死板，而我是平台化哦。
                    var parameters = new Dictionary<string, string>
{
    { "startDate", strDate1 },
    { "endDate", strDate2 },
    { "dateType", "0" },
    { "currentPage", strPage },
    { "pageSize", "20" }
};

                    if (string.IsNullOrWhiteSpace(strBillNo) == false)
                        parameters.Add("transactionSerialNumber", strBillNo);

                    if (string.IsNullOrWhiteSpace(strBankSerialNumber) == false)
                        parameters.Add("bankSerialNumber", strBankSerialNumber);

                    strJSon = JsonConvert.SerializeObject(parameters);

                    break;
                case "电子回单":
                    strBalanceUrl = "openapi/account/openapi/v1/electronic-bill/query";

                    //不适合用这种方式，结构是固定的。2024/7/4
                    //var data = new
                    //{
                    //    endDate = strDate1,
                    //    startDate = strDate2,
                    //    checkCodeList = new List<string> { strBillNo },
                    //    currentPage = 1,
                    //    pageSize = 20
                    //};
                    //strJSon = JsonConvert.SerializeObject(data);
                    strPage = "0";
                    var parameters2 = new Dictionary<string, object>
{
    { "startDate", strDate1 },
    { "endDate", strDate2 },
    { "currentPage", strPage },
    { "pageSize", "2000" }
};

                    if (string.IsNullOrWhiteSpace(strBillNo) == false)
                        parameters2.Add("checkCodeList", new List<string> { strBillNo });

                    strJSon = JsonConvert.SerializeObject(parameters2);

                    break;

                case "支付状态":
                    strBalanceUrl = "openapi/payment/openapi/v2/detail";
                    //string strReferenceNum = tbReferenceNum.Text.Trim();
                    string strReferenceNum = "";

                    string strError1 = "";
                    string strFBILLNO_PayBill = tbBillNo_PayBill.Text.Trim();
                    strReferenceNum = GetFBIZREFNOByFKD(strFBILLNO_PayBill, ref strError1);

                    if (strError1 != "")
                    {
                        rtbResult.Text = "根据付款单，取业务参考号遇到错误。 （即：referenceNum）";
                        rtbResult.AppendText(Environment.NewLine);
                        rtbResult.AppendText(strError1);
                        return;
                    }
                    if (strReferenceNum == "")
                    {
                        rtbResult.Text = "根据付款单，没有取到业务参考号 （即：referenceNum）";
                        rtbResult.AppendText(Environment.NewLine);
                        return;
                    }

                    var parameters3 = new Dictionary<string, object>
{
    { "referenceNum", strReferenceNum },
};

                    strJSon = JsonConvert.SerializeObject(parameters3);
                    break;

            }

            string strJsonResult = "";
            TestCBSDll TestCBSDll1 = new TestCBSDll();
            string strError = "";
            TestCBSDll1.CallCBS_Demo(strBalanceUrl, strJSon, ref strJsonResult, ref strError);

            if (strBillType == "支付状态")
            {
                JObject joReturn = JObject.Parse(strJsonResult);
                string strPayStatus = Convert.ToString(joReturn["data"][0]["payStatus"]);
                string strPayStatusChinese = "";
                bool bolAllowUnAudit = false;
                switch (strPayStatus)
                {
                    case "":
                        bolAllowUnAudit = false;
                        strPayStatusChinese = "未取到银行支付状态";
                        break;
                    case "a":
                        bolAllowUnAudit = false;
                        strPayStatusChinese = "待提交直连";
                        break;
                    case "b":
                        bolAllowUnAudit = false;
                        strPayStatusChinese = "已提交直连";
                        break;
                    case "c":
                        bolAllowUnAudit = false;
                        strPayStatusChinese = "银行已受理";
                        break;
                    case "d":
                        bolAllowUnAudit = true;
                        strPayStatusChinese = "银行未受理";
                        break;
                    case "e":
                        bolAllowUnAudit = false;
                        strPayStatusChinese = "可疑";
                        break;
                    case "f":
                        bolAllowUnAudit = false;
                        strPayStatusChinese = "待人工确认";
                        break;
                    case "g":
                        bolAllowUnAudit = false;
                        strPayStatusChinese = "支付成功";
                        break;
                    case "h":
                        bolAllowUnAudit = true;
                        strPayStatusChinese = "支付失败";
                        break;
                    case "j":
                        bolAllowUnAudit = true;
                        strPayStatusChinese = "退票";
                        break;
                    case "k":
                        bolAllowUnAudit = true;
                        strPayStatusChinese = "取消支付";
                        break;
                }

                if (bolAllowUnAudit == true)
                {
                    rtbResult.Text = "允许反审核，CBS支付状态为:" + strPayStatusChinese;
                }
                else
                {
                    rtbResult.Text = "不允许反审核，CBS支付状态为:" + strPayStatusChinese;
                }

                return;

            }


            rtbResult.AppendText(Environment.NewLine);

            rtbResult.Clear();

            if (cbOnlyJson.Checked == false)
            {
                string strURLOthers = "https://tmcapi.cmbchina.com/";

                rtbResult.AppendText(strURLOthers + strBalanceUrl);
                rtbResult.AppendText(Environment.NewLine);
                JObject jo1 = JObject.Parse(strJSon);
                strJSon = jo1.ToString();
                rtbResult.AppendText(strJSon);
                rtbResult.AppendText(Environment.NewLine);
            }
            if (strJsonResult!="")
            {
                rtbResult.AppendText(Environment.NewLine);
                rtbResult.AppendText(strJsonResult);

            }

            if (strError != "")
            {
                rtbResult.AppendText(Environment.NewLine);
                rtbResult.AppendText(strError);
            }

        }
        private void CreateWebClient(ref Struct_K3LoginInfo pStruct_K3LoginInfo)
        {

            int intMode = 0;
            if (radNormal.Checked == true)
                intMode = K3DatabaseMode.Normal;
            else if (radTest.Checked == true)
                intMode = K3DatabaseMode.Test;

            mK3CloudApiClient = CsPublicOA.CreateK3CloudApiClient(ref pStruct_K3LoginInfo, intMode);
        }
        private string ReadAndToMiddleTable(int p2Where, string pFBillNo, bool pOnlyError, ref bool pErrorStop)
        {

            string strBillType = cboBillType.Text.Trim().Trim();
            int intBillType = 0;


            string strStartTime = txtStartTime.Text;
            string strEndTime = txtEndDate.Text;

            string strPage = txtPage.Text.Trim();

            if (rad1BillNo.Checked == true)
            {
                if (strBillType == "")
                {
                    pErrorStop = true;
                    return "请输入：单据类型。";
                }

                if (pFBillNo == "" && p2Where != WDT2.OnlyCount)
                {
                    pErrorStop = true;
                    return "请输入：单据编号";
                }

            }

            else if (radBillType1.Checked == true)
            {
                if (strBillType == "")
                {
                    pErrorStop = true;
                    return "请输入：单据类型。";
                }

                if (strStartTime == "")
                {
                    pErrorStop = true;
                    return "请输入开始日期。";
                }
                if (strEndTime == "")
                {
                    pErrorStop = true;
                    return "请输入结束日期。";
                }

                if (pFBillNo != "" && pOnlyError == false)
                {
                    //错误批量处理，要排除掉 pOnlyError
                    pErrorStop = true;
                    return "困惑，选择批量同步，却又输入了物料编号，请检查一下哈，胡大人。";
                }

            }
            else if (radBillTypeAll.Checked == true)
            {
                if (strStartTime == "")
                {
                    pErrorStop = true;
                    return "请输入开始日期。";
                }
                if (strEndTime == "")
                {
                    pErrorStop = true;
                    return "请输入结束日期。";
                }
            }

            intBillType = CsPublic2.GetBillTypeID(strBillType);


            DateTime dtStartDate = DateTime.Today;
            DateTime dtEndDate = DateTime.Today;

            if (strStartTime != "")
                dtStartDate = Convert.ToDateTime(strStartTime);

            if (strStartTime != "")
                dtEndDate = Convert.ToDateTime(strEndTime);

            //旺店通才 需要的代码，这里，不需要。
            //旺店通，对过滤日期，作了限制。很多接口，也会做这个限制，避免一次，取太多的资料。
            //DateTime dtEndDate = dtStartDate.AddHours(1);

            string strDate1 = dtStartDate.ToString("yyyy-MM-dd");
            string strDate2 = dtEndDate.ToString("yyyy-MM-dd");

            //string strShow = "过滤开始日期:" + strDate1 + " 过滤结束日期:" + strDate2 + " 过滤单据号：" + pFBillNo + Environment.NewLine;
            //rtbResult.AppendText(strShow + "资料处理中，请稍候...."+ Environment.NewLine);
            //rtbResult.ScrollToCaret();
            //CsPublic2.HaveaRest();

            Struct_K3LoginInfo Struct_K3LoginInfo1 = new Struct_K3LoginInfo();
            if (mK3CloudApiClient == null)
            {
                CreateWebClient(ref Struct_K3LoginInfo1);
            }

            int intMode = 0;
            if (radNormal.Checked == true)
                intMode = K3DatabaseMode.Normal;
            else if (radTest.Checked == true)
                intMode = K3DatabaseMode.Test;


            string strError1 = "";
            Context Context1 = null;
            string strToken = CsCallCBS.getToken(Context1, mK3CloudApiClient, intMode,Struct_K3LoginInfo1, ref strError1);
            if (strError1 != "")
                return "";

            string strReturns = "";
            bool bolIsSchedule = false;
            ClsScheduleReadOthers ClsScheduleReadOthers1 = new ClsScheduleReadOthers();
            if (intBillType == 0)
            {
                //全部业务单据。
                for (intBillType = 1; intBillType <= 4; intBillType++)
                {
                    pFBillNo = "";
                    string strReturn1 = ClsScheduleReadOthers1.Only1BillType
                        (null, mK3CloudApiClient,ref Struct_K3LoginInfo1
                        , p2Where, strDate1, strDate2, strPage, pFBillNo, intBillType, intMode
                        , strToken, bolIsSchedule);

                    if (strReturn1 != "")
                        strReturns += strReturn1;
                }

            }
            else
            {
                //某种业务单据,比如，只同步采购入库单。
                strReturns = ClsScheduleReadOthers1.Only1BillType
                    (null, mK3CloudApiClient, ref Struct_K3LoginInfo1, p2Where, strDate1, strDate2
                    , strPage
                    , pFBillNo, intBillType, intMode
                    ,strToken, bolIsSchedule);
            }

            return strReturns;

        }

        private string ReadAnd2K3()
        {
            string strBillNo = txtBillNo.Text.Trim().Trim();

            string strBillType = cboBillType.Text.Trim().Trim();
            int intBillType = 0;

            if (rad1BillNo.Checked == true)
            {
                if (strBillType == "")
                {
                    return "请输入：单据类型。";
                }

                if (strBillNo == "" )
                {
                    return "请输入：单据编号";
                }

            }

            else if (radBillType1.Checked == true)
            {
                if (strBillType == "")
                {
                    return "请输入：单据类型。";
                }

                if (strBillNo != "")
                {
                    //错误批量处理，要排除掉 pOnlyError
                    return "困惑，选择批量同步，却又输入了物料编号，请检查一下哈，胡大人。";
                }

            }


            intBillType = CsPublic2.GetBillTypeID(strBillType);
            Struct_K3LoginInfo Struct_K3LoginInfo1 = new Struct_K3LoginInfo();

            if (mK3CloudApiClient == null)
            {
                CreateWebClient(ref Struct_K3LoginInfo1);
            }

            int intMode = 0;
            if (radNormal.Checked == true)
                intMode = K3DatabaseMode.Normal;
            else if (radTest.Checked == true)
                intMode = K3DatabaseMode.Test;

            string strTableNameMid = "";
            if (strBillType== "接收银行交易明细")
            {
                strTableNameMid = "t_CashFlow_Midd";
            }
            else if (strBillType == "电子回单")
            {
                strTableNameMid = "t_ReceiptBill_Midd";
            }
            else
            {
                strTableNameMid = "t_ReceiptBill_Midd";

            }
            string strSQL = string.Format(@"SELECT TOP (1) [FID]  
  FROM {0}
  where FBILLNO='{1}'", strTableNameMid, strBillNo);
            string strFId = CsData.GetTopValue(null,mK3CloudApiClient,strSQL);
            if (strFId=="")
            {
                return "单据编号不存在。";
            }


            ClsSchedule2K3 ClsSchedule2K3a =new ClsSchedule2K3();
            string strReturn = ClsSchedule2K3a.Middle2K3(null, mK3CloudApiClient,ref Struct_K3LoginInfo1
          , strBillNo, intBillType, strFId, intMode);

            return strReturn;

        }

        private void radTest_CheckedChanged(object sender, EventArgs e)
        {
            TestModeColor();
        }
        public void TestModeColor()
        {
            //mSqlConnectionK3 = null;
            //CreateConnection();

            if (radTest.Checked == true)
            {
                CsPublic2.RadioButtonCheckedColor(radTest);
                CsPublic2.RadioButtonNotCheckedColor(radNormal);
            }
            else
            {
                CsPublic2.RadioButtonCheckedColor(radNormal);
                CsPublic2.RadioButtonNotCheckedColor(radTest);
            }

        }

        private void radNormal_CheckedChanged(object sender, EventArgs e)
        {
            //TestModeColor();
        }

        //private void CreateConnection()
        //{
        //    if (mSqlConnectionK3 == null)
        //    {
        //        if (radTest.Checked == true)
        //        {

        //            mSqlConnectionK3 = clsSQLData.GetConnection(@"ReadTest.udl");

        //        }
        //        else if (radNormal.Checked == true)
        //        {
        //            mSqlConnectionK3 = clsSQLData.GetConnection(@"ReadNormal.udl");
        //        }
        //    }
        //}


        public void TestMode(bool pTestMode)
        {
            if (pTestMode == true)
            {
                CsPublic2.RadioButtonChecked(radTest);
                CsPublic2.RadioButtonNotChecked(radNormal);
            }
            else
            {
                CsPublic2.RadioButtonChecked(radNormal);
                CsPublic2.RadioButtonNotChecked(radTest);
            }
        }

        private void txtEndDate_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnRead2MiddleTable_Click(object sender, EventArgs e)
        {
            if (mbolRun == false)
                CallRead2MiddleTable();
            else
                MessageBox.Show("运行中.");
        }


        private void CallRead2MiddleTable()
        {
            DateTime dtStart = System.DateTime.Now;
            rtbResult.Clear();
            msb1.Clear();
            msb1 = new StringBuilder();
            msb1.AppendLine("资料处理中，请稍后....");
            msb1.AppendLine(dtStart.ToString());

            rtbResult.Text = msb1.ToString();

            msb1.Clear();
            CsPublic2.HaveaRest();


            string strBillNo = txtBillNo.Text.ToString().Trim();

            bool bolErrorStop = false;

            mbolRun = true;
            string strReturns = ReadAndToMiddleTable(WDT2.MiddleTable, strBillNo, false, ref bolErrorStop);
            mbolRun = false;

            msb1.Clear();
            //rtbResult.Clear();

            msb1.AppendLine();
            msb1.AppendLine(strReturns);
            msb1.AppendLine();
            DateTime dtEnd = System.DateTime.Now;
            msb1.AppendLine(dtEnd.ToString());


            // 计算两个时间点之间的时间差  
            TimeSpan difference = dtEnd.Subtract(dtStart);
            // 获取总分钟数  
            int totalMinutes = (int)difference.TotalMinutes;

            string strUse = "";
            if (totalMinutes == 0)
            {
                int totalSeconds = (int)difference.TotalSeconds;
                strUse = "[" + totalSeconds.ToString() + "] 秒";
            }
            else
                strUse = "[" + totalMinutes.ToString() + "] 分钟";

            msb1.AppendLine("恭喜，任务执行完毕,共用了 " + strUse + "* *********************************************************");
            rtbResult.AppendText(msb1.ToString());
            rtbResult.Select(rtbResult.TextLength, 0);  //光标定位到文本最后面。
            rtbResult.ScrollToCaret();  //滚动到光标处。
            CsPublic2.HaveaRest();

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            setTimer();
        }

        private void setTimer()
        {
            string strMinute = txtMinute.Text;
            int intMinute = Convert.ToInt32(strMinute);
            int intMillSecond = intMinute * 60000;
            timer1.Interval = intMillSecond;
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (radBillType1.Checked == false && radBillTypeAll.Checked == false)
                return;

            AutoRun();
        }

        private void AutoRun()
        {
            if (mbolRun == false)
                CallRead2MiddleTable();

            //定时执行，不希望每10分钟，显示提示。
            //else
            //    MessageBox.Show("已在执行。");

            //msb1.Clear();
            //CreateWebClient();
            //string strBillNo = txtBillNo.Text.ToString().Trim();
            //bool bolErrorStop = false;
            //ReadAndToK3(WDT2.OnlyCount, strBillNo, false, ref bolErrorStop);
        }

        private void rad1BillNo_CheckedChanged(object sender, EventArgs e)
        {
            ShowByAll(rad1BillNo.Checked);
        }

        private void radBillType_CheckedChanged(object sender, EventArgs e)
        {
            ShowByAll(rad1BillNo.Checked);
        }

        private void radBusiness_CheckedChanged(object sender, EventArgs e)
        {
            ShowByAll(rad1BillNo.Checked);
        }

        private void ShowByAll(bool pShow)
        {
            timer1.Enabled = pShow;
            //lblBillType.Visible = (pShow == false);
            //cboBillType.Visible = (pShow == false);
            //btnOk.Visible = pShow;
            //txtMinute.Visible = pShow;
            lblTimer.Visible = pShow;
            txtMinute.Visible = pShow;
        }


        private void btn2Count_Click(object sender, EventArgs e)
        {
            rtbResult.Clear();

            msb1.Clear();

            string strBillNo = txtBillNo.Text.ToString().Trim();
            bool bolErrorStop = false;
            ReadAndToMiddleTable(WDT2.OnlyCount, strBillNo, false, ref bolErrorStop);
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            msb1.Clear();
            string strBillType = cboBillType.Text.Trim().Trim();
            string strStartTime = txtStartTime.Text;
            string strEndTime = txtEndDate.Text;
            if (strStartTime == "")
            {
                rtbResult.Text = "请输入开始日期。";
                return;
            }
            if (strEndTime == "")
            {
                rtbResult.Text = "请输入结束日期。";
                return;
            }

            //if (strBillType == "")
            //{
            //    rtbResult.Text = "请输入：单据类型。";
            //    return;
            //}

            msb1.Append("资料处理中，请稍候....");

            string strShow = "开始执行时间：" + DateTime.Now;
            msb1.AppendLine(strShow);
            rtbResult.Text = msb1.ToString();
            CsPublic2.HaveaRest();

            DateTime datStartTime = Convert.ToDateTime(strStartTime);
            DateTime datEndTime = Convert.ToDateTime(strEndTime);

            int intMode = 0;
            if (radNormal.Checked == true)
                intMode = K3DatabaseMode.Normal;
            else if (radTest.Checked == true)
                intMode = K3DatabaseMode.Test;

            Struct_K3LoginInfo Struct_K3LoginInfo1 = new Struct_K3LoginInfo();
            CreateWebClient(ref Struct_K3LoginInfo1);

            int intBillType = 0;

            if (strBillType != "")
                intBillType = CsPublic2.GetBillTypeID(strBillType);

            //clsSyncCheck clsSyncCheck1 = new clsSyncCheck();
            string strReturns = "";
                //clsSyncCheck1.CalculateByBillTypes(null, mK3CloudApiClient, datStartTime, datEndTime, intBillType, intMode);

            strShow += Environment.NewLine + strReturns;
            msb1.AppendLine(strShow);
            msb1.AppendLine("恭喜，任务执行完毕。**********************************************************");
            rtbResult.Text = msb1.ToString();
            rtbResult.Select(rtbResult.TextLength, 0);  //光标定位到文本最后面。
            rtbResult.ScrollToCaret();  //滚动到光标处。
            CsPublic2.HaveaRest();


        }

        private void btn2CountSchedule_Click(object sender, EventArgs e)
        {
        }

        private void cboBillType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strBillType = cboBillType.Text;
            switch (strBillType)
            {
                case "接收银行交易明细":
                    lblBillNo.Text = "交易流水号";
                    txtBillNo.Text = "";
                    break;

                case "电子回单":
                    lblBillNo.Text = "对账码";
                    txtBillNo.Text = "DZM202511060001444741";
                    break;
            }
        }

        private void txtBillNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAutoSchedule_Click(object sender, EventArgs e)
        {
            msb1.Clear();
            msb1.AppendLine("资料处理中，请稍候....");
            DateTime dtStart = System.DateTime.Now;

            string strShow = "开始执行时间：" + dtStart;
            msb1.AppendLine(strShow);
            rtbResult.Text = msb1.ToString();
            CsPublic2.HaveaRest();

            string strStartTime = txtStartTime.Text;
            string strEndTime = txtEndDate.Text;

            DateTime datStartTime = Convert.ToDateTime(strStartTime);
            DateTime datEndTime = Convert.ToDateTime(strEndTime);
            Struct_K3LoginInfo Struct_K3LoginInfo1 = new Struct_K3LoginInfo();

            if (mK3CloudApiClient == null)
                CreateWebClient(ref Struct_K3LoginInfo1);

            int intMode = 0;
            if (radNormal.Checked == true)
                intMode = K3DatabaseMode.Normal;
            else if (radTest.Checked == true)
                intMode = K3DatabaseMode.Test;

            //ClsScheduleReadOthers ClsScheduleReadOthers1 = new ClsScheduleReadOthers();
            ClsScheduleAll ClsScheduleAll1 = new ClsScheduleAll();
            string strGetDllLastWriteTime = System.IO.File.GetLastWriteTime(this.GetType().Assembly.Location).ToString();
            //string strReturns = ClsScheduleReadOthers1.ReadAnd2MiddleTableOrK3(null, mK3CloudApiClient, ref Struct_K3LoginInfo1
            //    , WDT2.MiddleTable, "", "", "", 0, "", intMode, false, true);
            string strReturns = ClsScheduleAll1.MyScheduleAll(null, mK3CloudApiClient, intMode);

            msb1.AppendLine();
            msb1.AppendLine(strReturns);
            msb1.AppendLine();
            DateTime dtEnd = System.DateTime.Now;
            msb1.AppendLine(dtEnd.ToString());


            // 计算两个时间点之间的时间差  
            TimeSpan difference = dtEnd.Subtract(dtStart);
            // 获取总分钟数  
            int totalMinutes = (int)difference.TotalMinutes;

            string strUse = "";
            if (totalMinutes == 0)
            {
                int totalSeconds = (int)difference.TotalSeconds;
                strUse = "[" + totalSeconds.ToString() + "] 秒";
            }
            else
                strUse = "[" + totalMinutes.ToString() + "] 分钟";

            msb1.AppendLine("恭喜，任务执行完毕,共用了 " + strUse + "* *********************************************************");
            rtbResult.AppendText(msb1.ToString());
            rtbResult.Select(rtbResult.TextLength, 0);  //光标定位到文本最后面。
            rtbResult.ScrollToCaret();  //滚动到光标处。
            CsPublic2.HaveaRest();

        }


        private string GetFBIZREFNOByFKD(string pFBILLNO_PayBill, ref string pError)
        {

            Struct_K3LoginInfo Struct_K3LoginInfo1 = new Struct_K3LoginInfo();
            if (mK3CloudApiClient == null)
                CreateWebClient(ref Struct_K3LoginInfo1);

            //FCbsPay，是没有空值的，只有0和1
            string strSQL = string.Format(@"
select Top 1 FCbsPay
from T_AP_PAYBILL t1 
where t1.FBILLNO='{0}'", pFBILLNO_PayBill);
            string strFCbsPay = CsData.GetTopValue(null, mK3CloudApiClient, strSQL);
            if (strFCbsPay == "")
            {
                pError = "没找到付款单。";
                return "";
            }
            if (strFCbsPay == "0")
            {
                pError = "付款单可以反审，没有调用CBS支付指令。";
                return "";
            }

            strSQL = string.Format(@"
select Top 1 FBIZREFNO
from T_AP_PAYBILL t1 join
T_AP_PAYBILLENTRY t2 on t1.FID = t2.FID
where t1.FBILLNO='{0}'
union all
select Top 1 FBIZREFNO_M
from T_AP_PAYBILL t1 join
    T_CN_PAYBILLMORERECENTRY t2 on t1.FID = t2.FID
where t1.FBILLNO='{0}'", pFBILLNO_PayBill);

            string strFBIZREFNO = CsData.GetTopValue(null, mK3CloudApiClient, strSQL);
            if (strFBIZREFNO == "")
            {
                pError = "付款单没有业务参考号。";
                return "";
            }
            pError = "";
            return strFBIZREFNO;



        }

        private void btn2K3_Click(object sender, EventArgs e)
        {
            rtbResult.Clear();
            DateTime dtStart = System.DateTime.Now;
            StringBuilder sb1 = new StringBuilder();
            sb1.AppendLine(dtStart.ToString());
            sb1.AppendLine("资料处理中，请稍后....");

            rtbResult.Text = sb1.ToString();

            CsPublic2.HaveaRest();

            string strReturns = ReadAnd2K3();
            mbolRun = false;

            msb1.Clear();
            //rtbResult.Clear();


            msb1.AppendLine();
            msb1.AppendLine(strReturns);
            msb1.AppendLine();
            DateTime dtEnd = System.DateTime.Now;
            msb1.AppendLine(dtEnd.ToString());

            // 计算两个时间点之间的时间差  
            TimeSpan difference = dtEnd.Subtract(dtStart);
            // 获取总分钟数  
            int totalMinutes = (int)difference.TotalMinutes;

            string strUse = "";
            if (totalMinutes == 0)
            {
                int totalSeconds = (int)difference.TotalSeconds;
                strUse = "[" + totalSeconds.ToString() + "] 秒";
            }
            else
                strUse = "[" + totalMinutes.ToString() + "] 分钟";

            if (strReturns=="")
            msb1.AppendLine("恭喜，任务执行完毕,共用了 " + strUse + "* *********************************************************");

            rtbResult.AppendText(msb1.ToString());
            rtbResult.Select(rtbResult.TextLength, 0);  //光标定位到文本最后面。
            rtbResult.ScrollToCaret();  //滚动到光标处。
            CsPublic2.HaveaRest();


        }

        private void btnTestShartCode_Click(object sender, EventArgs e)
        {
            //Struct_K3LoginInfo Struct_K3LoginInfo1 = new Struct_K3LoginInfo();

            //if (mK3CloudApiClient == null)
            //{
            //    CreateWebClient(ref Struct_K3LoginInfo1);
            //}
            ////string strReturn = csCalculateTest.BobCalculateTest(mK3CloudApiClient);

            //string strSQL = string.Format(@"Select top 1 Formula from tblMatch Where FormID='{0}' And K3Field='FSETTLEORGID'", K3FormId.strWB_RecBankTradeDetail);
            //string strFormula = CsData.GetTopValue(null, mK3CloudApiClient, strSQL);

            //string strError = "";
            //string strReturn = csCalculate.BobCalculate(null, mK3CloudApiClient, null, "", "", null, ref strError,"", strFormula);
            ////string strReturn = csCalculateBak1202.BobCalculate(null, mK3CloudApiClient, null, "", "", null, ref strError, "");
            //rtbResult.Text = strReturn;

            //if (strError!="")
            //    rtbResult.Text = strError;


        }

        private void btnOptmization_Click(object sender, EventArgs e)
        {
            msb1.Clear();
            msb1.AppendLine("资料处理中，请稍候....");
            DateTime dtStart = System.DateTime.Now;

            string strShow = "开始执行时间：" + dtStart;
            msb1.AppendLine(strShow);
            rtbResult.Text = msb1.ToString();
            CsPublic2.HaveaRest();

            string strStartTime = txtStartTime.Text;
            string strEndTime = txtEndDate.Text;


            Struct_K3LoginInfo Struct_K3LoginInfo1 = new Struct_K3LoginInfo();

            if (mK3CloudApiClient == null)
                CreateWebClient(ref Struct_K3LoginInfo1);

            int intMode = 0;
            if (radNormal.Checked == true)
                intMode = K3DatabaseMode.Normal;
            else if (radTest.Checked == true)
                intMode = K3DatabaseMode.Test;

            ClsScheduleOptimization ClsScheduleOptimization1 = new ClsScheduleOptimization();
            string strGetDllLastWriteTime = System.IO.File.GetLastWriteTime(this.GetType().Assembly.Location).ToString();
            string strReturns = ClsScheduleOptimization1.MyOptimization(null, mK3CloudApiClient, intMode);

            msb1.AppendLine();
            msb1.AppendLine(strReturns);
            msb1.AppendLine();
            DateTime dtEnd = System.DateTime.Now;
            msb1.AppendLine(dtEnd.ToString());


            // 计算两个时间点之间的时间差  
            TimeSpan difference = dtEnd.Subtract(dtStart);
            // 获取总分钟数  
            int totalMinutes = (int)difference.TotalMinutes;

            string strUse = "";
            if (totalMinutes == 0)
            {
                int totalSeconds = (int)difference.TotalSeconds;
                strUse = "[" + totalSeconds.ToString() + "] 秒";
            }
            else
                strUse = "[" + totalMinutes.ToString() + "] 分钟";

            msb1.AppendLine("恭喜，任务执行完毕,共用了 " + strUse + "* *********************************************************");
            rtbResult.AppendText(msb1.ToString());
            rtbResult.Select(rtbResult.TextLength, 0);  //光标定位到文本最后面。
            rtbResult.ScrollToCaret();  //滚动到光标处。
            CsPublic2.HaveaRest();

        }

        private void btnUpdateAttachment_Click(object sender, EventArgs e)
        {
            msb1.AppendLine("资料处理中，请稍候....");
            DateTime dtStart = System.DateTime.Now;

            string strShow = "开始执行时间：" + dtStart;
            msb1.AppendLine(strShow);
            rtbResult.Text = msb1.ToString();
            CsPublic2.HaveaRest();

            string strStartTime = txtStartTime.Text;
            string strEndTime = txtEndDate.Text;

            DateTime datStartTime = Convert.ToDateTime(strStartTime);
            DateTime datEndTime = Convert.ToDateTime(strEndTime);
            Struct_K3LoginInfo Struct_K3LoginInfo1 = new Struct_K3LoginInfo();

            if (mK3CloudApiClient == null)
                CreateWebClient(ref Struct_K3LoginInfo1);

            int intMode = 0;
            if (radNormal.Checked == true)
                intMode = K3DatabaseMode.Normal;
            else if (radTest.Checked == true)
                intMode = K3DatabaseMode.Test;

            ClsSchedule2K3 ClsSchedule2K3_1 = new ClsSchedule2K3();
            string strReturns = ClsSchedule2K3_1.Middle2K3(null, mK3CloudApiClient, ref Struct_K3LoginInfo1
                , "", 3, "", intMode);

            msb1.AppendLine();
            msb1.AppendLine(strReturns);
            msb1.AppendLine();
            DateTime dtEnd = System.DateTime.Now;
            msb1.AppendLine(dtEnd.ToString());


            // 计算两个时间点之间的时间差  
            TimeSpan difference = dtEnd.Subtract(dtStart);
            // 获取总分钟数  
            int totalMinutes = (int)difference.TotalMinutes;

            string strUse = "";
            if (totalMinutes == 0)
            {
                int totalSeconds = (int)difference.TotalSeconds;
                strUse = "[" + totalSeconds.ToString() + "] 秒";
            }
            else
                strUse = "[" + totalMinutes.ToString() + "] 分钟";

            msb1.AppendLine("恭喜，任务执行完毕,共用了 " + strUse + "* *********************************************************");
            rtbResult.AppendText(msb1.ToString());
            rtbResult.Select(rtbResult.TextLength, 0);  //光标定位到文本最后面。
            rtbResult.ScrollToCaret();  //滚动到光标处。
            CsPublic2.HaveaRest();

        }

        private void btnRefreshPdf_Click(object sender, EventArgs e)
        {
            msb1.AppendLine("资料处理中，请稍候....");
            DateTime dtStart = System.DateTime.Now;

            string strShow = "开始执行时间：" + dtStart;
            msb1.AppendLine(strShow);
            rtbResult.Text = msb1.ToString();
            CsPublic2.HaveaRest();

            string strStartTime = txtStartTime.Text;
            string strEndTime = txtEndDate.Text;

            DateTime datStartTime = Convert.ToDateTime(strStartTime);
            DateTime datEndTime = Convert.ToDateTime(strEndTime);
            Struct_K3LoginInfo Struct_K3LoginInfo1 = new Struct_K3LoginInfo();

            if (mK3CloudApiClient == null)
                CreateWebClient(ref Struct_K3LoginInfo1);

            int intMode = 0;
            if (radNormal.Checked == true)
                intMode = K3DatabaseMode.Normal;
            else if (radTest.Checked == true)
                intMode = K3DatabaseMode.Test;

            ClsScheduleRefrshPdf ClsScheduleRefrshPdf1 = new ClsScheduleRefrshPdf();
            string strReturns = ClsScheduleRefrshPdf1.MySchedule(null, mK3CloudApiClient,ref Struct_K3LoginInfo1, intMode);

            msb1.AppendLine();
            msb1.AppendLine(strReturns);
            msb1.AppendLine();
            DateTime dtEnd = System.DateTime.Now;
            msb1.AppendLine(dtEnd.ToString());


            // 计算两个时间点之间的时间差  
            TimeSpan difference = dtEnd.Subtract(dtStart);
            // 获取总分钟数  
            int totalMinutes = (int)difference.TotalMinutes;

            string strUse = "";
            if (totalMinutes == 0)
            {
                int totalSeconds = (int)difference.TotalSeconds;
                strUse = "[" + totalSeconds.ToString() + "] 秒";
            }
            else
                strUse = "[" + totalMinutes.ToString() + "] 分钟";

            msb1.AppendLine("恭喜，任务执行完毕,共用了 " + strUse + "* *********************************************************");
            rtbResult.AppendText(msb1.ToString());
            rtbResult.Select(rtbResult.TextLength, 0);  //光标定位到文本最后面。
            rtbResult.ScrollToCaret();  //滚动到光标处。
            CsPublic2.HaveaRest();



        }
    }

}
