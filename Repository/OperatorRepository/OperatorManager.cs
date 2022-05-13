using Entities;
using Repository.CustomerAcquisitionRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.OperatorRepository
{
    public class OperatorManager : IOperatorManager
    {
        private RepositoryContext _repositoryContext;
        private ICallRepositoryForOperator _callRepositoryForOperator;
        private ICustomerRepositoryForOperator _customerRepositoryForOperator;
        private ICustomerDataAcquisitionRepository _customerDataAcquisitionRepository;

        public OperatorManager(RepositoryContext repositoryContext, ICustomerDataAcquisitionRepository customerDataAcquisitionRepository)
        {
            _repositoryContext = repositoryContext;
            _customerDataAcquisitionRepository = customerDataAcquisitionRepository;
        }

        public ICallRepositoryForOperator Calls
        {
            get
            {
                if (_callRepositoryForOperator == null) 
                    _callRepositoryForOperator = new CallRepositoryForOperator(_repositoryContext);
                return _callRepositoryForOperator;
            }
        }

        public ICustomerRepositoryForOperator Customers
        {
            get
            {
                if (_customerRepositoryForOperator == null)
                    _customerRepositoryForOperator = new CustomerRepositoryForOperator(_repositoryContext, _customerDataAcquisitionRepository);
                return _customerRepositoryForOperator;
            }
        }

        public async Task SaveAsync() => await _repositoryContext.SaveChangesAsync();
    }
}
