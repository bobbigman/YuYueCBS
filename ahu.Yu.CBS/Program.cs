
//using Bob.ZeroGreen.WDT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ahu.YuYue.CBS 
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //改一下，改成Form,点按钮的情况。
                //每次改main，不方便啊。我想，拿到服务器上去测试。******************************************************

                //ClsSchedule clsSchedule1 = new ClsSchedule();
                //clsSchedule1.Run(null, null);

                //ClsSchedule_2MiddleTable_Count ClsSchedule_2MiddleTable_Count1 = new ClsSchedule_2MiddleTable_Count();
                //ClsSchedule_2MiddleTable_Count1.Run(null, null);

                //ClsSchedule_2MiddleTable ClsSchedule_2MiddleTable1 = new ClsSchedule_2MiddleTable();
                //ClsSchedule_2MiddleTable1.Run(null, null);

                //ClsSchedule2MiddleTableSum ClsSchedule_2MiddleTableSum1 = new ClsSchedule2MiddleTableSum();
                //ClsSchedule_2MiddleTableSum1.Run(null, null);

                //ClsSchedule_MiddleTableSum2K3 ClsSchedule_MiddleTableSum2K31 = new ClsSchedule_MiddleTableSum2K3();
                //ClsSchedule_MiddleTableSum2K31.Run(null, null);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                //Application.Run(new Form1());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw ex;
            }

        }


    }
}
