using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace Repository.CustomerRepository
{
    public class CallRepositoryForCustomer : RepositoryBase<Call>, ICallRepositoryForCustomer
    {
        public CallRepositoryForCustomer(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<Call> GetCallAsync(int id) =>
            await FindByCondition(c => c.Id.Equals(id), false)
            .FirstOrDefaultAsync();

        public async Task<IEnumerable<Call>> GetCallsAsync(int customerId, CallParameters parameters) =>
            await FindByCondition(c => c.CallerId.Equals(customerId) || c.CalledBy.Equals(customerId), false)
            .CallParametersHandler(parameters)
            .ToListAsync();
    }
}
