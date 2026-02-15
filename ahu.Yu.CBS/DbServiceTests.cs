using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.Util;
using Kingdee.BOS.WebApi.Client;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
//using NUnit.Framework;
using System;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
//using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace ahu.YuYue.CBS 
{
    /// <summary>
    /// 【WebApi】访问数据库
    /// </summary>

    [Kingdee.BOS.Util.HotUpdate]
    public class DbServiceTests
    {

        /// <summary>
        /// 执行SQL并返回受影响的行数
        /// </summary>

        public string ExecuteScalar(K3CloudApiClient pK3CloudApiClient
           , string pSQL,ref string pError
            )
        {

            #region
            if (pSQL.IndexOf("dialect") == -1)
                pSQL = "/*dialect*/" + Environment.NewLine + pSQL;

            var encryptSql = EncryptDecryptUtil.Encode(pSQL);
            try
            {
                string rval = pK3CloudApiClient.Execute<string>("KingdeeInterface.DbService.DoSthScalar,KingdeeInterface", new object[] { encryptSql });
                return rval;
            }
            catch (Exception ex)
            {
                string strError = CsErrLog.GetExceptionInfo(ex);
                string strError2 = ExtractSQLError(strError);
                if (strError2 == "")
                    strError2 = strError;

                strError2 = "ExecuteScalar" + Environment.NewLine + strError2;
                strError2 += Environment.NewLine + pSQL;
                pError = strError2;
                return null;
                //throw new Exception(strError2);
            }
            #endregion
        }
        public int Execute(K3CloudApiClient pK3CloudApiClient
           , string pSQL, ref string pError
            )
        {

            #region
            //不加，不行；加多了，也不行。干脆，如果加了，拿掉，然后，我统一再加上一次。
            pSQL = pSQL.Replace("/*dialect*/", "");
            pSQL = "/*dialect*/" + Environment.NewLine + pSQL;

            var encryptSql = EncryptDecryptUtil.Encode(pSQL);
            try
            {
                int intReturn = pK3CloudApiClient.Execute<int>("KingdeeInterface.DbService.DoSth,KingdeeInterface", new object[] { encryptSql });
                return intReturn;
            }
            catch (Exception ex)
            {
                string strError = CsErrLog.GetExceptionInfo(ex);
                string strError2 = ExtractSQLError(strError);
                strError2 = "GetDataSet" + Environment.NewLine + strError2;
                strError2 += Environment.NewLine + pSQL;
                pError = strError2;
                return 0;
            }


            #endregion
        }
        /// <summary> 
        /// 执行SQL并返回查询结果
        /// </summary>


        //这个很有用，绑定到dataGridView用。
        //public DataSet GetDataSet(K3CloudApiClient pK3CloudApiClient
        //   , string pSQL, ref string pError
        //    )
        //{
        //    // 登录
        //    #region
        //    if (pSQL.IndexOf("dialect") == -1)
        //        pSQL = "/*dialect*/" + Environment.NewLine + pSQL;

        //    var encryptSql = EncryptDecryptUtil.Encode(pSQL);
        //    //Console.WriteLine("请求数据包：" + encryptSql);

        //    try
        //    {
        //        var ds = pK3CloudApiClient.Execute<DataSet>("KingdeeInterface.DbService.GetDataSet,KingdeeInterface", new object[] { encryptSql });
        //        return ds;
        //    }

        //    catch (Exception ex)
        //    {
        //        string strError = CsErrLog.GetExceptionInfo(ex);
        //        string strError2 = ExtractSQLError(strError);
        //        if (strError2 == "")
        //            strError2 = strError;

        //        strError2 = "GetDataSet" + Environment.NewLine + strError2;
        //        strError2 += Environment.NewLine + pSQL;
        //        pError = strError2;
        //        return null;
        //    }
        //    #endregion

        //}

        private string ExtractSQLError(string input)
        {
            // 正则表达式匹配 "错误信息：" 和 "调用堆栈：" 之间的内容  
            string strPattern = "错误信息：([^\r\n]*?)调用堆栈：";
            Match match = Regex.Match(input, @strPattern, RegexOptions.Singleline);
            if (match.Success == false)
            {
                strPattern = @"错误信息：([^""\r\n]*)";
                match = Regex.Match(input, strPattern, RegexOptions.Singleline);
            }

            if (match.Success == false)
            {//Message:未将对象引用设置到对象的实例。,Source
                strPattern = "Message:([^\r\n]*?),Source";
                match = Regex.Match(input, strPattern, RegexOptions.Singleline);
                //return null;
            }

            if (match.Success == false)
                return "";


            string strReturn = match.Value;
            strReturn = strReturn.Replace("调用堆栈：", "");
            //去掉乱码。&#39; 是HTML实体编码，用于在HTML中显示单引号（'）
            string decodedErrorMessage = System.Web.HttpUtility.HtmlDecode(strReturn);
            return decodedErrorMessage; // 返回匹配到的内容  

        }

        /// </summary>
        public DynamicObjectCollection GetDynamicObject(K3CloudApiClient pK3CloudApiClient
           , string pSQL, ref string pError
            )
        {

            string directory1 = AppDomain.CurrentDomain.BaseDirectory;

            string filePath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            string directory2 = System.IO.Path.GetDirectoryName(filePath);

            // 登录
            // 查询用户信息
            //string sql = "SELECT TOP 3 FUSERID,FNAME FROM T_SEC_USER";
            if (pSQL.IndexOf("dialect") == -1)
                pSQL = "/*dialect*/" + Environment.NewLine + pSQL;

            var encryptSql = EncryptDecryptUtil.Encode(pSQL);
            //Console.WriteLine("请求数据包：" + encryptSql);
            //string rval = K3CloudApiClient1.Execute<string>("KingdeeInterface.DbService.GetDynamicObject,KingdeeInterface", new object[] { encryptSql });

            string strK3Return = "";
            try
            {
                strK3Return = pK3CloudApiClient.Execute<string>("KingdeeInterface.DbService.GetDynamicObject,KingdeeInterface", new object[] { encryptSql });
            }
            catch (Exception ex)
            {
                string strError = CsErrLog.GetExceptionInfo(ex);
                string strError2 = ExtractSQLError(strError);
                if (strError2 == "")
                    strError2 = strError;

                strError2 = "GetDynamicObject" + Environment.NewLine + strError2;
                strError2 += Environment.NewLine + pSQL;
                pError = strError2;
                return null;
                //throw new Exception(strError2);
            }

            DynamicObjectCollection docK3Data;

            string strK3Data = JsonConvert.DeserializeObject<string>(strK3Return);

            byte[] byteArrayK3Data = Convert.FromBase64String(strK3Data);
            using (MemoryStream stream = new MemoryStream(byteArrayK3Data))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                docK3Data = (DynamicObjectCollection)formatter.Deserialize(stream);
            }
            return docK3Data;
        }

        public  DataTable GetDataTableByDynamicOjbect(DynamicObjectCollection doc1)

        {
            DataTable dataTable1 = new DataTable();
            //增加单据头。
            foreach (DynamicObject do1 in doc1)
            {
                var props = do1.DynamicObjectType.Properties;
                foreach (var prop in props)
                {
                    string strField1 = prop.Name;
                    string strPropertyType = prop.PropertyType.Name;
                    strPropertyType = "System." + strPropertyType;
                    dataTable1.Columns.Add(strField1, System.Type.GetType(strPropertyType));

                    //if (strPropertyType == "Boolean")
                    //    dataTable1.Columns.Add(strField1, System.Type.GetType("System.Boolean"));
                    //else
                    //    dataTable1.Columns.Add(strField1);
                }
                break;
            }

            foreach (DynamicObject do1 in doc1)
            {
                // 创建数据行，并为行的各个列赋值
                DataRow dataRow = dataTable1.NewRow();

                var props = do1.DynamicObjectType.Properties;
                foreach (var prop in props)
                {
                    string strField1 = prop.Name;
                    string strFieldValue1 = "";
                    if (ObjectUtils.IsNullOrEmpty(do1[strField1]) == false)
                        strFieldValue1 = do1[strField1].ToString();

                    dataRow[strField1] = strFieldValue1;
                }
                dataTable1.Rows.Add(dataRow);

            }


            return dataTable1;
        }


    }
}
