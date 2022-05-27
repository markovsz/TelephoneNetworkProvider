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
        Task<CustomerForReadInAdministratorDto> GetCustomerInfoAsync(int customerId);
        Task<IEnumerable<CustomerForReadInAdministratorDto>> GetCustomersInfoAsync(CustomerParameters parameters);
        Task<CallForReadInAdministratorDto> GetCallInfoAsync(int id);
        Task<IEnumerable<CallForReadInAdministratorDto>> GetCustomerCallsInfoAsync(int customerId, CallParameters parameters);
        Task<IEnumerable<CallForReadInAdministratorDto>> GetCallsInfoAsync(CallParameters parameters);
        Task<(int, CustomerForReadInAdministratorDto)> CreateCustomerAsync(CustomerForCreateInAdministratorDto customerDto);
        Task UpdateCustomerAsync(int customerId, CustomerForUpdateInAdministratorDto customer);
        Task BlockCustomerAsync(int customerId);
        Task DeleteCustomerAsync(int customerId);
        Task<IEnumerable<AdministratorMessageForReadInAdministratorDto>> GetAdministratorMessagesByCustomerIdAsync(int customerId, AdministratorMessageParameters parameters);
        Task SendMessageAsync(int customerId, AdministratorMessageForCreateInAdministratorDto messageDto);
        Task<DateTime> TimePastsFromLastWarnMessageAsync(int customerId);
        Task<bool> CheckPhoneNumberForExistenceAsync(string phoneNumber);
        Task<bool> TryToSetNewPhoneNumberAsync(int customerId, string phoneNumber);
    }
}
