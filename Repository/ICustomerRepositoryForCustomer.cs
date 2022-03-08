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
        Customer GetCustomer(Guid id, bool trackChanges);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(Customer customer);
        void DeleteCustomerById(Guid id);
    }
}
