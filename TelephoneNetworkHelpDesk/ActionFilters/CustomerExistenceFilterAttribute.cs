using BussinessLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

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
            var customerId = (int)context.ActionArguments["customerId"];
            if (customerId < 1)
                context.Result = new BadRequestObjectResult("In {controller}.{action}, invalid customer id");
            //int pos = 0;
            //var pathParts = context.HttpContext.Request.Path.Value.Split('/');
            //for(int i = 0; i < pathParts.Length; ++i)
            //{
            //    if (pathParts[i].Equals("customer"))
            //    {
            //        pos = i + 1;
            //    }
            //}
            //int customerId;
            //bool isValid = Int32.TryParse(pathParts[pos], out customerId);
            //if (!isValid)
            //    context.Result = new BadRequestObjectResult($"In {controller}.{action}, customer id was wrong");
            bool isExist = _administratorLogic.CheckCustomer(customerId);
            if (!isExist)
                context.Result = new BadRequestObjectResult($"In {controller}.{action}, customer with id = {customerId} not found");
        }
    }
}
