using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WCFCommons;

namespace SecureProxy
{
    class Processor
    {
        ICommonService proxy;

        public Processor(IWCFClient client)
        {
            proxy = client.GetProxy();
        }
        
        public bool Create(string fileName)
        {
            try
            {
                proxy.Create(fileName);
                return true;
            }
            catch(Exception e)
            {
                if (e is FaultException)
                {
                    Console.WriteLine("Error: " + ((FaultException<CommonServiceException>)e).Detail);
                }
                else
                {
                    Console.WriteLine("Error: " + e.Message);
                }
                return false;
            }
        }

        public bool Modify(string fileName, string data, EModifyType modifyType)
        {
            try
            {
                proxy.Modify(fileName, data, modifyType);
                return true;
            }
            catch(Exception e)
            {
                if(e is FaultException)
                {
                    Console.WriteLine("Error: " + ((FaultException<CommonServiceException>)e).Detail);
                }
                else
                {
                    Console.WriteLine("Error: " + e.Message);
                }
                return false;
            }
        }

        public bool Read(string fileName, out string read)
        {
            try
            {
                read =  proxy.Read(fileName);
                return true;
            }
            catch(Exception e)
            {
                if (e is FaultException)
                {
                    Console.WriteLine("Error: " + ((FaultException<CommonServiceException>)e).Detail);
                }
                else
                {
                    Console.WriteLine("Error: " + e.Message);
                }
                read = null;
                return false;
            }
        }

        public bool Delete(string fileName)
        {
            try
            {
                proxy.Delete(fileName);
                return true;
            }
            catch(Exception e)
            {
                if (e is FaultException)
                {
                    Console.WriteLine("Error: " + ((FaultException<CommonServiceException>)e).Detail.Message);
                }
                else
                {
                    Console.WriteLine("Error: " + e.Message);
                }
                return false;
            }
        }
    }
}
