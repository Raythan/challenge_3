using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace APIService.Model
{
    public class HeaderFilter : ActionFilterAttribute, IOperationFilter
    {
        public Microsoft.Extensions.Primitives.StringValues login = "";
        public Microsoft.Extensions.Primitives.StringValues password = "";

        /// <summary>
        /// Operation filter to add the requirement of the custom header
        /// </summary>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Login",
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema {
                    Type = "string",
                    Default = new OpenApiString("")
                },
                Required = true // set to false if this is optional
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Password",
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Default = new OpenApiString("")
                },
                Required = true // set to false if this is optional
            });
        }
        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                context.HttpContext.Request.Headers.TryGetValue("login", out login);
                context.HttpContext.Request.Headers.TryGetValue("password", out password);
            }
            catch (Exception ex)
            {
                context.Result = new ContentResult { Content = $"Exceção não tratada: {ex.Message}", StatusCode = 401 };
                return;
            }
        }
    }
}



