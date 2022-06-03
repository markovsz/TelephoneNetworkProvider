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
using Logger;

namespace TelephoneNetworkProvider.Controllers
{
    [AllowAnonymous]
    [Route("/guest-profile")]
    [ApiController]
    public class GuestController : ControllerBase
    {
        private IGuestLogic _guestLogic;
        private ILoggerManager _logger;

        public GuestController(IGuestLogic guestLogic, ILoggerManager logger)
        {
            _guestLogic = guestLogic;
            _logger = logger;
        }


        [HttpGet("/guest-profile/customers/customer/{customerId:int}")]
        public async Task<IActionResult> GetCustomerInfoAsync(int customerId)
        {

            CustomerForReadInGuestDto customer;
            try
            {
                customer = await _guestLogic.GetCustomerInfoAsync(customerId);
            }
            catch (CustomerDoesntExistException ex)
            {
                _logger.LogError(ex.Message);
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
                customers = await _guestLogic.GetCustomersInfoAsync(parameters);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            return Ok(customers);
        }
    }
}
