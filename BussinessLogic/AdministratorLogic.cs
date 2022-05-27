using System;
using System.Collections.Generic;
using System.Linq;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Repository.AdministratorRepository;
using AutoMapper;
using BussinessLogic.Exceptions;
using System.Threading.Tasks;

namespace BussinessLogic
{
    public class AdministratorLogic : IAdministratorLogic
    {
        const decimal StartingMoneyBalance = 0.0m;

        private IUserManipulationLogic _userManipulationLogic;
        private IAdministratorManager _administratorManager;
        private IMapper _mapper;


        public AdministratorLogic(IAdministratorManager administratorManager, IUserManipulationLogic userManipulationLogic, IMapper mapper)
        {
            _administratorManager = administratorManager;
            _userManipulationLogic = userManipulationLogic;
            _mapper = mapper;
        }

        private async Task<Customer> GetCustomerAsync(int customerId, bool trackChanges)
        {
            var customer = await _administratorManager.Customers.GetCustomerAsync(customerId, trackChanges);
            if (customer is null)
                throw new CustomerDoesntExistException("Customer with this id doesn't exist");
            return customer;
        }
        
        private async Task<Call> GetCallAsync(int callId)
        {
            var call = await _administratorManager.Calls.GetCallAsync(callId);
            if (call is null)
                throw new CallDoesntExistException("Call with this id doesn't exist");
            return call;
        }

        private async Task<bool> CheckCustomerAsync(int customerId)
        {
            return (await _administratorManager.Customers.GetCustomerAsync(customerId, false)) is not null;
        }

        public async Task<CallForReadInAdministratorDto> GetCallInfoAsync(int callId)
        {
            var call = await GetCallAsync(callId);
            var callDto = _mapper.Map<CallForReadInAdministratorDto>(call);
            return callDto;
        }

        public async Task<IEnumerable<CallForReadInAdministratorDto>> GetCallsInfoAsync(CallParameters parameters)
        {
            var call = await _administratorManager.Calls.GetCallsAsync(parameters);
            var callDto = _mapper.Map<IEnumerable<CallForReadInAdministratorDto>>(call);
            return callDto;
        }

        public async Task<IEnumerable<CallForReadInAdministratorDto>> GetCustomerCallsInfoAsync(int customerId, CallParameters parameters)
        {
            if (!(await CheckCustomerAsync(customerId)))
                throw new CustomerDoesntExistException("Customer with this id doesn't exist");

            var call = await _administratorManager.Calls.GetCustomerCallsAsync(customerId, parameters);
            var callDto = _mapper.Map<IEnumerable<CallForReadInAdministratorDto>>(call);
            return callDto;
        }

        public async Task<CustomerForReadInAdministratorDto> GetCustomerInfoAsync(int customerId)
        {
            var customer = await GetCustomerAsync(customerId, false);
            var customerDto = _mapper.Map<CustomerForReadInAdministratorDto>(customer);
            return customerDto;
        }

        public async Task<IEnumerable<CustomerForReadInAdministratorDto>> GetCustomersInfoAsync(CustomerParameters parameters)
        {
            var customers = await _administratorManager.Customers.GetCustomersAsync(parameters);
            var customersDto = _mapper.Map<IEnumerable<CustomerForReadInAdministratorDto>>(customers);
            return customersDto;
        }

        public async Task<DateTime> TimePastsFromLastWarnMessageAsync(int customerId)
        {
            var customer = await GetCustomerAsync(customerId, false);
            var message = (await _administratorManager.Messages.GetCustomerWarningMessagesFromTimeAsync(customerId, customer.LastBlockTime))
                .OrderByDescending(m => m.SendingTime)
                .FirstOrDefault();

            if (message is null)
                return new DateTime(1, 0, 0);
            return message.SendingTime;
        }

        public async Task<IEnumerable<AdministratorMessageForReadInAdministratorDto>> GetAdministratorMessagesByCustomerIdAsync(int customerId, AdministratorMessageParameters parameters)
        {
            if (!(await CheckCustomerAsync(customerId)))
                throw new CustomerDoesntExistException("Customer with this id doesn't exist");

            var messages = await _administratorManager.Messages.GetCustomerMessagesAsync(customerId, parameters);
            var messagesDto = _mapper.Map<IEnumerable<AdministratorMessageForReadInAdministratorDto>>(messages);
            return messagesDto;
        }

        public async Task SendMessageAsync(int customerId, AdministratorMessageForCreateInAdministratorDto messageDto)
        {
            var customer = await GetCustomerAsync(customerId, false);

            if (customer.IsBlocked)
                throw new InvalidOperationException("Customer is blocked and he cannot get the messages"); //CustomerBlockedException

            if ((await TimePastsFromLastWarnMessageAsync(customerId)).AddDays(1).CompareTo(DateTime.Now) >= 0)
                throw new InvalidOperationException("Didn't past enough time to send a new message"); //SendMessageException

            AdministratorMessage message = new AdministratorMessage();
            message.CustomerId = customer.Id;
            message.Status = messageDto.Status;
            message.Text = messageDto.Text;
            message.SendingTime = DateTime.Now;
            await _administratorManager.Messages.CreateMessageAsync(message);
        }

        public async Task<(int, CustomerForReadInAdministratorDto)> CreateCustomerAsync(CustomerForCreateInAdministratorDto customerDto)
        {
            if((await _administratorManager.Customers.FindCustomerByPhoneNumberAsync(customerDto.PhoneNumber, false)) is not null)
                throw new InvalidOperationException("Customer with given phone number is already existing");
            
            var userCreationResult = _userManipulationLogic.CreateUserAsync(customerDto.Login, customerDto.Password, "Customer");
            Customer customer = _mapper.Map<Customer>(customerDto);
            customer.MoneyBalance = StartingMoneyBalance;
            customer.RegistrationDate = DateTime.Now;
            customer.IsBlocked = false;

            var user = await userCreationResult;

            customer.UserId = user.Id;
            await _administratorManager.Customers.AddCustomerAsync(customer);
            await _administratorManager.SaveAsync();
            return (customer.Id, _mapper.Map<CustomerForReadInAdministratorDto>(customer));
        }

        public async Task<bool> CheckPhoneNumberForExistenceAsync(string phoneNumber)
        {
            return (await _administratorManager.Customers.FindCustomerByPhoneNumberAsync(phoneNumber, false)) is not null;
        }

        public async Task<bool> TryToSetNewPhoneNumberAsync(int customerId, string phoneNumber)
        {
            bool isAny = await CheckPhoneNumberForExistenceAsync(phoneNumber);
            if (!isAny)
            {
                (await GetCustomerAsync(customerId, true)).PhoneNumber = phoneNumber;
                await _administratorManager.SaveAsync();
            }
            return !isAny;
        }

        public async Task BlockCustomerAsync(int customerId)
        {
            var customer = await GetCustomerAsync(customerId, true);

            if (customer.IsBlocked)
                throw new InvalidOperationException("Customer is already blocked");

            var messages = await _administratorManager.Messages.GetCustomerWarningMessagesFromTimeAsync(customerId, customer.LastBlockTime);

            if (messages.Count() >= 3) customer.IsBlocked = true;
            else
                throw new InvalidOperationException("Too few messages to block customer");

            await _administratorManager.SaveAsync();
        }

        public async Task DeleteCustomerAsync(int customerId)
        {
            if (!(await CheckCustomerAsync(customerId)))
                throw new CustomerDoesntExistException("Customer with this id doesn't exist");

            await _administratorManager.Customers.DeleteCustomerByIdAsync(customerId);
        }

        public async Task UpdateCustomerAsync(int customerId, CustomerForUpdateInAdministratorDto customerDto)
        {
            if (!(await CheckCustomerAsync(customerId)))
                throw new CustomerDoesntExistException("Customer with this id doesn't exist");

            var customer = _mapper.Map<Customer>(customerDto);
            customer.Id = customerId;
            _administratorManager.Customers.UpdateCustomer(customer);
        }
    }
}
