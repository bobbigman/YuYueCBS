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
    public partial class frm2Match : Form
    {

        DataTable dt2Match;
        K3CloudApiClient mK3CloudApiClient1;

        public frm2Match()
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


        private string GetTopValueByAPI(string pSQL)
        {
            string strError = "";
            DbServiceTests DbServiceTests1 = new DbServiceTests();
            string strReturn = DbServiceTests1.ExecuteScalar(mK3CloudApiClient1, pSQL, ref strError);
            if (strError.IsNullOrEmptyOrWhiteSpace() == false)
            {
                throw new Exception(strError);
            }

            if (strReturn == null)
                strReturn = "";

            return strReturn;
        }
        private string GetTopValue(string pSQL)
        {
            string strReturn = GetTopValueByAPI(pSQL);

            return strReturn;
        }

        private void tsbNew_Click(object sender, EventArgs e)
        {

            string strBillType = cboBillType.Text;
            if (strBillType == "")
            {
                MessageBox.Show("请先择单据类型。");
                return;
            }

            if (dataGridView1.Rows.Count == 0)
            {
                bindingSource1.AddNew();
                return;
            }

            string strFormId = dataGridView1.Rows[0].Cells[1].Value.ToString();
            string strNodeIndex = dataGridView1.Rows[0].Cells[2].Value.ToString();

            //有时，是上次新增，但还没有点保存。
            string strIDLast = dataGridView1.Rows[bindingSource1.Count - 1].Cells[0].Value.ToString();
            int intIdLast = Convert.ToInt16(strIDLast);


            string strSQL = "Select Max(ID) From tblMatch WHERE FormId='" + strFormId + "'";
            string strID = GetTopValue(strSQL);

            int intId = Convert.ToInt16(strID);
            if (intIdLast > intId)
                intId = intIdLast;

            //默认递增3。
            intId += 3;

            //判断一下，不要重复了。
            strID = Convert.ToString(intId);
            while (true)
            {
                strSQL = "Select top 1 1  From tblMatch where ID=" + strID;
                string strValue = GetTopValue(strSQL);
                if (strValue == "")
                    break;  //没有重复，ok, 退出算了。

                intId = Convert.ToInt16(strID) + 3;
                strID = Convert.ToString(intId);


            }

            bindingSource1.AddNew();

            dataGridView1.Rows[bindingSource1.Count - 1].Cells[0].Value = strID;
            dataGridView1.Rows[bindingSource1.Count - 1].Cells[1].Value = strFormId;
            dataGridView1.Rows[bindingSource1.Count - 1].Cells[2].Value = strNodeIndex;
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            string strBillType = cboBillType.Text;
            if (strBillType == "")
            {
                MessageBox.Show("请先择单据类型。");
                return;
            }
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            lblWait.Visible = true;
            for (int i = 1; i <= 10; i++)
            { Application.DoEvents(); }

            //if (radTest.Checked == true)
            //    mSqlDataAdapter.Update(dt2Match);
            //else
            //    SaveByAPI();

            SaveByAPI();

            lblWait.Visible = false;

        }


        private void SaveByAPI()
        {
            // 假设你的 DataSet 名为 dataSet  
            if (dt2Match.DataSet.HasChanges())
            {

                SaveChangesToDatabase();

            }
            else
                MessageBox.Show("都保存过啦，不需要重复执行了。");
        }

        private void SaveChangesToDatabase()
        {
            // 获取绑定的 DataTable  

            if (dt2Match == null)
                return;

            //读取不出来，不读了。
            //先全部删除 ，再写入。
            //List<DataRow> delRows = dt2Match.Rows.Cast<DataRow>().Where(row => row.RowState == DataRowState.Modified || row.RowState == DataRowState.Deleted).ToList();
            //List<string> valuesList = new List<string>();
            //foreach (DataRow row in delRows)
            //{   //不能通过已删除的行访问该行的信息。”
            //    valuesList.Add(row["Id"].ToString());
            //}
            //string strDelIds = string.Join(", ", valuesList);

            string strFormId = dataGridView1.Rows[0].Cells[1].Value.ToString();

            // 获取列名列表  
            List<string> columnNames = new List<string>(); //每1行，哪些列有输入值，可能都不同。
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                columnNames.Add(col.Name);
            }

            //读取所有行，但删除除外,修改的，也不要读，删除算了。
            List<DataRow> RowExist = dt2Match.Rows.Cast<DataRow>().Where(row => row.RowState == DataRowState.Unchanged).ToList();

            List<string> valuesList = new List<string>();
            foreach (DataRow row in RowExist)
            {
                string strFormId1 = row["FormId"].ToString();
                if (strFormId1.Equals(strFormId) == false)
                    continue;

                string strId1 = row["Id"].ToString();
                valuesList.Add(strId1);
            }

            string strIdsExist = string.Join(", ", valuesList);

            string strDelSQL = "";
            if (strIdsExist.IsNullOrEmptyOrWhiteSpace() == false)
                strDelSQL = string.Format(@"Delete tblMatch Where FormId='{0}' and id not in ({1})", strFormId, strIdsExist);

            //读取修改行。
            List<DataRow> RowChanged = dt2Match.Rows.Cast<DataRow>().Where(row => row.RowState == DataRowState.Added || row.RowState == DataRowState.Modified).ToList();

            string strValueAllRows = "";
            foreach (DataRow row in RowChanged)
            {
                string strFormId1 = row["FormId"].ToString();
                if (strFormId1.Equals(strFormId) == false)
                    continue;

                valuesList = new List<string>();

                foreach (DataColumn col in row.Table.Columns)
                {
                    string strValue = row[col].ToString();
                    if (strValue == "")
                    {
                        switch (col.ColumnName)
                        {
                            case "FormID":
                            case "NodeIndex":
                            case "MatchField":
                            case "K3Caption":
                            case "K3Field":
                            case "memo":
                            case "DefaultValue":
                            case "Formula":
                                valuesList.Add("''");
                                break;
                            case "AllowNull":
                                valuesList.Add("1");
                                break;
                            default:
                                valuesList.Add("0");
                                break;
                        }

                        //没用，判断的，都是string
                        //if (col.DataType == typeof(string))
                        //{
                        //    valuesList.Add("''");

                        //}
                        //else if (col.DataType == typeof(int) || col.DataType == typeof(long) || col.DataType == typeof(decimal))
                        //{
                        //    // 对于数值类型，传递0  
                        //    valuesList.Add("0");
                        //}
                    }
                    else
                    {
                        // 正常情况，直接添加参数  
                        if (strValue == "False")
                            strValue = "0";
                        if (strValue == "True")
                            strValue = "1";

                        valuesList.Add(string.Format("'{0}'", strValue)); // 对于非null值，添加括号和值 
                    }
                }

                //去掉最后面的，
                string strValue1Row = string.Join(", ", valuesList);

                strValueAllRows += "(" + strValue1Row + ")," + Environment.NewLine;
            }

            string strNewSQL = "";
            if (strValueAllRows != "")
            {
                strValueAllRows = strValueAllRows.Substring(0, strValueAllRows.Length - 3);
                strNewSQL = "INSERT INTO tblMatch (" + string.Join(", ", columnNames) + ") VALUES " + Environment.NewLine + strValueAllRows;
            }

            string strSQL = strDelSQL + Environment.NewLine + strNewSQL;

            string strError = "";
            DbServiceTests DbServiceTests1 = new DbServiceTests();
            DbServiceTests1.Execute(mK3CloudApiClient1, strSQL, ref strError);
            if (strError.IsNullOrEmptyOrWhiteSpace() == false)
            {
                MessageBox.Show(strError);
                return;
            }

            dt2Match.DataSet.AcceptChanges();


        }

        private void tsbFilter_Click(object sender, EventArgs e)
        {
            Filter();
        }

        private void tsbLookSQL_Click(object sender, EventArgs e)
        {
            //不需要，来源是接口，又不是金蝶，要从后台取。2024/3/19 15:35 于博罗龙光天熙花园。
            LookSQL();
        }
        private void LookFormula()
        {
            String strBillType = cboBillType.Text;
            if (strBillType == "")
            {
                MessageBox.Show("请选择单据类型。");
                return;
            }

            DataGridViewCellCollection dgvcc1 = dataGridView1.CurrentRow.Cells;
            string strBobFormula = "";

            int intColIdIndex = 0;

            string strFormId = CsPublicOA.GetFormIdByBillType(strBillType);

            string strId_Match = dgvcc1[intColIdIndex].Value.ToString();

            string strSQL = string.Format(@"
Select Top 1 Formula
from tblMatch
WHERE  FormId = '{0}' 
And    Id={1}", strFormId, strId_Match);

            strBobFormula = CsData.GetTopValue(null, mK3CloudApiClient1, strSQL);

            frmUpdateSQL frmUpdateSQL1 = new frmUpdateSQL();
            frmUpdateSQL1.SetSQLText(mK3CloudApiClient1, strFormId,"", strId_Match
                , strBobFormula,false,true,"设定公式");
            frmUpdateSQL1.ShowDialog();
        }

        private void LookSQL()
        {
            String strBillType = cboBillType.Text;
            if (strBillType == "")
            {
                MessageBox.Show("请选择单据类型。");
                return;
            }

            DataGridViewCellCollection dgvcc1 = dataGridView1.CurrentRow.Cells;
            string strBobSql = "";

            int intColNodeIndex = 2;

            string strFormId = CsPublicOA.GetFormIdByBillType(strBillType);

            string strNodeIndex = dgvcc1[intColNodeIndex].Value.ToString();

            string strSQL = string.Format(@"
Select Top 1 BobSQL,k3TableName
from tblNode
WHERE  FormId = '{0}' 
And  NodeIndex={1}", strFormId, strNodeIndex);

            DynamicObjectCollection doc1 =CsData.GetDynamicObjects(null, mK3CloudApiClient1, strSQL);
            DynamicObject do1 = doc1[0];

            strBobSql = CsData.GetTopValue(null, mK3CloudApiClient1, strSQL);

            frmUpdateSQL frmUpdateSQL1 = new frmUpdateSQL();
            frmUpdateSQL1.SetSQLText(mK3CloudApiClient1, strFormId, strNodeIndex, ""
                , strBobSql, true, false,"设定SQL");
            frmUpdateSQL1.ShowDialog();
        }
        private void tsbRefresh_Click(object sender, EventArgs e)
        {

            RefreshGrid(true);
        }
        private void tsbSQL2Normal_Click(object sender, EventArgs e)
        {

        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            bindingSource1.DataSource = null;

            if (dt2Match.DataSet.HasChanges() == true)
            {
                if (MessageBox.Show("数据已更改，终止退出。至少，先点保存按钮，是吗？", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    return;
                }
            }

            Close();
        }


        private void Filter()
        {
            //string strFindBillType = cboBillType.Text.ToString();
            //string strK3Caption = txtK3Caption.Text.ToString();
            //string strK3Field = txtK3Field.Text.ToString();
            //string strNodeIndex = cboNodeIndex.Text.ToString();

            //if (strFindBillType == "" && strK3Caption == "" && strK3Field == "" && strNodeIndex == "")
            //{
            //    bindingSource1.Filter = "";
            //    return;
            //}

            //string strFilter = "";
            //if (strFindBillType != "")
            //{
            //    TBGZT TBGZT1 = new TBGZT();
            //    string strFormId = CsPublicOA.GetFormIdByBillType(strFindBillType);
            //    strFilter = " FormId ='" + strFormId + "'";
            //}

            //if (strK3Caption != "")
            //{
            //    if (strFilter != "")
            //        strFilter += " and ";
            //    strFilter += " K3Caption like '%" + strK3Caption + "%'";
            //}

            //if (strK3Field != "")
            //{
            //    if (strFilter != "")
            //        strFilter += " and ";
            //    strFilter += " K3Field like '%" + strK3Field + "%'";
            //}

            //if (strNodeIndex != "")
            //{
            //    if (strFilter != "")
            //        strFilter += " and ";
            //    strFilter += " NodeIndex='" + strNodeIndex + "'";
            //}

            //bindingSource1.Filter = strFilter;
        }



        private void GeteWebClient(int pRunMode)
        {
            //有时，切换帐套。
            //if (mK3CloudApiClient1 != null)
            //    return;

            Struct_K3LoginInfo Struct_K3LoginInfo1 = new Struct_K3LoginInfo();

            mK3CloudApiClient1 = CsPublicOA.CreateK3CloudApiClient(ref Struct_K3LoginInfo1, pRunMode);
            if (mK3CloudApiClient1 == null)
            {
                //不要反复提醒。
                // MessageBox.Show("登陆金蝶失败了。");
                return;
            }
        }

        private void GeteWebClient()
        {
            if (radTest.Checked == false && radNormal.Checked == false)
                return;

            if (radTest.Checked == true)
                GeteWebClient(K3DatabaseMode.Test);
            else if (radNormal.Checked == true)
                GeteWebClient(K3DatabaseMode.Normal);
        }

        private void RefreshGridByK3API()
        {
            if (mK3CloudApiClient1 == null)
                GeteWebClient();

            if (mK3CloudApiClient1 == null)
            {
                MessageBox.Show("登陆金蝶失败了。");
                return;
            }

            string strSQL = "Select * from tblMatch order by Id ";
            string strError = "";
            DbServiceTests DbServiceTests1 = new DbServiceTests();

            //strSQL = "Select 1 as abc ";
            //DbServiceTests1.ExecuteScalar(mK3CloudApiClient1, strSQL,ref strError);

            //DataSet DataSet1 = DbServiceTests1.GetDataSet(mK3CloudApiClient1, strSQL, ref strError);

            DataSet DataSet1 = new DataSet();
            DynamicObjectCollection doc1 = DbServiceTests1.GetDynamicObject(mK3CloudApiClient1, strSQL, ref strError);
            if (strError.IsNullOrEmptyOrWhiteSpace() == false)
                throw new Exception(strError);

            dt2Match = DbServiceTests1.GetDataTableByDynamicOjbect(doc1);
            DataSet1.Tables.Add(dt2Match);

            if (strError.IsNullOrEmptyOrWhiteSpace() == false)
            {
                MessageBox.Show(strError);
                return;
            }
            dt2Match = DataSet1.Tables[0];
            bindingSource1.DataSource = dt2Match;

            //bindingSource1.Filter过滤,不起作用。
            //另外，检查，数据源是否改了，也要改。
            //bindingSource1.DataSource = doc1;

            dt2Match.DataSet.AcceptChanges();//不希望，在判断dataSet.HasChanges()时为空。实际没动过。

            foreach (DataGridViewColumn DataGridViewColumn1 in dataGridView1.Columns)
            {
                string strDataPropertyName = DataGridViewColumn1.DataPropertyName;
                switch (strDataPropertyName)
                {
                    case "ID":
                    case "FormID":
                        DataGridViewColumn1.ReadOnly = true;
                        break;

                }

                switch (DataGridViewColumn1.DataPropertyName)
                {
                    case "AllowNull":
                    case "NumberField":
                    case "RemoveLast0":
                    case "Calculate":
                    case "IsForbid":
                    case "FormID_Basic":
                    case "IsDate":
                    case "FAuxPropId":
                    case "ForbidField":

                        DataGridViewColumn1.ValueType = typeof(bool); // 设置值类型为bool  
                        DataGridViewColumn1.DefaultCellStyle.Format = "N"; // 格式化为True/False  
                        DataGridViewColumn1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // 对齐方式  
                        break;


                }

            }




        }
        private void RefreshGrid(bool pShowError)
        {
            RefreshGridByK3API();
        }

        private void radTest_CheckedChanged(object sender, EventArgs e)
        {
            if (radTest.Checked == false && radNormal.Checked == false)
                return;

            GeteWebClient();

            TestModeColor();

            RefreshGrid(false);
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

        private void frm2Match_Load(object sender, EventArgs e)
        {
            if (Environment.MachineName.Equals("GREEN") == false)
            {
                radNormal.Visible = false;
            }

            //不能在界面绑定，要用代码，否则，不能显示。
            dataGridView1.DataSource = bindingSource1;


            if (radNormal.Checked == false && radTest.Checked == false)
                return;

            RefreshGrid(false);
        }

        private void cboBillType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Filter();

        }

        private void txtK3Caption_TextChanged(object sender, EventArgs e)
        {
            Filter();

        }

        private void txtK3Field_TextChanged(object sender, EventArgs e)
        {
            Filter();

        }

        private void cboNodeIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            Filter();

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }


        private void Match2Tests(string pFormId, bool p2Test)
        {
            if (radTest.Checked == true && p2Test == true)
            {
                MessageBox.Show("当前是测试系统，不能更新到自己呢。");
                return;
            }
            else if (radNormal.Checked == true && p2Test == false)
            {
                MessageBox.Show("当前是正式系统，不能更新到自己呢。");
                return;
            }

            string strDestinationExp = "";
            if (p2Test == true)
            {
                strDestinationExp = "测试系统";
            }
            else
            {
                strDestinationExp = "正试系统";
            }


            DialogResult result = MessageBox.Show(strDestinationExp + "系统数据将被覆盖，确定吗？", "确认覆盖", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.No)
            {
                MessageBox.Show("操作终止。", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Struct_K3LoginInfo Struct_K3LoginInfo1 = new Struct_K3LoginInfo();

            K3CloudApiClient K3CloudApiClientTest = CsPublicOA.CreateK3CloudApiClient(ref Struct_K3LoginInfo1, K3DatabaseMode.Test);
            if (K3CloudApiClientTest == null)
            {
                MessageBox.Show("测试账套，登陆失败了。");
                return;
            }

            K3CloudApiClient K3CloudApiClientNormal = CsPublicOA.CreateK3CloudApiClient(ref Struct_K3LoginInfo1, K3DatabaseMode.Normal);
            if (K3CloudApiClientNormal == null)
            {
                MessageBox.Show("正式账套，登陆失败了。");
                return;
            }


            lblWait.Visible = true;
            for (int i = 1; i <= 10; i++)
            { Application.DoEvents(); }

            //这里要改写，判断备份，目标数据库。传web client过去吧。
            if (p2Test == true)
            {
                BackupMatch(K3CloudApiClientTest, false);
                strDestinationExp = "测试系统";
            }
            else
            {
                BackupMatch(K3CloudApiClientNormal, false);
                strDestinationExp = "正试系统";
            }

            string strSQL = " SELECT DB_NAME() AS CurrentDatabaseName ";
            DbServiceTests DbServiceTests1 = new DbServiceTests();
            string strError = "";
            string strDatabaseName_Test = DbServiceTests1.ExecuteScalar(K3CloudApiClientTest, strSQL, ref strError);
            if (strError != "")
            {
                MessageBox.Show(strError, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string strDatabaseName_Normal = DbServiceTests1.ExecuteScalar(K3CloudApiClientNormal, strSQL, ref strError);
            if (strError != "")
            {
                MessageBox.Show(strError, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string strDatabaseName_Source = "";
            string strDatabaseName_Destination = "";

            if (p2Test == true)
            {
                strDatabaseName_Source = strDatabaseName_Normal;
                strDatabaseName_Destination = strDatabaseName_Test;
            }
            else
            {
                //同步到正式
                strDatabaseName_Source = strDatabaseName_Test;
                strDatabaseName_Destination = strDatabaseName_Normal;
            }

            StringBuilder sbSQL = new StringBuilder();
            strSQL = string.Format("use {0}", strDatabaseName_Destination);
            sbSQL.AppendLine(strSQL);
            strSQL = string.Format("delete {0}.dbo.tblMatch", strDatabaseName_Destination);
            sbSQL.AppendLine(strSQL);
            if (pFormId != "")
            {
                strSQL = string.Format(" where formId='{0}'", pFormId);
                sbSQL.AppendLine(strSQL);
            }

            strSQL = string.Format("insert into {0}.dbo.tblMatch Select * from {1}.dbo.tblMatch", strDatabaseName_Destination, strDatabaseName_Source);
            sbSQL.AppendLine(strSQL);
            if (pFormId != "")
            {
                strSQL = string.Format(" where formId='{0}'", pFormId);
                sbSQL.AppendLine(strSQL);
            }

            strSQL = sbSQL.ToString();

            int intAffected = DbServiceTests1.Execute(K3CloudApiClientTest, strSQL, ref strError);
            if (strError != "")
            {
                MessageBox.Show(strError, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string strInfo = string.Format(@"成功执行，影响了 {0} 条记录", intAffected) + Environment.NewLine + System.DateTime.Now;
            MessageBox.Show(strInfo, "", MessageBoxButtons.OK, MessageBoxIcon.Information);

            lblWait.Visible = false;
        }
        private void tsbBackup_Click(object sender, EventArgs e)
        {


            lblWait.Visible = true;
            for (int i = 1; i <= 10; i++)
            { Application.DoEvents(); }

            GeteWebClient();
            BackupMatch(mK3CloudApiClient1, true);

            lblWait.Visible = false;
        }


        private void BackupMatch(K3CloudApiClient pK3CloudApiClient, bool pShowDialog)
        {


            //判断是否已经存在了。

            //string strSQL =" SELECT DB_NAME() AS CurrentDatabaseName "; 


            DbServiceTests DbServiceTests1 = new DbServiceTests();
            string strError = "";

            string strTime = System.DateTime.Now.ToString("yyyyMMddHH");
            string strNewTable = "tblMatch_" + strTime;

            string strSQL = "SELECT TOP 1 1  FROM sys.tables WHERE   name = '" + strNewTable + "'";
            string strReturn = DbServiceTests1.ExecuteScalar(pK3CloudApiClient, strSQL, ref strError);
            if (strError != "")
            {
                MessageBox.Show(strError, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string strInfo = "";
            if (strReturn == "1" && pShowDialog == true)
            {
                strInfo = string.Format(@"之前备分了，表名:{0}", strNewTable);
                MessageBox.Show(strInfo, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if (strReturn == "1" && pShowDialog == false)
            {
                return;
            }

            strSQL = "Select * into " + strNewTable + " from  tblMatch ";
            int intAffected = DbServiceTests1.Execute(pK3CloudApiClient, strSQL, ref strError);
            if (strError != "")
            {
                MessageBox.Show(strError, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;

            }

            strInfo = string.Format(@"字段对照表备份成功啦，共 {0} 条记录", intAffected) + Environment.NewLine + System.DateTime.Now;
            if (pShowDialog == true)
                MessageBox.Show(strInfo, "", MessageBoxButtons.OK, MessageBoxIcon.Information);


        }



        private void 同步字段表到测试1个单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strFormId = dataGridView1.Rows[0].Cells[1].Value.ToString();
            Match2Tests(strFormId, true);
        }

        private void 还原字段表到测试所有单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Match2Tests("", true);
        }
        private void tsmi2Normal1_Click(object sender, EventArgs e)
        {
            string strFormId = dataGridView1.Rows[0].Cells[1].Value.ToString();
            Match2Tests(strFormId, false);

        }

        private void 到正式所有单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Match2Tests("", false);
        }

        private void radNormal_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void tsbFormula_Click(object sender, EventArgs e)
        {
            LookFormula();
        }
    }
}
