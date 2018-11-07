using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCFCommons;

namespace SecureHost
{
    public class SecureHostConfig
    {
        public EAuthType AuthenticationType { get; set; }

        public string Ip { get; set; }

        public int Port { get; set; }

        public SecureHostConfig Clone()
        {
            SecureHostConfig cloned = new SecureHostConfig
            {
                AuthenticationType = this.AuthenticationType,
                Ip = this.Ip.Clone().ToString(),
                Port = this.Port
            };

            return cloned;
        }


    }
}
