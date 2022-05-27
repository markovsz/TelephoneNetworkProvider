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
        Task<IEnumerable<CustomerForReadInOperatorDto>> GetCustomersInfoAsync(CustomerParameters parameters);
        Task<CustomerForReadInOperatorDto> GetCustomerInfoAsync(int customerId);
        Task<IEnumerable<CallForReadInOperatorDto>> GetCallsInfoAsync(CallParameters parameters);
        Task<IEnumerable<CallForReadInOperatorDto>> GetCustomerCallsInfoAsync(int customerId, CallParameters parameters);
        Task<CallForReadInOperatorDto> GetCallInfoAsync(int id);
        Task CreateCallAsync(CallForCreateInOperatorDto callDto);
        Task DeleteCallAsync(int id);
    }
}
