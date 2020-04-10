using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Predictor.Http;

namespace Predictor.Api.Validation
{
    public class ValidateModelStateFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext == null)
                throw new ArgumentNullException(nameof(context));

            if (!context.ModelState.IsValid)
            {
                // This should only be invoked if the request is malformed, otherwise
                // the ValidateRequestFilter will take care of validation
                var errorResponse = new ErrorResponse(
                    context.HttpContext.GetCorrelationId(),
                    "request_invalid",
                    "request_body_malformed "
                );

                context.Result = new BadRequestObjectResult(errorResponse);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}