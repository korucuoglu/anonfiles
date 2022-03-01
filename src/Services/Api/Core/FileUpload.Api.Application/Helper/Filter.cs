using AutoMapper;
using FileUpload.Api.Application.Dtos.Categories;
using FileUpload.Api.Application.Dtos.Files;
using FileUpload.Api.Application.Dtos.Files.Pager;
using FileUpload.Api.Application.Wrappers;
using FileUpload.Api.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUpload.Api.Application.Helper
{
    public static class Filter
    {
        public static async Task<Response<FilesPagerViewModel>> FilterFile(IQueryable<File> model, FileFilterModel filterModel, IMapper mapper)
        {

            model = ExtensionFilter(model, filterModel.Extension);

            model = CategoryFilter(model, filterModel.CategoryIds);

            if (!model.Any())
            {
                return Response<FilesPagerViewModel>.Success(new FilesPagerViewModel(), 200);
            }

            Pager pager = new(model.Count(), filterModel.Page, filterModel.PageSize);

            model = OrderFiles(model, filterModel.OrderBy);

            var categories = model.SelectMany(x => x.FilesCategories.Select(a => a.Category)).Distinct();

            model = PaginationData(model, pager.CurrentPage, filterModel.PageSize);


            FilesPagerViewModel dto = new()
            {
                Pages = pager,
                Files =  mapper.ProjectTo<GetFileDto>(model).ToList(),
                Categories = mapper.ProjectTo<GetCategoryDto>(categories).ToList(),
            };


            return Response<FilesPagerViewModel>.Success(dto, 200);
        }

        public static IQueryable<File> ExtensionFilter(IQueryable<File> model, string extension)
        {
            if (string.IsNullOrEmpty(extension))
            {
                return model;
            }

            return model.Where(x => x.Extension.ToUpper() == extension.ToUpper());
        }

        public static IQueryable<File> CategoryFilter(IQueryable<File> model, List<string> CategoryIds)
        {
            if (CategoryIds.Count == 0)
            {
                return model;
            }

            return model;

            // return model.Where(x => x.FilesCategories).Where(p => p.FilesCategories.Select(a => a.Category).Any(pp => CategoryIds.Contains(pp.Id)));
        }

        public static IQueryable<File> OrderFiles(IQueryable<File> model, int orderBy)
        {

            EnumOrderBy OrderEnum = (EnumOrderBy)orderBy;

            if (OrderEnum == EnumOrderBy.YenidenEskiye)
            {
                return model.OrderByDescending(x => x.CreatedDate);
            }

            if (OrderEnum == EnumOrderBy.EskidenYeniye)
            {
                return model.OrderBy(x => x.CreatedDate);
            }

            if (OrderEnum == EnumOrderBy.BoyutaGöreArtan)
            {
                return model.OrderBy(x => x.Size);
            }

            if (OrderEnum == EnumOrderBy.BoyutaGöreAzalan)
            {
                return model.OrderByDescending(x => x.Size);
            }

            return model;
        }

        public static IQueryable<File> PaginationData(IQueryable<File> model, int page, int number)
        {
            return model.Skip((page - 1) * number).Take(number);
        }

        public static async Task<Response<FilePagerViewModel>> GetDataInNextPageAfterRemovedFile(IQueryable<File> model, FileFilterModel filterModel, IMapper mapper)
        {
            model = ExtensionFilter(model, filterModel.Extension);

            model = CategoryFilter(model, filterModel.CategoryIds);

            var count = model.Count();

            Pager pager = new(count - 1, filterModel.Page, filterModel.PageSize);

            if (count <= (filterModel.Page * filterModel.PageSize))
            {
                FilePagerViewModel data = new()
                {
                    Pages = pager,
                };

                return Response<FilePagerViewModel>.Success(data, 200);
            }

           

            model = OrderFiles(model, filterModel.OrderBy);

            var categories = model.SelectMany(x => x.FilesCategories.Select(a => a.Category)).Distinct();

            model = model.Skip(filterModel.Page * filterModel.PageSize).Take(1);

            FilePagerViewModel dto = new()
            {
                Pages = pager,
                File = mapper.ProjectTo<GetFileDto>(model).FirstOrDefault(),
            };

          

            return Response<FilePagerViewModel>.Success(dto, 200);




        }

    }
}
