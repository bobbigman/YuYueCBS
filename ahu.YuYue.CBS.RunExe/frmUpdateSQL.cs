using ahu.YuYue.CBS;
using Kingdee.BOS.Util;
using Kingdee.BOS.WebApi.Client;
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
    public partial class frmUpdateSQL : Form
    {

        string mstrFormId;
        string mstrNodeIndex;
        string mstrId_Match;
        //string mstrUDLFileK3;
        //int mintRunMode;
        bool mbolBySQL;
        bool mbolByFormual;

        K3CloudApiClient mK3CloudApiClient1;
        public frmUpdateSQL()
        {
            InitializeComponent();
        }

        private void tsmiRun_Click(object sender, EventArgs e)
        {
            string strSQL = richSQL.Text;
            ToolStripMenuItem ToolStripMenuItem1 = sender as ToolStripMenuItem;
            RunSQL_K3(ToolStripMenuItem1.Text, strSQL);
        }

        public void SetSQLText(K3CloudApiClient pK3CloudApiClient1, string pFormId, string pNodeIndex
            , string pId_Match
            , string pSQL, bool pBySQL, bool pByFormual, string pText)
        {
            mK3CloudApiClient1 = pK3CloudApiClient1;
            mstrNodeIndex = pNodeIndex;
            mstrFormId = pFormId;
            richSQL.Text = pSQL;
            mbolBySQL = pBySQL;
            mbolByFormual = pByFormual;
            mstrId_Match = pId_Match;
            Text = pText;
        }

        private void tsmiUpdateSQL_Click(object sender, EventArgs e)
        {
            btnUpdateSQL();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        //private void GeteWebClient()
        //{
        //    GeteWebClient(mintRunMode);
        //}

        private void RunSQL_K3(string btnCaption, string pSQL)
        {
            string strSQL = pSQL;
            if (strSQL == "")
            {
                MessageBox.Show("没有SQL");
                return;
            }

            strSQL = strSQL.Replace("{fid}", "0");
            strSQL = strSQL.Replace("{FEntryId}", "0");

            //一定要换行，不则，如果最后是注释，--，就会出错。
            strSQL = string.Format(@"
Select Count(*) from (
{0}
) a", strSQL);

            //clsSQLData.ExecuteSQL("K3DataBase.udl", ref mSqlConnection, strSQL);
            //clsSQLData.ExecuteSQL("MidDatabaseTest.udl", ref mSqlConnection, strSQL);
            string strReturn;
            string strTime = System.DateTime.Now.ToString("G");
            try
            {
                //if (mstrUDLFileK3== "K3DataBaseNormal.Udl")
                //{
                string strError = "";
                DbServiceTests DbServiceTests1 = new DbServiceTests();
                strReturn = DbServiceTests1.ExecuteScalar(mK3CloudApiClient1, strSQL, ref strError);
                if (strError.IsNullOrEmptyOrWhiteSpace() == false)
                {
                    richResult.Text = strError;
                    return;
                }
                //}
                //else 
                //{
                //    strReturn = clsSQLData.GetTopValueByUdlFile(mstrUDLFileK3, ref mSqlConnectionK3, strSQL);
                //}

                richResult.Text = strTime + Environment.NewLine + btnCaption + Environment.NewLine + " 成功取到 " + strReturn + " 行数据。";
            }
            catch (Exception ex)
            {
                strReturn = ex.Message.ToString();
                richResult.Text = strTime + Environment.NewLine + btnCaption + Environment.NewLine + strReturn;
            }
        }

        private void btnUpdateSQL()
        {
            if (mbolBySQL == false)
            {
                MessageBox.Show("不是SQL ,要小心，别弄错了。");
                return;
            }

            if (mstrFormId == "")
            {
                MessageBox.Show("mstrFormId 没有值。");
                return;
            }

            if (mstrNodeIndex == "")
            {
                MessageBox.Show("mstrNodeIndex 没有值。");
                return;

            }

            //string strSQL1 = richSQLNew.Text;
            string strSQL1 = richSQL.Text;
            if (strSQL1 == "")
            {
                MessageBox.Show("旧 SQL 没有值。");
                return;
            }

            if (strSQL1.Length < 50)
            {
                MessageBox.Show("旧 SQL 好象不对，字符才 " + strSQL1.Length + " 个。");
                return;
            }


            strSQL1 = strSQL1.Replace("'", "''");
            string strSQL = string.Format(@"
Update tblMatch set bobSQL='{0}'
Where  FormId='{1}'
And    NodeIndex={2}", strSQL1, mstrFormId, mstrNodeIndex);

            int intReturn;
            string strTime = System.DateTime.Now.ToString("G");
            try
            {
                string strError = "";
                DbServiceTests DbServiceTests1 = new DbServiceTests();
                intReturn = DbServiceTests1.Execute(mK3CloudApiClient1, strSQL, ref strError);
                if (strError.IsNullOrEmptyOrWhiteSpace() == false)
                {
                    richResult.Text = strError;
                    return;
                }

                //}
                //else
                //{
                //    intReturn = clsSQLData.ExecuteSQLByUdlFile(mstrUDLFileK3, ref mSqlConnectionK3, strSQL);
                //}

                richResult.Text = strTime + Environment.NewLine + "影响 " + intReturn + " 行。";
            }
            catch (Exception ex)
            {
                string strReturn = ex.Message.ToString();
                richResult.Text = strTime + Environment.NewLine + strReturn;
            }
        }

        private void UpdateFormula()
        {
            if (mbolByFormual == false)
            {
                MessageBox.Show("不是公式 ,要小心，别弄错了。");
                return;
            }

            if (mstrFormId == "")
            {
                MessageBox.Show("mstrFormId 没有值。");
                return;
            }

            if (mstrId_Match == "")
            {
                MessageBox.Show("mstrId_Match 没有值。");
                return;

            }

            string strFormula = richSQL.Text;
            if (strFormula == "")
            {
                if (MessageBox.Show("公式 没有值，继续？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
            }

            if (strFormula.Length < 15 && strFormula!="")
            {
                if (MessageBox.Show("SQL 好象不对，字符才 " + strFormula.Length + " 个，继续？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)

                return;
            }

            strFormula = strFormula.Replace("'", "''");
            string strSQL = string.Format(@"
Update tblMatch set Formula='{0}'
Where  FormId='{1}'
And    Id={2}", strFormula, mstrFormId, mstrId_Match);

            string strTime = System.DateTime.Now.ToString("G");
            try
            {
                int intReturn = CsData.BobExecute(null, mK3CloudApiClient1, strSQL);
                richResult.Text = strTime + Environment.NewLine + "影响 " + intReturn + " 行。";
            }
            catch (Exception ex)
            {
                string strReturn = ex.Message.ToString();
                richResult.Text = strTime + Environment.NewLine + strReturn;
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void tbmiFormula_Click(object sender, EventArgs e)
        {
            UpdateFormula();
        }
    }
}
