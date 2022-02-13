using FileUpload.Shared.Enums;
using FileUpload.Shared.Models;
using FileUpload.Shared.Models.Files;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUpload.Api.Services
{
    public static class Filter
    {
        public static async Task<Response<MyFilesViewModel>> FilterFile(IQueryable<Data.Entity.File> model, FileFilterModel filterModel)
        {
            var modelCount = model.Count();

            if (modelCount == 0)
            {
                return Response<MyFilesViewModel>.Success(200);
            }

            Pager pager = new(modelCount, filterModel.Page, filterModel.PageSize);

            var extensionFilterData = ExtensionFilter(model, filterModel.Extension);

            var orderFilterData = OrderFiles(extensionFilterData, filterModel.OrderBy);

            model = PaginationData(orderFilterData, pager.CurrentPage, filterModel.PageSize);

            var fileDto = await model.Select(x => new FileDto()
            {
                FileId = x.Id,
                FileName = x.FileName,
                Size = x.Size,
                CreatedDate = x.CreatedDate

            }).ToListAsync();


            MyFilesViewModel dto = new()
            {
                Pages = pager,
                Files = fileDto
            };

            return Response<MyFilesViewModel>.Success(dto, 200);
        }

        public static IQueryable<Data.Entity.File> ExtensionFilter(IQueryable<Data.Entity.File> model, string extension)
        {

            if (string.IsNullOrEmpty(extension))
            {
                return model;
            }

            return model.Where(x => x.Extension.ToUpper() == extension.ToUpper());
        }

        public static IQueryable<Data.Entity.File> OrderFiles(IQueryable<Data.Entity.File> model, int orderBy)
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

        public static IQueryable<Data.Entity.File> PaginationData(IQueryable<Data.Entity.File> model, int page, int number)
        {
            return model.Skip((page - 1) * number).Take(number);
        }

        public static async Task<Response<MyFileViewModel>> GetOneFileAfterRemovedFile(IQueryable<Data.Entity.File> model, FileFilterModel filterModel)
        {
            var count = model.Count();

            FileDto fileDto = null;

            Pager pager = new(count-1, filterModel.Page, filterModel.PageSize);

            if (count <= (filterModel.Page * filterModel.PageSize))
            {
                MyFileViewModel data = new()
                {
                    Pages = pager,
                    File = fileDto
                };

                return Response<MyFileViewModel>.Success(data, 200);
            }

            var extensionFilterData = ExtensionFilter(model, filterModel.Extension);

            var orderFilterData = OrderFiles(extensionFilterData, filterModel.OrderBy);

            model = orderFilterData.Skip(filterModel.Page * filterModel.PageSize).Take(1);

             fileDto = await model.Select(x => new FileDto()
            {
                FileId = x.Id,
                FileName = x.FileName,
                Size = x.Size,
                CreatedDate = x.CreatedDate

            }).FirstOrDefaultAsync();

            MyFileViewModel dto = new()
            {
                Pages = pager,
                File = fileDto
            };


            return Response<MyFileViewModel>.Success(dto, 200);




        }

    }
}
