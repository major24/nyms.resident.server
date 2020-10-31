using NLog;
using nyms.resident.server.Services.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace nyms.resident.server.Filters
{
    public class CustomExceptionHandler: ExceptionFilterAttribute
    {
        private static Logger logger = Nlogger2.GetLogger();
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            string errorMessage = string.Empty;
            if (actionExecutedContext.Exception.InnerException == null)
            {
                errorMessage = actionExecutedContext.Exception.Message;
            }
            else
            {
                errorMessage = actionExecutedContext.Exception.InnerException.Message;
            }
            logger.Error($"Error: {errorMessage}");
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("An unhandled exception was thrown by service"),
                ReasonPhrase = "Internal Server Error. Please contact admin."
            };
            actionExecutedContext.Response = response;
        }
    }
}