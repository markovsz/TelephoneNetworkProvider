using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Entities.DataTransferObjects;
using BussinessLogic;

namespace TelephoneNetworkProvider.Controllers
{
    [Authorize(Roles = "Customer")]
    [Route("/customer")]
    public class CustomerController : Controller
    {
        private ICustomerLogic _customerLogic;

        public CustomerController(ICustomerLogic customerLogic)
        {
            _customerLogic = customerLogic;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("~/profile")]
        [HttpGet]
        public IActionResult GetCustomerInfo()
        {

            return View();
        }

        [Route("~/profile")]
        [HttpPut]
        public IActionResult UpdateCustomerInfo([FromBody] CustomerForUpdateInCustomerDto customerDto)
        {
            _customerLogic.UpdateCustomerInfo(customerDto);
            return Ok();
        }

        [Route("~/profile")]
        [HttpPost]
        public IActionResult ReplenishTheBalance([FromBody] Decimal currency)
        {
            _customerLogic.ReplenishTheBalance(currency);
            return Ok();
        }
    }
}
