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
        IEnumerable<AdministratorMessage> GetMessage(uint id);
        IEnumerable<AdministratorMessage> GetCustomerMessages(uint customerId, AdministratorMessageParameters parameters);
        IEnumerable<AdministratorMessage> GetCustomerWarningMessagesFromTime(uint customerId, DateTime startTime);
        void CreateMessage(AdministratorMessage message);
        void DeleteMessage(uint id);
        //IEnumerable<AdministratorMessage> GetAdministratorMessagesByCustomerId(uint customerId, AdministratorMessageParameters parameters);

    }
}
