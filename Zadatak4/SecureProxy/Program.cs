using CertificateManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
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

            /*NetTcpBinding binding = new NetTcpBinding();

            binding.Security.Mode = SecurityMode.Transport;
            //binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            //binding.Security.Transport.ProtectionLevel = ProtectionLevel.EncryptAndSign

            // za cert auth:
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, "wcfservice");


            //string address = "net.tcp://localhost:12354/ICommonService";
            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:12354/ICommonService"),
                                      new X509CertificateEndpointIdentity(srvCert));


            ChannelFactory<ICommonService> factory = new ChannelFactory<ICommonService>(binding, address);

            factory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.ChainTrust;
            factory.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            //factory.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, "wcfclient");
            factory.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromFile(@"C:\Users\Sedmak\Desktop\cert\WCFClient.pfx");
            

            ICommonService proxy = factory.CreateChannel();

            Console.WriteLine("Trying Create");
            try
            {
                proxy.Create("hopala");
                Console.WriteLine("Sucess!\n");
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("Trying Delete");
            try
            {
                proxy.Delete("hopala");
                Console.WriteLine("Sucess!\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("Trying Modify");
            try
            {
                proxy.Modify("hopala", "jojo", EModifyType.Overwrite);
                Console.WriteLine("Sucess!\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("Trying Read");
            try
            {
                proxy.Read("hopala");
                Console.WriteLine("Sucess!\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }*/

            //Processor p = new Processor(new WinAuthClient("net.tcp://10.1.212.156:12354/ICommonService"));
            Processor p = new Processor(new CertClient("net.tcp://10.1.212.156:12354/ICommonService", "B2Z4wcfservice"));

            p.Create("test.txt");
            p.Modify("test.txt", "data", EModifyType.Append);
            string test;
            p.Read("test.txt", out test);
            Console.WriteLine(test);
            p.Delete("test.txt");
            Console.Read();
        }
    }
}
