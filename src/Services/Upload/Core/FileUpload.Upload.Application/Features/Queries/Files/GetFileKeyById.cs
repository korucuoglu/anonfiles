using FileUpload.Upload.Application.Interfaces.UnitOfWork;
using FileUpload.Shared.Wrappers;
using FileUpload.Upload.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using FileUpload.Upload.Application.Interfaces.Services;

namespace FileUpload.Upload.Application.Features.Queries.Files
{
    public class GetFileKeyById : IRequest<Response<string>>
    {
        public int FileId { get; set; }
    }
    public class GetFileKeyByIdHandler : IRequestHandler<GetFileKeyById, Response<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISharedIdentityService _sharedIdentityService;
        public GetFileKeyByIdHandler(IUnitOfWork unitOfWork, ISharedIdentityService sharedIdentityService)
        {
            _unitOfWork = unitOfWork;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<Response<string>> Handle(GetFileKeyById request, CancellationToken cancellationToken)
        {
            var repository = _unitOfWork.ReadRepository<File>();

            var data = await repository.Where(
                x=> x.Id == request.FileId && x.ApplicationUserId == _sharedIdentityService.GetUserId, tracking: false).Select(x=> x.FileKey).FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(data))
            {
                return Response<string>.Fail("Böyle bir veri bulunamadı");
            }

            return Response<string>.Success(data: data);
        }

    }
}
