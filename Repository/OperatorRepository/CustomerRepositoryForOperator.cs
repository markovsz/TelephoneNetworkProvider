using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Repository.CustomerAcquisitionRepository;

namespace Repository.OperatorRepository
{
    class CustomerRepositoryForOperator : RepositoryBase<Customer>, ICustomerRepositoryForOperator
    {
        private ICustomerDataAcquisitionRepository _customerDataAcquisitionRepository;

        public CustomerRepositoryForOperator(RepositoryContext repositoryContext, ICustomerDataAcquisitionRepository customerDataAcquisitionRepository)
            : base(repositoryContext)
        {
            _customerDataAcquisitionRepository = customerDataAcquisitionRepository;
        }

        public async Task<IEnumerable<Customer>> GetCustomersAsync(CustomerParameters parameters) =>
            await _customerDataAcquisitionRepository.GetCustomersAsync(c => !c.IsBlocked && !c.IsPhoneNumberHided, parameters);

        public async Task<Customer> GetCustomerAsync(int customerId) =>
            await _customerDataAcquisitionRepository.GetCustomerAsync(customerId, c => !c.IsBlocked && !c.IsPhoneNumberHided, false);

        public async Task<Customer> GetUnblockedCustomerAsync(int customerId) =>
            await _customerDataAcquisitionRepository.GetCustomerAsync(customerId, c => !c.IsBlocked, false);
    }
}
