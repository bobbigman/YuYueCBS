
namespace ahu.YuYue.CBS.RunExe
{
    partial class frmCheck
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCheck));
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbFilter = new System.Windows.Forms.ToolStripButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radTest = new System.Windows.Forms.RadioButton();
            this.radNormal = new System.Windows.Forms.RadioButton();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.cbCountReGetFromInterface = new System.Windows.Forms.CheckBox();
            this.chkSum = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.FDate2 = new System.Windows.Forms.DateTimePicker();
            this.FDate1 = new System.Windows.Forms.DateTimePicker();
            this.chkIgnoreZero = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboBillType = new System.Windows.Forms.ComboBox();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.tsbRefresh = new System.Windows.Forms.ToolStripButton();
            this.tbCalculate = new System.Windows.Forms.ToolStripButton();
            this.tbCatchCountByDate = new System.Windows.Forms.ToolStripButton();
            this.lblWait = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.FormId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.日期 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WDTCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.K3Count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ErrorCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
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
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbFilter
            // 
            this.tsbFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbFilter.Image = ((System.Drawing.Image)(resources.GetObject("tsbFilter.Image")));
            this.tsbFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbFilter.Name = "tsbFilter";
            this.tsbFilter.Size = new System.Drawing.Size(36, 22);
            this.tsbFilter.Text = "过滤";
            this.tsbFilter.Click += new System.EventHandler(this.tsbFilter_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radTest);
            this.groupBox1.Controls.Add(this.radNormal);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(1214, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(233, 45);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            // 
            // radTest
            // 
            this.radTest.AutoSize = true;
            this.radTest.BackColor = System.Drawing.Color.Blue;
            this.radTest.Checked = true;
            this.radTest.ForeColor = System.Drawing.Color.Yellow;
            this.radTest.Location = new System.Drawing.Point(120, 16);
            this.radTest.Name = "radTest";
            this.radTest.Size = new System.Drawing.Size(71, 16);
            this.radTest.TabIndex = 1;
            this.radTest.TabStop = true;
            this.radTest.Text = "测试帐套";
            this.radTest.UseVisualStyleBackColor = false;
            this.radTest.CheckedChanged += new System.EventHandler(this.radTest_CheckedChanged);
            // 
            // radNormal
            // 
            this.radNormal.AutoSize = true;
            this.radNormal.BackColor = System.Drawing.SystemColors.Control;
            this.radNormal.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radNormal.Location = new System.Drawing.Point(7, 17);
            this.radNormal.Name = "radNormal";
            this.radNormal.Size = new System.Drawing.Size(71, 16);
            this.radNormal.TabIndex = 0;
            this.radNormal.Text = "正式帐套";
            this.radNormal.UseVisualStyleBackColor = false;
            // 
            // tsbClose
            // 
            this.tsbClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbClose.Image = ((System.Drawing.Image)(resources.GetObject("tsbClose.Image")));
            this.tsbClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Size = new System.Drawing.Size(36, 22);
            this.tsbClose.Text = "退出";
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
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
            this.splitContainer1.Panel1.Controls.Add(this.cbCountReGetFromInterface);
            this.splitContainer1.Panel1.Controls.Add(this.chkSum);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.FDate2);
            this.splitContainer1.Panel1.Controls.Add(this.FDate1);
            this.splitContainer1.Panel1.Controls.Add(this.chkIgnoreZero);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.cboBillType);
            this.splitContainer1.Panel1.Controls.Add(this.bindingNavigator1);
            this.splitContainer1.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel1_Paint);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lblWait);
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView1);
            this.splitContainer1.Size = new System.Drawing.Size(1284, 856);
            this.splitContainer1.SplitterDistance = 94;
            this.splitContainer1.TabIndex = 3;
            // 
            // cbCountReGetFromInterface
            // 
            this.cbCountReGetFromInterface.AutoSize = true;
            this.cbCountReGetFromInterface.Location = new System.Drawing.Point(801, 60);
            this.cbCountReGetFromInterface.Name = "cbCountReGetFromInterface";
            this.cbCountReGetFromInterface.Size = new System.Drawing.Size(180, 16);
            this.cbCountReGetFromInterface.TabIndex = 32;
            this.cbCountReGetFromInterface.Text = "统计数量时，重新从接口取数";
            this.cbCountReGetFromInterface.UseVisualStyleBackColor = true;
            // 
            // chkSum
            // 
            this.chkSum.AutoSize = true;
            this.chkSum.Location = new System.Drawing.Point(975, 37);
            this.chkSum.Name = "chkSum";
            this.chkSum.Size = new System.Drawing.Size(84, 16);
            this.chkSum.TabIndex = 31;
            this.chkSum.Text = "只显示汇总";
            this.chkSum.UseVisualStyleBackColor = true;
            this.chkSum.CheckedChanged += new System.EventHandler(this.chkSum_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(519, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 30;
            this.label3.Text = "结束日期";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(215, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 29;
            this.label2.Text = "开始日期";
            // 
            // FDate2
            // 
            this.FDate2.Location = new System.Drawing.Point(589, 37);
            this.FDate2.Name = "FDate2";
            this.FDate2.Size = new System.Drawing.Size(180, 21);
            this.FDate2.TabIndex = 28;
            this.FDate2.Value = new System.DateTime(2024, 1, 21, 0, 0, 0, 0);
            this.FDate2.ValueChanged += new System.EventHandler(this.FDate2_ValueChanged);
            // 
            // FDate1
            // 
            this.FDate1.Location = new System.Drawing.Point(291, 37);
            this.FDate1.Name = "FDate1";
            this.FDate1.Size = new System.Drawing.Size(177, 21);
            this.FDate1.TabIndex = 27;
            this.FDate1.Value = new System.DateTime(2024, 1, 1, 0, 0, 0, 0);
            this.FDate1.ValueChanged += new System.EventHandler(this.FDate1_ValueChanged);
            // 
            // chkIgnoreZero
            // 
            this.chkIgnoreZero.AutoSize = true;
            this.chkIgnoreZero.Checked = true;
            this.chkIgnoreZero.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIgnoreZero.Location = new System.Drawing.Point(801, 37);
            this.chkIgnoreZero.Name = "chkIgnoreZero";
            this.chkIgnoreZero.Size = new System.Drawing.Size(138, 16);
            this.chkIgnoreZero.TabIndex = 26;
            this.chkIgnoreZero.Text = "隐藏数据为 0 的记录";
            this.chkIgnoreZero.UseVisualStyleBackColor = true;
            this.chkIgnoreZero.CheckedChanged += new System.EventHandler(this.chkIgnoreZero_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 14;
            this.label1.Text = "单据类型";
            // 
            // cboBillType
            // 
            this.cboBillType.FormattingEnabled = true;
            this.cboBillType.Items.AddRange(new object[] {
            "采购入库单",
            "销售出库单",
            "直接调拨单"});
            this.cboBillType.Location = new System.Drawing.Point(67, 38);
            this.cboBillType.Name = "cboBillType";
            this.cboBillType.Size = new System.Drawing.Size(121, 20);
            this.cboBillType.TabIndex = 13;
            this.cboBillType.SelectedIndexChanged += new System.EventHandler(this.cboBillType_SelectedIndexChanged);
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = null;
            this.bindingNavigator1.BindingSource = this.bindingSource1;
            this.bindingNavigator1.CountItem = this.bindingNavigatorCountItem;
            this.bindingNavigator1.DeleteItem = null;
            this.bindingNavigator1.Dock = System.Windows.Forms.DockStyle.None;
            this.bindingNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.tsbFilter,
            this.tsbRefresh,
            this.tbCalculate,
            this.tbCatchCountByDate,
            this.tsbClose});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 0);
            this.bindingNavigator1.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.bindingNavigator1.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.bindingNavigator1.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.bindingNavigator1.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = this.bindingNavigatorPositionItem;
            this.bindingNavigator1.Size = new System.Drawing.Size(506, 25);
            this.bindingNavigator1.TabIndex = 0;
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
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveLastItem.Text = "移到最后一条记录";
            // 
            // tsbRefresh
            // 
            this.tsbRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbRefresh.Image = ((System.Drawing.Image)(resources.GetObject("tsbRefresh.Image")));
            this.tsbRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRefresh.Name = "tsbRefresh";
            this.tsbRefresh.Size = new System.Drawing.Size(36, 22);
            this.tsbRefresh.Text = "刷新";
            this.tsbRefresh.Click += new System.EventHandler(this.tsbRefresh_Click);
            // 
            // tbCalculate
            // 
            this.tbCalculate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tbCalculate.Image = ((System.Drawing.Image)(resources.GetObject("tbCalculate.Image")));
            this.tbCalculate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbCalculate.Name = "tbCalculate";
            this.tbCalculate.Size = new System.Drawing.Size(60, 22);
            this.tbCalculate.Text = "重新计算";
            this.tbCalculate.Click += new System.EventHandler(this.tbCalculate_Click);
            // 
            // tbCatchCountByDate
            // 
            this.tbCatchCountByDate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tbCatchCountByDate.Image = ((System.Drawing.Image)(resources.GetObject("tbCatchCountByDate.Image")));
            this.tbCatchCountByDate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbCatchCountByDate.Name = "tbCatchCountByDate";
            this.tbCatchCountByDate.Size = new System.Drawing.Size(132, 22);
            this.tbCatchCountByDate.Text = "用接口获取每天单据数";
            this.tbCatchCountByDate.Click += new System.EventHandler(this.tbCatchCountByDate_Click);
            // 
            // lblWait
            // 
            this.lblWait.AutoSize = true;
            this.lblWait.BackColor = System.Drawing.SystemColors.HotTrack;
            this.lblWait.Font = new System.Drawing.Font("宋体", 42F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblWait.ForeColor = System.Drawing.Color.Yellow;
            this.lblWait.Location = new System.Drawing.Point(369, 284);
            this.lblWait.Name = "lblWait";
            this.lblWait.Size = new System.Drawing.Size(668, 56);
            this.lblWait.TabIndex = 42;
            this.lblWait.Text = "资料处理中，请稍候.....";
            this.lblWait.Visible = false;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FormId,
            this.日期,
            this.WDTCount,
            this.K3Count,
            this.ErrorCount});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1284, 758);
            this.dataGridView1.TabIndex = 15;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // FormId
            // 
            this.FormId.DataPropertyName = "FormId";
            this.FormId.HeaderText = "单据类型";
            this.FormId.Name = "FormId";
            this.FormId.ReadOnly = true;
            // 
            // 日期
            // 
            this.日期.DataPropertyName = "FDate";
            this.日期.HeaderText = "单据日期";
            this.日期.Name = "日期";
            this.日期.Width = 200;
            // 
            // WDTCount
            // 
            this.WDTCount.DataPropertyName = "WDTCount";
            this.WDTCount.HeaderText = "业务系统单据数";
            this.WDTCount.Name = "WDTCount";
            this.WDTCount.Width = 200;
            // 
            // K3Count
            // 
            this.K3Count.DataPropertyName = "K3Count";
            this.K3Count.HeaderText = "同步成功数";
            this.K3Count.Name = "K3Count";
            this.K3Count.Width = 200;
            // 
            // ErrorCount
            // 
            this.ErrorCount.DataPropertyName = "ErrorCount";
            this.ErrorCount.HeaderText = "同步错误数";
            this.ErrorCount.Name = "ErrorCount";
            this.ErrorCount.Width = 200;
            // 
            // frmCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1284, 856);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmCheck";
            this.Text = "同步进度检查";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmCheck_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.ToolStripButton tsbFilter;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radTest;
        private System.Windows.Forms.RadioButton radNormal;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboBillType;
        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.Label lblWait;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.CheckBox chkIgnoreZero;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker FDate2;
        private System.Windows.Forms.DateTimePicker FDate1;
        private System.Windows.Forms.CheckBox chkSum;
        private System.Windows.Forms.ToolStripButton tsbRefresh;
        private System.Windows.Forms.ToolStripButton tbCalculate;
        private System.Windows.Forms.ToolStripButton tbCatchCountByDate;
        private System.Windows.Forms.CheckBox cbCountReGetFromInterface;
        private System.Windows.Forms.DataGridViewTextBoxColumn FormId;
        private System.Windows.Forms.DataGridViewTextBoxColumn 日期;
        private System.Windows.Forms.DataGridViewTextBoxColumn WDTCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn K3Count;
        private System.Windows.Forms.DataGridViewTextBoxColumn ErrorCount;
    }
}