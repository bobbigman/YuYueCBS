
namespace ahu.YuYue.CBS.RunExe
{
    partial class frmMenu
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsmi2Match = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmni2k3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.系统运维3DynamicObjectDatagridviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmniClose = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radNormal = new System.Windows.Forms.RadioButton();
            this.radTest = new System.Windows.Forms.RadioButton();
            this.lblWait = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi2Match,
            this.tsmni2k3,
            this.toolStripMenuItem4,
            this.tsmniClose});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1203, 25);
            this.menuStrip1.TabIndex = 43;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsmi2Match
            // 
            this.tsmi2Match.Name = "tsmi2Match";
            this.tsmi2Match.Size = new System.Drawing.Size(80, 21);
            this.tsmi2Match.Text = "字段对照表";
            this.tsmi2Match.Click += new System.EventHandler(this.tsmi2Match_Click);
            // 
            // tsmni2k3
            // 
            this.tsmni2k3.Name = "tsmni2k3";
            this.tsmni2k3.Size = new System.Drawing.Size(92, 21);
            this.tsmni2k3.Text = "同步到中间表";
            this.tsmni2k3.Click += new System.EventHandler(this.eRP同步ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.系统运维3DynamicObjectDatagridviewToolStripMenuItem});
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(68, 21);
            this.toolStripMenuItem4.Text = "系统运维";
            // 
            // 系统运维3DynamicObjectDatagridviewToolStripMenuItem
            // 
            this.系统运维3DynamicObjectDatagridviewToolStripMenuItem.Name = "系统运维3DynamicObjectDatagridviewToolStripMenuItem";
            this.系统运维3DynamicObjectDatagridviewToolStripMenuItem.Size = new System.Drawing.Size(311, 22);
            this.系统运维3DynamicObjectDatagridviewToolStripMenuItem.Text = "系统运维3(DynamicObject+Datagridview)";
            this.系统运维3DynamicObjectDatagridviewToolStripMenuItem.Click += new System.EventHandler(this.系统运维3DynamicObjectDatagridviewToolStripMenuItem_Click);
            // 
            // tsmniClose
            // 
            this.tsmniClose.Name = "tsmniClose";
            this.tsmniClose.Size = new System.Drawing.Size(44, 21);
            this.tsmniClose.Text = "退出";
            this.tsmniClose.Click += new System.EventHandler(this.tsmniClose_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radNormal);
            this.groupBox1.Controls.Add(this.radTest);
            this.groupBox1.Location = new System.Drawing.Point(-10, 169);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(271, 328);
            this.groupBox1.TabIndex = 44;
            this.groupBox1.TabStop = false;
            // 
            // radNormal
            // 
            this.radNormal.AutoSize = true;
            this.radNormal.BackColor = System.Drawing.SystemColors.Control;
            this.radNormal.Checked = true;
            this.radNormal.Font = new System.Drawing.Font("宋体", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radNormal.ForeColor = System.Drawing.Color.Black;
            this.radNormal.Location = new System.Drawing.Point(34, 80);
            this.radNormal.Name = "radNormal";
            this.radNormal.Size = new System.Drawing.Size(134, 52);
            this.radNormal.TabIndex = 0;
            this.radNormal.TabStop = true;
            this.radNormal.Text = "正式";
            this.radNormal.UseVisualStyleBackColor = false;
            this.radNormal.CheckedChanged += new System.EventHandler(this.radNormal_CheckedChanged);
            // 
            // radTest
            // 
            this.radTest.AutoSize = true;
            this.radTest.BackColor = System.Drawing.Color.Blue;
            this.radTest.Font = new System.Drawing.Font("宋体", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radTest.ForeColor = System.Drawing.Color.Yellow;
            this.radTest.Location = new System.Drawing.Point(34, 215);
            this.radTest.Name = "radTest";
            this.radTest.Size = new System.Drawing.Size(134, 52);
            this.radTest.TabIndex = 1;
            this.radTest.Text = "测试";
            this.radTest.UseVisualStyleBackColor = false;
            this.radTest.CheckedChanged += new System.EventHandler(this.radTest_CheckedChanged);
            // 
            // lblWait
            // 
            this.lblWait.AutoSize = true;
            this.lblWait.BackColor = System.Drawing.SystemColors.HotTrack;
            this.lblWait.Font = new System.Drawing.Font("宋体", 42F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblWait.ForeColor = System.Drawing.Color.Yellow;
            this.lblWait.Location = new System.Drawing.Point(533, 703);
            this.lblWait.Name = "lblWait";
            this.lblWait.Size = new System.Drawing.Size(724, 56);
            this.lblWait.TabIndex = 45;
            this.lblWait.Text = "服务器连接中，请稍候.....";
            this.lblWait.Visible = false;
            // 
            // frmMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1203, 745);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblWait);
            this.Name = "frmMenu";
            this.Text = "渝月，CBS同步到中间表";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMenu_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmi2Match;
        private System.Windows.Forms.ToolStripMenuItem tsmni2k3;
        private System.Windows.Forms.ToolStripMenuItem tsmniClose;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radNormal;
        private System.Windows.Forms.RadioButton radTest;
        private System.Windows.Forms.Label lblWait;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem 系统运维3DynamicObjectDatagridviewToolStripMenuItem;
    }
}