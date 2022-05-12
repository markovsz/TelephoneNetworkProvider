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
        private IUserManipulationLogic _userManipulationLogic;
        private IAdministratorManager _administratorManager;
        private IMapper _mapper;


        public AdministratorLogic(IAdministratorManager administratorManager, IUserManipulationLogic userManipulationLogic, IMapper mapper)
        {
            _administratorManager = administratorManager;
            _userManipulationLogic = userManipulationLogic;
            _mapper = mapper;
        }

        public bool CheckCall(int id)
        {
            return _administratorManager.Calls.GetCall(id) is not null;
        }

        public bool CheckCustomer(int customerId)
        {
            return _administratorManager.Customers.GetCustomerInfo(customerId, false) is not null;
        }

        public CallForReadInAdministratorDto GetCallInfo(int id)
        {
            var call = _administratorManager.Calls.GetCall(id);
            var callDto = _mapper.Map<CallForReadInAdministratorDto>(call);
            return callDto;
        }

        public IEnumerable<CallForReadInAdministratorDto> GetCalls(CallParameters parameters)
        {
            var call = _administratorManager.Calls.GetCalls(parameters);
            var callDto = _mapper.Map<IEnumerable<CallForReadInAdministratorDto>>(call);
            return callDto;
        }

        public IEnumerable<CallForReadInAdministratorDto> GetCustomerCalls(int customerId, CallParameters parameters)
        {
            var call = _administratorManager.Calls.GetCustomerCalls(customerId, parameters);
            var callDto = _mapper.Map<IEnumerable<CallForReadInAdministratorDto>>(call);
            return callDto;
        }

        public CustomerForReadInAdministratorDto GetCustomerInfo(int customerId)
        {
            var customer = _administratorManager.Customers.GetCustomerInfo(customerId, false);
            var customerDto = _mapper.Map<CustomerForReadInAdministratorDto>(customer);
            return customerDto;
        }

        public IEnumerable<CustomerForReadInAdministratorDto> GetCustomers(CustomerParameters parameters)
        {
            var customers = _administratorManager.Customers.GetCustomers(parameters, false);
            var customersDto = _mapper.Map<IEnumerable<CustomerForReadInAdministratorDto>>(customers);
            return customersDto;
        }

        public DateTime TimePastsFromLastWarnMessage(int customerId)
        {
            var customer = _administratorManager.Customers.GetCustomerInfo(customerId, false);
            var message = _administratorManager.Messages.GetCustomerWarningMessagesFromTime(customerId, customer.LastBlockTime)
                .OrderByDescending(m => m.SendingTime)
                .FirstOrDefault();
            if (message is null)
                return new DateTime(1, 0, 0);
            return message.SendingTime;
        }

        public IEnumerable<AdministratorMessageForReadInAdministratorDto> GetAdministratorMessagesByCustomerId(int customerId, AdministratorMessageParameters parameters)
        {
            var messages = _administratorManager.Messages.GetCustomerMessages(customerId, parameters);
            var messagesDto = _mapper.Map<IEnumerable<AdministratorMessageForReadInAdministratorDto>>(messages);
            return messagesDto;
        }

        public void SendMessage(int customerId, AdministratorMessageForCreateInAdministratorDto messageDto)
        {
            var customer = _administratorManager.Customers.GetCustomerInfo(customerId, true);
            //var messages = _administratorManager.Messages.GetCustomerWarningMessagesFromTime(customerId, customer.LastBlockTime);
            if (customer.IsBlocked)
                throw new CustomerBlockedException("Customer is blocked and cannot catch the messages");

            if (TimePastsFromLastWarnMessage(customerId).CompareTo(new DateTime(0, 0, 1/*1 day*/)) >= 0)
                throw new SendMessageException("Didn't past enough time to send a new message");

            AdministratorMessage message = new AdministratorMessage();
            message.CustomerId = customer.Id;
            message.Status = messageDto.Status;
            message.Text = messageDto.Text;
            message.SendingTime = DateTime.Now;
            _administratorManager.Messages.CreateMessage(message);
        }

        public async Task CreateCustomer(CustomerForCreateInAdministratorDto customerDto)
        {

            //bool isExist = _administratorManager.Customers.CheckCustomerByUserId();
            if(_administratorManager.Customers.FindCustomerByPhoneNumber(customerDto.PhoneNumber, false) is not null)
            {
                throw new UserExistException("user is already existing");
            }
            Customer customer = _mapper.Map<Customer>(customerDto);
            customer.MoneyBalance = 0;
            customer.RegistrationDate = DateTime.Now;
            customer.IsPhoneNumberHided = customerDto.IsPhoneNumberHidedStr == "true" ? true : false;
            customer.IsBlocked = false;
            var user = await _userManipulationLogic.CreateUser(customerDto.Login, customerDto.Password, "Customer");

            customer.UserId = user.Id;
            _administratorManager.Customers.AddCustomer(customer);
            _administratorManager.Save();
        }

        public bool CheckPhoneNumberForExistence(string phoneNumber)
        {
            return _administratorManager.Customers.FindCustomerByPhoneNumber(phoneNumber, false) is not null;
        }

        public bool TryToSetNewPhoneNumber(int customerId, string phoneNumber)
        {
            bool isAny = CheckPhoneNumberForExistence(phoneNumber);
            if (!isAny)
            {
                _administratorManager.Customers.GetCustomerInfo(customerId, true).PhoneNumber = phoneNumber;
                _administratorManager.Save();
            }
            return !isAny;
        }

        public void BlockCustomer(int customerId)
        {
            var customer = _administratorManager.Customers.GetCustomerInfo(customerId, true);

            if (customer.IsBlocked) return;
            var messages = _administratorManager.Messages.GetCustomerWarningMessagesFromTime(customerId, customer.LastBlockTime);

            if (messages.Count() >= 3) customer.IsBlocked = true;
            _administratorManager.Save();
        }

        public void DeleteCustomer(int customerId)
        {
            _administratorManager.Customers.DeleteCustomerByUserId(customerId);
        }

        public void UpdateCustomer(int customerId, CustomerForUpdateInAdministratorDto customerDto)
        {
            var customer = _mapper.Map<Customer>(customerDto);
            customer.Id = customerId;
            _administratorManager.Customers.UpdateCustomer(customer);
        }
    }
}
