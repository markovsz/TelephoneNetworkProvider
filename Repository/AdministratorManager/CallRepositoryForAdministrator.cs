using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository
{
    public class CallRepositoryForAdministrator : RepositoryBase<Call>, ICallRepositoryForAdministrator
    {
        public CallRepositoryForAdministrator(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public Call GetCall(uint id) =>
            FindByCondition(c => c.Id.Equals(id), false)
            .FirstOrDefault();

        public IEnumerable<Call> GetCalls(CallParameters parameters) =>
            FindAll(false)
            .CallParametersHandler(parameters)
            .ToList();

        public IEnumerable<Call> GetCustomerCalls(string userId, CallParameters parameters) =>
            FindByCondition(c => c.Caller.UserId.Equals(userId) || c.CalledBy.UserId.Equals(userId), false)
            .CallParametersHandler(parameters)
            .ToList();
    }
}
