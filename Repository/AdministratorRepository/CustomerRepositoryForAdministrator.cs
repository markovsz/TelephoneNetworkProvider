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


namespace Repository.AdministratorRepository
{
    public class CustomerRepositoryForAdministrator : RepositoryBase<Customer>, ICustomerRepositoryForAdministrator
    {
        private ICustomerDataAcquisitionRepository _customerDataAcquisitionRepository;

        public CustomerRepositoryForAdministrator(RepositoryContext repositoryContext, ICustomerDataAcquisitionRepository customerDataAcquisitionRepository)
            : base(repositoryContext)
        {
            _customerDataAcquisitionRepository = customerDataAcquisitionRepository;
        }

        public async Task<IEnumerable<Customer>> GetCustomersAsync(CustomerParameters parameters) =>
            await _customerDataAcquisitionRepository.GetCustomersAsync(parameters);

        public async Task<Customer> GetCustomerAsync(int customerId, bool trackChanges) =>
            await _customerDataAcquisitionRepository.GetCustomerAsync(customerId, trackChanges);

        public async Task AddCustomerAsync(Customer customer) => await CreateAsync(customer);

        public void UpdateCustomer(Customer customer) => Update(customer);

        public void DeleteCustomer(Customer customer) => Delete(customer);

        public async Task DeleteCustomerByIdAsync(int customerId) =>
            Delete(await GetCustomerAsync(customerId, true));
        
        public async Task<Customer> FindCustomerByPhoneNumberAsync(string phoneNumber, bool trackChanges) =>
            await FindByCondition(c => c.PhoneNumber.Equals(phoneNumber), trackChanges)
            .FirstOrDefaultAsync();
    }
}
