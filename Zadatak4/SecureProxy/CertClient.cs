using CertificateManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WCFCommons;

namespace SecureProxy
{
    class CertClient : IWCFClient
    {
        ICommonService proxy;

        public CertClient(string ipAddress, string serverCert)
        {
            NetTcpBinding binding = new NetTcpBinding();

            binding.Security.Mode = SecurityMode.Transport;
            //binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            //binding.Security.Transport.ProtectionLevel = ProtectionLevel.EncryptAndSign

            // za cert auth:
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, serverCert);


            //string address = "net.tcp://localhost:12354/ICommonService";
            EndpointAddress address = new EndpointAddress(new Uri(ipAddress),
                                      new X509CertificateEndpointIdentity(srvCert));


            ChannelFactory<ICommonService> factory = new ChannelFactory<ICommonService>(binding, address);

            factory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.ChainTrust;
            factory.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            factory.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.CurrentUser, Formatter.ParseName(WindowsIdentity.GetCurrent().Name));
            //factory.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromFile(@"C:\novi\B2Z4WCFClient.pfx");
            //factory.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromFile(@"C:\Users\Sedmak\Desktop\cert\WCFClient.pfx");


            proxy = factory.CreateChannel();
        }
        public ICommonService GetProxy()
        {
            return proxy;
        }
    }
}
