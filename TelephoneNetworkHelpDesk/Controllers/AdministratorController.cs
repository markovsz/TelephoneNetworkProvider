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

        //[ServiceFilter(typeof(ParametersValidationFilterAttribute))]
        [HttpGet("/administrator-profile/customers")]
        public IActionResult GetCustomers([FromQuery] CustomerParameters parameters)
        {
            var customers = _administratorLogic.GetCustomers(parameters);
            return Ok(customers);
        }

        [ServiceFilter(typeof(CustomerExistenceFilterAttribute))]
        [Route("/administrator-profile/customers/customer/{customerId:int}")]
        [HttpGet]
        public IActionResult GetCustomerInfo(int customerId)
        {
            var customer = _administratorLogic.GetCustomerInfo(customerId);
            return Ok(customer);
        }


        [ServiceFilter(typeof(ParametersValidationFilterAttribute))]
        [ServiceFilter(typeof(CustomerExistenceFilterAttribute))]
        [Route("/administrator-profile/customers/customer/{customerId:int}/calls")]
        [HttpGet]
        public IActionResult GetCustomerCalls(int customerId, [FromQuery] CallParameters parameters)
        {
            var calls = _administratorLogic.GetCustomerCalls(customerId, parameters);
            return Ok(calls);
        }


        [ServiceFilter(typeof(ParametersValidationFilterAttribute))]
        [HttpGet("/administrator-profile/calls")]
        public IActionResult GetCalls([FromQuery] CallParameters parameters)
        {
            var calls = _administratorLogic.GetCalls(parameters);
            return Ok(calls);
        }


        [ServiceFilter(typeof(CallExistenceFilterAttribute))]/**/
        [Route("/administrator-profile/calls/call/{id:int}")]
        [HttpGet]
        public IActionResult GetCallInfo(int id)
        {
            var call = _administratorLogic.GetCallInfo(id);
            return Ok(call);
        }


        [HttpPost("/administrator-profile/customers/phonenumber/check")]/**/
        public IActionResult CheckPhoneNumberForExistence(string phoneNumber)
        {
            bool isExist = _administratorLogic.CheckPhoneNumberForExistence(phoneNumber);
            return Ok(isExist);
        }


        [ServiceFilter(typeof(CustomerExistenceFilterAttribute))]
        [Route("/administrator-profile/customers/customer/{customerId:int}/phonenumber/set")]
        [HttpPost]
        public IActionResult TryToSetNewPhoneNumber(int customerId, string phoneNumber)
        {
            bool status = _administratorLogic.TryToSetNewPhoneNumber(customerId, phoneNumber);
            return Ok(status);
        }


        [ServiceFilter(typeof(CustomerExistenceFilterAttribute))]
        [Route("/administrator-profile/customers/customer/{customerId:int}/block")]
        [HttpPost]
        public IActionResult BlockCustomer(int customerId)
        {
            _administratorLogic.BlockCustomer(customerId);
            return NoContent();
        }


        [ServiceFilter(typeof(DtoValidationFilterAttribute))]
        [Route("/administrator-profile/customers/create")]
        [HttpPost]
        public IActionResult CreateCustomer([FromBody] CustomerForCreateInAdministratorDto customer)
        {
            try
            {
                _administratorLogic.CreateCustomer(customer);
            }
            catch (UserExistException e)
            {
                return BadRequest(e.Message);
            }

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
        public IActionResult DeleteCustomer(int customerId, [FromBody] CustomerForUpdateInAdministratorDto customer)
        {
            _administratorLogic.DeleteCustomer(customerId);
            return Ok();
        }


        [ServiceFilter(typeof(ParametersValidationFilterAttribute))]
        [ServiceFilter(typeof(CustomerExistenceFilterAttribute))]
        [Route("/administrator-profile/customers/customer/{customerId:int}/messages")]
        [HttpGet]
        public IActionResult GetAdministratorMessagesByCustomerUserId(int customerId, [FromQuery] AdministratorMessageParameters parameters)
        {
            var messages = _administratorLogic.GetAdministratorMessagesByCustomerId(customerId, parameters);
            return Ok(messages);
        }


        [ServiceFilter(typeof(CustomerExistenceFilterAttribute))]
        [Route("/administrator-profile/customers/customer/{customerId:int}/time-pasts-from-last-warn-message")]/*!*/
        [HttpGet]
        public IActionResult TimePastsFromLastWarnMessage(int customerId)
        {
            DateTime time = _administratorLogic.TimePastsFromLastWarnMessage(customerId);
            return Ok(time);
        }
    }
}