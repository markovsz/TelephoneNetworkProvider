using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Entities.Models;

namespace Repository
{
    public class CustomerRepositoryForCustomer : RepositoryBase<Customer>, ICustomerRepositoryForCustomer
    {
        public CustomerRepositoryForCustomer(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {

        }
        public Customer GetCustomer(Guid id, bool trackChanges) =>
            FindByCondition(c => c.Id.Equals(id), trackChanges).FirstOrDefault();

        public void UpdateCustomer(Customer customer) => Update(customer);
        public void DeleteCustomer(Customer customer) => Delete(customer);
        public void DeleteCustomerById(Guid id) => 
            Delete(FindByCondition(c => c.Id.Equals(id), true).FirstOrDefault());
    }
}
