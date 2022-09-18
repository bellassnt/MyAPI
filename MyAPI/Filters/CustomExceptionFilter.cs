using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyAPI.Filters
{
    public class CustomExceptionFilter : IExceptionFilter // Não herda de Attribute porque só é implementada globalmente (não é implementada em endpoints)
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is NullReferenceException)
            {
                context.Result = new ObjectResult(new
                {
                    message = "We could not find this song! )="
                })
                {
                    StatusCode = StatusCodes.Status404NotFound
                };
                
                context.ExceptionHandled = true;
            }
        }
    }
}
