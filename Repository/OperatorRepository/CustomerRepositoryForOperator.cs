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

        public IEnumerable<Customer> GetCustomers(CustomerParameters parameters) =>
            _customerDataAcquisitionRepository.GetCustomers(parameters);

        public Customer GetCustomerInfo(int customerId) =>
            _customerDataAcquisitionRepository.GetCustomerInfo(customerId, false);
    }
}
