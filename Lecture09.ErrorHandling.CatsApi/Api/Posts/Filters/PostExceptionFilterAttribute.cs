using Lecture09.ErrorHandling.CatsApi.Api.Posts.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Lecture09.ErrorHandling.CatsApi.Api.Posts.Filters
{
    public class PostExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if(context.Exception is PostUserNotFoundException)
            {
                context.Result = new BadRequestObjectResult(new { Error = context.Exception.Message });
                context.ExceptionHandled = true;
            }

            if (context.Exception is PostCatsNotFoundException)
            {
                context.Result = new BadRequestObjectResult(new { Error = context.Exception.Message });
                context.ExceptionHandled = true;
            }
        }
    }
}
