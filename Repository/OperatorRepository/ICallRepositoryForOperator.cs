using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository.OperatorRepository
{
    public interface ICallRepositoryForOperator
    {
        IEnumerable<Call> GetCalls(CallParameters parameters, bool trackChanges);
        Call GetCallInfo(int id, bool trackChanges);
        IEnumerable<Call> GetCustomerCalls(int customerId, CallParameters parameters);
        void CreateCall(Call call); 
        void DeleteCallById(int id); 
    }
}
