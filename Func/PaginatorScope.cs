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
            Func<IEnumerable<int>> PagesRight,
            Func<IEnumerable<int>> PagesRightMore,
            Func<IEnumerable<int>> PagesLeft,
            Func<IEnumerable<int>> PagesLeftMore,
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

                var pagesRight = PagesRight(currentPage, itemsPerPage, DbData);
                var pagesRightMore = PagesRight(currentPage, itemsPerPage, DbData);

                var pagesLeft = PagesLeft(currentPage, itemsPerPage, DbData);
                var pagesLeftMore = PagesRight(currentPage, itemsPerPage, DbData);

                return (
                    currentPage,
                    itemsPerPage,
                    pagesToSkip,
                    DbData,
                    pagesRight,
                    pagesRightMore,
                    pagesLeft,
                    pagesLeftMore,
                    numberOfPages,
                    isValidLeft,
                    isValidLeftMore,
                    isValidRight,
                    isValidRightMore);
            };
        }

        public static TRes PipeForward<TArg, TRes>(
           this TArg arg,
           Func<TArg, TRes> func)
        {
            return func(arg);
        }

        public static (int, int, int, IEnumerable<int>, Func<IEnumerable<int>>, Func<IEnumerable<int>>, Func<IEnumerable<int>>, Func<IEnumerable<int>>, int, bool, bool, bool, bool)
            GoRight<TRes>(
            this (
                int CurrentPage,
                int ItemsPerPage,
                int PagesToSkip,
                IEnumerable<int> DbData,
                Func<IEnumerable<int>> PagesRight,
                Func<IEnumerable<int>> PagesRightMore,
                Func<IEnumerable<int>> PagesLeft,
                Func<IEnumerable<int>> PagesLeftMore,
                int NumberOfPages,
                bool IsValidLeft,
                bool IsValidLeftMore,
                bool IsValidRight,
                bool IsValidRightMore
                ) arg)
        {
            return Init()(++arg.CurrentPage, arg.ItemsPerPage, arg.PagesToSkip, arg.DbData);
        }

        public static (int, int, int, IEnumerable<int>, Func<IEnumerable<int>>, Func<IEnumerable<int>>, Func<IEnumerable<int>>, Func<IEnumerable<int>>, int, bool, bool, bool, bool)
            GoLeft<TRes>(
            this (
                int CurrentPage,
                int ItemsPerPage,
                int PagesToSkip,
                IEnumerable<int> DbData,
                Func<IEnumerable<int>> PagesRight,
                Func<IEnumerable<int>> PagesRightMore,
                Func<IEnumerable<int>> PagesLeft,
                Func<IEnumerable<int>> PagesLeftMore,
                int NumberOfPages,
                bool IsValidLeft,
                bool IsValidLeftMore,
                bool IsValidRight,
                bool IsValidRightMore
                ) arg)
        {
            return Init()(--arg.CurrentPage, arg.ItemsPerPage, arg.PagesToSkip, arg.DbData);
        }

        public static Func<int, int, IEnumerable<int>, IEnumerable<int>>
            GetPages()
        {
            var itemsToShowFunc = GetItemsToShow(
                 GetLeftIndex(),
                 GetRightIndex(),
                 GetDataStartEndIndex());

            return itemsToShowFunc;
        }

        public static Func<IEnumerable<int>>
            PagesRight(
            int CurrentPage, int ItemsPerPage, IEnumerable<int> DbData)
        {
            return () =>
            {
                return GetPages()(CurrentPage, ItemsPerPage, DbData);
            };
        }

        public static Func<IEnumerable<int>>
            PagesLeft(
           Func CurrentPage(), int ItemsPerPage, IEnumerable<int> DbData
            )
        {
            return () =>
            {
                return GetPages()(CurrentPage(), ItemsPerPage, DbData);
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
            GetLeftIndex()
        {
            return (CurrentPage, ItemsPerPage) =>
            {
                return (CurrentPage - 1) * ItemsPerPage;
            };
        }

        private static Func<int, int, int>
            GetRightIndex()
        {
            return (CurrentPage, ItemsPerPage) =>
            {
                return CurrentPage * ItemsPerPage;
            };
        }

        private static Func<int, int, IEnumerable<int>, IEnumerable<int>>
            GetDataStartEndIndex()
        {
            return (startIndex, endIndex, DbData) =>
            {
                return DbData.Where(z => z > startIndex && z <= endIndex);
            };
        }

        private static Func<IEnumerable<int>, int>
            GetTotalNumberOfItemsInDB()
        {
            return (DbData) =>
            {
                return DbData.Count();
            };
        }

        private static Func<int, int, int>
            GetNumberOfPages()
        {
            return (TotalNumberOfItemsInDB, ItemsPerPage) =>
            {
                var res = (int)Math.Round((double)TotalNumberOfItemsInDB / ItemsPerPage, 0, MidpointRounding.AwayFromZero);
                return res;
            };
        }

        private static Func<int, bool>
            IsValidLeft()
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

        private static Func<int, int, bool>
            IsValidLeftMore()
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

        private static Func<int, int, bool>
            IsValidRight() => (CurrentPage, NumberOfPages) => CurrentPage < NumberOfPages;


        private static Func<int, int, int, bool>
            IsValidRightMore()
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

        private static Func<int, int, int, bool>
            IsValidItemsPerPage()
        {
            return (CurrentPage, PagesToSkip, NumberOfPages) =>
            {
                var res = false;

                //if (CurrentPage + PagesToSkip < NumberOfPages)
                //{
                //    res = true;
                //}

                return res;
            };
        }

        private static Func<int, int, int, bool>
            IsValidPagesToSkip()
        {
            return (CurrentPage, PagesToSkip, NumberOfPages) =>
            {
                var res = false;

                //if (CurrentPage + PagesToSkip < NumberOfPages)
                //{
                //    res = true;
                //}

                return res;
            };
        }
    }
}
