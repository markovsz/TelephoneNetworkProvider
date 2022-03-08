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
        public IEnumerable<Call> GetCalls(CallParameters parameters) =>
            FindByCondition(c => true, false)
            .ToList();
    }
}
