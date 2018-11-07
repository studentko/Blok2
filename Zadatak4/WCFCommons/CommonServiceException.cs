using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WCFCommons
{
    [DataContract]
    public class CommonServiceException
    {
        public CommonServiceException(string msg)
        {
            Message = msg;
        }
        [DataMember]
        string Message { get; set; }
    }
}
