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
            (
            int CurrentPage,
            int ItemsPerPage,
            int PagesToSkip,
            IEnumerable<int> DbData,
            int NumberOfPages,
            bool IsValidLeft,
            bool IsValidLeftMore,
            bool IsValidRight,
            bool IsValidRightMore
            )>
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

                return (currentPage, itemsPerPage, pagesToSkip, DbData, numberOfPages, isValidLeft, isValidLeftMore, isValidRight, isValidRightMore);
            };
        }

        public static IEnumerable<int>
            PageRight<TRes>
            (this
                (int CurrentPage,
                 int ItemsPerPage,
                 int PagesToSkip,
                 IEnumerable<int> DbData,
                 int NumberOfPages,
                 bool IsValidLeft,
                 bool IsValidLeftMore,
                 bool IsValidRight,
                 bool IsValidRightMore) tuple
            )
        {
                var itemsToShow = GetItemsToShow(
                     GetLeftIndex(),
                     GetRightIndex(),
                     GetDataStartEndIndex())
                     (tuple.CurrentPage, tuple.ItemsPerPage, tuple.DbData);

                return (itemsToShow);
        }

        public static Func<IEnumerable<int>>
            PageLeft<TRes>
            (this
                (int CurrentPage,
                 int ItemsPerPage,
                 int PagesToSkip,
                 IEnumerable<int> DbData,
                 int NumberOfPages,
                 bool IsValidLeft,
                 bool IsValidLeftMore,
                 bool IsValidRight,
                 bool IsValidRightMore) tuple
            )
        {
            return () =>
            {
                var itemsToShow = GetItemsToShow(
                     GetLeftIndex(),
                     GetRightIndex(),                    
                     GetDataStartEndIndex())
                     (tuple.CurrentPage-1, tuple.ItemsPerPage, tuple.DbData);

                return (itemsToShow);
            };
        }

        private static Func<int, int, IEnumerable<int>, IEnumerable<int>> GetItemsToShow(
            Func<int, int, int> getStartIndex,
            Func<int, int, int> getEndIndex,
            Func<int, int, IEnumerable<int>, IEnumerable<int>> GetDataStartEndIndex
            )
        {
            return (currentPage, itemsPerPage, DbData) =>
            {
                var startIndex = getStartIndex(currentPage, itemsPerPage);
                var endIndex = getEndIndex(currentPage, itemsPerPage);

                return GetDataStartEndIndex(startIndex, endIndex, DbData);
            };
        }

        private static Func<int, int, int> GetLeftIndex()
        {
            return (CurrentPage, ItemsPerPage) =>
            {
                return (CurrentPage - 1) * ItemsPerPage;
            };
        }

        private static Func<int, int, int> GetRightIndex()
        {
            return (CurrentPage, ItemsPerPage) =>
            {
                return CurrentPage * ItemsPerPage;
            };
        }

        private static Func<int, int, IEnumerable<int>, IEnumerable<int>> GetDataStartEndIndex()
        {
            return (startIndex, endIndex, DbData) =>
            {
                return DbData.Where(z => z > startIndex && z <= endIndex);
            };
        }

        private static Func<IEnumerable<int>, int> GetTotalNumberOfItemsInDB()
        {
            return (DbData) =>
            {
                return DbData.Count();
            };
        }

        private static Func<int, int, int> GetNumberOfPages()
        {
            return (TotalNumberOfItemsInDB, ItemsPerPage) =>
            {
                var res = (int)Math.Round((double)TotalNumberOfItemsInDB / ItemsPerPage, 0, MidpointRounding.AwayFromZero);
                return res;
            };
        }

        private static Func<int, bool> IsValidLeft()
        {
            return (CurrentPage) =>
            {
                var res = false;

                if (CurrentPage > 1)
                {
                    res = true;
                }

                return res;
            };
        }

        private static Func<int, int, bool> IsValidLeftMore()
        {
            return (CurrentPage, PagesToSkip) =>
            {
                var res = false;

                if (CurrentPage - PagesToSkip > 1)
                {
                    res = true;
                }

                return res;
            };
        }

        private static Func<int, int, bool> IsValidRight()
        {
            return (CurrentPage, NumberOfPages) =>
            {
                var res = false;

                if (CurrentPage < NumberOfPages)
                {
                    res = true;
                }

                return res;
            };
        }

        private static Func<int, int, int, bool> IsValidRightMore()
        {
            return (CurrentPage, PagesToSkip, NumberOfPages) =>
            {
                var res = false;

                if (CurrentPage + PagesToSkip < NumberOfPages)
                {
                    res = true;
                }

                return res;
            };
        }
    }
}
