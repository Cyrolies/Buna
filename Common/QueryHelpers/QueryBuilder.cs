using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Common
{
    public static class QueryBuilder<T, TInner, TKey, TResult> 
    {
        public static List<Expression<Func<T, bool>>> Whereclauses { get; set; }
        
        public static Expression<Func<T, TKey>> fieldselection { get; set; }

        public static Expression<Func<T, TKey>> OrderBy { get; set; }
        
        public static bool Distinct { get; set; }
    }
}
