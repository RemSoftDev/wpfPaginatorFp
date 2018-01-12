﻿using Func.Types;
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
                IEnumerable<int> pDbData
                )
        {
            var res = new PaginatorState();
            res.CurrentPage = pCurrentPage;
            res.ItemsPerPage = pItemsPerPage;
            res.PagesToSkip = pPagesToSkip;
            res.DbData = pDbData;

            return NextState(res);
        }

        private static PaginatorState
            NextState(PaginatorState pPaginatorState)
        {
            var res = pPaginatorState.
                      PipeForward(GetTotalNumberOfItemsInDB()).
                      PipeForward(GetNumberOfPages()).
                      PipeForward(IsValidLeft()).
                      PipeForward(IsValidLeftMore()).
                      PipeForward(IsValidRight()).
                      PipeForward(IsValidRightMore()).
                      PipeForward(PagesToShow());

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

        private static Func<PaginatorState, PaginatorState>
            PagesToShow() => (paginatorState) =>
                {
                    return paginatorState.With(z => z.PagesToShow = () => GetPages()(paginatorState));
                };

        private static Func<PaginatorState, IEnumerable<int>>
            GetPages()
        {
            return GetItemsToShow(
                     GetLeftIndex(),
                     RightIndex,
                     GetDataStartEndIndex());
        }

        private static Func<PaginatorState, IEnumerable<int>>
            GetItemsToShow(
            Func<IntMore0Less65535Exclsv, IntMore0Less65535Exclsv, uint> getStartIndex,
            Func<IntMore0Less65535Exclsv, IntMore0Less65535Exclsv, uint> getEndIndex,
            Func<uint, uint, IEnumerable<int>, IEnumerable<int>> GetDataStartEndIndex
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

        private static Func<IntMore0Less65535Exclsv, IntMore0Less65535Exclsv, uint>
            GetLeftIndex() => (CurrentPage, ItemsPerPage) =>
                {
                    return (uint)((CurrentPage.Value - 1) * ItemsPerPage.Value);
                };

        private static Func<IntMore0Less65535Exclsv, IntMore0Less65535Exclsv, uint>
            RightIndex => (CurrentPage, ItemsPerPage) =>
                {
                    return (uint)(CurrentPage.Value * ItemsPerPage.Value);
                };

        private static Func<uint, uint, IEnumerable<int>, IEnumerable<int>>
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
                    var res = (int)Math.Round((double)paginatorState.TotalNumberOfItemsInDB / paginatorState.ItemsPerPage.Value, 0, MidpointRounding.AwayFromZero);
                    return paginatorState.With(z => z.NumberOfPages = res);
                };

        private static Func<PaginatorState, PaginatorState>
            IsValidLeft() => (paginatorState) =>
                {
                    return paginatorState.With(z => z.IsValidLeft = paginatorState.CurrentPage.Value > 1);
                };

        private static Func<PaginatorState, PaginatorState>
            IsValidLeftMore() => (paginatorState) =>
                 {
                     return paginatorState.With(z => z.IsValidLeftMore = paginatorState.CurrentPage.Value - paginatorState.PagesToSkip.Value > 1);
                 };

        private static Func<PaginatorState, PaginatorState>
             IsValidRight() => (paginatorState) =>
                 {
                     return paginatorState.With(z => z.IsValidRight = paginatorState.CurrentPage.Value < paginatorState.NumberOfPages);
                 };

        private static Func<PaginatorState, PaginatorState>
             IsValidRightMore() => (paginatorState) =>
                 {
                     return paginatorState.With(z => z.IsValidRightMore = paginatorState.CurrentPage.Value + paginatorState.PagesToSkip.Value < paginatorState.NumberOfPages);
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
