using FileUpload.Shared.Wrappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;

namespace FileUpload.Upload.Infrastructure.Attribute
{
    public class ValidationFilterAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values.SelectMany(i => i.Errors).Select(x => x.ErrorMessage).ToList();

                context.Result = new ObjectResult(Response<NoContent>.Fail(errors)) { StatusCode = 500 };

                return;

            }
            await next();
        }
    }

}
