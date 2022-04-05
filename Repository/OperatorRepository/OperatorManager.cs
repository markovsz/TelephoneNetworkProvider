using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class OperatorManager : IOperatorManager
    {
        private RepositoryContext _repositoryContext;
        private ICallRepositoryForOperator _callRepositoryForOperator;

        public OperatorManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
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
        public void Save() => _repositoryContext.SaveChanges();
    }
}
