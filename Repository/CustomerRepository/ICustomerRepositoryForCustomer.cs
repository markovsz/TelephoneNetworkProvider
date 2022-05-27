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
        Task<Customer> GetCustomerAsync(int customerId, bool trackChanges);
        Task<IEnumerable<Customer>> GetCustomersAsync(CustomerParameters parameters);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(Customer customer);
        Task DeleteCustomerByIdAsync(int customerId);
    }
}
