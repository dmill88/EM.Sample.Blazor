using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace EM.Sample.WebApi.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)      //, ILogger logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        ErrorDetail errorDetail = new ErrorDetail(context.Response.StatusCode, contextFeature.Error);
                        string errorJson = System.Text.Json.JsonSerializer.Serialize<ErrorDetail>(errorDetail);
                        await context.Response.WriteAsync(errorJson);
                    }
                });
            });
        }

        public class ErrorDetail
        {
            public ErrorDetail(int statusCode, Exception exp)
            {
                StatusCode = statusCode;
                Message = exp.Message;
                if (exp.InnerException != null)
                {
                    InnerError = new ErrorDetail(statusCode, exp.InnerException);
                }
            }

            public ErrorDetail()
            {
                StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                Message = string.Empty;
            }

            public int StatusCode { get; set; }
            public string Message { get; set; }
            public ErrorDetail InnerError { get; set; }
        }

    }
}
