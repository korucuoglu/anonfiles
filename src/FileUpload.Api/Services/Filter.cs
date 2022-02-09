using FileUpload.Shared.Enums;
using FileUpload.Shared.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FileUpload.Api.Services
{
    public static class Filter
    {
        public static IQueryable<Data.Entity.File> FilterFile (IQueryable<Data.Entity.File> model, int page, int number, int orderBy)
        {
            var maxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(model.Count()) / Convert.ToDouble(number)));

            if (page > maxPage)
            {
                page = maxPage;
            }

            var orderedData = OrderFiles(model, orderBy);

            model = PaginationData(orderedData, page, number);

            return model;
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

        public static IQueryable<Data.Entity.File> PaginationData (IQueryable<Data.Entity.File> model, int page, int number)
        {
            return model.Skip((page - 1) * number).Take(number);
        }

    }
}
