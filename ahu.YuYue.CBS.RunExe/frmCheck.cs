using ahu.YuYue.CBS;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.Util;
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

namespace ahu.YuYue.CBS.RunExe
{
    public partial class frmCheck : Form
    {

        //SqlConnection mSqlConnectionK3;
        //SqlDataAdapter mSqlDataAdapter = new SqlDataAdapter();
        K3CloudApiClient mK3CloudApiClient1;  //同步时，再用到。
        DataTable mtblWDTCheckLC;

        public frmCheck()
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

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        //public void TestMode(bool pTestMode)
        //{
        //    if (pTestMode == true)
        //    {
        //        CsPublic2.RadioButtonChecked(radTest);
        //        CsPublic2.RadioButtonNotChecked(radNormal);
        //    }
        //    else
        //    {
        //        CsPublic2.RadioButtonChecked(radNormal);
        //        CsPublic2.RadioButtonNotChecked(radTest);
        //    }
        //}





        private void tsbFilter_Click(object sender, EventArgs e)
        {
            Filter();
        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            //退出时，总报错，想避免。
            if (mtblWDTCheckLC != null)
            {
                mtblWDTCheckLC.DataSet.AcceptChanges();
                bindingSource1.DataSource = null;
            }

            Close();
        }


        private void Filter()
        {
            if (bindingSource1.DataSource == null)
                return;

            if (chkSum.Checked == true)
                return;

            if (mtblWDTCheckLC.Rows.Count == 0)
                return;

            string strFindBillType = cboBillType.Text.ToString();
            string strFilter = "1=1";

            if (strFindBillType != "")
            {
                //TBGZT TBGZT1 = new TBGZT();
                //string strFormId = GetFormIdByBillType(strFindBillType);
                //strFilter += " And FormId ='" + strFormId + "'";
                strFilter += " And FormId ='" + strFindBillType + "'";
            }

            if (chkIgnoreZero.Checked)
            {
                strFilter += " And WDTCount>0 ";
            }

            //写到SQL中去了。
            //string strFDate1 = FDate1.Value.ToString("yyyy-MM-dd");
            //string strFDate2 = FDate2.Value.ToString("yyyy-MM-dd");

            ////“表达式包含不支持的运算符“Between”。”
            ////strFilter += " And FDate between '" + strFDate1 + "' and '" + strFDate2 + "'";
            //strFilter += " And FDate >= '" + strFDate1 + "' and FDate<='" + strFDate2 + "'";

            bindingSource1.Filter = strFilter;
        }

        private void Calculate()
        {
            //DateTime dt1 = FDate1.Value;
            //DateTime dt2 = FDate2.Value;

            ////本功能运行的前提：执行了：ClsSchedule2Count
            //int intMode = 0;
            //if (radNormal.Checked == true)
            //    intMode = RunMode.Normal;
            //else if (radTest.Checked == true)
            //    intMode = RunMode.Test;

            //ClsScheduleCheck ClsScheduleCheck1 = new B.ZP.ClsScheduleCheck();
            //ClsScheduleCheck1.Calculate(null, mK3CloudApiClient1, dt1, dt2, intMode);

        }

        private void RefreshGrid()
        {

            //MessageBox.Show("尚未实现");
            //return;

            string strFDate1 = FDate1.Value.ToString("yyyy-MM-dd");
            string strFDate2 = FDate2.Value.ToString("yyyy-MM-dd");

            string strSQL ;

            //如果是显示所有的，而不是显示每天有多少单据。
            //感觉这种应用场景，应该不多。
            if (chkSum.Checked)            
                strSQL = string.Format(@"
 Select null as  Fdate
,Case FormID
     when 'Stk_InStock' then '采购入库单'
     when 'Pur_MRB' then '采购退料单'
     when 'Sal_OutStock' then '销售出库单'
     when 'Sal_Returnstock' then '销售退货单'
     when 'Stk_TransferDirect' then '直接调拨单'
     when 'Stk_Misdelivery' then '其他出入库单'
     when 'Stk_Miscellaneous' then '其他入库单'
     end as FormID
,Sum(WDTCount) as WDTCount
,Sum(k3Count) as k3Count
,Sum(ErrorCount) as ErrorCount
From tblWDTCheckLC 
Where Fdate between '{0}' and '{1}'
and  WDTCount>0 
Group by FormId ", strFDate1, strFDate2);
            else
                strSQL = string.Format(@"
 Select  Fdate
,Case FormID
     when 'Stk_InStock' then '采购入库单'
     when 'Pur_MRB' then '采购退料单'
     when 'Sal_OutStock' then '销售出库单'
     when 'Sal_Returnstock' then '销售退货单'
     when 'Stk_TransferDirect' then '直接调拨单'
     when 'Stk_Misdelivery' then '其他出库单'
     when 'Stk_Miscellaneous' then '其他入库单'
     end as FormID
,WDTCount
,k3Count
,ErrorCount
From tblWDTCheckLC 
Where Fdate between '{0}' and '{1}'
--and  WDTCount>0 ", strFDate1, strFDate2);           
            
            //string strUDLFileK3 = "";
            //GetUDLFile(ref strUDLFileK3);

            //2023/9/27
            //不加ref ,mSqlDataAdapter 的 insertCommand 为null ,新增后，不能保存。
            //DataSet DataSet1 = clsSQLData.GetDataSet(strUDLFileK3, ref mSqlConnectionK3, strSQL, "tblWDTCheckLC", ref mSqlDataAdapter);

            string strError = "";
            DbServiceTests DbServiceTests1 = new DbServiceTests();
            DynamicObjectCollection doc1 = DbServiceTests1.GetDynamicObject(mK3CloudApiClient1, strSQL, ref strError);
            if (strError.IsNullOrEmptyOrWhiteSpace() == false)
            {
                MessageBox.Show(strError);
                return;
            }

            DataSet DataSet1 = new DataSet();
            mtblWDTCheckLC = DbServiceTests1.GetDataTableByDynamicOjbect(doc1);
            DataSet1.Tables.Add(mtblWDTCheckLC);

            bindingSource1.DataSource = mtblWDTCheckLC;
            //bindingSource1.DataMember = "tblWDTCheckLC";
            bindingSource1.Filter = "";

            if (chkSum.Checked == false && mtblWDTCheckLC.Rows.Count > 0)
                Filter();

            if (dataGridView1.Rows.Count == 0)
                MessageBox.Show("没找到记录。可能需要重算，或者，重新从接口获取。");


        }

        //private void GetUDLFile(ref string pUDLFileK3)
        //{
        //    pUDLFileK3 = "";
        //    if (radTest.Checked == true)
        //    {
        //        pUDLFileK3 = "K3DataBaseTest.udl";
        //    }
        //    else if (radNormal.Checked == true)
        //    {
        //        pUDLFileK3 = "K3DataBaseNormal.Udl";
        //    }
        //}

        private void radTest_CheckedChanged(object sender, EventArgs e)
        {
            if (radTest.Checked == false && radNormal.Checked == false)
                return;

            TestModeColor(radTest.Checked);

            //GetConnection();
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

        //private void GetConnection()
        //{
        //    string strUDLFileK3 = "";
        //    GetUDLFile(ref strUDLFileK3);

        //    if (strUDLFileK3 == "")
        //        return;

        //    mSqlConnectionK3 = clsSQLData.GetConnection(strUDLFileK3);
        //}

        private void frmCheck_Load(object sender, EventArgs e)
        {
            //不能在界面绑定，要用代码，否则，不能显示。
            dataGridView1.DataSource = bindingSource1;
            dataGridView1.ReadOnly = true;
            dataGridView1.AutoGenerateColumns = false;

            FDate1.Value = System.DateTime.Today.AddDays(-5);
            FDate2.Value = System.DateTime.Today.AddDays(-1);

            if (radNormal.Checked == false && radTest.Checked == false)
                return;

            GeteWebClient();
            //GetConnection();

            //Load 里调用，会出错，不知道什么原因。
            //CatchData();
            //dataGridView1.DataSource = bindingSource1;
        }

        private void GeteWebClient(int pRunMode)
        {
            Struct_K3LoginInfo Struct_K3LoginInfo1 = new Struct_K3LoginInfo();
            //有时，切换帐套。
            //if (mK3CloudApiClient1 != null)
            //    return;

            mK3CloudApiClient1 = CsPublicOA.CreateK3CloudApiClient(ref  Struct_K3LoginInfo1, pRunMode);
            if (mK3CloudApiClient1 == null)
            {
                MessageBox.Show("登陆金蝶失败了。");
                return;
            }
        }

        private void GeteWebClient()
        {
            if (radTest.Checked == false && radNormal.Checked == false)
                return;

            int intMode = 0;
            if (radNormal.Checked == true)
                intMode = K3DatabaseMode.Normal;
            else if (radTest.Checked == true)
                intMode = K3DatabaseMode.Test;

            GeteWebClient(intMode);
        }

        private void cboBillType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Filter();
        }

        private void chkIgnoreZero_CheckedChanged(object sender, EventArgs e)
        {
            Filter();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void chkSum_CheckedChanged(object sender, EventArgs e)
        {
            tsbRefresh_Click(null, null);
        }

        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            lblWait.Visible = true;
            CsPublic2.HaveaRest();

            RefreshGrid();

            if (chkSum.Checked == false)
                Filter();

            lblWait.Visible = false;

        }

        private void FDate1_ValueChanged(object sender, EventArgs e)
        {
            //tsbRefresh_Click(null, null);
        }

        private void FDate2_ValueChanged(object sender, EventArgs e)
        {
            //tsbRefresh_Click(null, null);
        }

        private void cbRecalculate_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void tbCalculate_Click(object sender, EventArgs e)
        {
            lblWait.Visible = true;
            CsPublic2.HaveaRest();

            Calculate();
            lblWait.Visible = false;
        }

        private void tbCatchCountByDate_Click(object sender, EventArgs e)
        {

            lblWait.Visible = true;
            CsPublic2.HaveaRest();

            string strFDate1 = FDate1.Value.ToString("yyyy-MM-dd");
            string strFDate2 = FDate2.Value.ToString("yyyy-MM-dd");

            int intRunMode = 0;
            if (radNormal.Checked == true)
                intRunMode = K3DatabaseMode.Normal;
            else
                intRunMode = K3DatabaseMode.Test;

            string strFindBillType = cboBillType.Text.ToString();
            int intBillType =CsPublic2.GetBillTypeID(strFindBillType);


            Struct_K3LoginInfo Struct_K3LoginInfo1 = new Struct_K3LoginInfo();
            ClsScheduleReadOthers ClsScheduleReadOthers1 = new ClsScheduleReadOthers();
            string strGetDllLastWriteTime = System.IO.File.GetLastWriteTime(this.GetType().Assembly.Location).ToString();
            string strReturn = ClsScheduleReadOthers1.ReadAnd2MiddleTableOrK3
                (null, mK3CloudApiClient1,ref Struct_K3LoginInfo1
                , WDT2.OnlyCount, strFDate1, strFDate2, "", intBillType, "", intRunMode
                , true);

            lblWait.Visible = false;

        }
    }
}
