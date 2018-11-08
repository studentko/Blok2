using RBACCommons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SecureHost
{
    class RBACPrincipalCache : IRBACObserver
    {
        Dictionary<IIdentity, RBACPrincipal> cacheDict = new Dictionary<IIdentity, RBACPrincipal>();

        private static RBACPrincipalCache instance = null;

        private RBACPrincipalCache()
        {

        }

        public static RBACPrincipalCache GetInstance()
        {
            if (instance == null)
            {
                instance = new RBACPrincipalCache();
                RBACManager.GetInstance().AddObserver(instance);
            }

            return instance;
        }

        public void NotifyUpdate()
        {
            cacheDict.Clear();
        }

        public RBACPrincipal GetPrincipal(IIdentity identity)
        {
            RBACPrincipal princpial = null;

            cacheDict.TryGetValue(identity, out princpial);

            return princpial;
        }

        public void PutPrincipal(IIdentity identity, RBACPrincipal principal)
        {
            cacheDict[identity] = principal;
        }
    }
}
