using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCFCommons;

namespace SecureHost
{
    class DefaultCommonService : ICommonService
    {
        public void Create(string fileName)
        {
            Console.WriteLine("Created: {0}", fileName);
        }

        public void Delete(string fileName)
        {
            Console.WriteLine("Deleted: {0}", fileName);
        }

        public void Modify(string fileName, string data, EModifyType modifyType)
        {
            Console.WriteLine("Modified: {0}", fileName);
        }

        public string Read(string fileName)
        {
            Console.WriteLine("Read: {0}", fileName);
            return "Dummy content";
        }
    }
}
