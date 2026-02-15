
namespace RunExe
{
    partial class frmSync2LC
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
            this.radNormal = new System.Windows.Forms.RadioButton();
            this.radTest = new System.Windows.Forms.RadioButton();
            this.btnJsonTranslate = new System.Windows.Forms.Button();
            this.btnJson = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.rtbResult = new System.Windows.Forms.RichTextBox();
            this.lblBillNo = new System.Windows.Forms.Label();
            this.txtBillNo = new System.Windows.Forms.TextBox();
            this.lblBillType = new System.Windows.Forms.Label();
            this.cboBillType = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radDelete = new System.Windows.Forms.RadioButton();
            this.rad2NewOrEdit = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
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
            // 
            // radTest
            // 
            this.radTest.AutoSize = true;
            this.radTest.Checked = true;
            this.radTest.Location = new System.Drawing.Point(100, 20);
            this.radTest.Name = "radTest";
            this.radTest.Size = new System.Drawing.Size(47, 16);
            this.radTest.TabIndex = 1;
            this.radTest.TabStop = true;
            this.radTest.Text = "测试";
            this.radTest.UseVisualStyleBackColor = true;
            // 
            // btnJsonTranslate
            // 
            this.btnJsonTranslate.Location = new System.Drawing.Point(213, 552);
            this.btnJsonTranslate.Name = "btnJsonTranslate";
            this.btnJsonTranslate.Size = new System.Drawing.Size(133, 23);
            this.btnJsonTranslate.TabIndex = 23;
            this.btnJsonTranslate.Text = "同步报文，翻译后";
            this.btnJsonTranslate.UseVisualStyleBackColor = true;
            this.btnJsonTranslate.Click += new System.EventHandler(this.btnJsonTranslate_Click);
            // 
            // btnJson
            // 
            this.btnJson.Location = new System.Drawing.Point(77, 552);
            this.btnJson.Name = "btnJson";
            this.btnJson.Size = new System.Drawing.Size(75, 23);
            this.btnJson.TabIndex = 22;
            this.btnJson.Text = "同步报文";
            this.btnJson.UseVisualStyleBackColor = true;
            this.btnJson.Click += new System.EventHandler(this.btnJson_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radNormal);
            this.groupBox1.Controls.Add(this.radTest);
            this.groupBox1.Location = new System.Drawing.Point(43, 107);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 52);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(213, 499);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 20;
            this.btnExit.Text = "退出";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click_1);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(77, 499);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 19;
            this.btnRun.Text = "开始同步";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(463, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 18;
            this.label3.Text = "同步结果";
            // 
            // rtbResult
            // 
            this.rtbResult.Location = new System.Drawing.Point(447, 152);
            this.rtbResult.Name = "rtbResult";
            this.rtbResult.Size = new System.Drawing.Size(610, 573);
            this.rtbResult.TabIndex = 17;
            this.rtbResult.Text = "";
            // 
            // lblBillNo
            // 
            this.lblBillNo.AutoSize = true;
            this.lblBillNo.Location = new System.Drawing.Point(51, 321);
            this.lblBillNo.Name = "lblBillNo";
            this.lblBillNo.Size = new System.Drawing.Size(53, 12);
            this.lblBillNo.TabIndex = 16;
            this.lblBillNo.Text = "单据编号";
            // 
            // txtBillNo
            // 
            this.txtBillNo.Location = new System.Drawing.Point(143, 317);
            this.txtBillNo.Name = "txtBillNo";
            this.txtBillNo.Size = new System.Drawing.Size(192, 21);
            this.txtBillNo.TabIndex = 15;
            this.txtBillNo.Text = "23453";
            // 
            // lblBillType
            // 
            this.lblBillType.AutoSize = true;
            this.lblBillType.Location = new System.Drawing.Point(50, 278);
            this.lblBillType.Name = "lblBillType";
            this.lblBillType.Size = new System.Drawing.Size(53, 12);
            this.lblBillType.TabIndex = 14;
            this.lblBillType.Text = "单据类型";
            // 
            // cboBillType
            // 
            this.cboBillType.FormattingEnabled = true;
            this.cboBillType.Items.AddRange(new object[] {
            "部门",
            "银行账号",
            "凭证"});
            this.cboBillType.Location = new System.Drawing.Point(143, 274);
            this.cboBillType.Name = "cboBillType";
            this.cboBillType.Size = new System.Drawing.Size(192, 20);
            this.cboBillType.TabIndex = 13;
            this.cboBillType.Text = "凭证";
            this.cboBillType.SelectedIndexChanged += new System.EventHandler(this.cboBillType_SelectedIndexChanged_1);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radDelete);
            this.groupBox2.Controls.Add(this.rad2NewOrEdit);
            this.groupBox2.Location = new System.Drawing.Point(43, 189);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(262, 52);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.TabStop = false;
            // 
            // radDelete
            // 
            this.radDelete.AutoSize = true;
            this.radDelete.Location = new System.Drawing.Point(170, 20);
            this.radDelete.Name = "radDelete";
            this.radDelete.Size = new System.Drawing.Size(47, 16);
            this.radDelete.TabIndex = 2;
            this.radDelete.Text = "删除";
            this.radDelete.UseVisualStyleBackColor = true;
            // 
            // rad2NewOrEdit
            // 
            this.rad2NewOrEdit.AutoSize = true;
            this.rad2NewOrEdit.BackColor = System.Drawing.SystemColors.Control;
            this.rad2NewOrEdit.Checked = true;
            this.rad2NewOrEdit.ForeColor = System.Drawing.SystemColors.ControlText;
            this.rad2NewOrEdit.Location = new System.Drawing.Point(34, 20);
            this.rad2NewOrEdit.Name = "rad2NewOrEdit";
            this.rad2NewOrEdit.Size = new System.Drawing.Size(83, 16);
            this.rad2NewOrEdit.TabIndex = 0;
            this.rad2NewOrEdit.TabStop = true;
            this.rad2NewOrEdit.Text = "新增或修改";
            this.rad2NewOrEdit.UseVisualStyleBackColor = false;
            // 
            // frmSync2LC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 833);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnJsonTranslate);
            this.Controls.Add(this.btnJson);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.rtbResult);
            this.Controls.Add(this.lblBillNo);
            this.Controls.Add(this.txtBillNo);
            this.Controls.Add(this.lblBillType);
            this.Controls.Add(this.cboBillType);
            this.Name = "frmSync2LC";
            this.Text = "金蝶同步到中航物业";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radNormal;
        private System.Windows.Forms.RadioButton radTest;
        private System.Windows.Forms.Button btnJsonTranslate;
        private System.Windows.Forms.Button btnJson;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox rtbResult;
        private System.Windows.Forms.Label lblBillNo;
        private System.Windows.Forms.TextBox txtBillNo;
        private System.Windows.Forms.Label lblBillType;
        private System.Windows.Forms.ComboBox cboBillType;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radDelete;
        private System.Windows.Forms.RadioButton rad2NewOrEdit;
    }
}