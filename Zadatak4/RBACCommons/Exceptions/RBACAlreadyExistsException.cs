using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBACCommons
{
    class RBACAlreadyExistsException : Exception
    {
        public RBACAlreadyExistsException()
        {
        }

        public RBACAlreadyExistsException(string message) : base(message)
        {
        }
    }
}
