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
    public class CallRepositoryForCustomer : RepositoryBase<Call>, ICallRepositoryForCustomer
    {
        public CallRepositoryForCustomer(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {

        }
        public IEnumerable<Call> GetCalls(Guid customerId, CallParameters parameters) =>
            FindByCondition(c => c.Caller.Equals(customerId) || c.CalledBy.Equals(customerId), false)
            .ToList();
    }
}
