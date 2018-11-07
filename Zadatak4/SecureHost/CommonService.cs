using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            if (!File.Exists("ownership.xml"))
            {
                File.Create("ownership.xml").Close();
            }
            XmlDocument ownership = new XmlDocument();
            if (!ownership.HasChildNodes)
            {
                ownership.AppendChild(ownership.CreateElement("Files"));
            }
            if (ownership.FirstChild[fileName] == null)
            {
                ownership.FirstChild.AppendChild(ownership.CreateElement(fileName));
                ownership.FirstChild[fileName].AppendChild(ownership.CreateTextNode(""));
            }
            ownership.FirstChild[fileName].FirstChild.Value = owner;
            ownership.Save("ownership.xml");
        }

        public void Create(string fileName)
        {
            if (File.Exists(fileName)) throw new FaultException<CommonServiceException>(new CommonServiceException("File already exists"));
            File.Create(fileName).Close();
            SetOwner(fileName, Thread.CurrentPrincipal.Identity.Name);
        }

        public void Delete(string fileName)
        {
            if (!File.Exists(fileName)) throw new FaultException<CommonServiceException>(new CommonServiceException("File doesnt exist"));
            if(GetOwner(fileName) != Thread.CurrentPrincipal.Identity.Name && !Thread.CurrentPrincipal.IsInRole("Administrate"))
                throw new FaultException<CommonServiceException>(new CommonServiceException("File doesnt exist"));
            File.Delete(fileName);
        }

        public void Modify(string fileName, string data, EModifyType modifyType)
        {
            if (!File.Exists(fileName)) throw new FaultException<CommonServiceException>(new CommonServiceException("File doesnt exist"));
            if (modifyType == EModifyType.Append)
            {
                File.AppendAllText(fileName, data);
            }
            else
            {
                File.WriteAllText(fileName, data);
            }
        }

        public string Read(string fileName)
        {
            if (!File.Exists(fileName)) throw new FaultException<CommonServiceException>(new CommonServiceException("File doesnt exist"));
            return File.ReadAllText(fileName);
        }
    }
}
