using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TelephoneNetworkProvider.ActionFilters
{
    public class ParametersValidationFilterAttribute : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var action = context.RouteData.Values["action"];
            var controller = context.RouteData.Values["controller"];
            if (!context.ModelState.IsValid)
            {
                //_logger.LogError($"Invalid model state for the object. Controller: { controller}, action: { action}");
                context.Result = new UnprocessableEntityObjectResult(context.ModelState);
            }
        }
    }
}
