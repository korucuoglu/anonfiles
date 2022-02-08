using FileUpload.Shared.Enums;
using FileUpload.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUpload.MVC.Services
{
    public static class FilteredData
    {

        public static async Task<IEnumerable<MyFilesViewModel>> GetFilteredData (IQueryable<MyFilesViewModel> model, int page, int number, int OrderBy)
        {
            var maxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(model.Count() / number)));

            if (page > maxPage)
            {
                page = maxPage;
            }

            var orderedData = OrderFiles(model, OrderBy);

            model = PaginationData(orderedData, page, number);

            return model;
        }

        public static IQueryable<MyFilesViewModel> OrderFiles(IQueryable<MyFilesViewModel> model, int OrderBy)
        {
            EnumOrderBy orderBy = (EnumOrderBy)OrderBy;
           
            if (orderBy == EnumOrderBy.YenidenEskiye)
            {
                return model.OrderBy(x => x.UploadedDate).AsQueryable();
            }

            if (orderBy == EnumOrderBy.EskidenYeniye)
            {
                return model.OrderByDescending(x => x.UploadedDate).AsQueryable();
            }

            if (orderBy == EnumOrderBy.BoyutaGöreArtan)
            {
                return model.OrderBy(x => x.Size).AsQueryable();
            }

            if (orderBy == EnumOrderBy.BoyutaGöreAzalan)
            {
                return model.OrderByDescending(x => x.Size).AsQueryable();
            }

            return model;
        }

        public static IQueryable<MyFilesViewModel> PaginationData (IQueryable<MyFilesViewModel> model, int page, int number)
        {
            return model.Skip((page - 1) * 10).Take(number);
        }

    }
}
