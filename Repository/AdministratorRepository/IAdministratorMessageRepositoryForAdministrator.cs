using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.RequestFeatures;

namespace Repository.AdministratorRepository
{
    public interface IAdministratorMessageRepositoryForAdministrator
    {
        Task<IEnumerable<AdministratorMessage>> GetMessageAsync(int id);
        Task<IEnumerable<AdministratorMessage>> GetCustomerMessagesAsync(int customerId, AdministratorMessageParameters parameters);
        Task<IEnumerable<AdministratorMessage>> GetCustomerWarningMessagesFromTimeAsync(int customerId, DateTime startTime);
        Task CreateMessageAsync(AdministratorMessage message);
        Task DeleteMessageAsync(int id);
    }
}
