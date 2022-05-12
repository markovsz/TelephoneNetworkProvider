using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository.OperatorRepository
{
    public class CallRepositoryForOperator : RepositoryBase<Call>, ICallRepositoryForOperator
    {
        public CallRepositoryForOperator(RepositoryContext repositoryContext)
            : base (repositoryContext)
        {
        }

        public IEnumerable<Call> GetCalls(CallParameters parameters, bool trackChanges) =>
            FindAll(trackChanges)
            .CallParametersHandler(parameters)
            .ToList();

        public IEnumerable<Call> GetCustomerCalls(int customerId, CallParameters parameters) =>
            FindByCondition(c => c.CallerId.Equals(customerId) || c.CalledBy.Equals(customerId), false)
            .CallParametersHandler(parameters)
            .ToList();

        public Call GetCallInfo(int id, bool trackChanges) =>
            FindByCondition(c => c.Id.Equals(id), trackChanges)
            .FirstOrDefault();

        public void CreateCall(Call call) => Create(call);

        public void DeleteCallById(int id) =>
            Delete(FindByCondition(c => c.Id.Equals(id), true).FirstOrDefault());

    }
}
