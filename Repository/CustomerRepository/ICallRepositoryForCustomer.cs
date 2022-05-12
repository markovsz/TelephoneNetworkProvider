using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository.CustomerRepository
{
    public interface ICallRepositoryForCustomer
    {
        IEnumerable<Call> GetCalls(int customerId, CallParameters parameters);
        Call GetCall(int id);
    }
}
