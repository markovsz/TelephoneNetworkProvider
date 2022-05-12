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
        IEnumerable<Customer> GetCustomers(CustomerParameters parameters, bool trackChanges);
        Customer GetCustomerInfo(int customerId, bool trackChanges);
        void AddCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(Customer customer);
        void DeleteCustomerByUserId(int customerId);
        Customer FindCustomerByPhoneNumber(string phoneNumber, bool trackChanges);
    }
}
