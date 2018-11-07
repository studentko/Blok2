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
            ICommonService proxy = (new WinAuthClient("net.tcp://localhost:12354/ICommonService")).Proxy;

            try
            {
                proxy.Create("hopala");
            } catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
            }
            Console.ReadLine();
            try
            {
                proxy.Delete("hopala");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
            }

            Console.ReadLine();
            try
            {
                proxy.Modify("hopala", "jojo", EModifyType.Overwrite);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
            }

            Console.ReadLine();
            try
            {
                proxy.Read("hopala");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
            }

            Console.Read();
        }
    }
}
