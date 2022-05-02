using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.OperatorRepository
{
    public interface IOperatorManager
    {
        ICallRepositoryForOperator Calls { get; }
        ICustomerRepositoryForOperator Customers { get; }
        void Save();
    }
}
