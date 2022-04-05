using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class GuestManager : IGuestManager
    {
        private RepositoryContext _repositoryContext;
        private ICustomerRepositoryForGuest _customerRepositoryForGuest;

        public GuestManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public ICustomerRepositoryForGuest Customers
        {
            get
            {
                if (_customerRepositoryForGuest == null)
                    _customerRepositoryForGuest = new CustomerRepositoryForGuest(_repositoryContext);
                return _customerRepositoryForGuest;
            }
        }

        public void Save() => _repositoryContext.SaveChanges();
    }
}
