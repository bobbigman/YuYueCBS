//using B.CBS2MiddleTable;
//using Kingdee.BOS.Orm.DataEntity;
//using Kingdee.BOS.Util;
//using Kingdee.BOS.WebApi.Client;
//using Newtonsoft.Json.Linq;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Data.OleDb;
//using System.Data.SqlClient;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace B.CBS2MiddleTable.RunExe
//{
//    public partial class frmTBGZT2 : Form
//    {

//        //int mintI;
//        K3CloudApiClient mK3CloudApiClient1;  //同步时，再用到。
//        Struct_K3LoginInfo mStruct_K3LoginInfo1;
//        //string mstrGetDllLastWriteTime;
//        //SqlDataAdapter mSqlDataAdapter = new SqlDataAdapter();
//        DataTable tblReadMes2;
//        //SqlConnection mSqlConnectionK3;
//        public frmTBGZT2()
//        {
//            InitializeComponent();
//        }

//        public void TestMode(bool pTestMode)
//        {
//            if (pTestMode == true)
//            {
//                CsPublic2.RadioButtonChecked(radTest);
//                CsPublic2.RadioButtonNotChecked(radNormal);
//            }
//            else
//            {
//                CsPublic2.RadioButtonChecked(radNormal);
//                CsPublic2.RadioButtonNotChecked(radTest);
//            }
//        }

//        //private void GeteWebClient(int pRunMode)
//        //{
//        //    //有时，切换帐套。
//        //    //if (mK3CloudApiClient1 != null)
//        //    //    return;

//        //    mK3CloudApiClient1 = CsPublic2.CreateWebClient(pRunMode);
//        //    if (mK3CloudApiClient1 == null)
//        //    {
//        //        MessageBox.Show("登陆金蝶失败了。");
//        //        return;
//        //    }
//        //}

//        //private void GeteWebClient()
//        //{
//        //    if (radTest.Checked == false && radNormal.Checked == false)
//        //        return;

//        //    if (radTest.Checked == true)
//        //        GeteWebClient(RunMode.Test);
//        //    else if (radNormal.Checked == true)
//        //        GeteWebClient(RunMode.Normal);
//        //}
//        public void tsbRead_Click(object sender, EventArgs e)
//        {
//            try
//            {

//                string strFindBillType = this.FFindBillType.Text;
//                if (strFindBillType == "")
//                {
//                    MessageBox.Show("请选择，要同步的单据类型。");
//                    return;
//                }

//                string strK3FormID_WebApi = CsPublicOA.GetFormIdByBillType(strFindBillType);

//                string strFDate1 = FDate1.Text;
//                string strFDate2 = FDate2.Text;
//                DateTime dt1, dt2;

//                if (strFDate1 == "")
//                {
//                    MessageBox.Show("请输入开始日期。");
//                    return;
//                }

//                if (strFDate2 == "")
//                {
//                    MessageBox.Show("请结束开始日期。");
//                    return;
//                }

//                DateTime.TryParse(strFDate1, out dt1);
//                DateTime.TryParse(strFDate2, out dt2);

//                if (dt1.ToShortDateString() == DateTime.Now.ToShortDateString())
//                {
//                    MessageBox.Show("开始日期,不能是今天，因为，浪潮，可能还会修改。");
//                    return;
//                }

//                if (dt2.ToShortDateString() == DateTime.Now.ToShortDateString())
//                {
//                    MessageBox.Show("结束日期,不能是今天，因为，浪潮，可能还会修改。");
//                    return;
//                }

//                //strFDate1,不要这种日期格式：2021年12月1日
//                strFDate1 = dt1.ToShortDateString();
//                strFDate2 = dt2.ToShortDateString();

//                int intFindDays = 10;
//                TimeSpan ts = dt1 - dt2;
//                int intDays = ts.Days;//整数天数，1天12小时或者1天20小时结果都是1
//                if (intDays > intFindDays)
//                {
//                    MessageBox.Show("日期范围，不能超过" + intFindDays.ToString() + "天。");
//                }

//                lblWait.Visible = true;

//                string strFBillNO = FFindSheet_no.Text;
//                string strFExec_status = Fexec_status.Text;
//                bool bolFromMES = FFromMES.Checked;

//                switch (strFExec_status)
//                {
//                    case "":
//                        //不过滤，取全部
//                        break;
//                    case "未同步":
//                        strFExec_status = "0";
//                        break;
//                    case "同步成功":
//                        strFExec_status = "1";
//                        break;
//                    case "同步中":
//                        strFExec_status = "8";
//                        break;
//                    case "同步错误":
//                        strFExec_status = "2";
//                        break;
//                    case "手工处理":
//                        strFExec_status = "3";
//                        break;
//                    case "同步成功,审核失败":
//                        strFExec_status = "5";
//                        break;

//                }

//                //mintI = 0;

//                if (bolFromMES == true)
//                {
//                    DateTime dtStartDate = dt1;
//                    DateTime dtEndDate = dt2;

//                    lblWait.Text = "";
//                    lblWait.Visible = true;
//                    CsPublic2.HaveaRest();


//                    while (dtStartDate <= dtEndDate)
//                    {
//                        string strStartDate1 = dtStartDate.ToShortDateString();
//                        DateTime dtEndDate1 = dtStartDate.AddDays(mStruct_K3LoginInfo1.SyncDateStep);

//                        if (dtEndDate1 > dtEndDate)
//                            dtEndDate1 = dtEndDate;

//                        string strEndDate1 = dtEndDate1.ToShortDateString();
//                        Read1DateFromMES(strK3FormID_WebApi, strFExec_status, strStartDate1, strEndDate1, strFBillNO, bolFromMES);
//                        dtStartDate = dtEndDate1.AddDays(1);

//                        lblWait.Text = string.Format("正在查询，请稍候。开始日期：{0},结束日期{1}", strStartDate1, strEndDate1);

//                        CsPublic2.HaveaRest();

//                    }
//                    lblWait.Visible = false;
//                    CsPublic2.HaveaRest();

//                }

//                Read1DateFromMES(strK3FormID_WebApi, strFExec_status, strFDate1, strFDate2, strFBillNO, bolFromMES);

//                lblWait.Visible = false;


//                if (dgv1.Rows.Count == 0)
//                {

//                    MessageBox.Show("当前范围，没找到数据。" + Environment.NewLine +
//                        "请检查过滤条件" + Environment.NewLine +
//                        "或者，先点击按钮：根据条件，从浪潮取数后再同步");
//                }

//            }
//            catch (Exception ex)
//            {
//                string strError = CsErrLog.GetExceptionWithLog(ex);
//                throw new Exception(strError);
//            }

//        }


//        //if (DOC1.Count == 0)
//        //{
//        //    string strReadNull = "";
//        //    strReadNull = "亲，没取到数据啊," + Environment.NewLine + "请扩大日期范围，再试试。";

//        //    if (strFExec_status == "")
//        //        strReadNull = strReadNull + Environment.NewLine + "如还没有，请查询下浪潮，是否没有数据。";

//        //    if (pFindBillType == "1" || pFindBillType == "2")
//        //        strReadNull = strReadNull.Replace("扩大日期范围，", "");

//        //    MessageBox.Show(strReadNull);

//        //}


//        public void Read1DateFromMES(string pK3FormID_WebApi, string pFExec_status
//    , string pFDate1, string pFDate2, string pFBillNO, bool pFromMES)
//        {

//            try
//            {

//                FillBillEntity(pK3FormID_WebApi, pFExec_status, pFDate1, pFDate2, pFBillNO);

//            }
//            catch (Exception ex)
//            {
//                string strError = CsErrLog.GetExceptionWithLog(ex);
//                throw new Exception(strError);
//            }

//        }

//        private bool FillBillEntity(string pK3FormID_WebApi, string pFexec_Status
//    , string pStartDate, string pEndDate, string pFBillNo)
//        {

//            try
//            {
//                //pEndDate = pEndDate.Replace("00:00:00", "23:55:55");
//                pEndDate = pEndDate + " 23:55:55";

//                //万一有删除的记录，还取出来了，怎么办？ThisShow
//                string strSQL = String.Format(@"
//select CONVERT(bit,0 ) as FSelect, FNumber
//,exec_status=case exec_status 
//   when '0' then '未同步'
//   when '1' then '同步成功'
//   when '2' then '同步错误'
//   when '5' then '同步成功,审核失败'
//end
//,K3Result,FDate ,FMiddleID
//  from [tblReadMes2] 
//where 1=1 
// ", pStartDate, pEndDate);

//                //浪潮，不需要按日期同步。
//                //strSQL += Environment.NewLine + string.Format(" Where FDate between '{0}' and '{1}' ", pStartDate, pEndDate);

//                if (pK3FormID_WebApi != "")
//                    strSQL += Environment.NewLine+ string.Format(" AND FormId = '{0}'", pK3FormID_WebApi);

//                if (pFexec_Status != "")
//                    strSQL +=  Environment.NewLine + string.Format(" AND Exec_Status = '{0}'", pFexec_Status);

//                if (pFBillNo != "")
//                    strSQL +=Environment.NewLine+ string.Format(" AND FNumber = '{0}'", pFBillNo);


//                strSQL += "  Order by FNumber ";

//                //DataTable docMes_MidTable = clsSQLData.GetDataTable(mSqlConnectionK3, strSQL);

//                string strError = "";
//                DbServiceTests DbServiceTests1 = new DbServiceTests();
//                DynamicObjectCollection doc1 = DbServiceTests1.GetDynamicObject(mK3CloudApiClient1, strSQL, ref strError);
//                if (strError.IsNullOrEmptyOrWhiteSpace() == false)
//                {
//                    MessageBox.Show(strError);
//                    return false;
//                }

//                tblReadMes2 = DbServiceTests1.GetDataTableByDynamicOjbect(doc1);
//                DataSet DataSet1 = new DataSet();
//                DataSet1.Tables.Add(tblReadMes2);

//                bindingSource1.DataSource = tblReadMes2;
//                DataSet1.AcceptChanges();//不希望，在判断dataSet.HasChanges()时为空。实际没动过。


//            }

//            catch (Exception ex)
//            {
//                string strError = CsErrLog.GetExceptionWithLog(ex);
//                throw new Exception(strError);

//            }

//            return true;


//        }


//        private void tsbGetDataFromInterface_Click(object sender, EventArgs e)
//        {
//            string strFindBillType = this.FFindBillType.Text;

//            if (strFindBillType == "")
//            {
//                MessageBox.Show("请选择，要同步的单据类型。");
//                return;
//            }

//            lblWait.Visible = true;
//            CsPublic2.HaveaRest();

//            RunByBillType();
//            lblWait.Visible = false;
//            return;
//        }

//        private void RunByBillType()
//        {
//            try
//            {
//                string strFindBillType = this.FFindBillType.Text;
//                if (strFindBillType == "")
//                {
//                    MessageBox.Show("请选择，要同步的单据类型。");
//                    return;
//                }

//                string strK3FormID_WebApi = CsPublicOA.GetFormIdByBillType(strFindBillType);

//                string strFDate1 = FDate1.Text;
//                string strFDate2 = FDate2.Text;
//                DateTime dt1, dt2;

//                if (strFDate1 == "")
//                {
//                    MessageBox.Show("请输入开始日期。");
//                    return;
//                }

//                if (strFDate2 == "")
//                {
//                    MessageBox.Show("请结束开始日期。");
//                    return;
//                }

//                DateTime.TryParse(strFDate1, out dt1);
//                DateTime.TryParse(strFDate2, out dt2);

//                if (dt1.ToShortDateString() == DateTime.Now.ToShortDateString())
//                {
//                    MessageBox.Show("开始日期,不能是今天，因为，浪潮，可能还会修改。");
//                    return;
//                }

//                if (dt2.ToShortDateString() == DateTime.Now.ToShortDateString())
//                {
//                    MessageBox.Show("结束日期,不能是今天，因为，浪潮，可能还会修改。");
//                    return;
//                }

//                //strFDate1,不要这种日期格式：2021年12月1日
//                strFDate1 = dt1.ToShortDateString();
//                strFDate2 = dt2.ToShortDateString();

//                int intFindDays = 10;
//                TimeSpan ts = dt1 - dt2;
//                int intDays = ts.Days;//整数天数，1天12小时或者1天20小时结果都是1
//                if (intDays > intFindDays)
//                {
//                    MessageBox.Show("日期范围，不能超过" + intFindDays.ToString() + "天。");
//                }

//                string strBillNo = FFindSheet_no.Text.Trim();

//                string strReturn = Sysnc1(strBillNo, "", strFindBillType, strFDate1, strFDate2);


//            }
//            catch (Exception ex)
//            {
//                string strError = CsErrLog.GetExceptionWithLog(ex);
//                throw new Exception(strError);
//            }
//        }

//        private void tsb2k3_Click(object sender, EventArgs e)
//        {
//            string strReturn1 = "";
//            string strReturns = "";
//            string strFindBillType = this.FFindBillType.Text;

//            bool boolSeleted = false;
//            foreach (DataGridViewRow Row1 in dgv1.Rows)
//            {
//                string strFSelect = Convert.ToString(Row1.Cells["FSelect"].Selected);

//                //再给一次机会，这次，读取：Value
//                if (strFSelect == "False")
//                    strFSelect = Convert.ToString(Row1.Cells["FSelect"].Value);

//                if (strFSelect == "False")
//                    continue;

//                boolSeleted = true;
//                string strFExec_Status2 = Convert.ToString(Row1.Cells["exec_status"].Value);
//                string strFBillNO = Convert.ToString(Row1.Cells["FNumber"].Value);
//                string strFMiddleID = Convert.ToString(Row1.Cells["FMiddleID"].Value);
//                string strFDate = Convert.ToString(Row1.Cells["FDate"].Value);

//                if (strFBillNO == "")
//                {
//                    strReturns = "请选择记录。";
//                    MessageBox.Show(strReturns);
//                    return;
//                }

//                lblWait.Visible = true;
//                for (int i = 1; i <= 10; i++)
//                { Application.DoEvents(); }

//                strReturn1 = Sysnc1(strFBillNO, strFMiddleID, strFindBillType, strFDate, strFDate);
//                Row1.Cells["K3Result"].Value = strReturn1;
//                if (strReturn1 == "" || strReturn1.IndexOf("同步成功") > -1)
//                    Row1.Cells["exec_status"].Value = "同步成功";
//                else
//                    Row1.Cells["exec_status"].Value = "同步错误";
//                strReturns = strReturns + Environment.NewLine + strReturn1;
//                lblWait.Visible = false;

//            }
//            if (boolSeleted == false)
//            {
//                MessageBox.Show("您没有选择记录。");
//                return;
//            }

//            MessageBox.Show(strReturns);

//        }

//        public string Sysnc1(string pFSourceBillNO, string pMiddleID
//            , string pFPTDJLY, string pStartDate, string pEndDate
//           )
//        {
//            try
//            {
//                int intBillType = 0;
//                switch (pFPTDJLY)
//                {
//                    case "会计科目":
//                        intBillType = 1;
//                        break;
//                    case "会计科目维度":
//                        intBillType = 2;
//                        break;
//                    case "部门":
//                        intBillType = 3;
//                        break;
//                    case "其他往来单位":
//                        intBillType = 4;
//                        break;
//                    default:
//                        return "switch中，没有单据类型:" + pFPTDJLY;
//                }

//                int intRunMode = 0;
//                if (radTest.Checked == true)
//                    intRunMode = RunMode.Test;
//                else if (radNormal.Checked == true)
//                    intRunMode = RunMode.Normal;

//                bool bolCountReGetFromInterface = FFromMES.Checked;
//                bool blnIsSchedule = (pFSourceBillNO.IsNullOrEmptyOrWhiteSpace() == false);

//                ClsScheduleReadOthers ClsScheduleReadOthers1 = new ClsScheduleReadOthers();
//                string strReturn = ClsScheduleReadOthers1.Only1BillType(null, mK3CloudApiClient1, mStruct_K3LoginInfo1, WDT2.K3
//                    , pStartDate, pEndDate,"", pFSourceBillNO, intBillType, pMiddleID, intRunMode
//                    , bolCountReGetFromInterface, blnIsSchedule);

//                return strReturn;
//            }
//            catch (Exception ex)
//            {
//                string strError = CsErrLog.GetExceptionWithLog(ex);
//                throw new Exception(strError);
//            }

//        }

//        private void tsbSchedule_Click(object sender, EventArgs e)
//        {
//            lblWait.Visible = true;
//            CsPublic2.HaveaRest();

//            //模拟自动同步，同时模拟：同步到金蝶，读取单据小计。

//            int intRunMode = 0;
//            if (radTest.Checked == true)
//                intRunMode = RunMode.Test;
//            else if (radNormal.Checked == true)
//                intRunMode = RunMode.Normal;

//            bool bolCountReGetFromInterface = FFromMES.Checked;

//            ClsScheduleReadOthers ClsScheduleReadOthers1 = new ClsScheduleReadOthers();
//            string strGetDllLastWriteTime = System.IO.File.GetLastWriteTime(this.GetType().Assembly.Location).ToString();
//            string strReturn = ClsScheduleReadOthers1.ReadAnd2K3(null, mK3CloudApiClient1
//                , mStruct_K3LoginInfo1, WDT2.K3, "", "", "", 0, "", intRunMode, bolCountReGetFromInterface
//                ,true);

//            strReturn += Environment.NewLine;
//            strReturn += ClsScheduleReadOthers1.ReadAnd2K3(null, mK3CloudApiClient1
//                , mStruct_K3LoginInfo1, WDT2.OnlyCount, "", "", "", 0, "", intRunMode, bolCountReGetFromInterface
//                ,true);

//            lblWait.Visible = false;
//            return;
//        }


//        private void tsbSynchronClear_Click(object sender, EventArgs e)
//        {

//            lblWait.Visible = true;
//            CsPublic2.HaveaRest();


//            string strFindBillType = this.FFindBillType.Text;

//            bool boolSeleted = false;
//            foreach (DataGridViewRow Row1 in dgv1.Rows)
//            {
//                string strFSelect = Convert.ToString(Row1.Cells["FSelect"].Selected);

//                //再给一次机会，这次，读取：Value
//                if (strFSelect == "False")
//                    strFSelect = Convert.ToString(Row1.Cells["FSelect"].Value);

//                if (strFSelect == "False")
//                    continue;

//                boolSeleted = true;
//                string strFExec_Status2 = Convert.ToString(Row1.Cells["exec_status"].Value);
//                string strFBillNO = Convert.ToString(Row1.Cells["FNumber"].Value);  //成品，这里是版本号
//                string strFMaterialId = Convert.ToString(Row1.Cells["FMaterialId"].Value); //只有物料清单用得上。
//                string strFMiddleID = Convert.ToString(Row1.Cells["FMiddleID"].Value);
//                string strFDate = Convert.ToString(Row1.Cells["FDate"].Value);

//                string strStrReturn = CleareSysncMark(strFBillNO, strFMaterialId, strFindBillType, strFMiddleID);
//                Row1.Cells["K3Result"].Value = strStrReturn;
//                if (strStrReturn == "")
//                    Row1.Cells["exec_status"].Value = "未同步";

//            }
//            lblWait.Visible = false;

//            if (boolSeleted == false)
//            {
//                MessageBox.Show("您没有选择记录。");
//                return;
//            }

//            //MessageBox.Show("处理完毕。");

//        }


//        private string CleareSysncMark(string pstrFBillNO, string pFMaterialID, string pFPTDJLY, string pFMiddleID)
//        {
//            try
//            {
//                //如果是平台消耗单，要先判断，是否产生了下游单据。如果产生，不允许取消。
//                string strK3FormID_WebApi =CsPublicOA.GetFormIdByBillType(pFPTDJLY);
//                string strReplaceTable = "tblReadMes2";
//                string strLogTable = "tbl2KingdeeLC";
//                bool bolK3BillExist = K3ExistByNumber(strK3FormID_WebApi, pstrFBillNO);

//                string strSQL;


//                string strError = "";

//                if (bolK3BillExist == true)
//                {
//                    strError = "不能取消：同步标记，原因：已产生金蝶单据。";
//                    strSQL = string.Format(@"/*dialect*/ 
//        Update  {0}
//          Set  K3Result='{2}'
//        WHERE   FMiddleID='{1}'", strReplaceTable, pFMiddleID, strError);

//                    CsData.BobExecute(null, mK3CloudApiClient1, strSQL);
//                    return strError;
//                }

//                //查询,单据不存在，所以，我们能取消啦。。
//                strSQL = string.Format(@"/*dialect*/ 
//        Update  {0}
//          Set   exec_status=0,K3Result=''
//        WHERE   FMiddleID='{1}'", strReplaceTable, pFMiddleID);
//                CsData.BobExecute(null, mK3CloudApiClient1, strSQL);

//                InsertMiddleLogResult(pstrFBillNO, "取消同步标记", strLogTable, pFMiddleID, strK3FormID_WebApi);

//                return "";

//            }
//            catch (Exception ex)
//            {
//                string strError = CsErrLog.GetExceptionWithLog(ex);
//                throw new Exception(strError);
//            }

//        }

//        private bool K3ExistByNumber(string pFormID, string pFBillNO)
//        {
//            try
//            {
//                //注意：基础资料和单据，要调用不同的方法。因为过滤字段不同。FNumber和FBillNo
//                string strReturn = B.CBS2MiddleTable.ClsSeek.CheckBillNo(mK3CloudApiClient1, pFBillNO, pFormID, "");
//                bool bolExist = (strReturn != "");
//                return bolExist;
//            }
//            catch (Exception ex)
//            {
//                string strError = CsErrLog.GetExceptionWithLog(ex);
//                throw new Exception(strError);
//            }

//        }

//        private void InsertMiddleLogResult(string pBillNo_PT, string pResult, string pTable
//            , string pFMiddleId
//            , string pFormId)
//        {
//            try
//            {
//                string strSQL = string.Format(@"/*dialect*/
//Insert into {3}(FSourceNO,Json,FMiddleID,FormId) 
//Values('{0}','{1}',{2},'{3}')", pBillNo_PT, pResult, pFMiddleId, pTable, pFormId);
//                CsData.BobExecute(null, mK3CloudApiClient1, strSQL);
//            }
//            catch (Exception ex)
//            {
//                string strError = CsErrLog.GetExceptionWithLog(ex);
//                throw new Exception(strError);
//            }

//        }

//        private void tsbJson_Click(object sender, EventArgs e)
//        {

//            string strFindBillType = this.FFindBillType.Text;
//            string strFormId = CsPublicOA.GetFormIdByBillType(strFindBillType);

//            bool boolSeleted = false;
//            foreach (DataGridViewRow Row1 in dgv1.Rows)
//            {
//                string strFSelect = Convert.ToString(Row1.Cells["FSelect"].Selected);

//                //再给一次机会，这次，读取：Value
//                if (strFSelect == "False")
//                    strFSelect = Convert.ToString(Row1.Cells["FSelect"].Value);

//                if (strFSelect == "False")
//                    continue;

//                boolSeleted = true;
//                string strFMiddleID = Convert.ToString(Row1.Cells["FMiddleID"].Value);

//                TBGZT TBGZT1 = new TBGZT();
//                string strReturn = CsData.GetJson2Kingdee(null, mK3CloudApiClient1, strFormId, strFMiddleID, "Json");

//                if (strReturn == "")
//                {
//                    MessageBox.Show("没找到。");
//                    return;
//                }

//                frmRichText frmRichText1 = new frmRichText();
//                frmRichText1.SetText(strReturn);
//                frmRichText1.Show();

//                return;


//            }
//            if (boolSeleted == false)
//            {
//                MessageBox.Show("您没有选择记录。");
//                return;
//            }


//        }

//        //private string GetJson(string pFMiddleID, string pstrJsonField)
//        //{
//        //    try
//        //    {
//        //        string strReturn1 = "";

//        //        string strSQL = string.Format(@"/*dialect*/ 
//        //                SELECT TOP 1 
//        //                      {0}
//        //                  FROM  tbl2KingdeeLC
//        //                WHERE   FMiddleID='{1}'
//        //                Order by ID Desc ", pstrJsonField,  pFMiddleID);
//        //        strReturn1 = ClsData.GetTopValue(null, mK3CloudApiClient1, strSQL);
//        //        if (strReturn1.Substring(0,1)=="{")
//        //        {
//        //            JObject JObject1 = JObject.Parse(strReturn1);
//        //            strReturn1 = JObject1.ToString();
//        //        }

//        //        string decodedReturn = System.Web.HttpUtility.HtmlDecode(strReturn1);

//        //        return strReturn1;
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        string strError = CsErrLog.GetExceptionWithLog(ex);
//        //        throw new Exception(strError);

//        //    }

//        //}


//        public string GetK3JSonLogTableName(string pFPTDJLY)
//        {
//            try
//            {
//                string strTableName = "";
//                switch (pFPTDJLY)
//                {
//                    case "原料":
//                        strTableName = "tbl2BD_Material";
//                        break;
//                    case "成品":
//                        strTableName = "tbl2BD_Product";
//                        break;
//                    case "物料清单":
//                        strTableName = "tbl2Eng_BOM";
//                        break;

//                    case "采购入库单":
//                        strTableName = "tbl2STK_InStock";
//                        break;

//                    case "采购退料单":
//                        strTableName = "tbl2PUR_MRB";
//                        break;

//                    case "销售出库单":
//                        strTableName = "tbl2Sal_OutStock";
//                        break;

//                    case "销售退货单":
//                        strTableName = "tbl2Sal_ReturnStock";
//                        break;
//                    default:
//                        string strBadInfo = "糟糕，swithc选项不存在。[" + pFPTDJLY + "]" + Environment.NewLine + "ClsTBGZT.GetK3LogTableName";
//                        throw new Exception(strBadInfo);

//                }
//                return strTableName;
//            }
//            catch (Exception ex)
//            {
//                string strError = CsErrLog.GetExceptionWithLog(ex);
//                throw new Exception(strError);
//            }
//        }

//        private void tsbResult_Click(object sender, EventArgs e)
//        {
//            string strFindBillType = this.FFindBillType.Text;
//            string strFormId = CsPublicOA.GetFormIdByBillType(strFindBillType);

//            bool boolSeleted = false;
//            foreach (DataGridViewRow Row1 in dgv1.Rows)
//            {
//                string strFSelect = Convert.ToString(Row1.Cells["FSelect"].Selected);

//                //再给一次机会，这次，读取：Value
//                if (strFSelect == "False")
//                    strFSelect = Convert.ToString(Row1.Cells["FSelect"].Value);

//                if (strFSelect == "False")
//                    continue;

//                boolSeleted = true;
//                string strFMiddleID = Convert.ToString(Row1.Cells["FMiddleID"].Value);

//                TBGZT TBGZT1 = new TBGZT();
//                string strReturn = CsData.GetJson2Kingdee(null, mK3CloudApiClient1, strFormId,strFMiddleID, "ResultJson");
//                //string strReturn = GetJson(strFMiddleID, "ResultJson");
//                if (strReturn == "")
//                {
//                    MessageBox.Show("没找到。");
//                    return;
//                }

//                frmRichText frmRichText1 = new frmRichText();
//                frmRichText1.SetText(strReturn);
//                frmRichText1.Show();

//                return;


//            }
//            if (boolSeleted == false)
//            {
//                MessageBox.Show("您没有选择记录。");
//                return;
//            }
//        }


//        private void tsbClose_Click(object sender, EventArgs e)
//        {
//            //退出时，总报错，想避免。
//            if (tblReadMes2 != null)
//            {
//                tblReadMes2.DataSet.AcceptChanges();
//                bindingSource1.DataSource = null;
//            }
//            this.Close();
//        }



//        private void FFindBillType_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            string strBillType = FFindBillType.Text;
//            if (strBillType == "物料清单")
//            {
//                dgv1.Columns["FMaterialId"].Visible = true;
//                dgv1.Columns["FNumber"].Width = 50;
//            }
//            else
//            {
//                dgv1.Columns["FMaterialId"].Visible = false;
//                dgv1.Columns["FNumber"].Width = 300;
//            }
//        }

//        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
//        {

//        }

//        private void frmTBGZT2_Load(object sender, EventArgs e)
//        {

//            FDate1.Value = System.DateTime.Today.AddDays(-5);
//            FDate2.Value = System.DateTime.Today.AddDays(-1);

//            //GetConnection();
//            GeteWebClient();

//            ClsScheduleReadOthers ClsScheduleReadOthers1 = new ClsScheduleReadOthers();
//            ClsScheduleReadOthers1.GetK3LoginInfo(null, mK3CloudApiClient1, ref mStruct_K3LoginInfo1);

//            mStruct_K3LoginInfo1.LoginK3Ok = true;
//            //不能在界面绑定，要用代码，否则，不能显示。
//            dgv1.DataSource = bindingSource1;


//        }

//        private void GeteWebClient(int pRunMode)
//        {
//            //有时，切换帐套。
//            //if (mK3CloudApiClient1 != null)
//            //    return;

//            mK3CloudApiClient1 = CsPublicOA.CreateK3CloudApiClient(mStruct_K3LoginInfo1,pRunMode);
//            if (mK3CloudApiClient1 == null)
//            {
//                MessageBox.Show("登陆金蝶失败了。");
//                return;
//            }
//        }

//        private void GeteWebClient()
//        {
//            if (radTest.Checked == false && radNormal.Checked == false)
//                return;

//            int intMode = 0;
//            if (radNormal.Checked == true)
//                intMode = RunMode.Normal;
//            else if (radTest.Checked == true)
//                intMode = RunMode.Test;

//            GeteWebClient(intMode);
//        }


//        private void 全选ToolStripMenuItem_Click(object sender, EventArgs e)
//        {
//            SelectChanged(true);

//        }

//        private void 全部清除ToolStripMenuItem_Click(object sender, EventArgs e)
//        {
//            SelectChanged(false);

//        }

//        private void SelectChanged(bool pSelected)
//        {
//            foreach (DataGridViewRow dgvr1 in dgv1.Rows)
//            {
//                dgvr1.Cells["FSelect"].Value = pSelected;
//            }
//        }

//        private void radTest_CheckedChanged(object sender, EventArgs e)
//        {
//            if (radTest.Checked == false && radNormal.Checked == false)
//                return;

//            GeteWebClient();
//            mStruct_K3LoginInfo1.ServerURL_Others = null;
//            ClsScheduleReadOthers ClsScheduleReadOthers1 = new ClsScheduleReadOthers();
//            ClsScheduleReadOthers1.GetK3LoginInfo(null, mK3CloudApiClient1, ref mStruct_K3LoginInfo1);

//            TestModeColor(radTest.Checked);
//        }

//        public void TestModeColor(bool pTestMode)
//        {
//            if (pTestMode == true)
//            {
//                CsPublic2.RadioButtonCheckedColor(radTest);
//                CsPublic2.RadioButtonNotCheckedColor(radNormal);
//            }
//            else
//            {
//                CsPublic2.RadioButtonCheckedColor(radNormal);
//                CsPublic2.RadioButtonNotCheckedColor(radTest);
//            }

//        }

//        private void tsbReadMes_Click(object sender, EventArgs e)
//        {

//            string strFindBillType = this.FFindBillType.Text;

//            bool boolSeleted = false;
//            foreach (DataGridViewRow Row1 in dgv1.Rows)
//            {
//                string strFSelect = Convert.ToString(Row1.Cells["FSelect"].Selected);

//                //再给一次机会，这次，读取：Value
//                if (strFSelect == "False")
//                    strFSelect = Convert.ToString(Row1.Cells["FSelect"].Value);

//                if (strFSelect == "False")
//                    continue;

//                boolSeleted = true;
//                string strFBillNO = Convert.ToString(Row1.Cells["FNumber"].Value);
//                string strFDate = Convert.ToString(Row1.Cells["FDate"].Value);

//                if (strFBillNO == "")
//                {
//                    MessageBox.Show("请选择记录。");
//                    return;
//                }

//                lblWait.Visible = true;
//                CsPublic2.HaveaRest();

//                string strK3FormID_WebApi = CsPublicOA.GetFormIdByBillType(strFindBillType);
//                string strError = "";
//                CsReadWdt CsReadWdt1 = new CsReadWdt();
//                CsReadWdt1.SetFormId(mStruct_K3LoginInfo1, strK3FormID_WebApi);
//                JToken JToken_WDT = CsReadWdt1.GetDataFromWDT(strFDate, strFDate, strFBillNO, ref strError,0,false);
//                frmRichText frmRichText1 = new frmRichText();
//                frmRichText1.SetText(JToken_WDT.ToString());
//                frmRichText1.Show();

//                lblWait.Visible = false;
//                break;

//            }
//            if (boolSeleted == false)
//            {
//                MessageBox.Show("您没有选择记录。");
//                return;
//            }



//        }

//        private void radNormal_CheckedChanged(object sender, EventArgs e)
//        {

//        }
//    }
//}
