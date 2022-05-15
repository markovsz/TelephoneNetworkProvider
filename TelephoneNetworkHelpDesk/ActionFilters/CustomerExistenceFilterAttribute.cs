using BussinessLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace TelephoneNetworkProvider.ActionFilters
{
    public class CustomerExistenceFilterAttribute : IAsyncActionFilter
    {
        private IAdministratorLogic _administratorLogic;

        public CustomerExistenceFilterAttribute(IAdministratorLogic administratorLogic)
        {
            _administratorLogic = administratorLogic;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var action = context.RouteData.Values["action"];
            var controller = context.RouteData.Values["controller"];
            var customerId = (int)context.ActionArguments["customerId"];
            if (customerId < 1)
                context.Result = new BadRequestObjectResult("In {controller}.{action}, invalid customer id");
            bool isExist = await _administratorLogic.CheckCustomerAsync(customerId);
            if (!isExist)
                context.Result = new BadRequestObjectResult($"In {controller}.{action}, customer with id = {customerId} not found");
            await next();
        }
    }
}
