using System;

namespace FileUpload.Api.Application.Dtos.Files.Pager
{
    public class Pager
    {
        public int CurrentPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPage { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }

        public Pager()
        {

        }

        public Pager(int totalItems, int currentPage, int pageSize = 10)
        {
            int totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);

            if (currentPage > totalPages)
            {
                currentPage = totalPages;
            }

            int startPage = currentPage - 5;
            int endPage = currentPage + 4;


            if (startPage <= 0)
            {
                endPage = endPage - (startPage - 1);
                startPage = 1;
            }

            if (endPage > totalPages)
            {
                endPage = totalPages;

                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }
            TotalItems = totalItems;
            CurrentPage = currentPage;
            TotalPage = totalPages;
            StartPage = startPage;
            EndPage = endPage;

        }
    }
}
