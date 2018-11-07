using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RBACCommons
{
    public class RBACManager : IRBACManager, RBACReader
    {
        static RBACManager instance = null;

        List<IRBACObserver> observers;

        DateTime lastEdit;

        Dictionary<string, List<string>> permissions = null;
        XmlDocument xml = null;
        static private object lObject = new object();

        private void WriteToXml()
        {
            XmlDocument writer = new XmlDocument();
            XmlNode groups = writer.AppendChild(writer.CreateElement("Groups"));
            foreach (var group in permissions)
            {
                XmlNode xGroup = groups.AppendChild(writer.CreateElement("Group"));
                xGroup.AppendChild(writer.CreateElement("Name")).AppendChild(writer.CreateTextNode(group.Key));
                XmlNode xPerms = xGroup.AppendChild(writer.CreateElement("Permissions"));
                foreach (var perm in group.Value)
                {
                    xPerms.AppendChild(writer.CreateElement("Perm")).AppendChild(writer.CreateTextNode(perm));
                }
            }
            writer.Save("RBAC.xml");
            lastEdit = File.GetLastWriteTime("RBAC.xml");
            NotifyAll();
        }

        private void ReloadCheck()
        {
            lock (lObject)
            {
                DateTime edit = File.GetLastWriteTime("RBAC.xml");
                if (lastEdit != edit) ReloadRBAC();
            }
        }

        private void ReloadRBAC()
        {

            permissions = new Dictionary<string, List<string>>();

            xml = new XmlDocument();
            xml.Load("RBAC.xml");

            foreach (XmlElement group in xml["Groups"])
            {
                List<string> perms = new List<string>();
                string name = group["Name"].FirstChild.Value;
                foreach (XmlElement perm in group["Permissions"])
                {
                    perms.Add(perm.FirstChild.Value);
                }
                permissions.Add(name, perms);
            }
            lastEdit = File.GetLastWriteTime("RBAC.xml");
            NotifyAll();
        }

        private RBACManager()
        {
            observers = new List<IRBACObserver>();
            ReloadRBAC();
        }


        public static RBACManager GetInstance()
        {
            if(instance == null)
            {
                lock (lObject)
                {
                    if (instance == null)
                    {
                        instance = new RBACManager();
                    }
                }
            }
            return instance;
        }

        public void AddGroup(string group)
        {
            ReloadCheck();
            if (permissions.ContainsKey(group))
                throw new RBACAlreadyExistsException("Group already exists");
            permissions.Add(group, new List<string>());
            WriteToXml();
        }

        public void AddPermission(string group, string perm)
        {
            ReloadCheck();
            if (!permissions.ContainsKey(group))
                throw new RBACNotFoundException("Group not found");
            if (permissions[group].Contains(perm))
                throw new RBACAlreadyExistsException("Permission already exists");
            permissions[group].Add(perm);
            WriteToXml();
        }

        public Dictionary<string, List<string>> GetAll()
        {
            ReloadCheck();
            return permissions;
        }

        public List<string> GetPermsForGroup(string group)
        {
            ReloadCheck();
            if (permissions.ContainsKey(group))
            {
                return permissions[group];
            }
            throw new RBACNotFoundException("Group not found");
        }

        public bool IsGroupAllowed(string group, string perm)
        {
            ReloadCheck();
            if (permissions.ContainsKey(group) && permissions[group].Contains(perm))
                return true;
            return false;
        }

        public void RemoveGroup(string group)
        {
            ReloadCheck();
            if (!permissions.ContainsKey(group))
                throw new RBACNotFoundException("Group doesnt exist");
            permissions.Remove(group);
            WriteToXml();
        }

        public void RemovePermission(string group, string perm)
        {
            ReloadCheck();
            if (!permissions.ContainsKey(group))
                throw new RBACNotFoundException("Group doesnt exist");
            if (!permissions[group].Contains(perm))
                throw new RBACNotFoundException("Permission doesnt exist");
            permissions[group].Remove(perm);
            WriteToXml();
        }

        public void AddObserver(IRBACObserver observer)
        {
            observers.Add(observer);
        }

        private void NotifyAll()
        {
            foreach (var observer in observers)
            {
                observer.NotifyUpdate();
            }
        }
    }
}
