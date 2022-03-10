using FileUpload.Shared.Wrappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUpload.Infrastructure.Attribute
{
    public class ValidationFilterAttribute: IAsyncActionFilter
    { 
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                List<string> errors = new();

                IEnumerable<ModelError> modelErrors = context.ModelState.Values.SelectMany(i => i.Errors);

                foreach (var item in modelErrors)
                {
                    errors.Add(item.ErrorMessage);
                }

                context.Result = new BadRequestObjectResult(Response<NoContent>.Fail(modelErrors.First().ErrorMessage, 400));

                return;

            }
            await next();
        }
    }

}
