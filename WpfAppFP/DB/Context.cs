﻿using System.Collections.Generic;
using System.Linq;

namespace WpfAppFP.DB
{
    public class Context
    {
        public static IEnumerable<int> GetDataStartEndIndex(
            uint startIndexInclsv,
            uint endIndexInclsv,
            IEnumerable<int> DbData)
        {
            return DbData.Where(z => z >= startIndexInclsv && z <= endIndexInclsv);
        }

        public static int GetTotalNumberOfItemsInDB(
            IEnumerable<int> DbData)
        {
            return DbData.Count();
        }
    }
}
