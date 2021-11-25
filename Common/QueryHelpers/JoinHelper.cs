using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Common 
{
    /// <summary>
    /// List of JoinQuery class used to pass a list of joins to the entity framework  
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TInner">The type of the inner.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public class JoinList<T, TInner, TKey, TResult>
    {
        List<JoinQuery<T, TInner, TKey, TResult>> joins = new List<JoinQuery<T, TInner, TKey, TResult>>();
        public void AddJoin(JoinQuery<T,TInner,TKey,TResult> join)
        {
            joins.Add(join);
        }

        public List<JoinQuery<T, TInner, TKey, TResult>> Joins 
        {
            get
            { return joins;}
            set
            { joins = value;}
        } 
    }
    /// <summary>
    /// Represents a join expression for entity framework query 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TInner">The type of the inner.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public class JoinQuery<T, TInner, TKey, TResult>
    {
        public JoinQuery(IEnumerable<TInner> inner,Expression<Func<T,TKey>> outerKeyResults,Expression<Func<TInner,TKey>> innerKeyResults,Expression<Func<T, TInner, TResult>> resultSelector)
        {
            Inner = inner;
            OuterKeyResults = outerKeyResults;
            InnerKeyResults = innerKeyResults;
            ResultSelector = resultSelector;
        }

        public  IEnumerable<TInner> Inner { get; set; } 
        public  Expression<Func<T,TKey>> OuterKeyResults { get; set; }
        public  Expression<Func<TInner,TKey>> InnerKeyResults { get; set; }
        public  Expression<Func<T, TInner, TResult>> ResultSelector { get; set; }
    }
}
