using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCFCommons
{
    [ServiceContract]
    public interface ICommonService
    {
        [OperationContract]
        [FaultContract(typeof(CommonServiceException))]
        void Create(string fileName);

        [OperationContract]
        [FaultContract(typeof(CommonServiceException))]
        void Modify(string fileName, string data, EModifyType modifyType);

        [OperationContract]
        [FaultContract(typeof(CommonServiceException))]
        string Read(string fileName);

        [OperationContract]
        [FaultContract(typeof(CommonServiceException))]
        void Delete(string fileName);

    }
}
