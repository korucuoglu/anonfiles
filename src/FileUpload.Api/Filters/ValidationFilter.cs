using FileUpload.Data.Entity;
using FileUpload.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUpload.Api.Filters
{
    public class ValidationFilterAttribute<TEntity>: IAsyncActionFilter where TEntity : class
    { 
        private readonly ILogger<TEntity> _logger;

        public ValidationFilterAttribute(ILogger<TEntity> logger)
        {
            _logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                _logger.LogError("Model bilgisi yanlış olarak dolduruldu.");

                List<string> errors = new();

                IEnumerable<ModelError> modelErrors = context.ModelState.Values.SelectMany(i => i.Errors);

                foreach (var item in modelErrors)
                {
                    errors.Add(item.ErrorMessage);
                }

                context.Result = new BadRequestObjectResult(Response<NoContent>.Fail(errors, 400));

                return;

            }
            await next();
        }
    }

}
