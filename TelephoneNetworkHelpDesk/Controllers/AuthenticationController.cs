using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Models;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Identity;

namespace TelephoneNetworkProvider.Controllers
{
    public class AuthenticationController : Controller
    {
        private UserManager<User> _userManager;

        public AuthenticationController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }


        [HttpGet]
        [Route("/registration")]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [Route("/registration")]
        public IActionResult Registration(RegistrationDto registrationDto)
        {
            return View();
        }

        [HttpGet]
        [Route("/login")]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [Route("/login")]
        public IActionResult LogIn(LoginDto loginDto)
        {

            return View();
        }
    }
}
