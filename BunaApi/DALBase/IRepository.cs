using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.Objects.DataClasses;


namespace DALBase
{
    /// <summary>
    /// Data access Interface For any ORM you use you need to implement this interface
    /// Author: Robin Cyrolies
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Gets the uo W.
        /// </summary>
        /// <value>The uo W.</value>
        IUnitOfWork UoW { get; }

        #region Get Entity Methods

        T Get<T>(List<Expression<Func<T, object>>> includepaths, Expression<Func<T, bool>> whereclause) where T : class;

        T Get<T>(Expression<Func<T, bool>> whereclause) where T : class;
        
        #endregion

        #region GetList Methods (IQueryable<T>)

        IQueryable<T> GetList<T, TKey>(List<Expression<Func<T, object>>> includepaths, Expression<Func<T, bool>> whereclause, Expression<Func<T, TKey>> orderBy, bool isReadUnCommitted = false) where T : class;

        IQueryable<T> GetList<T, TKey>(List<Expression<Func<T, object>>> includepaths, Expression<Func<T, bool>> whereclause, bool isReadUnCommitted = false) where T : class;

        IQueryable<T> GetList<T, TKey>(List<Expression<Func<T, object>>> includepaths, Expression<Func<T, TKey>> orderBy, bool isReadUnCommitted = false) where T : class;

        IQueryable<T> GetList<T, TKey>(List<Expression<Func<T, object>>> includepaths, bool isReadUnCommitted = false) where T : class;

        IQueryable<T> GetList<T, TKey>(Expression<Func<T, bool>> whereclause, Expression<Func<T, TKey>> orderBy, bool isReadUnCommitted = false) where T : class;

        IQueryable<T> GetList<T, TKey>(Expression<Func<T, TKey>> orderBy, bool isReadUnCommitted = false) where T : class;

        IQueryable<T> GetList<T>(Expression<Func<T, bool>> whereclause, bool isReadUnCommitted = false) where T : class;
                
        IQueryable<T> GetList<T>(bool isReadUnCommitted = false) where T : class;

        IQueryable<T> ExecuteLinqQuery<T>(string sqlQuery, object[] parameters) where T : new();

        int ExecuteLinqSqlQuery(string sqlQuery, object[] parameters);

        #endregion

        #region GetList Select Methods (IQueryable<TKey>)

        IQueryable<TKey> GetListWithSelect<T, TKey>(Expression<Func<T, TKey>> resultSelector, List<Expression<Func<T, object>>> includepaths, Expression<Func<T, bool>> whereclause) where T : class;

          //TODO Robin get this working JionList class is in Common project 
          // IQueryable<TKey> GetListWithSelect<T, TInner, TKey, TResult>(JoinList<T, TInner, TKey, TResult> Joinlist, Expression<Func<T, TKey>> resultSelector, Expression<Func<T, bool>> whereclause) where T : class;

        #endregion

        #region GetList SelectMany Methods (IQueryable<TKey>)
        IEnumerable<TKey> GetListSelectMany<T, TKey>(Expression<Func<T, bool>> whereclause, Expression<Func<T, IEnumerable<TKey>>> selector) where T : class;

        IEnumerable<TResult> GetListSelectMany<T, TKey, TResult>(Expression<Func<T, bool>> whereclause,
           Expression<Func<T, IEnumerable<TKey>>> collectionselector, Expression<Func<T, TKey, TResult>> resultSelector) where T : class;
        #endregion

        #region Action methods

        int AddEntity<T>(T entity) where T : class;
       
        int UpdateEntity<T>(T entity) where T : class;
       
        int DeleteEntity<T>(T entity) where T : class;

        #endregion

    }
}
