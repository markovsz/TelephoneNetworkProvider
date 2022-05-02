using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository.CustomerRepository
{
    public class CallRepositoryForCustomer : RepositoryBase<Call>, ICallRepositoryForCustomer
    {
        public CallRepositoryForCustomer(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public Call GetCall(uint id) =>
            FindByCondition(c => c.Id.Equals(id), false)
            .FirstOrDefault();

        public IEnumerable<Call> GetCalls(uint customerId, CallParameters parameters) =>
            FindByCondition(c => c.CallerId.Equals(customerId) || c.CalledBy.Equals(customerId), false)
            .CallParametersHandler(parameters)
            .ToList();
    }
}
