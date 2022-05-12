using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Entities.RequestFeatures;
using Entities.DataTransferObjects;
using Repository;

namespace BussinessLogic
{
    public interface IAdministratorLogic
    {
        bool CheckCustomer(int customerId);
        IEnumerable<CustomerForReadInAdministratorDto> GetCustomers(CustomerParameters parameters);
        CustomerForReadInAdministratorDto GetCustomerInfo(int customerId);
        IEnumerable<CallForReadInAdministratorDto> GetCustomerCalls(int customerId, CallParameters parameters);
        IEnumerable<CallForReadInAdministratorDto> GetCalls(CallParameters parameters);
        bool CheckCall(int id);
        CallForReadInAdministratorDto GetCallInfo(int id);
        void CreateCustomer(CustomerForCreateInAdministratorDto customerDto);
        void UpdateCustomer(int customerId, CustomerForUpdateInAdministratorDto customer);
        void DeleteCustomer(int customerId);
        void SendMessage(int customerId, AdministratorMessageForCreateInAdministratorDto messageDto);
        DateTime TimePastsFromLastWarnMessage(int customerId);
        IEnumerable<AdministratorMessageForReadInAdministratorDto> GetAdministratorMessagesByCustomerId(int customerId, AdministratorMessageParameters parameters);
        bool CheckPhoneNumberForExistence(string phoneNumber);
        bool TryToSetNewPhoneNumber(int customerId, string phoneNumber);
        void BlockCustomer(int customerId);
    }
}
