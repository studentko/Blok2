using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
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


            RBACPrincipalCache principalCache = RBACPrincipalCache.GetInstance();
            RBACPrincipal principal = principalCache.GetPrincipal(identities[0]);
            if (principal == null)
            {
                principal = GetPrincipal(identities[0]);
                principalCache.PutPrincipal(identities[0], principal);
            }

            evaluationContext.Properties["Principal"] = principal;
            return true;
        }

        protected virtual RBACPrincipal GetPrincipal(IIdentity identity)
        {
            lock (locker)
            {
                RBACPrincipal principal = null;
                WindowsIdentity windowsIdentity = identity as WindowsIdentity;

                if (windowsIdentity != null)
                {
                    //Audit.AuthenticationSuccess(windowsIdentity.Name);
                    principal = new RBACPrincipal(windowsIdentity);
                } else
                {
                    X509Certificate2 cert = GetCertificate(identity);
                    //Console.WriteLine(cert.SubjectName.Name);
                    principal = new RBACPrincipal(cert, identity);
                }

                return principal;
            }
        }
        private X509Certificate2 GetCertificate(IIdentity identity)
        {
            try
            {
                // X509Identity is an internal class, so we cannot directly access it
                Type x509IdentityType = identity.GetType();

                // The certificate is stored inside a private field of this class
                FieldInfo certificateField = x509IdentityType.GetField("certificate", BindingFlags.Instance | BindingFlags.NonPublic);

                return (X509Certificate2)certificateField.GetValue(identity);
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
