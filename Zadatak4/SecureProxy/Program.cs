using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WCFCommons;

namespace SecureProxy
{
    class Program
    {
        static void Main(string[] args)
        {
            Processor p = new Processor(new WinAuthClient("net.tcp://localhost:12354/ICommonService"));

            p.Delete("test");
            p.Create("test");
            Console.Read();
        }
    }
}
