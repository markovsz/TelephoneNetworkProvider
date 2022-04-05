using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.RequestFeatures;

namespace Repository
{
    public interface IAdministratorMessageRepositoryForAdministrator
    {
        IEnumerable<AdministratorMessage> GetMessage(uint id);
        IEnumerable<AdministratorMessage> GetCustomerMessages(string userId, AdministratorMessageParameters parameters);
        IEnumerable<AdministratorMessage> GetCustomerWarningMessagesFromTime(string userId, DateTime startTime);
        void CreateMessage(AdministratorMessage message);
        void DeleteMessage(uint id);

    }
}
