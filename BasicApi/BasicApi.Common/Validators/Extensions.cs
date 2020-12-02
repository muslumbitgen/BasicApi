using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using BasicApi.Items.Exceptions;
using System;
using System.Linq;

namespace BasicApi.Common.Validators
{
    public static class Extensions
    {
        public static IMvcBuilder AddValidators(this IMvcBuilder builder, Type validatorsAssemblyType)
        {
            builder.AddFluentValidation(x =>
            {
                x.RegisterValidatorsFromAssembly(validatorsAssemblyType.Assembly);
                x.LocalizationEnabled = true;
                x.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
            });

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {

                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var firstMessage = context.ModelState.Values.Where(v => v.Errors.Count > 0)
                                                              .SelectMany(v => v.Errors)
                                                              .Select(v => new
                                                              {
                                                                  v.ErrorMessage,
                                                                  Message = v.Exception == null ? "" : v.Exception.Message
                                                              })
                                                              .FirstOrDefault();

                    return new BadRequestObjectResult(new
                    {
                        ErrorCode = ExceptionCodes.ModelStateError,
                        ErrorDescription = FormatMessage(firstMessage.ErrorMessage, firstMessage.Message)
                    });
                };
            });

            return builder;
        }

        private static string FormatMessage(string errorMessage, string exceptionMessage)
        {
            if (string.IsNullOrEmpty(errorMessage) == false)
            {
                return $"{errorMessage}";
            }
            else
            {
                return $"{exceptionMessage}";
            }
        }

    }
}
