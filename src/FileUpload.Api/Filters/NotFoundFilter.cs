using FileUpload.Data.Entity;
using FileUpload.Data.Entity.Base;
using FileUpload.Data.Repository;
using FileUpload.Shared.Models;
using FileUpload.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace FileUpload.Api.Filters
{
    public class NotFoundFilter<TEntity> : IAsyncActionFilter where TEntity : BaseIdentity
    {
        private readonly IRepository<TEntity> _service;
        private readonly ISharedIdentityService _sharedIdentityService;

        public NotFoundFilter(IRepository<TEntity> service, ISharedIdentityService sharedIdentityService)
        {
            _service = service;
            _sharedIdentityService = sharedIdentityService;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var id = (string)context.RouteData.Values["id"];

            if (string.IsNullOrEmpty(id))
            {
                context.Result = new NotFoundObjectResult(Response<TEntity>.Fail("Id değeri boş olamaz", 404));
                return;
            }

            if (!_service.Any(x=> x.Id.ToString() == id && x.ApplicationUserId == _sharedIdentityService.GetUserId))
            {
                var error = $"Böyle bir veri bulunamadı.";
                context.Result = new NotFoundObjectResult(Response<TEntity>.Fail(error, 404));
                return;
            }

            await next();
          

        }
    }
}
