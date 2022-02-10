using FileUpload.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace FileUpload.Api.Filters
{
    public class ValidationFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                List<string> errors = new();

                IEnumerable<ModelError> modelErrors = context.ModelState.Values.SelectMany(i => i.Errors);

                foreach (var item in modelErrors)
                {
                    errors.Add(item.ErrorMessage);
                }

                context.Result = new BadRequestObjectResult(Response<NoContent>.Fail(errors, 400));
            }

        }
    }

}
