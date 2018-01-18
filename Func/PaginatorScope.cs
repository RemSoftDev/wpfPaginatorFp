using Func.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Func
{
    public static class PaginatorScope
    {
        public static PaginatorState
            Init(
                IntMore0Less65535Exclsv pCurrentPage,
                IntMore0Less65535Exclsv pItemsPerPage,
                IntMore0Less65535Exclsv pPagesToSkip,
                int pTotalNumberOfItemsInDB
                )
        {
            var res = new PaginatorState();
            res.CurrentPage = pCurrentPage;
            res.ItemsPerPage = pItemsPerPage;
            res.PagesToSkip = pPagesToSkip;
            res.TotalNumberOfItemsInDB = pTotalNumberOfItemsInDB;

            return NextState(res);
        }

        private static PaginatorState
            NextState(PaginatorState pPaginatorState)
        {
            var res = pPaginatorState.
                      PipeForward(GetNumberOfPages()).
                      PipeForward(IsValidLeft()).
                      PipeForward(IsValidLeftMore()).
                      PipeForward(IsValidRight()).
                      PipeForward(IsValidRightMore());

            return res;
        }

        public static PaginatorState
            GoRight(
            this PaginatorState paginatorState)
        {
            return NextState(paginatorState.With(z => z.CurrentPage.Value++));
        }

        public static PaginatorState
            GoLeft(
            this PaginatorState paginatorState)
        {
            return NextState(paginatorState.With(z => z.CurrentPage.Value--));
        }

        public static PaginatorState
            GoRightMore(
            this PaginatorState paginatorState)
        {
            return NextState(paginatorState.With(z => z.CurrentPage.Value += paginatorState.PagesToSkip.Value));
        }

        public static PaginatorState
            GoLeftMore(
            this PaginatorState paginatorState)
        {
            return NextState(paginatorState.With(z => z.CurrentPage.Value -= paginatorState.PagesToSkip.Value));
        }

        private static Func<PaginatorState, uint>
            GetLeftIndex() => (paginatorState) =>
                {
                    return (uint)((paginatorState.CurrentPage.Value - 1) * paginatorState.ItemsPerPage.Value);
                };

        private static Func<PaginatorState, uint>
            RightIndex => (paginatorState) =>
                {
                    return (uint)(paginatorState.CurrentPage.Value * paginatorState.ItemsPerPage.Value);
                };

        public static Func<uint, uint, IEnumerable<int>, IEnumerable<int>>
            GetDataStartEndIndex() => (startIndex, endIndex, DbData) =>
                {
                    return DbData.Where(z => z > startIndex && z <= endIndex);
                };

        private static Func<PaginatorState, PaginatorState>
            GetNumberOfPages() => (paginatorState) =>
                {
                    var res = (ushort)Math.Round((double)paginatorState.TotalNumberOfItemsInDB / paginatorState.ItemsPerPage.Value, 0, MidpointRounding.AwayFromZero);
                    return paginatorState.With(z => z.NumberOfPages = new IntMore0Less65535Exclsv() { Value = res });
                };

        private static Func<PaginatorState, PaginatorState>
            IsValidLeft() => (paginatorState) =>
                {
                    return paginatorState.With(z => z.IsValidLeft = paginatorState.CurrentPage.Value > 1);
                };

        private static Func<PaginatorState, PaginatorState>
            IsValidLeftMore() => (paginatorState) =>
                 {
                     return paginatorState.With(z => z.IsValidLeftMore = paginatorState.CurrentPage.Value - paginatorState.PagesToSkip.Value > 0);
                 };

        private static Func<PaginatorState, PaginatorState>
             IsValidRight() => (paginatorState) =>
                 {
                     return paginatorState.With(z => z.IsValidRight = paginatorState.CurrentPage.Value < paginatorState.NumberOfPages.Value);
                 };

        private static Func<PaginatorState, PaginatorState>
             IsValidRightMore() => (paginatorState) =>
                 {
                     return paginatorState.With(z => z.IsValidRightMore = paginatorState.CurrentPage.Value + paginatorState.PagesToSkip.Value < paginatorState.NumberOfPages.Value);
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
