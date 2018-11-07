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

            Console.WriteLine("Wrong modify for testing purpouse");
            try
            {
                proxy.Modify("hopala", "jojo", EModifyType.Overwrite);
                Console.WriteLine("File modified");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
            }

            Console.WriteLine("Creating file...");
            try
            {
                proxy.Create("hopala");
                Console.WriteLine("File made");
            } catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
            }
            Console.ReadLine();
            Console.WriteLine("Modifying file...");
            try
            {
                proxy.Modify("hopala", "jojo", EModifyType.Overwrite);
                Console.WriteLine("File modified");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
            }

            Console.ReadLine();
            Console.WriteLine("Reading file...");
            try
            {
                Console.WriteLine(proxy.Read("hopala"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
            }

            Console.ReadLine();
            Console.WriteLine("Deleting file...");
            try
            {
                proxy.Delete("hopala");
                Console.WriteLine("File deleted");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
            }

            

            Console.WriteLine("All done!");
            Console.Read();
        }
    }
}
