using AutoMapper;
using FileUpload.Application.Interfaces.Repositories;
using FileUpload.Application.Wrappers;
using FileUpload.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Application.Features.Commands.Categories.Add
{
    public class AddCategoryCommand : IRequest<Response<bool>>
    {
        public string Title { get; set; }
    }

    public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, Response<bool>>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Category> _categoryRepository;

        public AddCategoryCommandHandler(IMapper mapper, IRepository<Category> categoryRepository)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public async Task<Response<bool>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<Category>(request);
            await _categoryRepository.AddAsync(category);

            return Response<bool>.Success(true, 200);
        }
    }
}
