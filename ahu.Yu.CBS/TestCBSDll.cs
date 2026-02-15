using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ahu.YuYue.CBS 
{
    public class TestCBSDll
    {

        //static string domainName = "https://cbs8-openapi-reprd.csuat.cmburl.cn/";
        string mstrURLOthers = "https://tmcapi.cmbchina.com/";

        ////// 客户私钥，请替换实际客户私钥
        //string mstrCustomPrivateKey = @"C1A6D42B54E615B874F74BD4E4D555925CB2B9AF970119B197BD1BB73B8CED80";

        //// 平台公钥，请替换实际平台公钥
        //string mstrPlatformPublicKey = @"047ECD8453E890134F7B3844439BBD825F049639545A6551E9B4E91164FC1A661B78FCA80DE846A2CC05EBD900B67A44729277B745B84788C0231DF87E51AB0BD7";

        // 渝月，客户私钥，请替换实际客户私钥
        string mstrCustomPrivateKey = @"53b94b9844dabbca457ed05bf15c246615818bbabf6f144521de93d6f89e0153";

        // 渝月，平台公钥，请替换实际平台公钥
        string mstrPlatformPublicKey = @"04D7CF04F7E09F3E4335DBD430321A56A23954A032A443B6C72B24B78A528718CEADD52A68A5F9D125BCA8DEF3B66985926FFB55DE0DABC7B41CA01D5ED40874F0";

        string mstrAppId = @"FsDSfQLP";
        string mstrAppScret = "d959845e07176c3440a27737148fcc61d3375bb2";

        public void CallCBS_Demo(string pPartURL, string pJson,ref string pJsonResut,ref string pError
                )
        {
            string strFullUrl = mstrURLOthers + "cbs8-app-platform/app/oauth/token";
            string strToken = CsCallCBS.GetTokenBak(strFullUrl, ref pError, mstrAppId, mstrAppScret);
            if (pError != "")
                return ;

            strFullUrl = mstrURLOthers + pPartURL;
            CsCallCBS.setupRequest(strToken, pJson, strFullUrl, mstrPlatformPublicKey, mstrCustomPrivateKey,ref pJsonResut);

            return ;
        }




    }
}
