using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBACCommons
{
    class RBACNotFoundException : Exception
    {
        public RBACNotFoundException()
        {
        }

        public RBACNotFoundException(string message) : base(message)
        {
        }
    }
}
