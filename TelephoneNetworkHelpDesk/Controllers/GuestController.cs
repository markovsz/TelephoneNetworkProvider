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
        public async Task<IActionResult> GetCustomerInfoAsync(int customerId)
        {
            var customer = await _guestLogic.GetCustomerInfoAsync(customerId);
            return Ok(customer);
        }

        [ServiceFilter(typeof(ParametersValidationFilterAttribute))]
        [HttpGet("/guest-profile/customers")]
        public async Task<IActionResult> GetCustomersAsync([FromQuery] CustomerParameters parameters)
        {
            var customers = await _guestLogic.GetCustomersAsync(parameters);
            return Ok(customers);
        }
    }
}
