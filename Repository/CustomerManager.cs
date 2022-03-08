using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace Repository
{
    public class CustomerManager : ICustomerManager
    {
        private RepositoryContext _repositoryContext;
        private ICustomerRepositoryForCustomer _customerRepositoryForCustomer;
        private ICallRepositoryForCustomer _callRepositoryForCustomer;

        public CustomerManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public ICustomerRepositoryForCustomer Customer
        {
            get
            {
                if (_customerRepositoryForCustomer == null)
                    _customerRepositoryForCustomer = new CustomerRepositoryForCustomer(_repositoryContext);
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

        public void Save() => _repositoryContext.SaveChanges();
    }
}
