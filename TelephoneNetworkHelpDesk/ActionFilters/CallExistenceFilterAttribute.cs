﻿using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BussinessLogic;
using Microsoft.AspNetCore.Mvc;

namespace TelephoneNetworkProvider.ActionFilters
{
    public class CallExistenceFilterAttribute : IAsyncActionFilter
    {
        public CallExistenceFilterAttribute(IAdministratorLogic administratorLogic)
        {
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var action = context.RouteData.Values["action"];
            var controller = context.RouteData.Values["controller"];
            var callId = (int)context.ActionArguments["id"];
            if (callId < 1)
                context.Result = new BadRequestObjectResult("In {controller}.{action}, invalid call id");
            await next();
        }
    }
}
