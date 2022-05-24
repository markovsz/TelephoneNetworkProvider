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
using BussinessLogic.Exceptions;
using Entities.DataTransferObjects;

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
        [HttpGet("/guest-profile/customers/customer/{customerId}")]
        public async Task<IActionResult> GetCustomerInfoAsync(int customerId)
        {

            CustomerForReadInGuestDto customer;
            try
            {
                customer = await _guestLogic.GetCustomerInfoAsync(customerId);
            }
            catch (CustomerDoesntExistException ex)
            {
                return NotFound(ex.Message);
            }
            return Ok(customer);
        }

        [ServiceFilter(typeof(ParametersValidationFilterAttribute))]
        [HttpGet("/guest-profile/customers")]
        public async Task<IActionResult> GetCustomersAsync([FromQuery] CustomerParameters parameters)
        {
            IEnumerable<CustomerForReadInGuestDto> customers;
            try
            {
                customers = await _guestLogic.GetCustomersAsync(parameters);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(customers);
        }
    }
}
