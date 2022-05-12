using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.DataTransferObjects;
using Entities.RequestFeatures;

namespace BussinessLogic
{
    public interface IOperatorLogic
    {   
        IEnumerable<CustomerForReadInOperatorDto> GetCustomers(CustomerParameters parameters);
        CustomerForReadInOperatorDto GetCustomerInfo(int customerId);
        IEnumerable<CallForReadInOperatorDto> GetCalls(CallParameters parameters);
        IEnumerable<CallForReadInOperatorDto> GetCustomerCalls(int customerId, CallParameters parameters);
        CallForReadInOperatorDto GetCall(int id);
        void CreateCall(CallForCreateInOperatorDto callDto);
        void DeleteCall(int id);
    }
}
