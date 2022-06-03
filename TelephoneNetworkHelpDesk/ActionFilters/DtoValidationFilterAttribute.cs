using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logger;

namespace TelephoneNetworkProvider.ActionFilters
{
    public class DtoValidationFilterAttribute : IActionFilter
    {
        private ILoggerManager _logger;

        public DtoValidationFilterAttribute(ILoggerManager logger)
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
            var param = context.ActionArguments
            .SingleOrDefault(x => x.Value.ToString().Contains("Dto")).Value;
            if (param == null)
            {
                //_logger.LogError($"Object sent from client is null. Controller: {controller}, action: { action}");
                var errorsValue = context.ModelState.ToString();
                _logger.LogError($"Dto object is null. Controller: {controller}, action: {action}");
                context.Result = new BadRequestObjectResult($"Object is null. Controller:{ controller }, action: { action}");
                return;
            }
            if (!context.ModelState.IsValid)
            {
                var errorsValue = context.ModelState.ToString();
                _logger.LogError($"Invalid model state for the object. Controller: { controller}, action: { action}, errors: {errorsValue}");
                context.Result = new UnprocessableEntityObjectResult(context.ModelState);
            }
        }
    }
}
