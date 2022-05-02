using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Repository.CustomerAcquisitionRepository;

namespace Repository.CustomerRepository
{
    public class CustomerManager : ICustomerManager
    {
        private RepositoryContext _repositoryContext;
        private ICustomerRepositoryForCustomer _customerRepositoryForCustomer;
        private ICallRepositoryForCustomer _callRepositoryForCustomer;
        private IAdministratorMessageRepositoryForCustomer _administratorMessageRepositoryForCustomer;
        private ICustomerDataAcquisitionRepository _customerDataAcquisitionRepository;

        public CustomerManager(RepositoryContext repositoryContext, ICustomerDataAcquisitionRepository customerDataAcquisitionRepository)
        {
            _repositoryContext = repositoryContext;
            _customerDataAcquisitionRepository = customerDataAcquisitionRepository;
        }

        public ICustomerRepositoryForCustomer Customer
        {
            get
            {
                if (_customerRepositoryForCustomer == null)
                    _customerRepositoryForCustomer = new CustomerRepositoryForCustomer(_repositoryContext, _customerDataAcquisitionRepository);
                return _customerRepositoryForCustomer;
            }
        }

        public ICallRepositoryForCustomer Calls
        {
            get
            {
                if (_callRepositoryForCustomer == null)
                    _callRepositoryForCustomer = new CallRepositoryForCustomer(_repositoryContext);
                return _callRepositoryForCustomer;
            }
        }

        public IAdministratorMessageRepositoryForCustomer AdministratorMessages
        {
            get
            {
                if (_administratorMessageRepositoryForCustomer == null)
                    _administratorMessageRepositoryForCustomer = new AdministratorMessageRepositoryForCustomer(_repositoryContext);
                return _administratorMessageRepositoryForCustomer;
            }
        }

        public void Save() => _repositoryContext.SaveChanges();
    }
}
