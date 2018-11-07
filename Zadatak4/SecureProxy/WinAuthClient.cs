using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WCFCommons;

namespace SecureProxy
{
    class WinAuthClient : IWCFClient
    {
        public ICommonService Proxy { get; private set; }

        public ICommonService GetProxy()
        {
            return Proxy;
        }

        public WinAuthClient(string address)
        {
            NetTcpBinding binding = new NetTcpBinding();

            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            //binding.Security.Transport.ProtectionLevel = ProtectionLevel.EncryptAndSign;

            ChannelFactory<ICommonService> factory = new ChannelFactory<ICommonService>(binding, address);

            Proxy = factory.CreateChannel();
        }
    }
}
