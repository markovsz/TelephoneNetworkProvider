using System;
using System.Collections.Generic;
using System.Linq;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Repository;
using AutoMapper;
using BussinessLogic.Exceptions;

namespace BussinessLogic
{
    public class AdministratorLogic : IAdministratorLogic
    {
        private IAuthenticationLogic _authenticationLogic;
        private IUserManipulationLogic _userManipulationLogic;
        private IAdministratorManager _administratorManager;
        private IMapper _mapper;

        //public delegate void Message(string userId);
        //private event Message SendMessageEvent;

        public AdministratorLogic(IAdministratorManager administratorManager, IAuthenticationLogic authenticationLogic, IUserManipulationLogic userManipulationLogic, IMapper mapper)
        {
            _administratorManager = administratorManager;
            _userManipulationLogic = userManipulationLogic;
            _authenticationLogic = authenticationLogic;
            _mapper = mapper;
        }


        public bool CheckCall(uint id)
        {
            return _administratorManager.Calls.GetCall(id) is not null;
        }

        public bool CheckCustomer(string userId)
        {
            return _administratorManager.Customers.GetCustomerByUserId(userId, false) is not null;
        }

        public Call GetCallInfo(uint id)
        {
            return _administratorManager.Calls.GetCall(id);
        }

        public IEnumerable<Call> GetCalls(CallParameters parameters)
        {
            return _administratorManager.Calls.GetCalls(parameters);
        }

        public IEnumerable<Call> GetCustomerCalls(string userId, CallParameters parameters)
        {
            return _administratorManager.Calls.GetCustomerCalls(userId, parameters);
        }

        public Customer GetCustomerInfo(string userId)
        {
            return _administratorManager.Customers.GetCustomerByUserId(userId, false);
        }

        public IEnumerable<Customer> GetCustomers(CustomerParameters parameters)
        {
            return _administratorManager.Customers.GetCustomers(parameters, false);
        }

        public DateTime TimePastsFromLastWarningMessage(string userId)
        {
            var customer = _administratorManager.Customers.GetCustomerByUserId(userId, false);
            var message = _administratorManager.Messages.GetCustomerWarningMessagesFromTime(userId, customer.LastBlockTime)
                .OrderByDescending(m => m.SendingTime)
                .FirstOrDefault();
            return message.SendingTime;
        }

        public void SendMessage(string userId, AdministratorMessageForCreateDto messageDto)
        {
            var customer = _administratorManager.Customers.GetCustomerByUserId(userId, true);
            var messages = _administratorManager.Messages.GetCustomerWarningMessagesFromTime(userId, customer.LastBlockTime);
            if (customer.IsBlocked) return;

            AdministratorMessage message = new AdministratorMessage();
            message.UserId = customer.UserId;
            message.Status = messageDto.Status;
            message.Text = messageDto.Text;
            message.SendingTime = DateTime.Now;
            _administratorManager.Messages.CreateMessage(message);
            if (message.Status.Equals("warning"))
            {
            }
        }

        public void CreateCustomer(CustomerForCreateInAdministratorDto customerDto)
        {
            if(_administratorManager.Customers.GetCustomerByUserId(customerDto.UserId, false) is null)
            {
                throw new UserExistException("user is already existing");
            }
            Customer customer = _mapper.Map<Customer>(customerDto);
            customer.MoneyBalance = 0;
            customer.RegistrationDate = DateTime.Now;
            customer.IsPhoneNumberHided = false;
            customer.IsBlocked = false;
            User user = _mapper.Map<User>(customerDto);
            _userManipulationLogic.CreateUser(user);
            _administratorManager.Customers.AddCustomer(customer);
        }

        public bool CheckPhoneNumberForExistence(string phoneNumber)
        {
            return _administratorManager.Customers.FindCustomerByPhoneNumber(phoneNumber, false) is not null;
        }

        public bool TryToSetNewPhoneNumber(string userId, string phoneNumber)
        {
            bool isAny = CheckPhoneNumberForExistence(phoneNumber);
            _administratorManager.Customers.GetCustomerByUserId(userId, true).PhoneNumber = phoneNumber;
            return !isAny;
        }

        public void BlockCustomer(string userId)
        {
            var customer = _administratorManager.Customers.GetCustomerByUserId(userId, true);
            if (customer.IsBlocked) return;
            var messages = _administratorManager.Messages.GetCustomerWarningMessagesFromTime(userId, customer.LastBlockTime);

            if (messages.Count() >= 3) customer.IsBlocked = true;
        }

        public void DeleteCustomer(string userId)
        {
            _administratorManager.Customers.DeleteCustomerByUserId(userId);
        }

        public void UpdateCustomer(string userId, CustomerForUpdateInAdministratorDto customerDto)
        {
            var customer = _mapper.Map<Customer>(customerDto);
            customer.UserId = userId;
            _administratorManager.Customers.UpdateCustomer(customer);
        }
    }
}
