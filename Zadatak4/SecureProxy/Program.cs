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
            NetTcpBinding binding = new NetTcpBinding();

            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            //binding.Security.Transport.ProtectionLevel = ProtectionLevel.EncryptAndSign;

            string address = "net.tcp://localhost:12354/ICommonService";

            ChannelFactory<ICommonService> factory = new ChannelFactory<ICommonService>(binding, address);

            ICommonService proxy = factory.CreateChannel();

            try
            {
                proxy.Create("hopala");
            } catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
            }

            try
            {
                proxy.Delete("hopala");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
            }

            try
            {
                proxy.Modify("hopala", "jojo", EModifyType.Overwrite);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
            }

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
