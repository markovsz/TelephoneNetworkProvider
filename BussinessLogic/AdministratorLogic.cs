﻿using System;
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

        public async Task<bool> CheckCallAsync(int id)
        {
            return (await _administratorManager.Calls.GetCallAsync(id)) is not null;
        }

        public async Task<bool> CheckCustomerAsync(int customerId)
        {
            return (await _administratorManager.Customers.GetCustomerInfoAsync(customerId, false)) is not null;
        }

        public async Task<CallForReadInAdministratorDto> GetCallInfoAsync(int id)
        {
            var call = await _administratorManager.Calls.GetCallAsync(id);
            var callDto = _mapper.Map<CallForReadInAdministratorDto>(call);
            return callDto;
        }

        public async Task<IEnumerable<CallForReadInAdministratorDto>> GetCallsAsync(CallParameters parameters)
        {
            var call = await _administratorManager.Calls.GetCallsAsync(parameters);
            var callDto = _mapper.Map<IEnumerable<CallForReadInAdministratorDto>>(call);
            return callDto;
        }

        public async Task<IEnumerable<CallForReadInAdministratorDto>> GetCustomerCallsAsync(int customerId, CallParameters parameters)
        {
            var call = await _administratorManager.Calls.GetCustomerCallsAsync(customerId, parameters);
            var callDto = _mapper.Map<IEnumerable<CallForReadInAdministratorDto>>(call);
            return callDto;
        }

        public async Task<CustomerForReadInAdministratorDto> GetCustomerInfoAsync(int customerId)
        {
            var customer = await _administratorManager.Customers.GetCustomerInfoAsync(customerId, false);
            var customerDto = _mapper.Map<CustomerForReadInAdministratorDto>(customer);
            return customerDto;
        }

        public async Task<IEnumerable<CustomerForReadInAdministratorDto>> GetCustomersAsync(CustomerParameters parameters)
        {
            var customers = await _administratorManager.Customers.GetCustomersAsync(parameters, false);
            var customersDto = _mapper.Map<IEnumerable<CustomerForReadInAdministratorDto>>(customers);
            return customersDto;
        }

        public async Task<DateTime> TimePastsFromLastWarnMessageAsync(int customerId)
        {
            var customer = await _administratorManager.Customers.GetCustomerInfoAsync(customerId, false);
            var message = (await _administratorManager.Messages.GetCustomerWarningMessagesFromTimeAsync(customerId, customer.LastBlockTime))
                .OrderByDescending(m => m.SendingTime)
                .FirstOrDefault();
            if (message is null)
                return new DateTime(1, 0, 0);
            return message.SendingTime;
        }

        public async Task<IEnumerable<AdministratorMessageForReadInAdministratorDto>> GetAdministratorMessagesByCustomerIdAsync(int customerId, AdministratorMessageParameters parameters)
        {
            var messages = await _administratorManager.Messages.GetCustomerMessagesAsync(customerId, parameters);
            var messagesDto = _mapper.Map<IEnumerable<AdministratorMessageForReadInAdministratorDto>>(messages);
            return messagesDto;
        }

        public async Task SendMessageAsync(int customerId, AdministratorMessageForCreateInAdministratorDto messageDto)
        {
            var customer = await _administratorManager.Customers.GetCustomerInfoAsync(customerId, true);
            //var messages = _administratorManager.Messages.GetCustomerWarningMessagesFromTime(customerId, customer.LastBlockTime);
            if (customer.IsBlocked)
                throw new CustomerBlockedException("Customer is blocked and cannot catch the messages");

            if ((await TimePastsFromLastWarnMessageAsync(customerId)).CompareTo(new DateTime(0, 0, 1/*1 day*/)) >= 0)
                throw new SendMessageException("Didn't past enough time to send a new message");

            AdministratorMessage message = new AdministratorMessage();
            message.CustomerId = customer.Id;
            message.Status = messageDto.Status;
            message.Text = messageDto.Text;
            message.SendingTime = DateTime.Now;
            await _administratorManager.Messages.CreateMessageAsync(message);
        }

        public async Task CreateCustomerAsync(CustomerForCreateInAdministratorDto customerDto)
        {

            //bool isExist = _administratorManager.Customers.CheckCustomerByUserId();
            if((await _administratorManager.Customers.FindCustomerByPhoneNumberAsync(customerDto.PhoneNumber, false)) is not null)
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
            await _administratorManager.Customers.AddCustomerAsync(customer);
            await _administratorManager.SaveAsync();
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
                (await _administratorManager.Customers.GetCustomerInfoAsync(customerId, true)).PhoneNumber = phoneNumber;
                await _administratorManager.SaveAsync();
            }
            return !isAny;
        }

        public async Task BlockCustomerAsync(int customerId)
        {
            var customer = await _administratorManager.Customers.GetCustomerInfoAsync(customerId, true);

            if (customer.IsBlocked) return;
            var messages = await _administratorManager.Messages.GetCustomerWarningMessagesFromTimeAsync(customerId, customer.LastBlockTime);

            if (messages.Count() >= 3) customer.IsBlocked = true;
            await _administratorManager.SaveAsync();
        }

        public async Task DeleteCustomerAsync(int customerId)
        {
            await _administratorManager.Customers.DeleteCustomerByUserIdAsync(customerId);
        }

        public void UpdateCustomer(int customerId, CustomerForUpdateInAdministratorDto customerDto)
        {
            var customer = _mapper.Map<Customer>(customerDto);
            customer.Id = customerId;
            _administratorManager.Customers.UpdateCustomer(customer);
        }
    }
}
