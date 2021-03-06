using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository.CustomerRepository
{
    public interface IAdministratorMessageRepositoryForCustomer
    {
        Task<IEnumerable<AdministratorMessage>> GetMessagesAsync(int customerId, AdministratorMessageParameters parameters);
    }
}
