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
        Task<CustomerForReadInCustomerDto> GetCustomerInfoAsync(int customerId);
        Task<IEnumerable<CustomerForReadInCustomerDto>> GetCustomersInfoAsync(CustomerParameters parameters);
        Task<CallForReadInCustomerDto> GetCallInfoAsync(int id);
        Task<IEnumerable<CallForReadInCustomerDto>> GetCallsInfoAsync(int customerId, CallParameters parameters);
        Task UpdateCustomerInfo(int customerId, CustomerForUpdateInCustomerDto customerDto);
        Task ReplenishTheBalanceAsync(int customerId, Decimal currency);
    }
}
