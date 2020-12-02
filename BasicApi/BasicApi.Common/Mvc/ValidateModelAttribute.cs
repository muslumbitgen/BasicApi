using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using BasicApi.Items.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace BasicApi.Common.Mvc
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public ValidateModelAttribute()
        {
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errorsInModelState = context.ModelState
                                                .Where(x => x.Value.Errors.Count > 0)
                                                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(x => x.ErrorMessage))
                                                .ToArray();

                ErrorResponse errorResponse = new ErrorResponse
                {
                    ErrorCode = ExceptionCodes.ModelStateError,
                    Errors = new List<ModelStateErrorDto>()
                };

                foreach (var error in errorsInModelState)
                {
                    foreach (var subError in error.Value)
                    {
                        var errorModel = new ModelStateErrorDto
                        {
                            FieldName = error.Key,
                            Message = subError
                        };

                        errorResponse.Errors.Add(errorModel);
                    }
                }

                context.Result = new BadRequestObjectResult(errorResponse);

                return;
            }
        }
    }
}
