using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Entities.Models;
using AutoMapper;
using Repository;
using Entities.DataTransferObjects;
using Entities.RequestFeatures;
using BussinessLogic;
using TelephoneNetworkProvider.ActionFilters;
using BussinessLogic.Exceptions;

namespace TelephoneNetworkProvider.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Route("/administrator-profile")]
    [ApiController]
    public class AdministratorController : ControllerBase
    {
        private IAdministratorLogic _administratorLogic;

        public AdministratorController(IAdministratorLogic administratorLogic)
        {
            _administratorLogic = administratorLogic;
        }

        [ServiceFilter(typeof(ParametersValidationFilterAttribute))]
        [HttpGet("/administrator-profile/customers")]
        public async Task<IActionResult> GetCustomersAsync([FromQuery] CustomerParameters parameters)
        {
            IEnumerable<CustomerForReadInAdministratorDto> customers;
            try
            {
                customers = await _administratorLogic.GetCustomersAsync(parameters);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(customers);
        }

        [ServiceFilter(typeof(CustomerExistenceFilterAttribute))]
        [HttpGet("/administrator-profile/customers/customer/{customerId:int}", Name = "GetCustomer")]
        public async Task<IActionResult> GetCustomerInfoAsync(int customerId)
        {
            CustomerForReadInAdministratorDto customer;
            try
            {
                customer = await _administratorLogic.GetCustomerInfoAsync(customerId);
            }
            catch (CustomerDoesntExistException e)
            {
                return NotFound(e.Message);
            }
            return Ok(customer);
        }


        [ServiceFilter(typeof(ParametersValidationFilterAttribute))]
        [ServiceFilter(typeof(CustomerExistenceFilterAttribute))]
        [HttpGet("/administrator-profile/customers/customer/{customerId:int}/calls")]
        public async Task<IActionResult> GetCustomerCallsAsync(int customerId, [FromQuery] CallParameters parameters)
        {
            IEnumerable<CallForReadInAdministratorDto> calls;
            try
            {
                calls = await _administratorLogic.GetCustomerCallsAsync(customerId, parameters);
            }
            catch (InvalidOperationException e)
            {
                return Conflict(e.Message);
            }
            return Ok(calls);
        }


        [ServiceFilter(typeof(ParametersValidationFilterAttribute))]
        [HttpGet("/administrator-profile/calls")]
        public async Task<IActionResult> GetCallsAsync([FromQuery] CallParameters parameters)
        {
            IEnumerable<CallForReadInAdministratorDto> calls;
            try
            {
                calls = await _administratorLogic.GetCallsAsync(parameters);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(calls);
        }


        [ServiceFilter(typeof(CallExistenceFilterAttribute))]
        [HttpGet("/administrator-profile/calls/call/{id:int}")]
        public async Task<IActionResult> GetCallInfoAsync(int callId)
        {
            CallForReadInAdministratorDto call;
            try
            {
                call = await _administratorLogic.GetCallInfoAsync(callId);
            }
            catch (CallDoesntExistException e)
            {
                return NotFound(e.Message);
            }
            return Ok(call);
        }


        [HttpPost("/administrator-profile/customers/phonenumber/check")]
        public async Task<IActionResult> CheckPhoneNumberForExistenceAsync(string phoneNumber)
        {
            bool isExist = await _administratorLogic.CheckPhoneNumberForExistenceAsync(phoneNumber);
            return Ok(isExist);
        }


        [ServiceFilter(typeof(CustomerExistenceFilterAttribute))]
        [HttpPost("/administrator-profile/customers/customer/{customerId:int}/phonenumber/set")]
        public async Task<IActionResult> TryToSetNewPhoneNumberAsync(int customerId, string phoneNumber) //change 'try' to 'set'
        {
            bool status;
            try
            {
                status = await _administratorLogic.TryToSetNewPhoneNumberAsync(customerId, phoneNumber); /*!*/
            }
            catch (CustomerDoesntExistException e)
            {
                return NotFound(e.Message);
            }
            catch (InvalidOperationException e)
            {
                return Conflict(e.Message);
            }
            return Ok(status);
        }


        [ServiceFilter(typeof(CustomerExistenceFilterAttribute))]
        [HttpPost("/administrator-profile/customers/customer/{customerId:int}/block")]
        public async Task<IActionResult> BlockCustomerAsync(int customerId)
        {
            try
            {
                await _administratorLogic.BlockCustomerAsync(customerId);
            }
            catch (CustomerDoesntExistException e)
            {
                return NotFound(e.Message);
            }
            catch (InvalidOperationException e)
            {
                return Conflict(e.Message);
            }
            return NoContent();
        }


        [ServiceFilter(typeof(DtoValidationFilterAttribute))]
        [HttpPost("/administrator-profile/customers/create")]
        public async Task<IActionResult> CreateCustomerAsync([FromBody] CustomerForCreateInAdministratorDto customer)
        {
            int customerId;
            CustomerForReadInAdministratorDto customerDto;
            try
            {
                (customerId, customerDto) = await _administratorLogic.CreateCustomerAsync(customer);
            }
            catch (InvalidOperationException e)
            {
                return Conflict(e.Message);
            }
            return CreatedAtRoute("GetCustomer", new { customerId = customerId }, customerDto);
        }


        [ServiceFilter(typeof(DtoValidationFilterAttribute))]
        [ServiceFilter(typeof(CustomerExistenceFilterAttribute))]
        [HttpPut("/administrator-profile/customers/customer/{customerId:int}/update")]
        public IActionResult UpdateCustomer(int customerId, [FromBody] CustomerForUpdateInAdministratorDto customer)
        {
            try
            {
                _administratorLogic.UpdateCustomerAsync(customerId, customer);
            }
            catch (CustomerDoesntExistException e)
            {
                return NotFound(e.Message);
            }
            catch (InvalidOperationException e)
            {
                return Conflict(e.Message);
            }
            return NoContent();
        }


        [ServiceFilter(typeof(DtoValidationFilterAttribute))]
        [ServiceFilter(typeof(CustomerExistenceFilterAttribute))]
        [HttpPost("/administrator-profile/customers/customer/{customerId:int}/delete")]
        public async Task<IActionResult> DeleteCustomerAsync(int customerId, [FromBody] CustomerForUpdateInAdministratorDto customer)
        {
            try 
            {
                await _administratorLogic.DeleteCustomerAsync(customerId);
            }
            catch (CustomerDoesntExistException e)
            {
                return NotFound(e.Message);
            }
            catch (InvalidOperationException e)
            {
                return Conflict(e.Message);
            }
            return NoContent();
        }


        [ServiceFilter(typeof(ParametersValidationFilterAttribute))]
        [ServiceFilter(typeof(CustomerExistenceFilterAttribute))]
        [HttpGet("/administrator-profile/customers/customer/{customerId:int}/messages")]
        public async Task<IActionResult> GetAdministratorMessagesByCustomerIdAsync(int customerId, [FromQuery] AdministratorMessageParameters parameters)
        {
            IEnumerable<AdministratorMessageForReadInAdministratorDto> messages;
            try
            {
                messages = await _administratorLogic.GetAdministratorMessagesByCustomerIdAsync(customerId, parameters);
            }
            catch (CustomerDoesntExistException e)
            {
                return NotFound(e.Message);
            }
            return Ok(messages);
        }


        [ServiceFilter(typeof(CustomerExistenceFilterAttribute))]
        [HttpGet("/administrator-profile/customers/customer/{customerId:int}/time-pasts-from-last-warn-message")]/*!*/
        public async Task<IActionResult> TimePastsFromLastWarnMessageAsync(int customerId)
        {
            DateTime time;
            try
            {
                time = await _administratorLogic.TimePastsFromLastWarnMessageAsync(customerId);
            }
            catch (CustomerDoesntExistException e)
            {
                return NotFound(e.Message);
            }
            return Ok(time);
        }
    }
}