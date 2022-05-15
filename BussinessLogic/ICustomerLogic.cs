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
        Task ReplenishTheBalanceAsync(int customerId, Decimal currency);
        Task<IEnumerable<CallForReadInCustomerDto>> GetCallsAsync(int customerId, CallParameters parameters);
        Task<CallForReadInCustomerDto> GetCallAsync(int id);
        Task<IEnumerable<CustomerForReadInCustomerDto>> GetCustomersAsync(CustomerParameters parameters);
        Task<CustomerForReadInCustomerDto> GetCustomerInfoAsync(int customerId);
        void UpdateCustomerInfo(int customerId, CustomerForUpdateInCustomerDto customerDto);
    }
}
