using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Entities.DataTransferObjects;
using BussinessLogic;
using System.Security.Claims;
using Entities.RequestFeatures;
using TelephoneNetworkProvider.ActionFilters;

namespace TelephoneNetworkProvider.Controllers
{
    [Authorize(Roles = "Customer")]
    [Route("/customer-profile")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private ICustomerLogic _customerLogic;
        private readonly int customerId;

        public CustomerController(ICustomerLogic customerLogic)
        {
            _customerLogic = customerLogic;

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var claim = identity.Claims
                .Where(c => c.Type.Equals(ClaimTypes.Name))
                .FirstOrDefault();
            customerId = Int32.Parse(claim.Value);
        }

        [HttpGet("/customer-profile/info")]
        public IActionResult GetInfo()
        {           
            var customer = _customerLogic.GetCustomerInfo(customerId);
            return Ok(customer);
        }

        [ServiceFilter(typeof(DtoValidationFilterAttribute))]
        [HttpPut("/customer-profile/update")]
        public IActionResult UpdateCustomerInfo([FromBody] CustomerForUpdateInCustomerDto customerDto)
        {
            _customerLogic.UpdateCustomerInfo(customerId, customerDto);
            return NoContent();
        }

        [HttpPost("/customer-profile/replenish-balance")]
        public IActionResult ReplenishTheBalance([FromBody] Decimal currency)
        {
            _customerLogic.ReplenishTheBalance(customerId, currency);
            return NoContent();
        }

        [ServiceFilter(typeof(CustomerExistenceFilterAttribute))]
        [HttpGet("/customer-profile/customers/{customerId}")]
        public IActionResult GetCustomerInfo(int customerId)
        {
            var customer = _customerLogic.GetCustomerInfo(customerId);
            return Ok(customer);
        }

        [ServiceFilter(typeof(ParametersValidationFilterAttribute))]
        [HttpGet("/customer-profile/customers")]
        public IActionResult GetCustomers([FromQuery] CustomerParameters parameters)
        {
            var customers = _customerLogic.GetCustomers(parameters);
            return Ok(customers);
        }
    }
}
