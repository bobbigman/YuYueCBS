using B.ZP;
using Kingdee.BOS.WebApi.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace RunExe
{
    public partial class frmSync2LC : Form
    {

        K3CloudApiClient mK3CloudApiClient1;
        StringBuilder msb1 = new StringBuilder();
        public frmSync2LC()
        {
            InitializeComponent();
        }

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

        private void GeteWebClient()
        {
            //有时，切换帐套。
            //if (mK3CloudApiClient1 != null)
            //    return;

            if (radTest.Checked == false && radNormal.Checked == false)
                return;

            int intMode = RunMode.UnKnow;

            if (radTest.Checked == true)
                intMode = RunMode.Test;
            else if (radNormal.Checked == true)
                intMode = RunMode.Normal;


            mK3CloudApiClient1 = CsPublicOA.CreateK3CloudApiClient(intMode);
            if (mK3CloudApiClient1 == null)
            {
                //不要反复提醒。
                // MessageBox.Show("登陆金蝶失败了。");
                return;
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            msb1.Clear();
            AutoRun();
        }

        private void AutoRun()
        {

            string strBillType = cboBillType.Text;

            GeteWebClient();

            btnRun.Enabled = false;
            btnExit.Enabled = false;

            if (msb1.Length != 0)
            {
                msb1.AppendLine();
                msb1.AppendLine();
            }

            //msb1.AppendLine(rtbResult.Text);
            msb1.Append("资料处理中，请稍候....");
            string strTime = System.DateTime.Now.ToString("G");
            msb1.AppendLine(strTime);
            rtbResult.Text = msb1.ToString();

            Application.DoEvents();
            Application.DoEvents();
            Application.DoEvents();
            Application.DoEvents();
            Application.DoEvents();
            Application.DoEvents();
            Application.DoEvents();

            Test1();

            msb1.AppendLine("恭喜，任务执行完毕。**********************************************************");

            rtbResult.Text = msb1.ToString();
            rtbResult.Select(rtbResult.TextLength, 0);  //光标定位到文本最后面。
            rtbResult.ScrollToCaret();  //滚动到光标处。

            Application.DoEvents();
            Application.DoEvents();
            Application.DoEvents();
            Application.DoEvents();
            Application.DoEvents();
            Application.DoEvents();
            Application.DoEvents();

            btnRun.Enabled = true;
            btnExit.Enabled = true;


        }

        private void Test1()
        {

            string strBillType = cboBillType.Text;
            if (strBillType == "")
            {
                msb1.AppendLine("请输入单据单据类型.");
                return;
            }

            string strBillNo = txtBillNo.Text.Trim();
            if (strBillNo == "" )
            {
                //msb1.AppendLine("请输入 " + strBillType + " 编号。");
                //return;
            }

            bool bolIsDelete = radDelete.Checked;

            string strReturn = CsPublic2.TestSync2Others(mK3CloudApiClient1, strBillType, strBillNo, bolIsDelete);

            if (strReturn == "")
                strReturn = "完成";

            msb1.AppendLine(strBillType + new string(' ', 3) + strBillNo + Environment.NewLine + Environment.NewLine + strReturn + new string(' ', 6));
            msb1.AppendLine();
            string strTime = System.DateTime.Now.ToString("G");
            msb1.AppendLine(strTime + new string(' ', 3) + "同步结束");

            rtbResult.Text = msb1.ToString();

            Application.DoEvents();
            Application.DoEvents();
            Application.DoEvents();
            Application.DoEvents();
            Application.DoEvents();
            Application.DoEvents();
            Application.DoEvents();

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmTest_Load(object sender, EventArgs e)
        {

        }


        private void txtBillNo_TextChanged(object sender, EventArgs e)
        {

        }



        //private void GetJson(bool pTranslate)
        //{
        //    msb1 = new StringBuilder();
        //    string strBillType = cboBillType.Text;
        //    if (strBillType == "")
        //    {
        //        rtbResult.Text = "请输入单据单据类型。";
        //        return;

        //    }

        //    string strBillNo = txtBillNo.Text.Trim();
        //    if (strBillNo == "")
        //    {
        //        rtbResult.Text = "请输入单据编号。";
        //        return;

        //    }

        //    CreateConnection();

        //    //读出来，就是中文，不会转换，2->客户

        //    ClsPosTBGZT ClsPosTBGZT1 = new ClsPosTBGZT();

        //    string strFormID = ClsPosTBGZT1.GetFormIdByBillType(strBillType);

        //    string strReturn = CsPublic.GetJson(null, mK3CloudApiClient1, strFormID, strBillNo, "Json", pTranslate);

        //    rtbResult.Text = strReturn;


        //}

        private void btnJsonDeco_Click(object sender, EventArgs e)
        {
            GetJson(true);

        }

        private void radTest_CheckedChanged(object sender, EventArgs e)
        {
            //mK3CloudApiClient1 = null; ;
            //mSqlConnectionMidDatabase=null;
            //CreateConnection();

            TestModeColor(radTest.Checked);
        }

        private void radNormal_CheckedChanged(object sender, EventArgs e)
        {
            mK3CloudApiClient1 = null; ;
            GeteWebClient();
        }

        public void TestModeColor(bool pTestMode)
        {
            if (pTestMode == true)
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





        private void GetJson(bool pTranslate)
        {
            string strFindBillType = cboBillType.Text.ToString();
            if (strFindBillType == "")
            {
                MessageBox.Show("请选择单据类型.");
                return;
            }

            string strFormId = CsPublicOA.GetFormIdByBillType(strFindBillType);
            string strBillNo = txtBillNo.Text;
            if (strFindBillType == "")
            {
                MessageBox.Show("请输入单据编号。");
                return;
            }

            if (mK3CloudApiClient1 == null)
                GeteWebClient();

            string strFormOperation = "GetJson";

            string strReturn = CsPublicOA.GetJsonOA(null, mK3CloudApiClient1
                , strFormId, strFormOperation, "", strBillNo, pTranslate);
            frmRichText frmRichText1 = new frmRichText();
            frmRichText1.SetText(strReturn);
            frmRichText1.ShowDialog();

        }

        private void cboBillType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cboBillType.Text)
            {
                case "付款单":
                    txtBillNo.Text = "FKD10100034586";
                    //txtBillNo.Text = "FKD10100035703";
                    break;
                case "资金调拨单":
                    txtBillNo.Text = "ZJDB10100001";
                    break;
                case "银行转账单":
                case "银行转账单-资金":
                    txtBillNo.Text = "YHZZ00000451";
                    break;
                case "收款退款单-资金":
                    txtBillNo.Text = "SKTKD10100000062";
                    break;
            }
        }



        private void btnJson_Click(object sender, EventArgs e)
        {
            GetJson(false);
        }

        private void btnExit_Click_1(object sender, EventArgs e)
        {
            Close();
        }



        private void btnJsonTranslate_Click(object sender, EventArgs e)
        {
            GetJson(true);

        }

        private void cboBillType_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            switch (cboBillType.Text)
            {
                case "银行账号":
                    txtBillNo.Text = "4000021709200465066";
                    break;
                case "部门":
                    txtBillNo.Text = "B01";
                    break;
                case "凭证":
                    txtBillNo.Text = "23453";
                    break;

            }
        }
    }
}
