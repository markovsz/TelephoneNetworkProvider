using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository.OperatorRepository
{
    public interface ICustomerRepositoryForOperator
    {
        IEnumerable<Customer> GetCustomers(CustomerParameters parameters);
        Customer GetCustomerInfo(int customerId);
    }
}
