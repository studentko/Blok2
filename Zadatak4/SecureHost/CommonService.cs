using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using WCFCommons;

namespace SecureHost
{
    class CommonService : ICommonService
    {
        Dictionary<string, object> locks;
        object lObject;
        protected SIEM log;

        public CommonService()
        {
            locks = new Dictionary<string, object>();
            lObject = new object();
            log = new SIEM("Zadatak 4");
        }

        private string GetOwner(string fileName)
        {
            if (!File.Exists("ownership.xml"))
            {
                return null;
            }
            XmlDocument ownership = new XmlDocument();
            ownership.Load("ownership.xml");
            if (ownership.FirstChild[fileName] != null)
            {
                return ownership.FirstChild[fileName].FirstChild.Value;
            }
            else
            {
                return null;
            }
        }

        private void SetOwner(string fileName, string owner)
        {
            XmlDocument ownership = new XmlDocument();
            if (!File.Exists("ownership.xml"))
            {
                ownership.AppendChild(ownership.CreateElement("Files"));
                ownership.Save("ownership.xml");
            }
            else
            {
                ownership.Load("ownership.xml");
            }
            if (ownership.FirstChild[fileName] == null)
            {
                ownership.FirstChild.AppendChild(ownership.CreateElement(fileName));
                ownership.FirstChild[fileName].AppendChild(ownership.CreateTextNode(""));
            }
            ownership.FirstChild[fileName].FirstChild.Value = owner;
            ownership.Save("ownership.xml");
        }

        private void AuthAndAutorize(string funcName, string[] roles)
        {
            if (!Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                log.LogError(String.Format("{0}@{1}: Authentication failed for user {2}", DateTime.Now.ToLongTimeString(), funcName, Thread.CurrentPrincipal.Identity.Name));
                throw new SecurityException("Access denied: Not authenticated");
            }
            foreach (var role in roles)
            {
                if (!Thread.CurrentPrincipal.IsInRole(role))
                {
                    log.LogError(String.Format("{0}@{1}: Authorization failed for user {2}, permission {3}", DateTime.Now.ToLongTimeString(), funcName, Thread.CurrentPrincipal.Identity.Name, role));
                    throw new SecurityException("Access denied: Not authorized");
                }
            }

        }

        //[PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = "Administrate")]
        //[PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = "Access")]
        public void Create(string fileName)
        {
            AuthAndAutorize("Create", new string[] { "Administrate", "Access" });
            lock (lObject)
            {
                //DEBUG
                if (Thread.CurrentPrincipal.Identity.Name == "")
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("Test"), new string[] { "Administrate" });
                //END
                if (File.Exists(fileName))
                {
                    log.LogError(String.Format("{0}@Create: User {1}, file {2} already exists", DateTime.Now.ToLongTimeString(), Thread.CurrentPrincipal.Identity.Name, fileName));
                    throw new FaultException<CommonServiceException>(new CommonServiceException("File already exists"));
                }
                log.LogInformation(String.Format("{0}@Create: User {1}, created file {2}", DateTime.Now.ToLongTimeString(), Thread.CurrentPrincipal.Identity.Name, fileName));
                File.Create(fileName).Close();
                SetOwner(fileName, Thread.CurrentPrincipal.Identity.Name);
                locks.Add(fileName, new object());
            }
        }

        //[PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = "Access")]
        public void Delete(string fileName)
        {
            AuthAndAutorize("Delete", new string[] { "Access" });
            object lo;
            if (!locks.TryGetValue(fileName, out lo))
            {
                lo = lObject;
            }
            lock (lo)
            {
                //DEBUG
                if (Thread.CurrentPrincipal.Identity.Name == "")
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("Test"), new string[] { "Administrate" });
                //END
                if (!File.Exists(fileName))
                {
                    log.LogError(String.Format("{0}@Delete: User {1}, file {2} doesnt exist", DateTime.Now.ToLongTimeString(), Thread.CurrentPrincipal.Identity.Name, fileName));
                    throw new FaultException<CommonServiceException>(new CommonServiceException("File doesnt exist"));
                }
                if (GetOwner(fileName) != Thread.CurrentPrincipal.Identity.Name && !Thread.CurrentPrincipal.IsInRole("Administrate"))
                {
                    log.LogError(String.Format("{0}@Delete: User {1}, is not the owner of file {2} and is not Administrator", DateTime.Now.ToLongTimeString(), Thread.CurrentPrincipal.Identity.Name, fileName));
                    throw new FaultException<CommonServiceException>(new CommonServiceException("File doesnt exist"));
                }
                log.LogInformation(String.Format("{0}@Delete: User {1}, deleted file {2}", DateTime.Now.ToLongTimeString(), Thread.CurrentPrincipal.Identity.Name, fileName));
                File.Delete(fileName);
                locks.Remove(fileName);
            }
        }

        //[PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = "Write")]
        //[PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = "Access")]
        public void Modify(string fileName, string data, EModifyType modifyType)
        {
            AuthAndAutorize("Modify", new string[] { "Access", "Write" });
            object lo;
            if (!locks.TryGetValue(fileName, out lo))
            {
                lo = lObject;
            }
            lock (lo)
            {
                if (!File.Exists(fileName))
                {
                    log.LogError(String.Format("{0}@Modify: User {1}, file {2} doesnt exist", DateTime.Now.ToLongTimeString(), Thread.CurrentPrincipal.Identity.Name, fileName));
                    throw new FaultException<CommonServiceException>(new CommonServiceException("File doesnt exist"));
                }
                if (modifyType == EModifyType.Append)
                {
                    log.LogInformation(String.Format("{0}@Modify: User {1}, appended to file {2}", DateTime.Now.ToLongTimeString(), Thread.CurrentPrincipal.Identity.Name, fileName));
                    File.AppendAllText(fileName, data);
                }
                else
                {
                    log.LogInformation(String.Format("{0}@Modify: User {1}, owerwrote file {2}", DateTime.Now.ToLongTimeString(), Thread.CurrentPrincipal.Identity.Name, fileName));
                    File.WriteAllText(fileName, data);
                }
            }
        }

        //[PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = "Read")]
        //[PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = "Access")]
        public string Read(string fileName)
        {
            AuthAndAutorize("Read", new string[] { "Access", "Read" });
            object lo;
            if (!locks.TryGetValue(fileName, out lo))
            {
                lo = lObject;
            }
            lock (lo)
            {
                if (!File.Exists(fileName))
                {
                    log.LogError(String.Format("{0}@Read: User {1}, file {2} doesnt exist", DateTime.Now.ToLongTimeString(), Thread.CurrentPrincipal.Identity.Name, fileName));
                    throw new FaultException<CommonServiceException>(new CommonServiceException("File doesnt exist"));
                }
                log.LogInformation(String.Format("{0}@Read: User {1}, read file {2}", DateTime.Now.ToLongTimeString(), Thread.CurrentPrincipal.Identity.Name, fileName));
                return File.ReadAllText(fileName);
            }
        }
    }

    class WinCommonService : CommonService
    {
        public WinCommonService()
        {
            log = new SIEM(SecureHost.SOURCE_NAME_WIN);
        }
    }

    class CertCommonService : CommonService
    {
        public CertCommonService()
        {
            log = new SIEM(SecureHost.SOURCE_NAME_CERT);
        }
    }
}
