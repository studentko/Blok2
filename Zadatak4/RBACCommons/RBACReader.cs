using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBACCommons
{
    interface RBACReader
    {
        bool IsGroupAllowed(string group, string perm);
        List<string> GetPermsForGroup(string group);
        Dictionary<string, List<string>> GetAll();
    }
}
