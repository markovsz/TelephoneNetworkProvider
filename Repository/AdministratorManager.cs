using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
;

namespace Repository
{
    public class AdministratorManager : IAdministratorManager
    {
        private RepositoryContext _repositoryContext;
        private ICustomerRepositoryForAdministrator _customerRepositoryForAdministrator;
        private ICallRepositoryForAdministrator _callRepositoryForAdministrator;
        public AdministratorManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public ICustomerRepositoryForAdministrator Customers
        {
            get
            {
                if (_customerRepositoryForAdministrator == null)
                    _customerRepositoryForAdministrator = new CustomerRepositoryForAdministrator(_repositoryContext);
                return _customerRepositoryForAdministrator;
            }
        }

        public ICallRepositoryForAdministrator Calls
        {
            get
            {
                if (_callRepositoryForAdministrator == null)
                    _callRepositoryForAdministrator = new CallRepositoryForAdministrator(_repositoryContext);
                return _callRepositoryForAdministrator;
            }
        }

        public void Save() => _repositoryContext.SaveChanges();
    }
}
