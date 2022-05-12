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

        public IEnumerable<Customer> GetCustomers(CustomerParameters parameters, bool trackChanges) =>
            _customerDataAcquisitionRepository.GetCustomers(parameters);

        public Customer GetCustomerInfo(int customerId, bool trackChanges) =>
            _customerDataAcquisitionRepository.GetCustomerInfo(customerId, trackChanges);

        public void AddCustomer(Customer customer) => Create(customer);

        public void UpdateCustomer(Customer customer) => Update(customer);

        public void DeleteCustomer(Customer customer) => Delete(customer);

        public void DeleteCustomerByUserId(int customerId) =>
            Delete(GetCustomerInfo(customerId, true));
        
        public Customer FindCustomerByPhoneNumber(string phoneNumber, bool trackChanges) =>
            FindByCondition(c => c.PhoneNumber.Equals(phoneNumber), trackChanges)
            .FirstOrDefault();
    }
}
