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
        Task<IEnumerable<Customer>> GetCustomersAsync(CustomerParameters parameters);
        Task<Customer> GetCustomerAsync(int customerId);
        Task<Customer> GetUnblockedCustomerAsync(int customerId);
    }
}
