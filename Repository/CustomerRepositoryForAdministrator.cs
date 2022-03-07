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
    public class CustomerRepositoryForAdministrator : RepositoryBase<Customer>, ICustomerRepositoryForAdministrator
    {
        public CustomerRepositoryForAdministrator(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public IQueryable<Customer> GetCustomers(Expression<Func<Customer, bool>> expression, bool trackChanges) =>
            FindByCondition(expression, trackChanges);

        public void AddCustomer(Customer customer) => Create(customer);

        public void DeleteCustomer(Customer customer) => Delete(customer);
        public void DeleteCustomerById(Guid id) => 
            Delete(FindByCondition(c => c.Id.Equals(id), true).FirstOrDefault());
        public void BlockCustomer(Guid id) =>
            FindByCondition(c => c.Id.Equals(id), true).FirstOrDefault().IsBlocked = true;
    }
}
