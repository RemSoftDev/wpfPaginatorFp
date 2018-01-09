using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Func
{
    public static class PaginatorScope
    {
        public static Func<int, int, int, IEnumerable<int>,
            PaginatorState>
            Init()
        {
            return (currentPage, itemsPerPage, pagesToSkip, DbData) =>
            {
                var totalNumberOfItemsInDB = GetTotalNumberOfItemsInDB()(DbData);
                var numberOfPages = GetNumberOfPages()(totalNumberOfItemsInDB, itemsPerPage);

                var isValidLeft = IsValidLeft()(currentPage);
                var isValidLeftMore = IsValidLeftMore()(currentPage, pagesToSkip);
                var isValidRight = IsValidRight()(currentPage, numberOfPages);
                var isValidRightMore = IsValidRightMore()(currentPage, pagesToSkip, numberOfPages);

                var pagesRight = PagesRight(currentPage, itemsPerPage, DbData);
                var pagesRightMore = PagesRight(currentPage, itemsPerPage, DbData);

                var pagesLeft = PagesLeft(currentPage, itemsPerPage, DbData);
                var pagesLeftMore = PagesRight(currentPage, itemsPerPage, DbData);

                return new PaginatorState(
                    currentPage,
                    itemsPerPage,
                    pagesToSkip,
                    numberOfPages,
                    isValidLeft,
                    isValidLeftMore,
                    isValidRight,
                    isValidRightMore,
                    DbData,
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
            return Init()(++arg.CurrentPage, arg.ItemsPerPage, arg.PagesToSkip, arg.DbData);
        }

        public static PaginatorState
            GoLeft<TRes>(
            this PaginatorState arg)
        {
            return Init()(--arg.CurrentPage, arg.ItemsPerPage, arg.PagesToSkip, arg.DbData);
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
