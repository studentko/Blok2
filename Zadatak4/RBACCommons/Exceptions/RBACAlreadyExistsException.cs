using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBACCommons
{
    public class RBACAlreadyExistsException : Exception
    {
        public RBACAlreadyExistsException()
        {
        }

        public RBACAlreadyExistsException(string message) : base(message)
        {
        }
    }
}
