
namespace ahu.YuYue.CBS.RunExe
{
    partial class frmUpdateSQL
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
            this.tsmiRunOld = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiUpdateSQL = new System.Windows.Forms.ToolStripMenuItem();
            this.tbmiFormula = new System.Windows.Forms.ToolStripMenuItem();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.richSQL = new System.Windows.Forms.RichTextBox();
            this.richResult = new System.Windows.Forms.RichTextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiRunOld,
            this.tsmiUpdateSQL,
            this.tbmiFormula,
            this.退出ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1034, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // tsmiRunOld
            // 
            this.tsmiRunOld.Name = "tsmiRunOld";
            this.tsmiRunOld.Size = new System.Drawing.Size(68, 21);
            this.tsmiRunOld.Text = "运行测试";
            this.tsmiRunOld.Click += new System.EventHandler(this.tsmiRun_Click);
            // 
            // tsmiUpdateSQL
            // 
            this.tsmiUpdateSQL.Name = "tsmiUpdateSQL";
            this.tsmiUpdateSQL.Size = new System.Drawing.Size(67, 21);
            this.tsmiUpdateSQL.Text = "更新SQL";
            this.tsmiUpdateSQL.Click += new System.EventHandler(this.tsmiUpdateSQL_Click);
            // 
            // tbmiFormula
            // 
            this.tbmiFormula.Name = "tbmiFormula";
            this.tbmiFormula.Size = new System.Drawing.Size(68, 21);
            this.tbmiFormula.Text = "更新公式";
            this.tbmiFormula.Click += new System.EventHandler(this.tbmiFormula_Click);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.richSQL);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.richResult);
            this.splitContainer4.Size = new System.Drawing.Size(1034, 751);
            this.splitContainer4.SplitterDistance = 608;
            this.splitContainer4.TabIndex = 5;
            // 
            // richSQL
            // 
            this.richSQL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richSQL.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richSQL.Location = new System.Drawing.Point(0, 0);
            this.richSQL.Name = "richSQL";
            this.richSQL.Size = new System.Drawing.Size(608, 751);
            this.richSQL.TabIndex = 0;
            this.richSQL.Text = "";
            // 
            // richResult
            // 
            this.richResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richResult.Location = new System.Drawing.Point(0, 0);
            this.richResult.Name = "richResult";
            this.richResult.Size = new System.Drawing.Size(422, 751);
            this.richResult.TabIndex = 1;
            this.richResult.Text = "";
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
            this.splitContainer1.Panel1.Controls.Add(this.menuStrip1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer4);
            this.splitContainer1.Size = new System.Drawing.Size(1034, 785);
            this.splitContainer1.SplitterDistance = 30;
            this.splitContainer1.TabIndex = 7;
            // 
            // frmUpdateSQL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1034, 785);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmUpdateSQL";
            this.Text = "SQL源-中间表";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmiRunOld;
        private System.Windows.Forms.ToolStripMenuItem tsmiUpdateSQL;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.RichTextBox richSQL;
        private System.Windows.Forms.RichTextBox richResult;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripMenuItem tbmiFormula;
    }
}