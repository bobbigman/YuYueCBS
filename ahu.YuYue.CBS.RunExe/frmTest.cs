using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.CodeDom.Compiler;
using System.Reflection;
using Microsoft.CSharp;

namespace B.CBS2MiddleTable.RunExe
{
    public partial class frmTest : Form
    {
        public frmTest()
        {
            InitializeComponent();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            //bobRun();

            string CSharpCode = rtbCSharpCode.Text;

            string strReadK3Value = "{c#}\"{ FNUMBER}\".Substring(1, 7);";
            strReadK3Value = "\"abcdefghi\".Substring(1, 7);";
            string strReadK3Value2 = ComplierCode(strReadK3Value).ToString();


        }

        private void bobRun()
        {
            string strA = "1";
            switch (strA)
            {
                case "1":
                    MessageBox.Show("找到了，匹配到：" + strA);
                    break;
                default:
                    MessageBox.Show("匹配不到：" + strA + ",请检查。");
                    break;

            }
        }


        private void btnRun_ClickBak(object sender, EventArgs e)
        {
            try
            {
                // 假设你的RichTextBox的名称为richTextBox1  
                string code = rtbCSharpCode.Text;

                // 创建一个C#代码提供程序  
                CSharpCodeProvider provider = new CSharpCodeProvider();

                // 编译参数（例如输出程序集的位置）  
                CompilerParameters parameters = new CompilerParameters
                {
                    GenerateExecutable = false, // 生成DLL而不是EXE  
                    GenerateInMemory = true, // 在内存中生成DLL  
                    ReferencedAssemblies =
                {  
                    // 添加你需要的引用，例如System.dll等  
                    "System.dll",  
                    // ... 其他必要的引用  
                }
                };

                // 编译代码  
                CompilerResults results = provider.CompileAssemblyFromSource(parameters, code);

                // 检查编译错误  
                if (results.Errors.HasErrors)
                {
                    StringBuilder errorMessages = new StringBuilder();
                    foreach (CompilerError error in results.Errors)
                    {
                        errorMessages.AppendLine(string.Format("Error ({0}): {1}", error.ErrorNumber, error.ErrorText));
                    }
                    MessageBox.Show(errorMessages.ToString(), "Compile Errors");
                    return;
                }

                // 如果没有错误，则加载生成的程序集并执行其中的方法（你需要知道要执行哪个方法）  
                // 这通常涉及到反射来调用程序集中的方法  
                // 注意：这里只是一个示例，你可能需要根据你的代码来调用具体的方法  
                // 例如：Assembly assembly = results.CompiledAssembly;  
                // 然后使用反射来调用assembly中的方法或类型  

                MessageBox.Show("Code compiled and executed successfully!"); // 只是一个示例消息  
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Executing Code");
            }
        }

        private static object ComplierCode(string expression)
        {
            string code = WrapExpression(expression);

            CSharpCodeProvider csharpCodeProvider = new CSharpCodeProvider();

            //编译的参数
            CompilerParameters compilerParameters = new CompilerParameters();
            //compilerParameters.ReferencedAssemblies.AddRange();
            compilerParameters.CompilerOptions = "/t:library";
            compilerParameters.GenerateInMemory = true;
            //开始编译
            CompilerResults compilerResults = csharpCodeProvider.CompileAssemblyFromSource(compilerParameters, code);
            if (compilerResults.Errors.Count > 0)
                throw new Exception("编译出错！");

            Assembly assembly = compilerResults.CompiledAssembly;
            Type type = assembly.GetType("ExpressionCalculate");
            MethodInfo method = type.GetMethod("Calculate");
            return method.Invoke(null, null);
        }
        private static string WrapExpression(string expression)
        {
            string code = @"
                using System;

                class ExpressionCalculate
                {
                    public static DateTime start_dt = Convert.ToDateTime(""{start_dt}"");
                    public static DateTime end_dt = Convert.ToDateTime(""{end_dt}"");
                    public static DateTime current_dt = DateTime.Now;

                    public static object Calculate()
                    {
                        return {0};
                    }
                }
            ";

            return code.Replace("{0}", expression);
        }
    }
}
