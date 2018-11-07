using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WCFCommons;

namespace SecureHost
{
    public class WinAuthCommonService : ICommonService
    {
        ICommonService implementedService = new DefaultCommonService();

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = "Administrate")]
        public void Create(string fileName)
        {
            Console.WriteLine("Create --> Name: {0}, IsAuthenticated: {1}, AuthenticationType: {2}, IsInRole: {3}", 
                Thread.CurrentPrincipal.Identity.Name, Thread.CurrentPrincipal.Identity.IsAuthenticated, 
                Thread.CurrentPrincipal.Identity.AuthenticationType, Thread.CurrentPrincipal.IsInRole("admin"));

            WindowsIdentity winId = Thread.CurrentPrincipal.Identity as WindowsIdentity;

            Console.Write("Groups: ");
            foreach (var gr in winId.Groups)
            {
                Console.Write("{0}, ", gr.Value);
            }
            Console.WriteLine();

            implementedService.Create(fileName);
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = "Administrate")]
        public void Delete(string fileName)
        {
            implementedService.Delete(fileName);
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = "Write")]
        public void Modify(string fileName, string data, EModifyType modifyType)
        {
            implementedService.Modify(fileName, data, modifyType);
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = "Read")]
        public string Read(string fileName)
        {
            return implementedService.Read(fileName);
        }
    }
}
