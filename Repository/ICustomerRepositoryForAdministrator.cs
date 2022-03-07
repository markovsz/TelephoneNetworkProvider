using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;

namespace Repository
{
    public interface ICustomerRepositoryForAdministrator
    {
        IQueryable<Customer> GetCustomers(Expression<Func<Customer, bool>> expression, bool trackChanges);
        void AddCustomer(Customer customer);
        void DeleteCustomer(Customer customer);
        void DeleteCustomerById(Guid id);
        void BlockCustomer(Guid id);
    }
}
