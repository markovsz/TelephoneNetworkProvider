using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Repository.CustomerAcquisitionRepository;

namespace Repository.CustomerRepository
{
    public class CustomerRepositoryForCustomer : RepositoryBase<Customer>, ICustomerRepositoryForCustomer
    {
        private ICustomerDataAcquisitionRepository _customerDataAcquisitionRepository;

        public CustomerRepositoryForCustomer(RepositoryContext repositoryContext, ICustomerDataAcquisitionRepository customerDataAcquisitionRepository)
            : base(repositoryContext)
        {
            _customerDataAcquisitionRepository = customerDataAcquisitionRepository;
        }

        public Customer GetCustomer(uint customerId, bool trackChanges) =>
            _customerDataAcquisitionRepository.GetCustomerInfo(customerId, trackChanges);

        public IEnumerable<Customer> GetCustomers(CustomerParameters parameters) =>
            _customerDataAcquisitionRepository.GetCustomers(parameters);
            
        public void UpdateCustomer(Customer customer) => Update(customer);
        public void DeleteCustomer(Customer customer) => Delete(customer);
        public void DeleteCustomerByUserId(uint customerId) => 
            Delete(FindByCondition(c => c.Id.Equals(customerId), true).FirstOrDefault());

    }
}
