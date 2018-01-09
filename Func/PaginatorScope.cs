using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Func
{
    public static class PaginatorScope
    {
        public static Func<PaginatorState,
            PaginatorState>
            Init()
        {
            return (paginatorState) =>
            {
                var totalNumberOfItemsInDB = GetTotalNumberOfItemsInDB()(paginatorState.DbData);
                var numberOfPages = GetNumberOfPages()(totalNumberOfItemsInDB, paginatorState.ItemsPerPage);

                var isValidLeft = IsValidLeft()(paginatorState.CurrentPage);
                var isValidLeftMore = IsValidLeftMore()(paginatorState.CurrentPage, paginatorState.PagesToSkip);
                var isValidRight = IsValidRight()(paginatorState.CurrentPage, numberOfPages);
                var isValidRightMore = IsValidRightMore()(paginatorState.CurrentPage, paginatorState.PagesToSkip, numberOfPages);

                var pagesRight = PagesRight(paginatorState.CurrentPage, paginatorState.ItemsPerPage, paginatorState.DbData);
                var pagesRightMore = PagesRight(paginatorState.CurrentPage, paginatorState.ItemsPerPage, paginatorState.DbData);

                var pagesLeft = PagesLeft(paginatorState.CurrentPage, paginatorState.ItemsPerPage, paginatorState.DbData);
                var pagesLeftMore = PagesRight(paginatorState.CurrentPage, paginatorState.ItemsPerPage, paginatorState.DbData);

                return new PaginatorState(
                    paginatorState.CurrentPage,
                    paginatorState.ItemsPerPage,
                    paginatorState.PagesToSkip,
                    numberOfPages,
                    isValidLeft,
                    isValidLeftMore,
                    isValidRight,
                    isValidRightMore,
                    paginatorState.DbData,
                    pagesRight,
                    pagesRightMore,
                    pagesLeft,
                    pagesLeftMore);
            };
        }

        public static PaginatorState
            GoRight<TRes>(
            this PaginatorState arg)
        {
            ++arg.CurrentPage;
            return Init()(arg);
        }

        public static PaginatorState
            GoLeft<TRes>(
            this PaginatorState arg)
        {
            --arg.CurrentPage;
            return Init()(arg);
        }

        public static Func<int, int, IEnumerable<int>, IEnumerable<int>>
            GetPages()
        {
            var itemsToShowFunc = GetItemsToShow(
                 GetLeftIndex(),
                 RightIndex,
                 GetDataStartEndIndex());

            return itemsToShowFunc;
        }

        public static Func<IEnumerable<int>>
            PagesRight(
            int CurrentPage, int ItemsPerPage, IEnumerable<int> DbData) => () =>
                {
                    return GetPages()(CurrentPage, ItemsPerPage, DbData);
                };

        public static Func<IEnumerable<int>>
            PagesLeft(
           int CurrentPage, int ItemsPerPage, IEnumerable<int> DbData
            )
        {
            return () =>
            {
                return GetPages()(CurrentPage, ItemsPerPage, DbData);
            };
        }

        private static Func<int, int, IEnumerable<int>, IEnumerable<int>>
            GetItemsToShow(
            Func<int, int, int> getStartIndex,
            Func<int, int, int> getEndIndex,
            Func<int, int, IEnumerable<int>, IEnumerable<int>> GetDataStartEndIndex
            )
        {
            return (currentPage, itemsPerPage, DbData) =>
            {
                // how to handle the error (currentPage < 1) ???  //some Options !!!
                var startIndex = getStartIndex(currentPage, itemsPerPage); //Cary... PipeTo
                var endIndex = getEndIndex(currentPage, itemsPerPage);

                return GetDataStartEndIndex(startIndex, endIndex, DbData);
            };
        }

        private static Func<int, int, int>
            GetLeftIndex() => (CurrentPage, ItemsPerPage) =>
                {
                    return (CurrentPage - 1) * ItemsPerPage;
                };

        private static Func<int, int, int> 
            RightIndex => (CurrentPage, ItemsPerPage) =>
                {
                    return CurrentPage * ItemsPerPage;
                };

        private static Func<int, int, IEnumerable<int>, IEnumerable<int>>
            GetDataStartEndIndex() => (startIndex, endIndex, DbData) =>
                {
                    return DbData.Where(z => z > startIndex && z <= endIndex);
                };

        private static Func<IEnumerable<int>, int>
            GetTotalNumberOfItemsInDB() => (DbData) => DbData.Count();

        private static Func<int, int, int>
            GetNumberOfPages() => (TotalNumberOfItemsInDB, ItemsPerPage) =>
                {
                    var res = (int)Math.Round((double)TotalNumberOfItemsInDB / ItemsPerPage, 0, MidpointRounding.AwayFromZero);
                    return res;
                };

        private static Func<int, bool>
            IsValidLeft() => (CurrentPage) => CurrentPage > 1;

        private static Func<int, int, bool>
            IsValidLeftMore() => (CurrentPage, PagesToSkip) => CurrentPage - PagesToSkip > 1;

        private static Func<int, int, bool>
            IsValidRight() => (CurrentPage, NumberOfPages) => CurrentPage < NumberOfPages;

        private static Func<int, int, int, bool>
            IsValidRightMore() => (CurrentPage, PagesToSkip, NumberOfPages) => CurrentPage + PagesToSkip < NumberOfPages;

        private static Func<int, int, int, bool>
            IsValidItemsPerPage()
        {
            return (CurrentPage, PagesToSkip, NumberOfPages) =>
            {
                return false;
            };
        }

        private static Func<int, int, int, bool>
            IsValidPagesToSkip()
        {
            return (CurrentPage, PagesToSkip, NumberOfPages) =>
            {
                return false;
            };
        }
    }
}
