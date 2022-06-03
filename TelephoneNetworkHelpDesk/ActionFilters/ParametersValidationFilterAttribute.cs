using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logger;

namespace TelephoneNetworkProvider.ActionFilters
{
    public class ParametersValidationFilterAttribute : IActionFilter /*!*/
    {
        private ILoggerManager _logger;

        public ParametersValidationFilterAttribute(ILoggerManager logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var action = context.RouteData.Values["action"];
            var controller = context.RouteData.Values["controller"];
            if (!context.ModelState.IsValid)
            {
                var errorsValue = context.ModelState.ToString();
                _logger.LogError($"Invalid parameters. Controller: {controller}, action: {action}, errors: {errorsValue}");
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }
}
