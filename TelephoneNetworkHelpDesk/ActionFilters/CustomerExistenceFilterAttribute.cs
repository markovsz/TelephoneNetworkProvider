using BussinessLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace TelephoneNetworkProvider.ActionFilters
{
    public class CustomerExistenceFilterAttribute : IAsyncActionFilter
    {
        public CustomerExistenceFilterAttribute(IAdministratorLogic administratorLogic)
        {
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var action = context.RouteData.Values["action"];
            var controller = context.RouteData.Values["controller"];
            var customerId = (int)context.ActionArguments["customerId"];
            if (customerId < 1)
                context.Result = new BadRequestObjectResult("In {controller}.{action}, invalid customer id");
            await next();
        }
    }
}
