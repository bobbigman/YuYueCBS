namespace B.CBS2MiddleTable.RunExe
{
    partial class frmTBGZT2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing && (components != null))
        //    {
        //        components.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTBGZT2));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.FFromMES = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radNormal = new System.Windows.Forms.RadioButton();
            this.radTest = new System.Windows.Forms.RadioButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbRead = new System.Windows.Forms.ToolStripLabel();
            this.tsb2k3 = new System.Windows.Forms.ToolStripButton();
            this.tsbSynchronClear = new System.Windows.Forms.ToolStripButton();
            this.tsbJson = new System.Windows.Forms.ToolStripButton();
            this.tsbResult = new System.Windows.Forms.ToolStripButton();
            this.tsbGetDataFromInterface = new System.Windows.Forms.ToolStripButton();
            this.tsbSchedule = new System.Windows.Forms.ToolStripButton();
            this.tsbReadMes = new System.Windows.Forms.ToolStripButton();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.FFindSheet_no = new System.Windows.Forms.TextBox();
            this.Fexec_status = new System.Windows.Forms.ComboBox();
            this.FDate2 = new System.Windows.Forms.DateTimePicker();
            this.FDate1 = new System.Windows.Forms.DateTimePicker();
            this.FFindBillType = new System.Windows.Forms.ComboBox();
            this.lblWait = new System.Windows.Forms.Label();
            this.dgv1 = new System.Windows.Forms.DataGridView();
            this.FSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.FMaterialId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.exec_status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.K3Result = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FMiddleID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.全选ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.全部清除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.menuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.FFromMES);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
            this.splitContainer1.Panel1.Controls.Add(this.label7);
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.FFindSheet_no);
            this.splitContainer1.Panel1.Controls.Add(this.Fexec_status);
            this.splitContainer1.Panel1.Controls.Add(this.FDate2);
            this.splitContainer1.Panel1.Controls.Add(this.FDate1);
            this.splitContainer1.Panel1.Controls.Add(this.FFindBillType);
            this.splitContainer1.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel1_Paint);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lblWait);
            this.splitContainer1.Panel2.Controls.Add(this.dgv1);
            this.splitContainer1.Panel2.Controls.Add(this.bindingNavigator1);
            this.splitContainer1.Panel2.Controls.Add(this.menuStrip2);
            this.splitContainer1.Size = new System.Drawing.Size(1284, 662);
            this.splitContainer1.SplitterDistance = 132;
            this.splitContainer1.TabIndex = 0;
            // 
            // FFromMES
            // 
            this.FFromMES.AutoSize = true;
            this.FFromMES.Location = new System.Drawing.Point(842, 51);
            this.FFromMES.Name = "FFromMES";
            this.FFromMES.Size = new System.Drawing.Size(174, 16);
            this.FFromMES.TabIndex = 63;
            this.FFromMES.Text = "重新从CBS系统取数(会很慢)";
            this.FFromMES.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radNormal);
            this.groupBox1.Controls.Add(this.radTest);
            this.groupBox1.Location = new System.Drawing.Point(1062, 37);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 52);
            this.groupBox1.TabIndex = 62;
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
            this.radTest.Location = new System.Drawing.Point(100, 20);
            this.radTest.Name = "radTest";
            this.radTest.Size = new System.Drawing.Size(47, 16);
            this.radTest.TabIndex = 1;
            this.radTest.TabStop = true;
            this.radTest.Text = "测试";
            this.radTest.UseVisualStyleBackColor = false;
            this.radTest.CheckedChanged += new System.EventHandler(this.radTest_CheckedChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbRead,
            this.tsb2k3,
            this.tsbSynchronClear,
            this.tsbJson,
            this.tsbResult,
            this.tsbGetDataFromInterface,
            this.tsbSchedule,
            this.tsbReadMes,
            this.tsbClose});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1284, 25);
            this.toolStrip1.TabIndex = 30;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbRead
            // 
            this.tsbRead.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbRead.Image = ((System.Drawing.Image)(resources.GetObject("tsbRead.Image")));
            this.tsbRead.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRead.Name = "tsbRead";
            this.tsbRead.Size = new System.Drawing.Size(67, 22);
            this.tsbRead.Text = "从CBS读取";
            //this.tsbRead.Click += new System.EventHandler(this.tsbRead_Click);
            // 
            // tsb2k3
            // 
            this.tsb2k3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsb2k3.Image = ((System.Drawing.Image)(resources.GetObject("tsb2k3.Image")));
            this.tsb2k3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb2k3.Name = "tsb2k3";
            this.tsb2k3.Size = new System.Drawing.Size(72, 22);
            this.tsb2k3.Text = "同步到金蝶";
            //this.tsb2k3.Click += new System.EventHandler(this.tsb2k3_Click);
            // 
            // tsbSynchronClear
            // 
            this.tsbSynchronClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbSynchronClear.Image = ((System.Drawing.Image)(resources.GetObject("tsbSynchronClear.Image")));
            this.tsbSynchronClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSynchronClear.Name = "tsbSynchronClear";
            this.tsbSynchronClear.Size = new System.Drawing.Size(84, 22);
            this.tsbSynchronClear.Text = "清除同步标记";
            //this.tsbSynchronClear.Click += new System.EventHandler(this.tsbSynchronClear_Click);
            // 
            // tsbJson
            // 
            this.tsbJson.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbJson.Image = ((System.Drawing.Image)(resources.GetObject("tsbJson.Image")));
            this.tsbJson.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbJson.Name = "tsbJson";
            this.tsbJson.Size = new System.Drawing.Size(60, 22);
            this.tsbJson.Text = "同步报文";
            //this.tsbJson.Click += new System.EventHandler(this.tsbJson_Click);
            // 
            // tsbResult
            // 
            this.tsbResult.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbResult.Image = ((System.Drawing.Image)(resources.GetObject("tsbResult.Image")));
            this.tsbResult.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbResult.Name = "tsbResult";
            this.tsbResult.Size = new System.Drawing.Size(60, 22);
            this.tsbResult.Text = "同步结果";
            this.tsbResult.Click += new System.EventHandler(this.tsbResult_Click);
            // 
            // tsbGetDataFromInterface
            // 
            this.tsbGetDataFromInterface.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbGetDataFromInterface.Image = ((System.Drawing.Image)(resources.GetObject("tsbGetDataFromInterface.Image")));
            this.tsbGetDataFromInterface.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbGetDataFromInterface.Name = "tsbGetDataFromInterface";
            this.tsbGetDataFromInterface.Size = new System.Drawing.Size(179, 22);
            this.tsbGetDataFromInterface.Text = "根据条件，从CBS取数后再同步";
            this.tsbGetDataFromInterface.Click += new System.EventHandler(this.tsbGetDataFromInterface_Click);
            // 
            // tsbSchedule
            // 
            this.tsbSchedule.BackColor = System.Drawing.SystemColors.Control;
            this.tsbSchedule.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbSchedule.Image = ((System.Drawing.Image)(resources.GetObject("tsbSchedule.Image")));
            this.tsbSchedule.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSchedule.Name = "tsbSchedule";
            this.tsbSchedule.Size = new System.Drawing.Size(84, 22);
            this.tsbSchedule.Text = "模拟自动同步";
            this.tsbSchedule.Click += new System.EventHandler(this.tsbSchedule_Click);
            // 
            // tsbReadMes
            // 
            this.tsbReadMes.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbReadMes.Image = ((System.Drawing.Image)(resources.GetObject("tsbReadMes.Image")));
            this.tsbReadMes.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbReadMes.Name = "tsbReadMes";
            this.tsbReadMes.Size = new System.Drawing.Size(149, 22);
            this.tsbReadMes.Text = "从业务系统取数-报文格式";
            this.tsbReadMes.Click += new System.EventHandler(this.tsbReadMes_Click);
            // 
            // tsbClose
            // 
            this.tsbClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbClose.Image = ((System.Drawing.Image)(resources.GetObject("tsbClose.Image")));
            this.tsbClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Size = new System.Drawing.Size(36, 22);
            this.tsbClose.Text = "关闭";
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 96);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 12);
            this.label7.TabIndex = 29;
            this.label7.Text = "浪潮单据类型";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(625, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 25;
            this.label6.Text = "同步状态";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(-353, 86);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 12);
            this.label5.TabIndex = 24;
            this.label5.Text = "MES单据类型";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(339, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 23;
            this.label4.Text = "单据单号";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(335, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 22;
            this.label3.Text = "结束日期";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 21;
            this.label2.Text = "开始日期";
            // 
            // FFindSheet_no
            // 
            this.FFindSheet_no.Location = new System.Drawing.Point(405, 92);
            this.FFindSheet_no.Name = "FFindSheet_no";
            this.FFindSheet_no.Size = new System.Drawing.Size(200, 21);
            this.FFindSheet_no.TabIndex = 19;
            // 
            // Fexec_status
            // 
            this.Fexec_status.FormattingEnabled = true;
            this.Fexec_status.Items.AddRange(new object[] {
            "未同步",
            "同步成功",
            "同步错误",
            "手工处理",
            "同步中",
            "同步成功,审核失败"});
            this.Fexec_status.Location = new System.Drawing.Point(697, 51);
            this.Fexec_status.Name = "Fexec_status";
            this.Fexec_status.Size = new System.Drawing.Size(121, 20);
            this.Fexec_status.TabIndex = 18;
            // 
            // FDate2
            // 
            this.FDate2.Location = new System.Drawing.Point(405, 51);
            this.FDate2.Name = "FDate2";
            this.FDate2.Size = new System.Drawing.Size(180, 21);
            this.FDate2.TabIndex = 17;
            this.FDate2.Value = new System.DateTime(2024, 1, 25, 0, 0, 0, 0);
            // 
            // FDate1
            // 
            this.FDate1.Location = new System.Drawing.Point(107, 51);
            this.FDate1.Name = "FDate1";
            this.FDate1.Size = new System.Drawing.Size(177, 21);
            this.FDate1.TabIndex = 16;
            this.FDate1.Value = new System.DateTime(2024, 1, 23, 0, 0, 0, 0);
            // 
            // FFindBillType
            // 
            this.FFindBillType.FormattingEnabled = true;
            this.FFindBillType.Items.AddRange(new object[] {
            "接收银行交易明细",
            "电子回单"});
            this.FFindBillType.Location = new System.Drawing.Point(107, 92);
            this.FFindBillType.Name = "FFindBillType";
            this.FFindBillType.Size = new System.Drawing.Size(177, 20);
            this.FFindBillType.TabIndex = 15;
            this.FFindBillType.Text = "接收银行交易明细";
            this.FFindBillType.SelectedIndexChanged += new System.EventHandler(this.FFindBillType_SelectedIndexChanged);
            // 
            // lblWait
            // 
            this.lblWait.AutoSize = true;
            this.lblWait.BackColor = System.Drawing.SystemColors.HotTrack;
            this.lblWait.Font = new System.Drawing.Font("宋体", 42F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblWait.ForeColor = System.Drawing.Color.Yellow;
            this.lblWait.Location = new System.Drawing.Point(365, 237);
            this.lblWait.Name = "lblWait";
            this.lblWait.Size = new System.Drawing.Size(668, 56);
            this.lblWait.TabIndex = 42;
            this.lblWait.Text = "资料处理中，请稍候.....";
            this.lblWait.Visible = false;
            // 
            // dgv1
            // 
            this.dgv1.AllowUserToAddRows = false;
            this.dgv1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FSelect,
            this.FMaterialId,
            this.FNumber,
            this.exec_status,
            this.K3Result,
            this.FDate,
            this.FMiddleID});
            this.dgv1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv1.Location = new System.Drawing.Point(0, 50);
            this.dgv1.Name = "dgv1";
            this.dgv1.RowTemplate.Height = 23;
            this.dgv1.Size = new System.Drawing.Size(1284, 476);
            this.dgv1.TabIndex = 2;
            // 
            // FSelect
            // 
            this.FSelect.DataPropertyName = "FSelect";
            this.FSelect.HeaderText = "选择";
            this.FSelect.Name = "FSelect";
            // 
            // FMaterialId
            // 
            this.FMaterialId.DataPropertyName = "FMaterialId";
            this.FMaterialId.HeaderText = "产品编码";
            this.FMaterialId.Name = "FMaterialId";
            this.FMaterialId.Visible = false;
            this.FMaterialId.Width = 300;
            // 
            // FNumber
            // 
            this.FNumber.DataPropertyName = "FNumber";
            this.FNumber.HeaderText = "编号";
            this.FNumber.Name = "FNumber";
            this.FNumber.ReadOnly = true;
            this.FNumber.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.FNumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.FNumber.Width = 300;
            // 
            // exec_status
            // 
            this.exec_status.DataPropertyName = "exec_status";
            this.exec_status.HeaderText = "同步状态";
            this.exec_status.Name = "exec_status";
            this.exec_status.ReadOnly = true;
            this.exec_status.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.exec_status.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.exec_status.Width = 250;
            // 
            // K3Result
            // 
            this.K3Result.DataPropertyName = "K3Result";
            this.K3Result.HeaderText = "同步结果信息";
            this.K3Result.Name = "K3Result";
            this.K3Result.ReadOnly = true;
            this.K3Result.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.K3Result.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.K3Result.Width = 500;
            // 
            // FDate
            // 
            this.FDate.DataPropertyName = "FDate";
            this.FDate.HeaderText = "浪潮数据日期";
            this.FDate.Name = "FDate";
            this.FDate.ReadOnly = true;
            this.FDate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.FDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.FDate.Width = 140;
            // 
            // FMiddleID
            // 
            this.FMiddleID.DataPropertyName = "FMiddleID";
            this.FMiddleID.HeaderText = "FMiddleID";
            this.FMiddleID.Name = "FMiddleID";
            this.FMiddleID.Visible = false;
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = null;
            this.bindingNavigator1.BindingSource = this.bindingSource1;
            this.bindingNavigator1.CountItem = this.bindingNavigatorCountItem;
            this.bindingNavigator1.DeleteItem = null;
            this.bindingNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 25);
            this.bindingNavigator1.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.bindingNavigator1.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.bindingNavigator1.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.bindingNavigator1.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = this.bindingNavigatorPositionItem;
            this.bindingNavigator1.Size = new System.Drawing.Size(1284, 25);
            this.bindingNavigator1.TabIndex = 1;
            this.bindingNavigator1.Text = "bindingNavigator1";
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(32, 22);
            this.bindingNavigatorCountItem.Text = "/ {0}";
            this.bindingNavigatorCountItem.ToolTipText = "总项数";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveFirstItem.Text = "移到第一条记录";
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMovePreviousItem.Text = "移到上一条记录";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "位置";
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 23);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "当前位置";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveNextItem.Text = "移到下一条记录";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveLastItem.Text = "移到最后一条记录";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // menuStrip2
            // 
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.全选ToolStripMenuItem,
            this.全部清除ToolStripMenuItem});
            this.menuStrip2.Location = new System.Drawing.Point(0, 0);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Size = new System.Drawing.Size(1284, 25);
            this.menuStrip2.TabIndex = 3;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // 全选ToolStripMenuItem
            // 
            this.全选ToolStripMenuItem.Name = "全选ToolStripMenuItem";
            this.全选ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.全选ToolStripMenuItem.Text = "全部选中";
            this.全选ToolStripMenuItem.Click += new System.EventHandler(this.全选ToolStripMenuItem_Click);
            // 
            // 全部清除ToolStripMenuItem
            // 
            this.全部清除ToolStripMenuItem.Name = "全部清除ToolStripMenuItem";
            this.全部清除ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.全部清除ToolStripMenuItem.Text = "全部清除";
            this.全部清除ToolStripMenuItem.Click += new System.EventHandler(this.全部清除ToolStripMenuItem_Click);
            // 
            // frmTBGZT2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1284, 662);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmTBGZT2";
            this.Text = "同步工作台-金蝶同步到CBS";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmTBGZT2_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox FFindSheet_no;
        private System.Windows.Forms.ComboBox Fexec_status;
        private System.Windows.Forms.DateTimePicker FDate2;
        private System.Windows.Forms.DateTimePicker FDate1;
        private System.Windows.Forms.ComboBox FFindBillType;
        private System.Windows.Forms.DataGridView dgv1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel tsbRead;
        private System.Windows.Forms.ToolStripButton tsb2k3;
        private System.Windows.Forms.ToolStripButton tsbSynchronClear;
        private System.Windows.Forms.ToolStripButton tsbJson;
        private System.Windows.Forms.ToolStripButton tsbResult;
        private System.Windows.Forms.ToolStripButton tsbGetDataFromInterface;
        private System.Windows.Forms.ToolStripButton tsbSchedule;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radNormal;
        private System.Windows.Forms.RadioButton radTest;
        private System.Windows.Forms.CheckBox FFromMES;
        private System.Windows.Forms.MenuStrip menuStrip2;
        private System.Windows.Forms.ToolStripMenuItem 全选ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 全部清除ToolStripMenuItem;
        private System.Windows.Forms.Label lblWait;
        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.ToolStripButton tsbReadMes;
        private System.Windows.Forms.DataGridViewCheckBoxColumn FSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn FMaterialId;
        private System.Windows.Forms.DataGridViewTextBoxColumn FNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn exec_status;
        private System.Windows.Forms.DataGridViewTextBoxColumn K3Result;
        private System.Windows.Forms.DataGridViewTextBoxColumn FDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn FMiddleID;
    }
}