﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBACCommons
{
    class RBACException : Exception
    {
        public RBACException()
        {
        }

        public RBACException(string message) : base(message)
        {
        }
    }
}
