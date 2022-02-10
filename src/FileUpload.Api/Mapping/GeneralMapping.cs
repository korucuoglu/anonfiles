using AutoMapper;
using FileUpload.Data.Entity;
using FileUpload.Shared.Dtos.Categories;

namespace FileUpload.Api.Mapping
{
    public class GeneralMapping: Profile
    {

        public GeneralMapping()
        {
            CreateMap<Category, GetCategoriesDto>();
        }
    }
}
