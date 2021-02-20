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
            string errorMessage = actionExecutedContext.Exception.Message;

            if (actionExecutedContext.Exception.InnerException != null)
            {
                errorMessage += actionExecutedContext.Exception.InnerException.Message;
            }
            // take first 100 chars from stacktrace
            errorMessage += "::" + actionExecutedContext.Exception.StackTrace.ToString().Substring(0, 1000);
            logger.Error($"Error: {errorMessage}");

            var response = new HttpResponseMessage();
            if (errorMessage.ToUpper().Contains("UNIQUE"))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.ReasonPhrase = "Data already exists in the database. Please verify and submit";
            }
            else if (errorMessage.ToUpper().Contains("CANNOT INSERT"))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.ReasonPhrase = "Cannot insert values to table. Please verify and submit";
            }
            else if (errorMessage.ToUpper().Contains("FOREIGN"))
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.ReasonPhrase = "Database foreign key error. Please contact admin";
            }
            else
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.ReasonPhrase = "Internal Server Error. Please contact admin.";
            }

            response.Content = new StringContent("An exception was thrown by service");
            actionExecutedContext.Response = response;
        }
    }
}