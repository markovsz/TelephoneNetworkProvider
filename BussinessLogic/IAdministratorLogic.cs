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
        Task<bool> CheckCustomerAsync(int customerId);
        Task<IEnumerable<CustomerForReadInAdministratorDto>> GetCustomersAsync(CustomerParameters parameters);
        Task<CustomerForReadInAdministratorDto> GetCustomerInfoAsync(int customerId);
        Task<IEnumerable<CallForReadInAdministratorDto>> GetCustomerCallsAsync(int customerId, CallParameters parameters);
        Task<IEnumerable<CallForReadInAdministratorDto>> GetCallsAsync(CallParameters parameters);
        Task<bool> CheckCallAsync(int id);
        Task<CallForReadInAdministratorDto> GetCallInfoAsync(int id);
        Task<(int, CustomerForReadInAdministratorDto)> CreateCustomerAsync(CustomerForCreateInAdministratorDto customerDto);
        Task UpdateCustomerAsync(int customerId, CustomerForUpdateInAdministratorDto customer);
        Task DeleteCustomerAsync(int customerId);
        Task SendMessageAsync(int customerId, AdministratorMessageForCreateInAdministratorDto messageDto);
        Task<DateTime> TimePastsFromLastWarnMessageAsync(int customerId);
        Task<IEnumerable<AdministratorMessageForReadInAdministratorDto>> GetAdministratorMessagesByCustomerIdAsync(int customerId, AdministratorMessageParameters parameters);
        Task<bool> CheckPhoneNumberForExistenceAsync(string phoneNumber);
        Task<bool> TryToSetNewPhoneNumberAsync(int customerId, string phoneNumber);
        Task BlockCustomerAsync(int customerId);
    }
}
