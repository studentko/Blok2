using RBACCommons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using WCFCommons;

namespace SecureHost
{
    class RBACPrincipal : IPrincipal
    {

        private WindowsIdentity ident;

        HashSet<String> roles;

        SIEM log;

        public IIdentity Identity
        {
            get
            {
                return ident;
            }
        }

        public bool IsInRole(string role)
        {
            bool pass = roles.Contains(role);
            if (!pass)
            {
                log.LogError(String.Format( "User {0} does not have the {1} permission!", ident.Name, role));
            }
            return pass;
        }

        public RBACPrincipal(WindowsIdentity ident)
        {
            this.ident = ident;

            roles = new HashSet<string>();

            log = new SIEM("Projekat 4");

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
