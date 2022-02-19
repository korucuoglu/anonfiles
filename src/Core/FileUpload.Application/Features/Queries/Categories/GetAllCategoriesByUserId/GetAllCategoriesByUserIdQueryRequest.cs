using FileUpload.Application.Dtos.Categories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUpload.Application.Features.Queries.Categories.GetAllCategoriesByUserId
{
    public class GetAllCategoriesByUserIdQueryRequest : IRequest<List<GetCategoryDto>>
    {

    }
}
