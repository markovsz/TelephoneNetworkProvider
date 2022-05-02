using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository.CustomerRepository
{
    public interface ICustomerRepositoryForCustomer
    {
        Customer GetCustomer(uint customerId, bool trackChanges);
        IEnumerable<Customer> GetCustomers(CustomerParameters parameters);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(Customer customer);
        void DeleteCustomerByUserId(uint customerId);
    }
}
