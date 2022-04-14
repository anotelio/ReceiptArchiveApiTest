using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ReceiptArchiveApi.Contracts.Responses.Base;
using ReceiptArchiveApi.Contracts.Defaults;

namespace ReceiptArchiveApi.Core.ApiFilters
{
    public class ValidateModelFilterAttribute : ActionFilterAttribute
    {
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values
                    .Where(v => v.Errors.Count > 0)
                    .SelectMany(v => v.Errors)
                    .Select(v => v.ErrorMessage)
                    .AsQueryable();

                BaseResponse responseObj = new()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorData = new List<ErrorData>()
                    {
                        new ErrorData()
                        {
                            ErrorCode = LogAndExceptionMessagesDefaults.ValidationErrorCode,
                            ErrorMessage = LogAndExceptionMessagesDefaults.ValidationError,
                            ErrorTrace = string.Join(" :: ", errors)
                        }
                    }
                };

                context.Result = new JsonResult(responseObj)
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }

            await next();
        }
    }
}
