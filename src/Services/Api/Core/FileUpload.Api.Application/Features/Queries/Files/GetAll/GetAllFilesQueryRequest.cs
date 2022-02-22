using FileUpload.Application.Dtos.Files;
using FileUpload.Application.Interfaces.UnitOfWork;
using FileUpload.Application.Wrappers;
using FileUpload.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Application.Features.Queries.Files.GetAll
{
    public class GetAllFilesQueryRequest : IRequest<Response<MyFilesViewModel>>
    {
        public FileFilterModel FilterModel { get; set; }
        public Guid UserId { get; set; }
    }
    public class GetAllFilesQueryRequestHandler : IRequestHandler<GetAllFilesQueryRequest, Response<MyFilesViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllFilesQueryRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<MyFilesViewModel>> Handle(GetAllFilesQueryRequest request, CancellationToken cancellationToken)
        {
            var repository = _unitOfWork.GetRepository<File>();

            if (repository.Any(x => x.ApplicationUserId == request.UserId))
            {
                return await Helper.Filter.FilterFile(repository.Where(x => x.ApplicationUserId == request.UserId), request.FilterModel);
            }

            return Response<MyFilesViewModel>.Success(new MyFilesViewModel(), 200);
        }
    }
}
