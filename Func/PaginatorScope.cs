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
            this PaginatorState pState)
        {
            return NextState(pState.With(z => z.CurrentPage.Value++));
        }

        public static PaginatorState
            GoLeft(
            this PaginatorState pState)
        {
            return NextState(pState.With(z => z.CurrentPage.Value--));
        }

        public static PaginatorState
            GoRightMore(
            this PaginatorState pState)
        {
            return NextState(pState.With(z => z.CurrentPage.Value += pState.PagesToSkip.Value));
        }

        public static PaginatorState
            GoLeftMore(
            this PaginatorState pState)
        {
            return NextState(pState.With(z => z.CurrentPage.Value -= pState.PagesToSkip.Value));
        }

        private static Func<PaginatorState, uint>
            GetLeftIndex() => (pState) =>
                {
                    return (uint)((pState.CurrentPage.Value - 1) * pState.ItemsPerPage.Value);
                };

        private static Func<PaginatorState, uint>
            RightIndex => (pState) =>
                {
                    return (uint)(pState.CurrentPage.Value * pState.ItemsPerPage.Value);
                };

        public static Func<uint, uint, IEnumerable<int>, IEnumerable<int>>
            GetDataStartEndIndex() => (startIndex, endIndex, DbData) =>
                {
                    return DbData.Where(z => z > startIndex && z <= endIndex);
                };

        private static Func<PaginatorState, PaginatorState>
            GetNumberOfPages() => (pState) =>
                {
                    var res = (ushort)Math.Round((double)pState.TotalNumberOfItemsInDB / pState.ItemsPerPage.Value, 0, MidpointRounding.AwayFromZero);
                    return pState.With(z => z.NumberOfPages = new IntMore0Less65535Exclsv() { Value = res });
                };

        private static Func<PaginatorState, PaginatorState>
            IsValidLeft() => (pState) =>
                {
                    return pState.With(z => z.IsValidLeft = pState.CurrentPage.Value > 1);
                };

        private static Func<PaginatorState, PaginatorState>
            IsValidLeftMore() => (pState) =>
                 {
                     return pState.With(z => z.IsValidLeftMore = pState.CurrentPage.Value - pState.PagesToSkip.Value > 0);
                 };

        private static Func<PaginatorState, PaginatorState>
             IsValidRight() => (pState) =>
                 {
                     return pState.With(z => z.IsValidRight = pState.CurrentPage.Value < pState.NumberOfPages.Value);
                 };

        private static Func<PaginatorState, PaginatorState>
             IsValidRightMore() => (pState) =>
                 {
                     return pState.With(z => z.IsValidRightMore = pState.CurrentPage.Value + pState.PagesToSkip.Value < pState.NumberOfPages.Value);
                 };

        private static Func<PaginatorState, bool>
           IsValidItemsPerPage() => (pState) =>
                 {
                     return false;
                 };

        private static Func<PaginatorState, bool>
            IsValidPagesToSkip() => (pState) =>
                 {
                     return false;
                 };
    }
}
