using FileUpload.Shared.Wrappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUpload.Upload.Infrastructure.Attribute
{
    public class ValidationFilterAttribute: ActionFilterAttribute
    { 
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {

                var errors = context.ModelState.Values.SelectMany(i => i.Errors).Select(x => x.ErrorMessage).ToList();

                context.Result = new NotFoundObjectResult(Response<NoContent>.Fail(errors, 400));

                return;

            }
            await next();
        }
    }

}
