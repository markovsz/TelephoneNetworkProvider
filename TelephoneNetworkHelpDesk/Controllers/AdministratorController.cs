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
    [Route("/administrator")]
    public class AdministratorController : Controller
    {
        private IAdministratorLogic _administratorLogic;

        public AdministratorController(IAdministratorLogic administratorLogic)
        {
            _administratorLogic = administratorLogic;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("~/customers")]
        [HttpGet]
        public IActionResult GetCustomers([FromQuery] CustomerParameters parameters)
        {
            var customers = _administratorLogic.GetCustomers(parameters);
            return View(customers);
        }

        [ServiceFilter(typeof(CustomerExistenceFilterAttribute))]
        [Route("~/customers/{id}")]
        [HttpGet]
        public IActionResult GetCustomerInfo([FromQuery] string userId)
        {
            var customer = _administratorLogic.GetCustomerInfo(userId);
            return Ok(customer);
        }

        [ServiceFilter(typeof(CustomerExistenceFilterAttribute))]
        [Route("~/customers/{id}/calls")]
        [HttpGet]
        public IActionResult GetCustomerCalls([FromQuery] string userId, [FromQuery] CallParameters parameters)
        {
            var calls = _administratorLogic.GetCustomerCalls(userId, parameters);
            return Ok(calls);
        }

        [Route("~/calls")]
        [HttpGet]
        public IActionResult GetCalls([FromQuery] CallParameters parameters)
        {
            var calls = _administratorLogic.GetCalls(parameters);
            return Ok(calls);
        }

        [ServiceFilter(typeof(CallExistenceFilterAttribute))]
        [Route("~/calls/{id}")]
        [HttpGet]
        public IActionResult GetCallInfo([FromQuery] uint id)
        {
            var call = _administratorLogic.GetCallInfo(id);
            return Ok(call);
        }

        [Route("~/customers/...?")]/**/
        [HttpPost]
        public IActionResult CheckPhoneNumberForExistence(string phoneNumber)
        {
            bool isExist = _administratorLogic.CheckPhoneNumberForExistence(phoneNumber);
            return Ok(isExist);
        }

        [Route("~/customers/{userId}/phonenumber")]
        [HttpPost]
        public IActionResult TryToSetNewPhoneNumber(string userId, string phoneNumber)
        {
            bool status = _administratorLogic.TryToSetNewPhoneNumber(userId, phoneNumber);
            return Ok(status);
        }

        [ServiceFilter(typeof(CustomerExistenceFilterAttribute))]
        [Route("~/customer/block")]
        [HttpPost]
        public IActionResult BlockCustomer(string userId)
        {
            _administratorLogic.BlockCustomer(userId);
            return NoContent();
        }

        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Route("~/customers")]
        [HttpPost]
        public IActionResult CreateCustomer([FromBody] CustomerForCreateInAdministratorDto customer)
        {
            try
            {
                _administratorLogic.CreateCustomer(customer);
            }
            catch (UserExistException e)
            {
                return BadRequest(e.Message);/*NOOOOOOOOO*/
            }

            return NoContent();
        }

        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(CustomerExistenceFilterAttribute))]
        [Route("~/customers/{id}")]
        [HttpPut]
        public IActionResult UpdateCustomer(string userId, [FromBody] CustomerForUpdateInAdministratorDto customer)
        {
            _administratorLogic.UpdateCustomer(userId, customer);
            return View();
        }


    }
}
