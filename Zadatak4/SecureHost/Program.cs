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
            SecureHostConfig hostConfig = new SecureHostConfig()
            {
                AuthenticationType = WCFCommons.EAuthType.Cert,
                Ip = "0.0.0.0",
                Port = 12354
            };

            SecureHost host = new SecureHost(hostConfig);
            Console.WriteLine("Opening secure host...");
            host.Open();
            Console.WriteLine("Host opened. Press key to close");
            Console.Read();
        }
    }
}
