using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository.AdministratorRepository
{
    public interface ICallRepositoryForAdministrator
    {
        Task<Call> GetCallAsync(int id);
        Task<IEnumerable<Call>> GetCallsAsync(CallParameters parameters);
        Task<IEnumerable<Call>> GetCustomerCallsAsync(int customerId, CallParameters parameters);
    }
}
