
namespace B.CBS2MiddleTable.RunExe
{
    partial class frmTest
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
            this.rtbCSharpCode = new System.Windows.Forms.RichTextBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rtbCSharpCode
            // 
            this.rtbCSharpCode.Location = new System.Drawing.Point(12, 12);
            this.rtbCSharpCode.Name = "rtbCSharpCode";
            this.rtbCSharpCode.Size = new System.Drawing.Size(807, 580);
            this.rtbCSharpCode.TabIndex = 0;
            this.rtbCSharpCode.Text = "";
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(22, 611);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(123, 45);
            this.btnRun.TabIndex = 1;
            this.btnRun.Text = "运行";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(164, 611);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(123, 45);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "退出";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // frmTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 668);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.rtbCSharpCode);
            this.Name = "frmTest";
            this.Text = "frmTest";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbCSharpCode;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnExit;
    }
}