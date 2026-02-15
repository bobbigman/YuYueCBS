using ahu.YuYue.CBS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ahu.YuYue.CBS.RunExe
{
    public partial class frmMenu : Form
    {
        public frmMenu()
        {
            InitializeComponent();
        }

        private void tsmi2Match_Click(object sender, EventArgs e)
        {
            if (radTest.Checked == false && radNormal.Checked == false)
            {
                MessageBox.Show("请先选择账套类型。");
                return;
            }

            frm2Match frm2Match1 = new frm2Match();
            frm2Match1.TestMode(radTest.Checked);

            //frm2Match1.ShowDialog();
            frm2Match1.Show();
        }

        private void eRP同步ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (radTest.Checked == false && radNormal.Checked == false)
            {
                MessageBox.Show("请先选择账套类型。");
                return;
            }

            //MessageBox.Show("没实现。");

            frmSync2MiddleTable frmSync2MiddleTable1 = new frmSync2MiddleTable();
            frmSync2MiddleTable1.TestMode(radTest.Checked);
            //frmSync2MiddleTable1.ShowDialog();
            frmSync2MiddleTable1.Show();
        }

        private void radNormal_CheckedChanged(object sender, EventArgs e)
        {
            //TestModeColor();
        }

        public void TestModeColor()
        {
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

        private void radTest_CheckedChanged(object sender, EventArgs e)
        {
            TestModeColor();
        }

        private void frmMenu_Load(object sender, EventArgs e)
        {
            CsPublic2.HaveaRest();

            //Ping();
        }

        private void Ping()
        {
            //------------使用ping类------
            string host = "192.168.7.14";
            Ping ping1 = new Ping();
            PingReply reply = ping1.Send(host); //发送主机名或Ip地址
                                                //StringBuilder sbuilder;
            lblWait.Visible = false;
            CsPublic2.HaveaRest();

            if (reply.Status == IPStatus.Success)
            {
                //sbuilder = new StringBuilder();
                //sbuilder.AppendLine(string.Format("Address: {0} ", reply.Address.ToString()));
                //sbuilder.AppendLine(string.Format("RoundTrip time: {0} ", reply.RoundtripTime));
                //sbuilder.AppendLine(string.Format("Time to live: {0} ", reply.Options.Ttl));
                //sbuilder.AppendLine(string.Format("Don't fragment: {0} ", reply.Options.DontFragment));
                //sbuilder.AppendLine(string.Format("Buffer size: {0} ", reply.Buffer.Length));
                //string strReturn = sbuilder.ToString();
                //Console.WriteLine(strReturn);
                //ok
            }
            else if (reply.Status == IPStatus.TimedOut)
            {
                MessageBox.Show("您还没有登陆VPN，无法正常使用。");
            }
            else
            {
                MessageBox.Show("您还没有登陆VPN，无法正常使用。");
            }

        }

        private void tsmniClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tsmni2k3New_Click(object sender, EventArgs e)
        {
            if (radTest.Checked == false && radNormal.Checked == false)
            {
                MessageBox.Show("请先选择账套类型。");
                return;
            }

            //frmTBGZT2 frmTBGZT2a = new frmTBGZT2();
            //frmTBGZT2a.TestMode(radTest.Checked);
            //frmTBGZT2a.Show();

        }

        private void tsmiCheck_Click(object sender, EventArgs e)
        {
            if (radTest.Checked == false && radNormal.Checked == false)
            {
                MessageBox.Show("请先选择账套类型。");
                return;
            }

            frmCheck frmCheck1 = new frmCheck();
            frmCheck1.TestMode(radTest.Checked);
            frmCheck1.Show();

        }

        //private void 系统运维dataset_datagridview_Click(object sender, EventArgs e)
        //{
        //    frmSightseeing frmSightseeing1 = new frmSightseeing();
        //    frmSightseeing1.TestMode(radTest.Checked);
        //    frmSightseeing1.Show();
        //}

        //private void 系统运维2DynamicObjectTextToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    frmSightseeing2 frmSightseeing1 = new frmSightseeing2();
        //    frmSightseeing1.TestMode(radTest.Checked);
        //    frmSightseeing1.Show();
        //}

        private void 系统运维3DynamicObjectDatagridviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSightseeing3 frmSightseeing1 = new frmSightseeing3();
            frmSightseeing1.TestMode(radTest.Checked);
            frmSightseeing1.Show();

        }

        private void tsmi2LC_Click(object sender, EventArgs e)
        {
            if (radTest.Checked == false && radNormal.Checked == false)
            {
                MessageBox.Show("请先选择账套类型。");
                return;
            }

            //frmSync2Others Form1_Bob = new frmSync2Others();
            //Form1_Bob.TestMode(radTest.Checked);
            ////frmTest1.ShowDialog();
            //Form1_Bob.Show();
        }

    }
}
