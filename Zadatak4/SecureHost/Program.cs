using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureHost
{
    class Program
    {
        static void Main(string[] args)
        {
            SecureHostConfig winConfig = new SecureHostConfig()
            {
                AuthenticationType = WCFCommons.EAuthType.Windows,
                Ip = "0.0.0.0",
                Port = 12354
            };

            SecureHostConfig certConfig = new SecureHostConfig()
            {
                AuthenticationType = WCFCommons.EAuthType.Cert,
                Ip = "0.0.0.0",
                Port = 12355
            };

            Const.SourceName = "Projekat 4/" + (hostConfig.AuthenticationType == EAuthType.Windows ? "Win" : "Cert")

            SecureHost winHost = new SecureHost(winConfig);
            SecureHost certHost = new SecureHost(certConfig);
            Console.WriteLine("Opening secure host...");
            winHost.Open();
            certHost.Open();

            Console.WriteLine("Host opened. Press key to close");
            Console.Read();
            


            //CommonService cs = new CommonService();
            //cs.Modify("TestFile.txt", "Failed Test", WCFCommons.EModifyType.Overwrite);
            /*cs.Create("TestFile.txt");
            cs.Modify("TestFile.txt", "Test 1", WCFCommons.EModifyType.Append);
            cs.Modify("TestFile.txt", "Test 2", WCFCommons.EModifyType.Append);
            cs.Modify("TestFile.txt", "Test 3", WCFCommons.EModifyType.Overwrite);
            Console.WriteLine(cs.Read("TestFile.txt"));
            cs.Delete("TestFile.txt");*/
        }
    }
}
