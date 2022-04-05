using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository
{
    public class CustomerRepositoryForCustomer : RepositoryBase<Customer>, ICustomerRepositoryForCustomer
    {
        public CustomerRepositoryForCustomer(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
        public Customer GetCustomer(string userId, bool trackChanges) =>
            FindByCondition(c => c.UserId.Equals(userId), trackChanges).FirstOrDefault();

        public IEnumerable<Customer> GetCustomers(CustomerParameters parameters) =>
            FindAll(false)
            .CustomerParametersHandler(parameters)
            .ToList();
            
        public void UpdateCustomer(Customer customer) => Update(customer);
        public void DeleteCustomer(Customer customer) => Delete(customer);
        public void DeleteCustomerByUserId(string userId) => 
            Delete(FindByCondition(c => c.UserId.Equals(userId), true).FirstOrDefault());

    }
}
