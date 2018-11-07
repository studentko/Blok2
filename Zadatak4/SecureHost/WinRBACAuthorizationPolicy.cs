using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SecureHost
{
    class WinRBACAuthorizationPolicy : IAuthorizationPolicy
    {

        private object locker = new object();

        public string Id { get; private set; }

        public ClaimSet Issuer
        {
            get
            {
                return ClaimSet.System;
            }
        }

        public WinRBACAuthorizationPolicy()
        {
            Id = Guid.NewGuid().ToString();
        }

        public bool Evaluate(EvaluationContext evaluationContext, ref object state)
        {
            object list;

            if (!evaluationContext.Properties.TryGetValue("Identities", out list))
            {
                return false;
            }

            IList<IIdentity> identities = list as IList<IIdentity>;
            if (list == null || identities.Count <= 0)
            {
                return false;
            }

            evaluationContext.Properties["Principal"] = GetPrincipal(identities[0]);
            return true;
        }

        protected virtual IPrincipal GetPrincipal(IIdentity identity)
        {
            lock (locker)
            {
                IPrincipal principal = null;
                WindowsIdentity windowsIdentity = identity as WindowsIdentity;

                if (windowsIdentity != null)
                {
                    //Audit.AuthenticationSuccess(windowsIdentity.Name);
                    principal = new RBACPrincipal(windowsIdentity);
                }

                return principal;
            }
        }


    }
}
