
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.Util;
using Kingdee.BOS.WebApi.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ahu.YuYue.CBS.RunExe
{
    public partial class frmSightseeing3 : Form
    {

        K3CloudApiClient mK3CloudApiClient = new K3CloudApiClient();

        public frmSightseeing3()
        {
            InitializeComponent();
        }

        private void frmSightseeing3_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = bindingSource1;

            GetK3CloudApiClient();

            DbServiceTests DbServiceTests1 = new DbServiceTests();
            string strSQL = "SELECT  name AS TableName  FROM sys.tables  WHERE   name LIKE 'tbl%' ORDER BY  name; ";
            string strError = "";
            DynamicObjectCollection doc1 = DbServiceTests1.GetDynamicObject(mK3CloudApiClient, strSQL, ref strError);
            foreach(DynamicObject do1 in doc1)
            {
                string strTableName = do1[0].ToString();
                comboBox1.Items.Add(strTableName);
            }
            comboBox1.Items.Add("T_CN_BANKCASHFLOW");
            comboBox1.Items.Add("T_WB_RECEIPT");
            comboBox1.Items.Add("删除交易明细(全)");
            comboBox1.Items.Add("删除交易明细(简)");


        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void GetK3CloudApiClient()
        {
            int intMode = 0;
            if (radNormal.Checked == true)
                intMode = K3DatabaseMode.Normal;
            else if (radTest.Checked == true)
                intMode = K3DatabaseMode.Test;

            Struct_K3LoginInfo Struct_K3LoginInfo1 = new Struct_K3LoginInfo();
            mK3CloudApiClient = CsPublicOA.CreateK3CloudApiClient(ref Struct_K3LoginInfo1, intMode);
            if (mK3CloudApiClient == null)
            {
                lblWait.Visible = false;
                MessageBox.Show("登陆金蝶失败了。");
                return;
            }
        }

        private void GetDataAndFill()
        {
            tsbRunSQL.Enabled = false;
            lblWait2.Visible = true;
            for (int i = 1; i <= 10; i++)
            { Application.DoEvents(); }

            GetK3CloudApiClient();

            string strSQL = rtbSQL.Text.Trim();
            if (strSQL == "")
            {
                lblWait2.Visible = false;
                tsbRunSQL.Enabled = true;
                MessageBox.Show("请输入SQL。");
                return;
            }

            //替换后，还会报错。
            //strSQL = strSQL.Replace("/*dialect*/", "--不用加：*dialect*，后面会加的。如果加了，不会返回任何记录。");
            strSQL = strSQL.Replace("/*dialect*/", "");
            rtbSQL.Text = strSQL;

            DbServiceTests DbServiceTests1 = new DbServiceTests();
            int intSelect = strSQL.ToLower().IndexOf("select");
            int intWith = strSQL.ToLower().IndexOf("with");

            //if (intSelect > -1 || (intWith > -1 && rbGrid.Checked == true))
            if (rbGrid.Checked || rbText.Checked)
            {
                DynamicObjectCollection doc1;
                try
                {
                    string strError = "";
                    doc1 = DbServiceTests1.GetDynamicObject(mK3CloudApiClient, strSQL, ref strError);
                    if (strError.IsNullOrEmptyOrWhiteSpace() == false)
                    {
                        rtbResult.Visible = true;
                        dataGridView1.Visible = false;
                        rtbResult.Text = strError;
                        lblWait2.Visible = false;
                        tsbRunSQL.Enabled = true;
                        return;
                    }

                    if (doc1.Count == 0)
                    {
                        rtbResult.Visible = true;
                        dataGridView1.Visible = false;
                        rtbResult.Text = "没找到记录。" + Environment.NewLine + System.DateTime.Now;
                        lblWait2.Visible = false;
                        tsbRunSQL.Enabled = true;
                        return;
                    }

                    if (rbGrid.Checked == true)
                    {
                        rtbResult.Visible = false;
                        dataGridView1.Visible = true;

                        //DataTable dataTable1 = DbServiceTests1.GetDataTableByDynamicOjbect(doc1);
                        //bindingSource1.DataSource = dataTable1;
                        bindingSource1.DataSource = doc1;
                    }
                    else
                    {
                        rtbResult.Visible = true;
                        dataGridView1.Visible = false;
                        FillText(doc1);

                    }
                }
                catch (Exception ex)
                {
                    rtbResult.Text = ex.ToString();
                }
            }
            else
            {

                rtbResult.Visible = true;
                dataGridView1.Visible = false;
                int intAffected = 0;
                try
                {
                    string strError = "";
                    intAffected = DbServiceTests1.Execute(mK3CloudApiClient, strSQL, ref strError);
                    if (strError.IsNullOrEmptyOrWhiteSpace() == false)
                        rtbResult.Text = strError;
                    else
                    {
                        string strInfo = string.Format(@"成功执行，影响了 {0} 条记录", intAffected) + Environment.NewLine + System.DateTime.Now;
                        rtbResult.Text = strInfo;
                    }
                }
                catch (Exception EX)
                {
                    rtbResult.Text = EX.ToString();
                }
            }

            lblWait2.Visible = false;
            tsbRunSQL.Enabled = true;
        }

        private void FillText(DynamicObjectCollection pDOC)
        {
            bool bolFirst = true;
            string strReturnShow;
            string strReturnLines;
            string strField1;
            string strFieldValue1;
            string strFields = "";
            foreach (DynamicObject do1 in pDOC)
            {
                var props = do1.DynamicObjectType.Properties;
                //字段头,只写一次。
                if (bolFirst == true)
                {
                    bolFirst = false;
                    foreach (var prop in props)
                    {
                        strField1 = prop.Name;
                        strFields = strFields + strField1 + " | ";
                    }
                }

                //换行，准备写明细了。
                strFields = strFields + Environment.NewLine;
                foreach (var prop in props)
                {
                    strField1 = prop.Name;
                    strFieldValue1 = "Null";
                    if (ObjectUtils.IsNullOrEmpty(do1[strField1]) == false)
                        strFieldValue1 = do1[strField1].ToString();

                    strFields = strFields + strFieldValue1 + " | ";
                }
                strFields = strFields + Environment.NewLine;
            }
            strReturnLines = strFields;
            strReturnShow = strReturnLines;
            rtbResult.Text = strReturnShow;
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strBillType = comboBox1.Text;

            if (strBillType == "")
                return;

            if (strBillType == "删除交易明细(简)")
            {
                rtbSQL.Text = string.Format(@"
delete d
from T_CN_BANKCASHFLOW_O d inner join
     T_CN_BANKCASHFLOW s on d.fid=s.fid
where s.FBILLNO = '2024071501417297'  

delete d
from   T_CN_BANKCASHFLOW d
where FBILLNO = '2024071501417297'  

delete FROM  T_WB_RECEIPT  WHERE FDetailNo='DZM202407150000576676_C0246WY001N58LZ'
");
            }
            else if (strBillType == "删除交易明细(全)")
            {
                rtbSQL.Text = string.Format(@"
-truncate table T_WB_RECEIPT  --电子回单
----delete from T_WB_RECEIPTFILE  --没有这个，是写到附件中，不要弄混了。
--delete from T_BAS_ATTACHMENT where FBILLTYPE='WB_ReceiptBill'
--truncate table tblReadMes2 
--truncate  table  tbl2Kingdee 
--truncate  table  T_CN_BANKCASHFLOW
--truncate  table  T_CN_BANKCASHFLOW_O


----只删除电子回单
--truncate table T_WB_RECEIPT  --电子回单
----delete from T_WB_RECEIPTFILE  --没有这个，是写到附件中，不要弄混了。
--delete from T_BAS_ATTACHMENT where FBILLTYPE='WB_ReceiptBill'
--delete from tblReadMes2  where formid='WB_ReceiptBill'
--delete  from tbl2Kingdee where formid='WB_ReceiptBill'

--delete d
--from T_CN_BANKCASHFLOW s inner join
--     T_CN_BANKCASHFLOW_O d on s.FID=d.fid
--where s.FTRANSDATE between '2024/6/1' AND '2024/6/30'

--delete
--from T_CN_BANKCASHFLOW 
--where FTRANSDATE between '2024/6/1' AND '2024/6/30'


--delete from T_CN_BANKCASHFLOW  --where fid=100047 --接收银行交易明细单据
--delete from T_CN_BANKCASHFLOW_O --where fid=100047  --接收银行交易明细单据



delete d
from T_CN_BANKCASHFLOW_O d inner join
     T_CN_BANKCASHFLOW s on d.fid=s.fid
where s.FBILLNO like 'test%'  

delete d
from   T_CN_BANKCASHFLOW d
where FBILLNO like 'test%'  

delete from T_BAS_ATTACHMENT
where FBILLTYPE='WB_ReceiptBill'
AND FINTERID IN (
SELECT  FID FROM T_WB_RECEIPT  WHERE FDetailNo='DZM测试01'
)
delete FROM  T_WB_RECEIPT  WHERE FDetailNo='DZM测试01'
");

            }
            else if (strBillType == "查看表的字段类型")
                rtbSQL.Text = string.Format(@"
SELECT   COLUMN_NAME,   
         DATA_TYPE,   
         CHARACTER_MAXIMUM_LENGTH
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'tblReadMes2' ");
            else if (strBillType == "T_BAS_ATTACHMENT")
                rtbSQL.Text = " Select top 5 * from T_BAS_ATTACHMENT where FBILLTYPE='WB_ReceiptBill'";
            else
            {
                rtbSQL.Text = "Select top 5 * from " + strBillType;
            }

        }

        private void tsbRunSQL_Click(object sender, EventArgs e)
        {
            //lblWait2.Visible = true;

            GetDataAndFill();
            //lblWait2.Visible = false;
        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            bindingSource1.DataSource = null;
            Close();
        }

        private void bindingNavigator1_RefreshItems(object sender, EventArgs e)
        {

        }

        private void radNormal_CheckedChanged(object sender, EventArgs e)
        {

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

        private void splitContainer2_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
