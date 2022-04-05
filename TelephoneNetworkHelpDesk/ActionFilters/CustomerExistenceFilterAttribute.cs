using BussinessLogic;
using Entities.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TelephoneNetworkProvider.ActionFilters
{
    public class CustomerExistenceFilterAttribute : IActionFilter
    {
        private IAdministratorLogic _administratorLogic;

        public CustomerExistenceFilterAttribute(IAdministratorLogic administratorLogic)
        {
            _administratorLogic = administratorLogic;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var action = context.RouteData.Values["action"];
            var controller = context.RouteData.Values["controller"];
            var customerId = (uint)context.ActionArguments["id"];
            bool isExist = _administratorLogic.CheckCustomer(customerId);
            if (isExist)
                context.Result = new BadRequestObjectResult($"In {controller}.{action}, customer with id = {customerId} not found");
        }
    }
}
