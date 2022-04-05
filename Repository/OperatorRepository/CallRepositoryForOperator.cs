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
    public class CallRepositoryForOperator : RepositoryBase<Call>, ICallRepositoryForOperator
    {
        public CallRepositoryForOperator(RepositoryContext repositoryContext)
            : base (repositoryContext)
        {
        }

        public IEnumerable<Call> GetCalls(CallParameters parameters, bool trackChanges) =>
            FindByCondition(c => true, trackChanges)
            .CallParametersHandler(parameters)
            .ToList();

        public void CreateCall(Call call) => Create(call);

        public void DeleteCallById(uint id) =>
            Delete(FindByCondition(c => c.Id.Equals(id), true).FirstOrDefault());
    }
}
