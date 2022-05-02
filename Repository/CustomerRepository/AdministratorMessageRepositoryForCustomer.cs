using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.RequestFeatures;

namespace Repository.CustomerRepository
{
    public class AdministratorMessageRepositoryForCustomer : RepositoryBase<AdministratorMessage>, IAdministratorMessageRepositoryForCustomer
    {
        public AdministratorMessageRepositoryForCustomer(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public IEnumerable<AdministratorMessage> GetMessages(uint customerId, AdministratorMessageParameters parameters) =>
            FindByCondition(m => m.CustomerId.Equals(customerId), false)
            .AdministratorMessageParametersHandler(parameters)
            .ToList();
    }
}
