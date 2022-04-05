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
        IEnumerable<Customer> GetCustomers(CustomerParameters parameters);
        bool CheckCustomer(string userId);
        Customer GetCustomerInfo(string userId);
        IEnumerable<Call> GetCustomerCalls(string userId, CallParameters parameters);
        IEnumerable<Call> GetCalls(CallParameters parameters);
        bool CheckCall(uint id);
        Call GetCallInfo(uint id);
        void CreateCustomer(CustomerForCreateInAdministratorDto customerDto);
        void UpdateCustomer(string userId, CustomerForUpdateInAdministratorDto customer);
        void SendMessage(string userId, AdministratorMessageForCreateDto messageDto);
        bool CheckPhoneNumberForExistence(string phoneNumber);
        bool TryToSetNewPhoneNumber(string userId, string phoneNumber);
        void BlockCustomer(string userId);
    }
}
