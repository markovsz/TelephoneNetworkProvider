using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.DataTransferObjects;
using Entities.RequestFeatures;
using Entities.Models;

namespace BussinessLogic
{
    public interface ICustomerLogic
    {
        void ReplenishTheBalance(int customerId, Decimal currency);
        IEnumerable<CallForReadInCustomerDto> GetCalls(int customerId, CallParameters parameters);
        CallForReadInCustomerDto GetCall(int id);
        IEnumerable<CustomerForReadInCustomerDto> GetCustomers(CustomerParameters parameters);
        CustomerForReadInCustomerDto GetCustomerInfo(int customerId);
        void UpdateCustomerInfo(int customerId, CustomerForUpdateInCustomerDto customerDto);
    }
}
