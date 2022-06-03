using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace Repository.AdministratorRepository
{
    public class CallRepositoryForAdministrator : RepositoryBase<Call>, ICallRepositoryForAdministrator
    {
        public CallRepositoryForAdministrator(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<Call> GetCallAsync(int id) =>
            await FindByCondition(c => c.Id.Equals(id), false)
            .FirstOrDefaultAsync();

        public async Task<IEnumerable<Call>> GetCallsAsync(CallParameters parameters) =>
            await FindAll(false)
            .CallParametersHandler(parameters)
            .ToListAsync();

        public async Task<IEnumerable<Call>> GetCustomerCallsAsync(int customerId, CallParameters parameters) =>
            await FindByCondition(c => c.CallerId.Equals(customerId) || c.CalledById.Equals(customerId), false)
            .CallParametersHandler(parameters)
            .ToListAsync();
    }
}
