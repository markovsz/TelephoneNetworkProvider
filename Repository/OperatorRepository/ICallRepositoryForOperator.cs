using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository.OperatorRepository
{
    public interface ICallRepositoryForOperator
    {
        Task<IEnumerable<Call>> GetCallsAsync(CallParameters parameters);
        Task<Call> GetCallAsync(int id, bool trackChanges);
        Task<IEnumerable<Call>> GetCustomerCallsAsync(int customerId, CallParameters parameters);
        Task CreateCallAsync(Call call); 
        Task DeleteCallByIdAsync(int id); 
    }
}
