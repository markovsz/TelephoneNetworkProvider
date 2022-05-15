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
        Task<IEnumerable<CustomerForReadInOperatorDto>> GetCustomersAsync(CustomerParameters parameters);
        Task<CustomerForReadInOperatorDto> GetCustomerInfoAsync(int customerId);
        Task<IEnumerable<CallForReadInOperatorDto>> GetCallsAsync(CallParameters parameters);
        Task<IEnumerable<CallForReadInOperatorDto>> GetCustomerCallsAsync(int customerId, CallParameters parameters);
        Task<CallForReadInOperatorDto> GetCallAsync(int id);
        Task CreateCallAsync(CallForCreateInOperatorDto callDto);
        Task DeleteCallAsync(int id);
    }
}
