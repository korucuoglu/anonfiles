using FileUpload.Upload.Application.Interfaces.UnitOfWork;
using FileUpload.Shared.Wrappers;
using FileUpload.Upload.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace FileUpload.Upload.Application.Features.Queries.Files.GetById
{
    public class GetFileKeyById : IRequest<Response<string>>
    {
        public int FileId { get; set; }
        public int UserId { get; set; }
    }
    public class GetFileKeyByIdHandler : IRequestHandler<GetFileKeyById, Response<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetFileKeyByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<string>> Handle(GetFileKeyById request, CancellationToken cancellationToken)
        {
            var repository = _unitOfWork.ReadRepository<File>();

            var data = await repository.Where(x=> x.Id == request.FileId && x.ApplicationUserId == request.UserId, tracking: false).Select(x=> x.FileKey).FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(data))
            {
                return Response<string>.Fail("Böyle bir veri bulunamadı");
            }

            return Response<string>.Success(data: data);
        }

    }
}
