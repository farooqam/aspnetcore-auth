using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using TokenApi.Common;

namespace TokenApi.Filters
{
    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
            {
                return;
            }
            
            var errors = context.ModelState.ToDictionary(kvp => kvp.Key,
                    kvp => kvp.Value.Errors
                        .Select(e => e.ErrorMessage).ToList())
                .Where(m => m.Value.Any());

            var apiErrors = errors
                .Select(keyValuePair => keyValuePair.Value.FirstOrDefault())
                .Select(errorMessage =>
                    ApiErrors.ErrorList.SingleOrDefault(e => string.Compare(e.Message, errorMessage, StringComparison.InvariantCultureIgnoreCase) == 0))
                .ToList();

            context.Result = new BadRequestObjectResult(new ApiErrors
            {
                Operation = ((ControllerActionDescriptor)context.ActionDescriptor).ActionName,
                Errors = apiErrors
            });
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}