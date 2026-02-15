using Kingdee.BOS;
using Kingdee.BOS.App.Data;
using Kingdee.BOS.Util;
using Kingdee.BOS.WebApi.Client;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ahu.YuYue.CBS 
{
    public class CsErrLog
    {
        public static void Error2SQL(Context pContext, K3CloudApiClient pK3CloudApiClient, string pError)
        {
            try
            {
                pError = pError.Replace("'", "''");
                string strSQL = "/*dialect*/" + System.Environment.NewLine +
                                " INSERT INTO [dbo].[tblError]" + System.Environment.NewLine +
                                " (Error) " + System.Environment.NewLine +
                                " VALUES " + System.Environment.NewLine +
                                "('" + pError + "')";

                //这里，就不要调用bob.execute了，怕死循环。
                if (pContext.IsNullOrEmptyOrWhiteSpace() == false)
                    DBUtils.Execute(pContext, strSQL);
                else if (pK3CloudApiClient.IsNullOrEmptyOrWhiteSpace() == false)
                {
                    string strError = "";
                    DbServiceTests DbServiceTests1 = new DbServiceTests();
                    DbServiceTests1.Execute(pK3CloudApiClient, strSQL, ref strError);
                    if (strError.IsNullOrEmptyOrWhiteSpace() == false)
                        throw new Exception(strError);

                    //SqlCommand sqlcomm = new SqlCommand(strSQL, pK3CloudApiClient);
                    //sqlcomm.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(CsErrLog.GetExceptionWithLog(ex));
            }
            finally
            {
            }

        }

        public void Exception2SQL(Context pContext, Exception pException, string pForm)
        {

            //注意，公有云读不到:StackTrace，不用绕那么大的弯。
            //throw new Exception(pException.Message + pForm);

            try
            {
                string strError;
                CsErrLog clsErrLog1 = new CsErrLog();
                StackTrace st = new StackTrace(pException, true);
                StackFrame[] frames = st.GetFrames();
                // Iterate over the frames extracting the information you need
                foreach (StackFrame frame in frames)
                {
                    strError = pException.Message + System.Environment.NewLine +
                        string.Format("{0}:{1}({2},{3})", frame.GetFileName(), frame.GetMethod().Name, frame.GetFileLineNumber(), frame.GetFileColumnNumber());
                    //clsErrLog1.Error2SQL(pContext, strError);
                    //下面会触发.
                    throw new Exception(strError);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }

        }

        public static string GetExceptionInfo(Exception pException)
        {
            try
            {
                string strErrors = "";
                //string strErrorsShort = "";
                string strError1 = "";
                //string strError1Short = "";
                CsErrLog clsErrLog1 = new CsErrLog();
                StackTrace st = new StackTrace(pException, true);
                StackFrame[] frames = st.GetFrames();
                // Iterate over the frames extracting the information you need

                foreach (StackFrame frame in frames)
                {
                    strError1 = string.Format("{0}:{1}({2},{3})", frame.GetFileName(), frame.GetMethod().Name, frame.GetFileLineNumber(), frame.GetFileColumnNumber());
                    if (strErrors.IndexOf(strError1) == -1)
                        strErrors = strErrors + Environment.NewLine + strError1;
                }

                //2023/01/11 8:50
                //问题，从前边取，还是从后边取呢？
                //算了，从后边开始取。

                //不要截断，SQL报错，分析不全。当是调用接口，执行SQL时。2024/1/15 15:53
                //strErrorsShort = strErrors;
                //if (strErrorsShort.Length > 300)
                //    strErrorsShort = strErrorsShort.Substring(strErrorsShort.Length - 300) + "后面省略千字啦.....";

                strError1 = pException.Message;
                //strError1Short = strError1;
                //if (strError1.Length > 700)
                //    strError1 = strError1.Substring(0, 700) + "前面省略千字啦.....";

                strErrors = strError1 + strErrors + "*";

                return strErrors;

            }
            catch (Exception ex)
            {
                throw ex;
                //throw new Exception(clsErrLog.GetExceptionWithLog(ex) + "*");
            }
            finally
            {
            }

        }

        public static string GetExceptionWithLog( Exception  pException)
        {
            try
            {
                string strError = GetExceptionInfo(pException);

                string strlogFile = ClsPublic.KingdeeLogPath + DateTime.Now.ToString("yyyy-MM-dd");
                ClsPublic.WriteLog(ClsPublic.KingdeeLogPath, strlogFile, strError);


                return strError;

            }
            catch (Exception ex)
            {
                throw ex;
                //throw new Exception(clsErrLog.GetExceptionWithLog(ex) + "*");
            }
            finally
            {
            }

        }
    }
}
