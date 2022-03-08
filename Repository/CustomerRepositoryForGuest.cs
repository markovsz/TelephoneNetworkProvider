using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository
{
    public class CustomerRepositoryForGuest : RepositoryBase<Customer>, ICustomerRepositoryForGuest
    {
        public CustomerRepositoryForGuest(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {

        }
        public IEnumerable<Customer> GetCustomers(CustomerParameters parameters) =>
            FindByCondition(c => true, false)
            .ToList();
    }
}
