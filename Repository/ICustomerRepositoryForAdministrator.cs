using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository
{
    public interface ICustomerRepositoryForAdministrator
    {
        IQueryable<Customer> GetCustomers(Expression<Func<Customer, bool>> expression, CustomerParameters parameters, bool trackChanges);
        void AddCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(Customer customer);
        void DeleteCustomerById(Guid id);
        void BlockCustomer(Guid id);
    }
}
