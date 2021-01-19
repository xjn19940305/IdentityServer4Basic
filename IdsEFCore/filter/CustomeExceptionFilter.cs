using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdsEFCore.filter
{
    public class CustomeExceptionFilter : IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.ExceptionHandled == false)
            {
                string msg = context.Exception.Message;
                context.Result = new ContentResult
                {
                    Content = msg,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ContentType = "application/json"
                };
            }
            context.ExceptionHandled = true; //异常已处理了

            return Task.CompletedTask;
        }
    }
}