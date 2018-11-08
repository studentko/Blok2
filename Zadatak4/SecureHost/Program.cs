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

            SecureHost winHost = new SecureHost(winConfig);
            SecureHost certHost = new SecureHost(certConfig);
            Console.WriteLine("Opening secure host...");
            winHost.Open();
            certHost.Open();

            Console.WriteLine("Host opened. Press key to close");
            Console.Read();

            Console.WriteLine("Closing...");
            winHost.Close();
            certHost.Close();
        }
    }
}
