using FileUpload.Application.Dtos.Files;
using FileUpload.Application.Dtos.Files.Pager;
using FileUpload.Application.Wrappers;
using FileUpload.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FileUpload.Application.Helper
{
    public static class Filter
    {
        public static async Task<Response<FilesPagerViewModel>> FilterFile(IQueryable<File> model, FileFilterModel filterModel)
        {

            model = ExtensionFilter(model, filterModel.Extension);

            model = CategoryFilter(model, filterModel.Category);

            var modelCount = model.Count();

            if (modelCount == 0)
            {
                return Response<FilesPagerViewModel>.Success(new FilesPagerViewModel(), 200);
            }

            Pager pager = new(modelCount, filterModel.Page, filterModel.PageSize);

            model = OrderFiles(model, filterModel.OrderBy);

            model = PaginationData(model, pager.CurrentPage, filterModel.PageSize);


            FilesPagerViewModel dto = new()
            {
                Pages = pager,
                Files = await model.Select(x => new GetFileDto()
                {
                    Id = x.Id,
                    FileName = x.FileName,
                    Size = x.Size,
                    CreatedDate = x.CreatedDate

                }).ToListAsync()
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

        public static IQueryable<File> CategoryFilter(IQueryable<File> model, string CategoryName)
        {
            if (string.IsNullOrEmpty(CategoryName))
            {
                return model;
            }
            return model.Include(x => x.Files_Categories).Where(d => d.Files_Categories.Select(a => a.Category.Title.ToLower()).Contains(CategoryName.ToLower()));
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

        public static async Task<Response<FilePagerViewModel>> GetOneFileAfterRemovedFile(IQueryable<File> model, FileFilterModel filterModel)
        {
            var count = model.Count();

            GetFileDto fileDto = null;

            Pager pager = new(count -1, filterModel.Page, filterModel.PageSize);

            if (count <= (filterModel.Page * filterModel.PageSize))
            {
                FilePagerViewModel data = new()
                {
                    Pages = pager,
                    File = fileDto
                };

                return Response<FilePagerViewModel>.Success(data, 200);
            }

            var extensionFilterData = ExtensionFilter(model, filterModel.Extension);

            var orderFilterData = OrderFiles(extensionFilterData, filterModel.OrderBy);

            model = orderFilterData.Skip(filterModel.Page * filterModel.PageSize).Take(1);

            fileDto = await model.Select(x => new GetFileDto()
            {
                Id = x.Id,
                FileName = x.FileName,
                Size = x.Size,
                CreatedDate = x.CreatedDate

            }).FirstOrDefaultAsync();

            FilePagerViewModel dto = new()
            {
                Pages = pager,
                File = fileDto
            };

            return Response<FilePagerViewModel>.Success(dto, 200);




        }

    }
}
