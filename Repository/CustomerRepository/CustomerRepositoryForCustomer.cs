using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Customer> GetCustomerAsync(int customerId, bool trackChanges) =>
            await _customerDataAcquisitionRepository.GetCustomerAsync(customerId, c => !c.IsBlocked && !c.IsPhoneNumberHided, trackChanges);

        public async Task<IEnumerable<Customer>> GetCustomersAsync(CustomerParameters parameters) =>
            await _customerDataAcquisitionRepository.GetCustomersAsync(c => !c.IsBlocked && !c.IsPhoneNumberHided, parameters);
            
        public void UpdateCustomer(Customer customer) => Update(customer);
        public void DeleteCustomer(Customer customer) => Delete(customer);
        public async Task DeleteCustomerByIdAsync(int customerId) => 
            Delete(await FindByCondition(c => c.Id.Equals(customerId), true).FirstOrDefaultAsync());

    }
}
