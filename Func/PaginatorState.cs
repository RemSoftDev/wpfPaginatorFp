using System;
using System.Collections.Generic;

namespace Func
{
    public class PaginatorState
    {
        public PaginatorState()
        {

        }

        public PaginatorState(PaginatorState old)
        {
            CurrentPage = old.CurrentPage;
            ItemsPerPage = old.ItemsPerPage;
            PagesToSkip = old.PagesToSkip;
            NumberOfPages = old.NumberOfPages;
            TotalNumberOfItemsInDB = old.TotalNumberOfItemsInDB;

            IsValidLeft = old.IsValidLeft;
            IsValidLeftMore = old.IsValidLeftMore;
            IsValidRight = old.IsValidRight;
            IsValidRightMore = old.IsValidRightMore;

            DbData = old.DbData;

            PagesRight = old.PagesRight;
            PagesRightMore = old.PagesRightMore;
            PagesLeft = old.PagesLeft;
            PagesLeftMore = old.PagesLeftMore;
        }

        public PaginatorState(
            int pCurrentPage,
            int pItemsPerPage,
            int pPagesToSkip,
            int pNumberOfPages,
            int pTotalNumberOfItemsInDB,

            bool pIsValidLeft,
            bool pIsValidLeftMore,
            bool pIsValidRight,
            bool pIsValidRightMore,

            IEnumerable<int> pDbData,

            Func<IEnumerable<int>> pPagesRight,
            Func<IEnumerable<int>> pPagesRightMore,
            Func<IEnumerable<int>> pPagesLeft,
            Func<IEnumerable<int>> pPagesLeftMore
            )
        {
            CurrentPage = pCurrentPage;
            ItemsPerPage = pItemsPerPage;
            PagesToSkip = pPagesToSkip;
            NumberOfPages = pNumberOfPages;
            TotalNumberOfItemsInDB = pTotalNumberOfItemsInDB;

            IsValidLeft = pIsValidLeft;
            IsValidLeftMore = pIsValidLeftMore;
            IsValidRight = pIsValidRight;
            IsValidRightMore = pIsValidRightMore;

            DbData = pDbData;

            PagesRight = pPagesRight;
            PagesRightMore = pPagesRightMore;
            PagesLeft = pPagesLeft;
            PagesLeftMore = pPagesLeftMore;
        }

        public int CurrentPage;
        public int ItemsPerPage;
        public int PagesToSkip;
        public int NumberOfPages;
        public int TotalNumberOfItemsInDB;

        public bool IsValidLeft;
        public bool IsValidLeftMore;
        public bool IsValidRight;
        public bool IsValidRightMore;

        public IEnumerable<int> DbData;

        public Func<IEnumerable<int>> PagesRight;
        public Func<IEnumerable<int>> PagesRightMore;
        public Func<IEnumerable<int>> PagesLeft;
        public Func<IEnumerable<int>> PagesLeftMore;
    }
}
