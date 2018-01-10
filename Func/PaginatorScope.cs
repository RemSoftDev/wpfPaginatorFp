using System;
using System.Collections.Generic;
using System.Linq;

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
                          PipeForward(IsValidRightMore()).
                          PipeForward(PagesToShow());

                return res;
            };
        }

        public static PaginatorState
            GoRight
            <TRes>(
            this PaginatorState paginatorState)
        {
            return Init()(paginatorState.With(z => z.CurrentPage++));
        }

        public static PaginatorState
            GoLeft
            <TRes>(
            this PaginatorState paginatorState)
        {
            return Init()(paginatorState.With(z => z.CurrentPage--));
        }

        public static PaginatorState
            GoRightMore
            <TRes>(
            this PaginatorState paginatorState)
        {
            return Init()(paginatorState.With(z => z.CurrentPage += paginatorState.PagesToSkip));
        }

        public static PaginatorState
            GoLeftMore
            <TRes>(
            this PaginatorState paginatorState)
        {
            return Init()(paginatorState.With(z => z.CurrentPage -= paginatorState.PagesToSkip));
        }

        public static Func<PaginatorState, PaginatorState>
            PagesToShow() => (paginatorState) =>
                {
                    return paginatorState.With(z => z.PagesToShow = () => GetPages()(paginatorState));
                };

        public static Func<PaginatorState, IEnumerable<int>>
            GetPages()
        {
            return GetItemsToShow(
                     GetLeftIndex(),
                     RightIndex,
                     GetDataStartEndIndex());
        }

        private static Func<PaginatorState, IEnumerable<int>>
            GetItemsToShow(
            Func<int, int, int> getStartIndex,
            Func<int, int, int> getEndIndex,
            Func<int, int, IEnumerable<int>, IEnumerable<int>> GetDataStartEndIndex
            )
        {
            return (paginatorState) =>
            {
                // how to handle the error (currentPage < 1) ???  //some Options !!!
                var startIndex = getStartIndex.Curry(paginatorState.CurrentPage)(paginatorState.ItemsPerPage);
                var endIndex = getEndIndex.Curry(paginatorState.CurrentPage)(paginatorState.ItemsPerPage);

                return GetDataStartEndIndex(startIndex, endIndex, paginatorState.DbData);
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
           IsValidItemsPerPage() => (paginatorState) =>
                 {
                     return false;
                 };

        private static Func<PaginatorState, bool>
            IsValidPagesToSkip() => (paginatorState) =>
                 {
                     return false;
                 };
    }
}
