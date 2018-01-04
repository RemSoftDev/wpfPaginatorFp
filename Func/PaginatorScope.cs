using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Func
{
    public static class PaginatorScope
    {
        public static Func<int, int, IEnumerable<int>, IEnumerable<int>> GetItemsToShow(
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

        public static Func<int, int, int> GetStartIndex()
        {
            return (CurrentPage, ItemsPerPage) =>
            {
                return (CurrentPage - 1) * ItemsPerPage;
            };
        }

        public static Func<int, int, int> GetEndIndex()
        {
            return (CurrentPage, ItemsPerPage) =>
            {
                return CurrentPage * ItemsPerPage;
            };
        }

        public static Func<int, int, IEnumerable<int>, IEnumerable<int>> GetDataStartEndIndex()
        {
            return (startIndex, endIndex, DbData) =>
            {
                return DbData.Where(z => z > startIndex && z <= endIndex);
            };
        }
    }
}
