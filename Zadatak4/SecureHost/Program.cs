using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureHost
{
    class Program
    {
        static void Main(string[] args)
        {
            CommonService cs = new CommonService();
            //cs.Modify("TestFile.txt", "Failed Test", WCFCommons.EModifyType.Overwrite);
            /*cs.Create("TestFile.txt");
            cs.Modify("TestFile.txt", "Test 1", WCFCommons.EModifyType.Append);
            cs.Modify("TestFile.txt", "Test 2", WCFCommons.EModifyType.Append);
            cs.Modify("TestFile.txt", "Test 3", WCFCommons.EModifyType.Overwrite);
            Console.WriteLine(cs.Read("TestFile.txt"));
            cs.Delete("TestFile.txt");*/
        }
    }
}
