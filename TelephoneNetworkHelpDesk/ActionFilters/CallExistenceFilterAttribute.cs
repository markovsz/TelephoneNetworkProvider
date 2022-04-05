using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BussinessLogic;
using Microsoft.AspNetCore.Mvc;

namespace TelephoneNetworkProvider.ActionFilters
{
    public class CallExistenceFilterAttribute : IActionFilter
    {
        private AdministratorLogic _administratorLogic;

        public CallExistenceFilterAttribute(AdministratorLogic administratorLogic)
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
            bool isExist = _administratorLogic.CheckCall(customerId);
            if (isExist)
                context.Result = new BadRequestObjectResult($"In {controller}.{action}, call with id = {customerId} not found");
        }
    }
}
