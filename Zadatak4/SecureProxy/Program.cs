using CertificateManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WCFCommons;

namespace SecureProxy
{
    class Program
    {
        static void Main(string[] args)
        {
            string input, data;
            char inChar;
            Processor p;

            Console.WriteLine("Enter IP address:");
            input = Console.ReadLine();

            Console.WriteLine("Select Security type:\n1) Windows\n2) Certificate");
            inChar = (char)Console.Read();

            if (inChar == '1')
            {
                p = new Processor(new WinAuthClient("net.tcp://" + input + ":12354/ICommonService"));
            }
            else if (inChar == '2')
            {
                p = new Processor(new CertClient("net.tcp://" + input + ":12355/ICommonService", "B2Z4wcfservice"));
            }

            else return;

            while (true)
            {
                Console.WriteLine("Enter key to use function: ");
                Console.WriteLine("1) Create file");
                Console.WriteLine("2) Modify file");
                Console.WriteLine("3) Read file");
                Console.WriteLine("4) Delete file");
                Console.WriteLine("5) Exit");
                inChar = (char)Console.Read();
                switch (inChar)
                {
                    case '1':
                        Console.WriteLine("Enter file name:");
                        input = Console.ReadLine();
                        p.Create(input);
                        break;
                    case '2':
                        Console.WriteLine("Enter file name:");
                        input = Console.ReadLine();
                        Console.WriteLine("Enter data to write");
                        data = Console.ReadLine();
                        p.Modify(input, data, EModifyType.Overwrite);
                        break;
                    case '3':
                        Console.WriteLine("Enter file name:");
                        input = Console.ReadLine();
                        if (p.Read(input, out data))
                        {
                            Console.WriteLine("Data read: " + data);
                        }
                        break;
                    case '4':
                        Console.WriteLine("Enter file name:");
                        input = Console.ReadLine();
                        p.Delete(input);
                        break;
                    case '5':
                        return;
                    default:
                        Console.WriteLine("Wrong key");
                        break;
                }
            }

            /*p.Create("test.txt");
            p.Modify("test.txt", "data", EModifyType.Append);
            string test;
            p.Read("test.txt", out test);
            Console.WriteLine(test);
            p.Delete("test.txt");
            Console.Read();*/
        }
    }
}
