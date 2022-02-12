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
        public static async Task<Response<List<MyFilesViewModel>>> FilterFile(IQueryable<Data.Entity.File> model, FileFilterModel filterModel)
        {
            var modelCount = model.Count();

            if (modelCount == 0)
            {
                return Response<List<MyFilesViewModel>>.Success(200);
            }

            var maxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(modelCount) / Convert.ToDouble(filterModel.Number)));

            if (filterModel.Page > maxPage)
            {
                filterModel.Page = maxPage;
            }

            var extensionFilterData = ExtensionFilter(model, filterModel.Extension);

            var orderFilterData = OrderFiles(extensionFilterData, filterModel.OrderBy);

            model = PaginationData(orderFilterData, filterModel.Page, filterModel.Number);

            var data = await model.Select(x => new MyFilesViewModel()
            {
                FileId = x.Id,
                FileName = x.FileName,
                Size = x.Size,
                UploadedDate = x.CreatedDate

            }).ToListAsync();

            return Response<List<MyFilesViewModel>>.Success(data, 200);
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

        public static async Task<Response<MyFilesViewModel>> GetOneFileAfterRemovedFile(IQueryable<Data.Entity.File> model, FileFilterModel filterModel)
        {
            if (model.Count() <= (filterModel.Page * filterModel.Number))
            {
                return Response<MyFilesViewModel>.Success(200);
            }

            var extensionFilterData = ExtensionFilter(model, filterModel.Extension);

            var orderFilterData = OrderFiles(extensionFilterData, filterModel.OrderBy);

            model = orderFilterData.Skip(filterModel.Page * filterModel.Number).Take(1);

            var data = await model.Select(x => new MyFilesViewModel()
            {
                FileId = x.Id,
                FileName = x.FileName,
                Size = x.Size,
                UploadedDate = x.CreatedDate

            }).FirstOrDefaultAsync();

            return Response<MyFilesViewModel>.Success(data, 200);




        }

    }
}
