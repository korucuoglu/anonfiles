using FileUpload.Upload.Application.Interfaces.Repositories;
using FileUpload.Upload.Application.Interfaces.Services;
using FileUpload.Shared.Wrappers;
using FileUpload.Upload.Domain.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;

namespace FileUpload.Upload.Filters
{
    public class NotFoundFilterAttribute<TEntity> : IAsyncActionFilter where TEntity : BaseIdentity
    {
        private readonly IReadRepository<TEntity> _service;
        private readonly ISharedIdentityService _sharedIdentityService;

        public NotFoundFilterAttribute(IReadRepository<TEntity> service, ISharedIdentityService sharedIdentityService)
        {
            _service = service;
            _sharedIdentityService = sharedIdentityService;
        }

        public bool GetData(string id)
        {
            return _service.Any(x => x.Id.ToString() == id && x.ApplicationUserId == _sharedIdentityService.GetUserId, tracking: false);
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string GetId()
            {
                string routeId = (string)context.RouteData.Values["id"];

                if (!string.IsNullOrEmpty(routeId))
                {
                    return routeId;
                }

                var obj = context.ActionArguments.First().Value;

                string modelId = obj.GetType().GetProperties().First(o => o.Name == "Id").GetValue(obj, null).ToString();

                if (!string.IsNullOrEmpty(modelId))
                {
                    return modelId;
                }

                return null;

            }

            var id = GetId();

            if (string.IsNullOrEmpty(id))
            {
                context.Result = new NotFoundObjectResult(Response<NoContent>.Fail("Id değeri boş olamaz", 404));
                return;
            }

            if (!GetData(id))
            {
                context.Result = new NotFoundObjectResult(Response<NoContent>.Fail("Böyle bir veri bulunamadı.", 404));
                return;
            }
            await next();
        }
    }

   
}
