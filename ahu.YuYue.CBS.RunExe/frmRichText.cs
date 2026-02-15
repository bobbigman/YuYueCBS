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
    public partial class frmRichText : Form
    {
        public frmRichText()
        {
            InitializeComponent();
        }

        private void 退部门ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void SetText(string pText)
        {
            richTextBox1.Text = pText;
            richTextBox1.Select(richTextBox1.TextLength, 0);  //光标定位到文本最后面。
            richTextBox1.ScrollToCaret();  //滚动到光标处。


        }


    }
}
