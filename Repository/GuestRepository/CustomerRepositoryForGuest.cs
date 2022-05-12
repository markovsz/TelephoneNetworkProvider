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

        public Customer GetCustomerInfo(int customerId) =>
            _customerDataAcquisitionRepository.GetCustomerInfo(customerId, false);

        public IEnumerable<Customer> GetCustomers(CustomerParameters parameters) =>
            _customerDataAcquisitionRepository.GetCustomers(parameters);
    }
}
