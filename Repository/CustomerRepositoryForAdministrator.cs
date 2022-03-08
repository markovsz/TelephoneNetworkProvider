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
    public class CustomerRepositoryForAdministrator : RepositoryBase<Customer>, ICustomerRepositoryForAdministrator
    {
        public CustomerRepositoryForAdministrator(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public IEnumerable<Customer> GetCustomers(CustomerParameters parameters, bool trackChanges) =>
            FindByCondition(c => true, trackChanges)
            .ToList();

        public void AddCustomer(Customer customer) => Create(customer);

        public void UpdateCustomer(Customer customer) =>
            Update(customer);

        public void DeleteCustomer(Customer customer) => Delete(customer);

        public void DeleteCustomerById(Guid id) => 
            Delete(FindByCondition(c => c.Id.Equals(id), true).FirstOrDefault());

        public void BlockCustomer(Guid id) =>
            FindByCondition(c => c.Id.Equals(id), true).FirstOrDefault().IsBlocked = true;
    }
}
