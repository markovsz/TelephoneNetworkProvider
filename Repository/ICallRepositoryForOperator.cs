using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository
{
    public interface ICallRepositoryForOperator
    {
        IEnumerable<Call> GetCalls(CallParameters parameters, bool trackChanges);
        void CreateCall(Call call); 
        void DeleteCallById(Guid id); 
    }
}
