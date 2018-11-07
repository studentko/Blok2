using RBACCommons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SecureHost
{
    class RBACPrincipal : IPrincipal
    {

        private WindowsIdentity ident;

        HashSet<String> roles;

        public IIdentity Identity
        {
            get
            {
                return ident;
            }
        }

        public bool IsInRole(string role)
        {
            return roles.Contains(role);
        }

        public RBACPrincipal(WindowsIdentity ident)
        {
            this.ident = ident;

            roles = new HashSet<string>();

            RBACManager rbacMgr = RBACManager.GetInstance();

            foreach (var group in ident.Groups)
            {
                try
                {
                    IdentityReference ntAcc = group.Translate(typeof(NTAccount));
                    List<string> perms = rbacMgr.GetPermsForGroup(ntAcc.Value);
                    roles.UnionWith(perms);
                }
                catch (Exception e)
                {
                    Console.WriteLine("RBACPrincipal exception: {0}", e.Message);
                }
            }
        }
    }
}
