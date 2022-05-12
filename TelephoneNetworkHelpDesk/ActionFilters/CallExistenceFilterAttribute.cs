﻿using Microsoft.AspNetCore.Mvc.Filters;
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
        private IAdministratorLogic _administratorLogic;

        public CallExistenceFilterAttribute(IAdministratorLogic administratorLogic)
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
            var callId = (int)context.ActionArguments["id"];
            if (callId < 1)
                context.Result = new BadRequestObjectResult("In {controller}.{action}, invalid call id");
            bool isExist = _administratorLogic.CheckCall(callId);
            if (isExist)
                context.Result = new BadRequestObjectResult($"In {controller}.{action}, call with id = {callId} not found");
        }
    }
}
