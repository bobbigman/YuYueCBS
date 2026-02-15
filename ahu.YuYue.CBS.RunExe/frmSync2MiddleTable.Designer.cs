
namespace ahu.YuYue.CBS.RunExe
{
    partial class frmSync2MiddleTable
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label4 = new System.Windows.Forms.Label();
            this.txtEndDate = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtStartTime = new System.Windows.Forms.TextBox();
            this.lblBillNo = new System.Windows.Forms.Label();
            this.txtBillNo = new System.Windows.Forms.TextBox();
            this.btnRead = new System.Windows.Forms.Button();
            this.rtbResult = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radNormal = new System.Windows.Forms.RadioButton();
            this.radTest = new System.Windows.Forms.RadioButton();
            this.cboBillType = new System.Windows.Forms.ComboBox();
            this.btn2MiddleTable = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radBillType1 = new System.Windows.Forms.RadioButton();
            this.radBillTypeAll = new System.Windows.Forms.RadioButton();
            this.rad1BillNo = new System.Windows.Forms.RadioButton();
            this.lblBillType = new System.Windows.Forms.Label();
            this.lblTimer = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.txtMinute = new System.Windows.Forms.TextBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.cbCountReGetFromInterface = new System.Windows.Forms.CheckBox();
            this.btnAutoSchedule = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPage = new System.Windows.Forms.TextBox();
            this.cbOnlyJson = new System.Windows.Forms.CheckBox();
            this.lblBankSerialNumber = new System.Windows.Forms.Label();
            this.txtBankSerialNumber = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbWB_ReceiptBill = new System.Windows.Forms.TextBox();
            this.btnUnAuditCheck = new System.Windows.Forms.Button();
            this.tbBillNo_PayBill = new System.Windows.Forms.TextBox();
            this.btn2K3 = new System.Windows.Forms.Button();
            this.btnOptmization = new System.Windows.Forms.Button();
            this.btnUpdateAttachment = new System.Windows.Forms.Button();
            this.btnRefreshPdf = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 480);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 53;
            this.label4.Text = "结束时间";
            // 
            // txtEndDate
            // 
            this.txtEndDate.Location = new System.Drawing.Point(124, 475);
            this.txtEndDate.Name = "txtEndDate";
            this.txtEndDate.Size = new System.Drawing.Size(209, 21);
            this.txtEndDate.TabIndex = 52;
            this.txtEndDate.Text = "2025-11-13";
            this.txtEndDate.TextChanged += new System.EventHandler(this.txtEndDate_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 438);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 51;
            this.label3.Text = "开始时间";
            // 
            // txtStartTime
            // 
            this.txtStartTime.Location = new System.Drawing.Point(124, 433);
            this.txtStartTime.Name = "txtStartTime";
            this.txtStartTime.Size = new System.Drawing.Size(209, 21);
            this.txtStartTime.TabIndex = 50;
            this.txtStartTime.Text = "2025-11-01";
            // 
            // lblBillNo
            // 
            this.lblBillNo.AutoSize = true;
            this.lblBillNo.Location = new System.Drawing.Point(16, 403);
            this.lblBillNo.Name = "lblBillNo";
            this.lblBillNo.Size = new System.Drawing.Size(41, 12);
            this.lblBillNo.TabIndex = 49;
            this.lblBillNo.Text = "对账码";
            // 
            // txtBillNo
            // 
            this.txtBillNo.Location = new System.Drawing.Point(124, 395);
            this.txtBillNo.Name = "txtBillNo";
            this.txtBillNo.Size = new System.Drawing.Size(209, 21);
            this.txtBillNo.TabIndex = 48;
            this.txtBillNo.Text = "DZM202511060001444741";
            this.txtBillNo.TextChanged += new System.EventHandler(this.txtBillNo_TextChanged);
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(12, 649);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(265, 57);
            this.btnRead.TabIndex = 47;
            this.btnRead.Text = "从业务系统取数";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnReadOthers_Click);
            // 
            // rtbResult
            // 
            this.rtbResult.Location = new System.Drawing.Point(359, 1);
            this.rtbResult.Name = "rtbResult";
            this.rtbResult.Size = new System.Drawing.Size(533, 794);
            this.rtbResult.TabIndex = 46;
            this.rtbResult.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radNormal);
            this.groupBox1.Controls.Add(this.radTest);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(27, 31);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(289, 52);
            this.groupBox1.TabIndex = 61;
            this.groupBox1.TabStop = false;
            // 
            // radNormal
            // 
            this.radNormal.AutoSize = true;
            this.radNormal.BackColor = System.Drawing.SystemColors.Control;
            this.radNormal.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radNormal.Location = new System.Drawing.Point(26, 20);
            this.radNormal.Name = "radNormal";
            this.radNormal.Size = new System.Drawing.Size(47, 16);
            this.radNormal.TabIndex = 0;
            this.radNormal.Text = "正式";
            this.radNormal.UseVisualStyleBackColor = false;
            this.radNormal.CheckedChanged += new System.EventHandler(this.radNormal_CheckedChanged);
            // 
            // radTest
            // 
            this.radTest.AutoSize = true;
            this.radTest.BackColor = System.Drawing.Color.Blue;
            this.radTest.Checked = true;
            this.radTest.ForeColor = System.Drawing.Color.Yellow;
            this.radTest.Location = new System.Drawing.Point(154, 20);
            this.radTest.Name = "radTest";
            this.radTest.Size = new System.Drawing.Size(47, 16);
            this.radTest.TabIndex = 1;
            this.radTest.TabStop = true;
            this.radTest.Text = "测试";
            this.radTest.UseVisualStyleBackColor = false;
            this.radTest.CheckedChanged += new System.EventHandler(this.radTest_CheckedChanged);
            // 
            // cboBillType
            // 
            this.cboBillType.FormattingEnabled = true;
            this.cboBillType.Items.AddRange(new object[] {
            "接收银行交易明细",
            "电子回单",
            "电子回单附件",
            "支付状态"});
            this.cboBillType.Location = new System.Drawing.Point(124, 359);
            this.cboBillType.Name = "cboBillType";
            this.cboBillType.Size = new System.Drawing.Size(209, 20);
            this.cboBillType.TabIndex = 62;
            this.cboBillType.Text = "电子回单";
            this.cboBillType.SelectedIndexChanged += new System.EventHandler(this.cboBillType_SelectedIndexChanged);
            // 
            // btn2MiddleTable
            // 
            this.btn2MiddleTable.Location = new System.Drawing.Point(12, 712);
            this.btn2MiddleTable.Name = "btn2MiddleTable";
            this.btn2MiddleTable.Size = new System.Drawing.Size(265, 57);
            this.btn2MiddleTable.TabIndex = 63;
            this.btn2MiddleTable.Text = "先取数，再同步到中间表";
            this.btn2MiddleTable.UseVisualStyleBackColor = true;
            this.btn2MiddleTable.Click += new System.EventHandler(this.btnRead2MiddleTable_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radBillType1);
            this.groupBox2.Controls.Add(this.radBillTypeAll);
            this.groupBox2.Controls.Add(this.rad1BillNo);
            this.groupBox2.Location = new System.Drawing.Point(27, 115);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(289, 107);
            this.groupBox2.TabIndex = 64;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "范围";
            // 
            // radBillType1
            // 
            this.radBillType1.AutoSize = true;
            this.radBillType1.Location = new System.Drawing.Point(170, 31);
            this.radBillType1.Name = "radBillType1";
            this.radBillType1.Size = new System.Drawing.Size(71, 16);
            this.radBillType1.TabIndex = 2;
            this.radBillType1.Text = "单据类型";
            this.radBillType1.UseVisualStyleBackColor = true;
            this.radBillType1.CheckedChanged += new System.EventHandler(this.radBillType_CheckedChanged);
            // 
            // radBillTypeAll
            // 
            this.radBillTypeAll.AutoSize = true;
            this.radBillTypeAll.Location = new System.Drawing.Point(26, 67);
            this.radBillTypeAll.Name = "radBillTypeAll";
            this.radBillTypeAll.Size = new System.Drawing.Size(95, 16);
            this.radBillTypeAll.TabIndex = 1;
            this.radBillTypeAll.Text = "全部业务单据";
            this.radBillTypeAll.UseVisualStyleBackColor = true;
            this.radBillTypeAll.CheckedChanged += new System.EventHandler(this.radBusiness_CheckedChanged);
            // 
            // rad1BillNo
            // 
            this.rad1BillNo.AutoSize = true;
            this.rad1BillNo.Checked = true;
            this.rad1BillNo.Location = new System.Drawing.Point(26, 31);
            this.rad1BillNo.Name = "rad1BillNo";
            this.rad1BillNo.Size = new System.Drawing.Size(65, 16);
            this.rad1BillNo.TabIndex = 0;
            this.rad1BillNo.TabStop = true;
            this.rad1BillNo.Text = "1个单据";
            this.rad1BillNo.UseVisualStyleBackColor = true;
            this.rad1BillNo.CheckedChanged += new System.EventHandler(this.rad1BillNo_CheckedChanged);
            // 
            // lblBillType
            // 
            this.lblBillType.AutoSize = true;
            this.lblBillType.Location = new System.Drawing.Point(17, 367);
            this.lblBillType.Name = "lblBillType";
            this.lblBillType.Size = new System.Drawing.Size(53, 12);
            this.lblBillType.TabIndex = 65;
            this.lblBillType.Text = "单据类型";
            // 
            // lblTimer
            // 
            this.lblTimer.AutoSize = true;
            this.lblTimer.Location = new System.Drawing.Point(151, 257);
            this.lblTimer.Name = "lblTimer";
            this.lblTimer.Size = new System.Drawing.Size(77, 12);
            this.lblTimer.TabIndex = 68;
            this.lblTimer.Text = "分钟执行一次";
            this.lblTimer.Visible = false;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(244, 252);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 67;
            this.btnOk.Text = "应用";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Visible = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // txtMinute
            // 
            this.txtMinute.Location = new System.Drawing.Point(37, 253);
            this.txtMinute.Name = "txtMinute";
            this.txtMinute.Size = new System.Drawing.Size(100, 21);
            this.txtMinute.TabIndex = 66;
            this.txtMinute.Text = "1440";
            this.txtMinute.Visible = false;
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(7, 855);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(270, 57);
            this.btnExit.TabIndex = 69;
            this.btnExit.Text = "退出";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 600000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // cbCountReGetFromInterface
            // 
            this.cbCountReGetFromInterface.AutoSize = true;
            this.cbCountReGetFromInterface.Location = new System.Drawing.Point(37, 304);
            this.cbCountReGetFromInterface.Name = "cbCountReGetFromInterface";
            this.cbCountReGetFromInterface.Size = new System.Drawing.Size(180, 16);
            this.cbCountReGetFromInterface.TabIndex = 72;
            this.cbCountReGetFromInterface.Text = "统计数量时，重新从接口取数";
            this.cbCountReGetFromInterface.UseVisualStyleBackColor = true;
            // 
            // btnAutoSchedule
            // 
            this.btnAutoSchedule.Location = new System.Drawing.Point(959, 295);
            this.btnAutoSchedule.Name = "btnAutoSchedule";
            this.btnAutoSchedule.Size = new System.Drawing.Size(214, 57);
            this.btnAutoSchedule.TabIndex = 74;
            this.btnAutoSchedule.Text = "模拟自动同步，增量";
            this.btnAutoSchedule.UseVisualStyleBackColor = true;
            this.btnAutoSchedule.Click += new System.EventHandler(this.btnAutoSchedule_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 521);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 76;
            this.label1.Text = "页数";
            // 
            // txtPage
            // 
            this.txtPage.Location = new System.Drawing.Point(124, 516);
            this.txtPage.Name = "txtPage";
            this.txtPage.Size = new System.Drawing.Size(209, 21);
            this.txtPage.TabIndex = 75;
            // 
            // cbOnlyJson
            // 
            this.cbOnlyJson.AutoSize = true;
            this.cbOnlyJson.Checked = true;
            this.cbOnlyJson.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbOnlyJson.Location = new System.Drawing.Point(938, 24);
            this.cbOnlyJson.Name = "cbOnlyJson";
            this.cbOnlyJson.Size = new System.Drawing.Size(204, 16);
            this.cbOnlyJson.TabIndex = 79;
            this.cbOnlyJson.Text = "只读取报文，不要读同步进度信息";
            this.cbOnlyJson.UseVisualStyleBackColor = true;
            // 
            // lblBankSerialNumber
            // 
            this.lblBankSerialNumber.AutoSize = true;
            this.lblBankSerialNumber.Location = new System.Drawing.Point(16, 560);
            this.lblBankSerialNumber.Name = "lblBankSerialNumber";
            this.lblBankSerialNumber.Size = new System.Drawing.Size(65, 12);
            this.lblBankSerialNumber.TabIndex = 96;
            this.lblBankSerialNumber.Text = "银行流水号";
            // 
            // txtBankSerialNumber
            // 
            this.txtBankSerialNumber.Location = new System.Drawing.Point(124, 560);
            this.txtBankSerialNumber.Name = "txtBankSerialNumber";
            this.txtBankSerialNumber.Size = new System.Drawing.Size(209, 21);
            this.txtBankSerialNumber.TabIndex = 95;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(16, 592);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 99;
            this.label9.Text = "电子回单号";
            // 
            // tbWB_ReceiptBill
            // 
            this.tbWB_ReceiptBill.Location = new System.Drawing.Point(124, 589);
            this.tbWB_ReceiptBill.Name = "tbWB_ReceiptBill";
            this.tbWB_ReceiptBill.Size = new System.Drawing.Size(209, 21);
            this.tbWB_ReceiptBill.TabIndex = 98;
            // 
            // btnUnAuditCheck
            // 
            this.btnUnAuditCheck.Location = new System.Drawing.Point(482, 875);
            this.btnUnAuditCheck.Name = "btnUnAuditCheck";
            this.btnUnAuditCheck.Size = new System.Drawing.Size(199, 37);
            this.btnUnAuditCheck.TabIndex = 107;
            this.btnUnAuditCheck.Text = "付款单号，查支付状态用";
            this.btnUnAuditCheck.UseVisualStyleBackColor = true;
            // 
            // tbBillNo_PayBill
            // 
            this.tbBillNo_PayBill.Location = new System.Drawing.Point(701, 884);
            this.tbBillNo_PayBill.Name = "tbBillNo_PayBill";
            this.tbBillNo_PayBill.Size = new System.Drawing.Size(204, 21);
            this.tbBillNo_PayBill.TabIndex = 106;
            this.tbBillNo_PayBill.Text = "FKD00080505";
            // 
            // btn2K3
            // 
            this.btn2K3.Location = new System.Drawing.Point(12, 789);
            this.btn2K3.Name = "btn2K3";
            this.btn2K3.Size = new System.Drawing.Size(265, 57);
            this.btn2K3.TabIndex = 108;
            this.btn2K3.Text = "从中间表同步到金蝶";
            this.btn2K3.UseVisualStyleBackColor = true;
            this.btn2K3.Click += new System.EventHandler(this.btn2K3_Click);
            // 
            // btnOptmization
            // 
            this.btnOptmization.Location = new System.Drawing.Point(959, 393);
            this.btnOptmization.Name = "btnOptmization";
            this.btnOptmization.Size = new System.Drawing.Size(214, 57);
            this.btnOptmization.TabIndex = 109;
            this.btnOptmization.Text = "模拟自动同步，增量，优化";
            this.btnOptmization.UseVisualStyleBackColor = true;
            this.btnOptmization.Click += new System.EventHandler(this.btnOptmization_Click);
            // 
            // btnUpdateAttachment
            // 
            this.btnUpdateAttachment.Location = new System.Drawing.Point(959, 480);
            this.btnUpdateAttachment.Name = "btnUpdateAttachment";
            this.btnUpdateAttachment.Size = new System.Drawing.Size(214, 57);
            this.btnUpdateAttachment.TabIndex = 110;
            this.btnUpdateAttachment.Text = "模拟自动同步附件";
            this.btnUpdateAttachment.UseVisualStyleBackColor = true;
            this.btnUpdateAttachment.Click += new System.EventHandler(this.btnUpdateAttachment_Click);
            // 
            // btnRefreshPdf
            // 
            this.btnRefreshPdf.Location = new System.Drawing.Point(959, 560);
            this.btnRefreshPdf.Name = "btnRefreshPdf";
            this.btnRefreshPdf.Size = new System.Drawing.Size(214, 57);
            this.btnRefreshPdf.TabIndex = 111;
            this.btnRefreshPdf.Text = "模拟刷新附件";
            this.btnRefreshPdf.UseVisualStyleBackColor = true;
            this.btnRefreshPdf.Click += new System.EventHandler(this.btnRefreshPdf_Click);
            // 
            // frmSync2MiddleTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1284, 978);
            this.Controls.Add(this.btnRefreshPdf);
            this.Controls.Add(this.btnUpdateAttachment);
            this.Controls.Add(this.btnOptmization);
            this.Controls.Add(this.btn2K3);
            this.Controls.Add(this.btnUnAuditCheck);
            this.Controls.Add(this.tbBillNo_PayBill);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tbWB_ReceiptBill);
            this.Controls.Add(this.lblBankSerialNumber);
            this.Controls.Add(this.txtBankSerialNumber);
            this.Controls.Add(this.cbOnlyJson);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPage);
            this.Controls.Add(this.btnAutoSchedule);
            this.Controls.Add(this.cbCountReGetFromInterface);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.lblTimer);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtMinute);
            this.Controls.Add(this.lblBillType);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btn2MiddleTable);
            this.Controls.Add(this.cboBillType);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtEndDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtStartTime);
            this.Controls.Add(this.lblBillNo);
            this.Controls.Add(this.txtBillNo);
            this.Controls.Add(this.btnRead);
            this.Controls.Add(this.rtbResult);
            this.Name = "frmSync2MiddleTable";
            this.Text = "渝月CBS，同步到中间表";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtEndDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtStartTime;
        private System.Windows.Forms.Label lblBillNo;
        private System.Windows.Forms.TextBox txtBillNo;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.RichTextBox rtbResult;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radNormal;
        private System.Windows.Forms.RadioButton radTest;
        private System.Windows.Forms.ComboBox cboBillType;
        private System.Windows.Forms.Button btn2MiddleTable;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radBillType1;
        private System.Windows.Forms.RadioButton radBillTypeAll;
        private System.Windows.Forms.RadioButton rad1BillNo;
        private System.Windows.Forms.Label lblBillType;
        private System.Windows.Forms.Label lblTimer;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TextBox txtMinute;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox cbCountReGetFromInterface;
        private System.Windows.Forms.Button btnAutoSchedule;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPage;
        private System.Windows.Forms.CheckBox cbOnlyJson;
        private System.Windows.Forms.Label lblBankSerialNumber;
        private System.Windows.Forms.TextBox txtBankSerialNumber;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbWB_ReceiptBill;
        private System.Windows.Forms.Button btnUnAuditCheck;
        private System.Windows.Forms.TextBox tbBillNo_PayBill;
        private System.Windows.Forms.Button btn2K3;
        private System.Windows.Forms.Button btnOptmization;
        private System.Windows.Forms.Button btnUpdateAttachment;
        private System.Windows.Forms.Button btnRefreshPdf;
    }
}

