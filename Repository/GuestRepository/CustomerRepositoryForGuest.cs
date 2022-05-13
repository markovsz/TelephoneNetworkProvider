using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Repository.CustomerAcquisitionRepository;

namespace Repository.GuestRepository
{
    public class CustomerRepositoryForGuest : RepositoryBase<Customer>, ICustomerRepositoryForGuest
    {
        private ICustomerDataAcquisitionRepository _customerDataAcquisitionRepository;

        public CustomerRepositoryForGuest(RepositoryContext repositoryContext, ICustomerDataAcquisitionRepository customerDataAcquisitionRepository)
            : base(repositoryContext)
        {
            _customerDataAcquisitionRepository = customerDataAcquisitionRepository;
        }

        public async Task<Customer> GetCustomerInfoAsync(int customerId) =>
            await _customerDataAcquisitionRepository.GetCustomerInfoAsync(customerId, false);

        public async Task<IEnumerable<Customer>> GetCustomersAsync(CustomerParameters parameters) =>
            await _customerDataAcquisitionRepository.GetCustomersAsync(parameters);
    }
}
