using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Models;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Repository;
using TelephoneNetworkProvider.ActionFilters;
using BussinessLogic;

namespace TelephoneNetworkProvider.Controllers
{
    [Route("/login")]
    public class AuthenticationController : Controller
    {
        private IAuthenticationLogic _authenticationLogic;

        public AuthenticationController(IAuthenticationLogic authenticationLogic)
        {
            _authenticationLogic = authenticationLogic;
        }

        [HttpGet]
        public IActionResult GetLogInPage()
        {
            return View();
        }

        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPost]
        public async Task<IActionResult> Authentication(UserForAuthenticationDto user)
        {
            bool status = await _authenticationLogic.ValidateUser(user);
            if (!status)
            {
                return BadRequest();//NO! (I don't remember)
            } 

            return View();
        }
    }
}
