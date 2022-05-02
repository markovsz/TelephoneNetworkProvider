using Entities;
using Repository.CustomerAcquisitionRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.GuestRepository
{
    public class GuestManager : IGuestManager
    {
        private RepositoryContext _repositoryContext;
        private ICustomerRepositoryForGuest _customerRepositoryForGuest;
        private ICustomerDataAcquisitionRepository _customerDataAcquisitionRepository;

        public GuestManager(RepositoryContext repositoryContext, ICustomerDataAcquisitionRepository customerDataAcquisitionRepository)
        {
            _repositoryContext = repositoryContext;
            _customerDataAcquisitionRepository = customerDataAcquisitionRepository;
        }

        public ICustomerRepositoryForGuest Customers
        {
            get
            {
                if (_customerRepositoryForGuest == null)
                    _customerRepositoryForGuest = new CustomerRepositoryForGuest(_repositoryContext, _customerDataAcquisitionRepository);
                return _customerRepositoryForGuest;
            }
        }

        public void Save() => _repositoryContext.SaveChanges();
    }
}
