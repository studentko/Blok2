using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFCommons
{
    public class CommonServiceException
    {
        public CommonServiceException(string msg)
        {
            Message = msg;
        }
        string Message { get; set; }
    }
}
