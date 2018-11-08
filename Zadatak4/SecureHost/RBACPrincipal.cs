using RBACCommons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using WCFCommons;

namespace SecureHost
{
    class RBACPrincipal : IPrincipal
    {

        private IIdentity ident;

        HashSet<String> roles = new HashSet<string>();

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
            return roles.Contains(role);
        }

        public RBACPrincipal(WindowsIdentity ident)
        {
            this.ident = ident;

           

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

        public RBACPrincipal(X509Certificate2 clientCert, IIdentity ident)
        {
            this.ident = ident;

            string organization = null;
            string group = null;

            string[] nameParts = clientCert.SubjectName.Name.Split(',');
            foreach(var pp in nameParts)
            {
                string[] keyVal = pp.Trim().Split('=');
                if (keyVal[0] == "O")
                {
                    organization = keyVal[1];
                } else if (keyVal[0] == "OU")
                {
                    group = keyVal[1];
                }

            }

            string finalGroupName = organization == null ? group : organization + "\\" + group;

            try
            {
                roles.UnionWith(RBACManager.GetInstance().GetPermsForGroup(finalGroupName));
            }
            catch (Exception)
            {
                // todo for log
            }

        }
    }
}
