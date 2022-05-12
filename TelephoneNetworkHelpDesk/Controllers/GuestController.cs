using BussinessLogic;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelephoneNetworkProvider.ActionFilters;
using Microsoft.AspNetCore.Authorization;

namespace TelephoneNetworkProvider.Controllers
{
    [Route("/guest-profile")]
    [AllowAnonymous]
    public class GuestController : Controller
    {
        private IGuestLogic _guestLogic;

        public GuestController(IGuestLogic guestLogic)
        {
            _guestLogic = guestLogic;
        }

        [ServiceFilter(typeof(CustomerExistenceFilterAttribute))]
        [HttpGet("/guest-profile/customers/{customerId}")]
        public IActionResult GetCustomerInfo(int customerId)
        {
            var customer = _guestLogic.GetCustomerInfo(customerId);
            return Ok(customer);
        }

        [ServiceFilter(typeof(ParametersValidationFilterAttribute))]
        [HttpGet("/guest-profile/customers")]
        public IActionResult GetCustomers([FromQuery] CustomerParameters parameters)
        {
            var customers = _guestLogic.GetCustomers(parameters);
            return Ok(customers);
        }
    }
}
