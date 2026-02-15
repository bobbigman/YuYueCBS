using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ahu.YuYue.CBS 
{
    public struct K3DatabaseMode
    {
        public const int Design = 1;
        public const int Test = 2;
        public const int Normal = 3;
        public const int IntegrationK3 = 4;
    }


    public struct struct_K3LoginTest
    {
        public const string ServerURL = "http://183.238.177.139:58871/k3cloud/";
        public const string AcctID = "68df6221388d56";
        public const string Username = "bob";
        public const string Password = "test1234.0";
    }


    public struct struct_K3LoginNormal
    {
        public const string ServerURL = "http://183.238.177.139:58871/k3cloud/";
        public const string AcctID = "68df6221388d56";
        public const string Username = "bob";
        public const string Password = "test1234.0";
    }

    class stru_k3login
    {

    }
}
