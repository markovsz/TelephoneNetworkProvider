using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace Repository.CustomerAcquisitionRepository
{
    public class CustomerDataAcquisitionRepository : RepositoryBase<Customer>, ICustomerDataAcquisitionRepository
    {
        public CustomerDataAcquisitionRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public Customer GetCustomerInfo(uint customerId, bool trackChanges)
        {
            return FindByCondition(c => c.UserId.Equals(customerId), trackChanges)
                .FirstOrDefault();
        }

        public IEnumerable<Customer> GetCustomers(CustomerParameters parameters)
        {
            return FindAll(false)
                .CustomerParametersHandler(parameters);
        }
    }
}
