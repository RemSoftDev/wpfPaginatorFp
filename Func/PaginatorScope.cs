using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Func
{
    public static class PaginatorScope
    {
        public static Func<PaginatorState, PaginatorState>
            Init()
        {
            return (paginatorState) =>
            {
                var res = paginatorState.
                          PipeForward(GetTotalNumberOfItemsInDB()).
                          PipeForward(GetNumberOfPages()).
                          PipeForward(IsValidLeft()).
                          PipeForward(IsValidLeftMore()).
                          PipeForward(IsValidRight()).
                          PipeForward(IsValidRightMore());

                //var totalNumberOfItemsInDB = GetTotalNumberOfItemsInDB()(paginatorState);
                //var numberOfPages = GetNumberOfPages()(paginatorState);

                //var isValidLeft = IsValidLeft()(paginatorState);
                //var isValidLeftMore = IsValidLeftMore()(paginatorState);
                //var isValidRight = IsValidRight()(paginatorState);
                //var isValidRightMore = IsValidRightMore()(paginatorState);

                var pagesRight = PagesRight(paginatorState);
                var pagesRightMore = PagesRight(paginatorState);

                var pagesLeft = PagesLeft(paginatorState);
                var pagesLeftMore = PagesRight(paginatorState);

                return res;
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
            return GetItemsToShow(
                     GetLeftIndex(),
                     RightIndex,
                     GetDataStartEndIndex());
        }

        public static Func<IEnumerable<int>>
            PagesRight(
            PaginatorState paginatorState) => () =>
                {
                    return GetPages()(paginatorState.CurrentPage, paginatorState.ItemsPerPage, paginatorState.DbData);
                };

        public static Func<IEnumerable<int>>
            PagesLeft(
           PaginatorState paginatorState
            )
        {
            return () =>
            {
                return GetPages()(paginatorState.CurrentPage, paginatorState.ItemsPerPage, paginatorState.DbData);
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
                var startIndex = getStartIndex.Curry(currentPage)(itemsPerPage);
                var endIndex = getEndIndex.Curry(currentPage)(itemsPerPage);

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

        private static Func<PaginatorState, PaginatorState>
            GetTotalNumberOfItemsInDB() => (paginatorState) =>
                {
                    paginatorState.TotalNumberOfItemsInDB = paginatorState.DbData.Count();
                    return new PaginatorState(paginatorState);
                };

        private static Func<PaginatorState, PaginatorState>
            GetNumberOfPages() => (paginatorState) =>
                {
                    var res = (int)Math.Round((double)paginatorState.TotalNumberOfItemsInDB / paginatorState.ItemsPerPage, 0, MidpointRounding.AwayFromZero);
                    return paginatorState.With(z => z.NumberOfPages = res);
                };

        private static Func<PaginatorState, PaginatorState>
            IsValidLeft() => (paginatorState) =>
                {
                    //var res = GetPages()(paginatorState.CurrentPage, paginatorState.ItemsPerPage, paginatorState.DbData);
                    //return paginatorState.With(z => z.PagesRight = res);
                    return paginatorState.With(z => z.IsValidLeft = paginatorState.CurrentPage > 1);
                };

        private static Func<PaginatorState, PaginatorState>
            IsValidLeftMore() => (paginatorState) =>
                 {
                     return paginatorState.With(z => z.IsValidLeftMore = paginatorState.CurrentPage - paginatorState.PagesToSkip > 1);
                 };

        private static Func<PaginatorState, PaginatorState>
                IsValidRight() => (paginatorState) =>
                 {
                     return paginatorState.With(z => z.IsValidRight = paginatorState.CurrentPage < paginatorState.NumberOfPages);
                 };

        private static Func<PaginatorState, PaginatorState>
                IsValidRightMore() => (paginatorState) =>
                 {
                     return paginatorState.With(z => z.IsValidRightMore = paginatorState.CurrentPage + paginatorState.PagesToSkip < paginatorState.NumberOfPages);
                 };

        private static Func<PaginatorState, bool>
                    IsValidItemsPerPage()
        {
            return (paginatorState) =>
            {
                return false;
            };
        }

        private static Func<PaginatorState, bool>
            IsValidPagesToSkip()
        {
            return (paginatorState) =>
            {
                return false;
            };
        }
    }
}
