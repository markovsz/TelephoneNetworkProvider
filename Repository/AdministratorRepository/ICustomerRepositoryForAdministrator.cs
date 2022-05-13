using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository.AdministratorRepository
{
    public interface ICustomerRepositoryForAdministrator
    {
        Task<IEnumerable<Customer>> GetCustomersAsync(CustomerParameters parameters, bool trackChanges);
        Task<Customer> GetCustomerInfoAsync(int customerId, bool trackChanges);
        Task AddCustomerAsync(Customer customer);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(Customer customer);
        Task DeleteCustomerByUserIdAsync(int customerId);
        Task<Customer> FindCustomerByPhoneNumberAsync(string phoneNumber, bool trackChanges);
    }
}
