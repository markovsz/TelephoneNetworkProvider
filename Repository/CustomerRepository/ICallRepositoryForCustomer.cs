using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository.CustomerRepository
{
    public interface ICallRepositoryForCustomer
    {
        Task<IEnumerable<Call>> GetCallsAsync(int customerId, CallParameters parameters);
        Task<Call> GetCallAsync(int id);
    }
}
