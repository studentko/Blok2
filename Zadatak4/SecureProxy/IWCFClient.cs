using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCFCommons;

namespace SecureProxy
{
    interface IWCFClient
    {
        ICommonService GetProxy();
    }
}
