using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace Repository.OperatorRepository
{
    public class CallRepositoryForOperator : RepositoryBase<Call>, ICallRepositoryForOperator
    {
        public CallRepositoryForOperator(RepositoryContext repositoryContext)
            : base (repositoryContext)
        {
        }

        public async Task<IEnumerable<Call>> GetCallsAsync(CallParameters parameters) =>
            await FindAll(false)
            .CallParametersHandler(parameters)
            .ToListAsync();

        public async Task<IEnumerable<Call>> GetCustomerCallsAsync(int customerId, CallParameters parameters) =>
            await FindByCondition(c => c.CallerId.Equals(customerId) || c.CalledById.Equals(customerId), false)
            .CallParametersHandler(parameters)
            .ToListAsync();

        public async Task<Call> GetCallAsync(int id, bool trackChanges) =>
            await FindByCondition(c => c.Id.Equals(id), trackChanges)
            .FirstOrDefaultAsync();

        public async Task CreateCallAsync(Call call) => await CreateAsync(call);

        public async Task DeleteCallByIdAsync(int id) =>
            Delete(await FindByCondition(c => c.Id.Equals(id), true).FirstOrDefaultAsync());

    }
}
