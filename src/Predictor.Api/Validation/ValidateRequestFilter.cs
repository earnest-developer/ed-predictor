using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Predictor.Http;
using Predictor.Logging;
using Serilog;

namespace Predictor.Api.Validation
{
    public class ValidateRequestFilter : IActionFilter
    {
        private const string RequestInvalidErrorType = "request_invalid";
        
        private readonly ILogger _logger;
        private readonly IValidatorFactory _validatorFactory;
        private readonly IDiagnosticContextAdaptor _diagnosticContext;

        public ValidateRequestFilter(ILogger logger, IValidatorFactory validatorFactory, IDiagnosticContextAdaptor diagnosticContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _validatorFactory = validatorFactory ?? throw new ArgumentNullException(nameof(validatorFactory));
            _diagnosticContext = diagnosticContext ?? throw new ArgumentNullException(nameof(diagnosticContext));
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            foreach (ControllerParameterDescriptor parameter in context.ActionDescriptor.Parameters)
            {
                TypeInfo typeInfo = parameter.ParameterType.GetTypeInfo();

                if (typeInfo.IsClass && context.ActionArguments.TryGetValue(parameter.Name, out object parameterValue))
                {
                    if (parameterValue == null && !parameter.ParameterInfo.IsOptional)
                    {
                        context.Result = new ObjectResult(
                            new ErrorResponse(
                                requestId: context.HttpContext.GetCorrelationId(), 
                                errorType: RequestInvalidErrorType, 
                                errorCode: "request_body_required"))
                        {
                            StatusCode = StatusCodes.Status422UnprocessableEntity
                        };

                        return;
                    }

                    if (parameterValue == null)
                        return;

                    ValidationResult validationResult = Validate(parameterValue);
                    if (validationResult != null && !validationResult.IsValid)
                    {
                        ErrorResponse errorResponse = CreateErrorResponse(context.HttpContext.GetCorrelationId(), validationResult);

                        // Push errors in the completion event
                        _diagnosticContext.Set(nameof(ErrorResponse.ErrorType), errorResponse.ErrorType);
                        _diagnosticContext.Set(nameof(ErrorResponse.ErrorCode), errorResponse.ErrorCode);

                        context.Result = new UnprocessableEntityResult(errorResponse);
                    }
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        private ValidationResult Validate(object parameterValue)
        {
            IValidator validator = _validatorFactory.GetValidator(parameterValue.GetType());

            if (validator == null)
                return null;

            using (_logger.TimeDebug("Validating type {RequestObjectType} using {ValidatorType}",
                parameterValue.GetType().GetDisplayName(), validator.GetType().GetDisplayName()))
            {
                return validator.Validate(parameterValue);
            }
        }

        private static ErrorResponse CreateErrorResponse(string requestId, ValidationResult validationResult)
        {
            IEnumerable<string> errorCodes = validationResult.Errors.Select(e => e.ErrorCode);
            return new ErrorResponse(requestId, RequestInvalidErrorType, string.Join(", ", errorCodes));
        }
    }
}