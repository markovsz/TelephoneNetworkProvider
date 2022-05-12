using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository.AdministratorRepository
{
    public class CallRepositoryForAdministrator : RepositoryBase<Call>, ICallRepositoryForAdministrator
    {
        public CallRepositoryForAdministrator(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public Call GetCall(int id) =>
            FindByCondition(c => c.Id.Equals(id), false)
            .FirstOrDefault();

        public IEnumerable<Call> GetCalls(CallParameters parameters) =>
            FindAll(false)
            .CallParametersHandler(parameters)
            .ToList();

        public IEnumerable<Call> GetCustomerCalls(int customerId, CallParameters parameters) =>
            FindByCondition(c => c.Caller.Id.Equals(customerId) || c.CalledBy.Id.Equals(customerId), false)
            .CallParametersHandler(parameters)
            .ToList();
    }
}
