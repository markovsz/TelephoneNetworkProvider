using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;

namespace Repository
{
    public interface ICustomerRepositoryForCustomer
    {
        IQueryable<Customer> GetCustomer(bool trackChanges);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(Customer customer);
    }
}
