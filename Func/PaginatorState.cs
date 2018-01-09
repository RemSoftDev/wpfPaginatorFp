using System;
using System.Collections.Generic;

namespace Func
{
    public class PaginatorState
    {
        public PaginatorState(
            int pCurrentPage,
            int pItemsPerPage,
            int pPagesToSkip,
            int pNumberOfPages,

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
