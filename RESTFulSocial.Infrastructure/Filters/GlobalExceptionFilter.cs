using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RESTFulSocial.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace RESTFulSocial.Infrastructure.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            // validamos el tipo de excepcion
            if (context.Exception.GetType() == typeof(BusinessException))
            {
                // se captura la excepcion y la convetimo en un BusinessException
                var exception = (BusinessException)context.Exception;

                // enviamos una respuesta. se genera una clase anonimo
                var validation = new
                {
                    Status = 400,
                    Title = "Bad Request",
                    Detail = exception.Message
                };

                // se genera un json. enviamos errors
                var json = new
                {
                    errors = new[] { validation }
                };

                context.Result = new BadRequestObjectResult(json);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.ExceptionHandled = true;
                
            }
        }
    }
}
