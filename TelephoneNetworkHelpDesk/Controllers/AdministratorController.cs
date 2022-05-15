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
            var customers = await _administratorLogic.GetCustomersAsync(parameters);
            return Ok(customers);
        }

        [ServiceFilter(typeof(CustomerExistenceFilterAttribute))]
        [Route("/administrator-profile/customers/customer/{customerId:int}")]
        [HttpGet]
        public async Task<IActionResult> GetCustomerInfoAsync(int customerId)
        {
            var customer = await _administratorLogic.GetCustomerInfoAsync(customerId);
            return Ok(customer);
        }


        [ServiceFilter(typeof(ParametersValidationFilterAttribute))]
        [ServiceFilter(typeof(CustomerExistenceFilterAttribute))]
        [Route("/administrator-profile/customers/customer/{customerId:int}/calls")]
        [HttpGet]
        public async Task<IActionResult> GetCustomerCallsAsync(int customerId, [FromQuery] CallParameters parameters)
        {
            var calls = await _administratorLogic.GetCustomerCallsAsync(customerId, parameters);
            return Ok(calls);
        }


        [ServiceFilter(typeof(ParametersValidationFilterAttribute))]
        [HttpGet("/administrator-profile/calls")]
        public async Task<IActionResult> GetCallsAsync([FromQuery] CallParameters parameters)
        {
            var calls = await _administratorLogic.GetCallsAsync(parameters);
            return Ok(calls);
        }


        [ServiceFilter(typeof(CallExistenceFilterAttribute))]
        [Route("/administrator-profile/calls/call/{id:int}")]
        [HttpGet]
        public async Task<IActionResult> GetCallInfoAsync(int id)
        {
            var call = await _administratorLogic.GetCallInfoAsync(id);
            return Ok(call);
        }


        [HttpPost("/administrator-profile/customers/phonenumber/check")]/**/
        public async Task<IActionResult> CheckPhoneNumberForExistenceAsync(string phoneNumber)
        {
            bool isExist = await _administratorLogic.CheckPhoneNumberForExistenceAsync(phoneNumber);
            return Ok(isExist);
        }


        [ServiceFilter(typeof(CustomerExistenceFilterAttribute))]
        [Route("/administrator-profile/customers/customer/{customerId:int}/phonenumber/set")]
        [HttpPost]
        public async Task<IActionResult> TryToSetNewPhoneNumberAsync(int customerId, string phoneNumber)
        {
            bool status = await _administratorLogic.TryToSetNewPhoneNumberAsync(customerId, phoneNumber);
            return Ok(status);
        }


        [ServiceFilter(typeof(CustomerExistenceFilterAttribute))]
        [Route("/administrator-profile/customers/customer/{customerId:int}/block")]
        [HttpPost]
        public async Task<IActionResult> BlockCustomerAsync(int customerId)
        {
            await _administratorLogic.BlockCustomerAsync(customerId);
            return NoContent();
        }


        [ServiceFilter(typeof(DtoValidationFilterAttribute))]
        [Route("/administrator-profile/customers/create")]
        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync([FromBody] CustomerForCreateInAdministratorDto customer)
        {
            await _administratorLogic.CreateCustomerAsync(customer);
            return NoContent();
        }


        [ServiceFilter(typeof(DtoValidationFilterAttribute))]
        [ServiceFilter(typeof(CustomerExistenceFilterAttribute))]
        [Route("/administrator-profile/customers/customer/{customerId:int}/update")]
        [HttpPut]
        public IActionResult UpdateCustomer(int customerId, [FromBody] CustomerForUpdateInAdministratorDto customer)
        {
            _administratorLogic.UpdateCustomer(customerId, customer);
            return Ok();
        }


        [ServiceFilter(typeof(DtoValidationFilterAttribute))]
        [ServiceFilter(typeof(CustomerExistenceFilterAttribute))]
        [Route("/administrator-profile/customers/customer/{customerId:int}/delete")]
        [HttpPost]
        public async Task<IActionResult> DeleteCustomerAsync(int customerId, [FromBody] CustomerForUpdateInAdministratorDto customer)
        {
            await _administratorLogic.DeleteCustomerAsync(customerId);
            return Ok();
        }


        [ServiceFilter(typeof(ParametersValidationFilterAttribute))]
        [ServiceFilter(typeof(CustomerExistenceFilterAttribute))]
        [Route("/administrator-profile/customers/customer/{customerId:int}/messages")]
        [HttpGet]
        public async Task<IActionResult> GetAdministratorMessagesByCustomerIdAsync(int customerId, [FromQuery] AdministratorMessageParameters parameters)
        {
            var messages = await _administratorLogic.GetAdministratorMessagesByCustomerIdAsync(customerId, parameters);
            return Ok(messages);
        }


        [ServiceFilter(typeof(CustomerExistenceFilterAttribute))]
        [Route("/administrator-profile/customers/customer/{customerId:int}/time-pasts-from-last-warn-message")]/*!*/
        [HttpGet]
        public async Task<IActionResult> TimePastsFromLastWarnMessageAsync(int customerId)
        {
            DateTime time = await _administratorLogic.TimePastsFromLastWarnMessageAsync(customerId);
            return Ok(time);
        }
    }
}