using RBACCommons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBACAdmin
{
    class Program
    {
        static void Main(string[] args)
        {
            IRBACManager RBAC = RBACManager.GetInstance();

            char selection;
            bool running = true;

            while (running)
            {
                PrintMenuSelection();

                selection = Console.ReadKey().KeyChar;

                switch (selection)
                {
                    case '1':
                        OptionAddGroup(RBAC);
                        break;
                    case '2':
                        OptionAddPermission(RBAC);
                        break;
                    case '3':
                        OptionRemoveGroup(RBAC);
                        break;
                    case '4':
                        OptionRemovePermission(RBAC);
                        break;
                    case '5':
                        running = false;
                        break;
                    default:
                        break;
                }
            }
        }

        private static void PrintMenuSelection()
        {
            Console.WriteLine("    RBAC admin UI");
            Console.WriteLine("\n=============================");
            Console.WriteLine("\n>>1.Add Group");
            Console.WriteLine("\n>>2.Add Permission to Group");
            Console.WriteLine("\n>>3.Remove Group");
            Console.WriteLine("\n>>4.Remove Permission");
            Console.WriteLine("\n>>5.Close");
            Console.WriteLine("\n=============================");
        }

        private static void OptionAddGroup(IRBACManager manager)
        {
            Console.WriteLine("\n>> Enter group name you wish to add\n>>");

            string groupName = Console.ReadLine();

            try
            {
                manager.AddGroup(groupName);
                Console.WriteLine("Group [{0}] successfully added", groupName);
            }
            catch (RBACAlreadyExistsException e)
            {
                Console.WriteLine("Fault exception trown: {0}", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Fault exception trown: {0}", e.Message);
            }

            return;
        }

        private static void OptionAddPermission(IRBACManager manager)
        {
            Console.WriteLine("\n>> Enter group name you wish to add the permission to\n>>");

            string groupName = Console.ReadLine();

            Console.WriteLine("\n>> Enter permission name you wish to add\n>>");

            string permission = Console.ReadLine();

            try
            {
                manager.AddPermission(groupName, permission);
                Console.WriteLine("Permission [{0}] successfully added to group [{1}]", groupName, permission);
            }
            catch (RBACNotFoundException e)
            {
                Console.WriteLine("Fault exception trown: {0}", e.Message); 
            }
            catch (RBACAlreadyExistsException e)
            {
                Console.WriteLine("Fault exception trown: {0}", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Fault exception trown: {0}", e.Message);
            }

            return;
        }

        private static void OptionRemoveGroup(IRBACManager manager)
        {
            Console.WriteLine("\n>> Enter group name you wish to remove\n>>");

            string groupName = Console.ReadLine();

            try
            {
                manager.RemoveGroup(groupName);
                Console.WriteLine("Group [{0}] successfully removed", groupName);
            }
            catch (RBACNotFoundException e)
            {
                Console.WriteLine("Fault exception trown: {0}", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Fault exception trown: {0}", e.Message);
            }

            return;
        }

        private static void OptionRemovePermission(IRBACManager manager)
        {
            Console.WriteLine("\n>> Enter group name you wish to remove the permission from\n>>");

            string groupName = Console.ReadLine();

            Console.WriteLine("\n>> Enter permission name you wish to remove\n>>");

            string permission = Console.ReadLine();

            try
            {
                manager.RemovePermission(groupName, permission);
                Console.WriteLine("Permission [{0}] successfully added to group [{1}]", groupName, permission);
            }
            catch (RBACNotFoundException e)
            {
                Console.WriteLine("Fault exception trown: {0}", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Fault exception trown: {0}", e.Message);
            }

            return;
        }
    }
}
