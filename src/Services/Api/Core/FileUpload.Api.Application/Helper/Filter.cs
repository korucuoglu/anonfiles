using AutoMapper;
using FileUpload.Application.Wrappers;
using FileUpload.Domain.Entities;
using FileUpload.Shared.Dtos.Categories;
using FileUpload.Shared.Dtos.Files;
using FileUpload.Shared.Dtos.Files.Enums;
using FileUpload.Shared.Dtos.Files.Pager;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUpload.Application.Helper
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


            var categories = from file in model
                             join category in model.SelectMany(x => x.FilesCategories) on file.Id equals category.FileId
                             group category by new
                             {
                                 category.CategoryId,
                                 category.Category.Title
                             } into grp
                             select new GetCategoryDto
                             {
                                 Id = grp.Key.CategoryId,
                                 Title = grp.Key.Title,
                                 Count = grp.Select(x => x.CategoryId).Count()
                             };

            model = PaginationData(model, pager.CurrentPage, filterModel.PageSize);


            FilesPagerViewModel dto = new()
            {
                Pages = pager,
                Files = await mapper.ProjectTo<GetFileDto>(model).ToListAsync(),
                Categories = categories.ToList()
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

        public static IQueryable<File> CategoryFilter(IQueryable<File> model, List<Guid> categoriesIds)
        {
            if (!categoriesIds.Any())
            {
                return model;
            }

            return model.Include(x => x.FilesCategories).Where(p => p.FilesCategories.Select(a => a.Category).Any(pp => categoriesIds.Contains(pp.Id)));
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

            Pager pager = new(count, filterModel.Page, filterModel.PageSize);
            var categories = from file in model
                             join category in model.SelectMany(x => x.FilesCategories) on file.Id equals category.FileId
                             group category by new
                             {
                                 category.CategoryId,
                                 category.Category.Title
                             } into grp
                             select new GetCategoryDto
                             {
                                 Id = grp.Key.CategoryId,
                                 Title = grp.Key.Title,
                                 Count = grp.Select(x => x.CategoryId).Count()
                             };

            if (count <= (filterModel.Page * filterModel.PageSize))
            {
                FilePagerViewModel data = new()
                {
                    Pages = pager,
                    Categories = categories.ToList()
                };

                return Response<FilePagerViewModel>.Success(data, 200);
            }

            model = OrderFiles(model, filterModel.OrderBy);

            model = model.Skip(filterModel.Page * filterModel.PageSize).Take(1);

            FilePagerViewModel dto = new()
            {
                Pages = pager,
                File = await mapper.ProjectTo<GetFileDto>(model).FirstOrDefaultAsync(),
                Categories = categories.ToList()
            };

            return Response<FilePagerViewModel>.Success(dto, 200);
        }
    }
}
