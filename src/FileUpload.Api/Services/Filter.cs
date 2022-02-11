﻿using FileUpload.Shared.Enums;
using FileUpload.Shared.Models;
using FileUpload.Shared.Models.Files;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FileUpload.Api.Services
{
    public static class Filter
    {
        public static IQueryable<Data.Entity.File> FilterFile(IQueryable<Data.Entity.File> model, FileFilterModel filterModel)
        {
            var maxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(model.Count()) / Convert.ToDouble(filterModel.Number)));


            if (filterModel.Page > maxPage)
            {
                filterModel.Page = maxPage;
            }

            var extensionFilterData = ExtensionFilter(model, filterModel.Extension);

            var orderFilterData = OrderFiles(extensionFilterData, filterModel.OrderBy);

            model = PaginationData(orderFilterData, filterModel.Page, filterModel.Number);

            return model;
        }

        public static IQueryable<Data.Entity.File> ExtensionFilter(IQueryable<Data.Entity.File> model, string extension)
        {

            if (string.IsNullOrEmpty(extension))
            {
                return model;
            }

            return model.Where(x => x.Extension == extension);
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
            filterModel.Page += 1;

            var extensionFilterData = ExtensionFilter(model, filterModel.Extension);

            var orderFilterData = OrderFiles(extensionFilterData, filterModel.OrderBy);

            model = orderFilterData.Skip((filterModel.Page - 1) * filterModel.Number).Take(1);

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
