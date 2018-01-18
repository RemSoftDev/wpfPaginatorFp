using Func.Types;
using System;
using System.Collections.Generic;

namespace Func
{
    public class PaginatorState
    {
        public PaginatorState()
        {
            NumberOfPages = new IntMore0Less65535Exclsv();
            CurrentPage = new IntMore0Less65535Exclsv();
            ItemsPerPage = new IntMore0Less65535Exclsv();
            PagesToSkip = new IntMore0Less65535Exclsv();          
        }

        public PaginatorState(PaginatorState old):this()
        {
            NumberOfPages = old.NumberOfPages;
            CurrentPage = old.CurrentPage;
            ItemsPerPage = old.ItemsPerPage;
            PagesToSkip = old.PagesToSkip;          
            TotalNumberOfItemsInDB = old.TotalNumberOfItemsInDB;

            IsValidLeft = old.IsValidLeft;
            IsValidLeftMore = old.IsValidLeftMore;
            IsValidRight = old.IsValidRight;
            IsValidRightMore = old.IsValidRightMore;

            PagesToShow = old.PagesToShow;
            PagesRight = old.PagesRight;
            PagesRightMore = old.PagesRightMore;
            PagesLeft = old.PagesLeft;
            PagesLeftMore = old.PagesLeftMore;
        }

        public PaginatorState(
            IntMore0Less65535Exclsv pCurrentPage,
            IntMore0Less65535Exclsv pItemsPerPage,
            IntMore0Less65535Exclsv pPagesToSkip,
            IntMore0Less65535Exclsv pNumberOfPages,

            int pTotalNumberOfItemsInDB,

            bool pIsValidLeft,
            bool pIsValidLeftMore,
            bool pIsValidRight,
            bool pIsValidRightMore,

            IEnumerable<int> pDbData,

            Func<IEnumerable<int>> pPagesToShow,
            Func<IEnumerable<int>> pPagesRight,
            Func<IEnumerable<int>> pPagesRightMore,
            Func<IEnumerable<int>> pPagesLeft,
            Func<IEnumerable<int>> pPagesLeftMore
            ):this()
        {
            NumberOfPages = pNumberOfPages;
            CurrentPage = pCurrentPage;
            ItemsPerPage = pItemsPerPage;
            PagesToSkip = pPagesToSkip;          
            TotalNumberOfItemsInDB = pTotalNumberOfItemsInDB;

            IsValidLeft = pIsValidLeft;
            IsValidLeftMore = pIsValidLeftMore;
            IsValidRight = pIsValidRight;
            IsValidRightMore = pIsValidRightMore;

            DbData = pDbData;

            PagesToShow = pPagesToShow;
            PagesRight = pPagesRight;
            PagesRightMore = pPagesRightMore;
            PagesLeft = pPagesLeft;
            PagesLeftMore = pPagesLeftMore;
        }

        private IntMore0Less65535Exclsv _CurrentPage;
        public IntMore0Less65535Exclsv CurrentPage
        {
            get
            {
                return _CurrentPage;
            }
            set
            {
                if (value.Value <= NumberOfPages.Value)
                {
                    _CurrentPage = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException($"You try to show page {value} of {NumberOfPages.Value}");
                }
            }
        }
        public IntMore0Less65535Exclsv ItemsPerPage;
        public IntMore0Less65535Exclsv PagesToSkip;
        public IntMore0Less65535Exclsv NumberOfPages;

        public int TotalNumberOfItemsInDB;

        public bool IsValidLeft;
        public bool IsValidLeftMore;
        public bool IsValidRight;
        public bool IsValidRightMore;

        public IEnumerable<int> DbData;

        public Func<IEnumerable<int>> PagesToShow;
        public Func<IEnumerable<int>> PagesRight;
        public Func<IEnumerable<int>> PagesRightMore;
        public Func<IEnumerable<int>> PagesLeft;
        public Func<IEnumerable<int>> PagesLeftMore;
    }
}
