using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
//using System.Data.Objects;
//using System.Data.Objects.DataClasses;
using System.Linq;
using System.Data.Linq;
using System.Linq.Expressions;
using DALBase;
using DALEFBase;

namespace DALEFBase
{
    /// <summary>
    /// This is the main class to be used for all data access by the business layer
    /// it has all the crud functionality etc.
    /// Author: Robin Cyrolies
    /// </summary>
    public class RepositoryBase : IRepository     
    {
        #region Members
        
        private DbContext dbcontext = null;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public RepositoryBase(IUnitOfWork unitOfWork)
        {
            UoW = unitOfWork;
            
        }

        /// <summary>
        /// Gets the uo W.
        /// </summary>
        /// <value>The uo W.</value>
        public IUnitOfWork UoW { get; private set; }

        /// <summary>
        /// Gets the object Context.
        /// </summary>
        /// <returns></returns>
        private DbContext GetObjectContext()
        {
            this.dbcontext = (DbContext)UoW.Orm;

            this.dbcontext.Configuration.ProxyCreationEnabled = false;
            this.dbcontext.Configuration.LazyLoadingEnabled = false;
            
            return dbcontext;
        }

        #endregion

        #region Get Entity Methods

        /// <summary>
        /// Gets the specified predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public virtual T Get<T>(List<Expression<Func<T, object>>> includepaths, Expression<Func<T, bool>> whereclause) where T : class
        {
           
                try
                {

                    DbSet<T> dbset = GetObjectContext().Set<T>();

                    if (includepaths != null && includepaths.Count > 0)
                    {
                        foreach (Expression<Func<T, object>> path in includepaths)
                        {
                            dbset.Include(path).Load();
                        }
                    }
                   return dbset.Where(whereclause).SingleOrDefault();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                
           
        }

        public virtual T Get<T>(Expression<Func<T, bool>> whereclause) where T : class
        {
                try
                {
                    DbSet<T> dbset = GetObjectContext().Set<T>();
                    return dbset.Where(whereclause).SingleOrDefault();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
  
        }
        
        #endregion

        #region GetList Select Methods (IQueryable<TKey>)

        /// <summary>
        /// Returns the list with only the fields specified in the resultSelector by using the Select method.
        /// Example: GetListWithSelect<StpData, object>(p => new {p.StpDataID,p.StpDataTypeID,p.DataCode,p.DataDescription,p.IsSystem}, p => p.StpDataTypeID == (int)typeID)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="fieldselection">The resultSelector.</param>
        /// <param name="whereclause">The whereclause.</param>
        /// <returns></returns>
        public virtual IQueryable<TKey> GetListWithSelect<T, TKey>(Expression<Func<T, TKey>> resultSelector, List<Expression<Func<T, object>>> includepaths, Expression<Func<T, bool>> whereclause) where T : class
        {
            try
            {
                DbSet<T> dbset = GetObjectContext().Set<T>();
                if (includepaths != null && includepaths.Count > 0)
                {
                    foreach (Expression<Func<T, object>> path in includepaths)
                    {
                        dbset.Include(path).Load();
                    }
                }
                if (whereclause != null)
                {
                    dbset.Where(whereclause);
                }
                
                return dbset.Select(resultSelector);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        /// <summary>
        /// METHOD NOT FUNCTIONAL
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TInner">The type of the inner.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="Joinlist">The joinlist.</param>
        /// <param name="resultSelector">The result selector.</param>
        /// <param name="whereclause">The whereclause.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
       // public virtual IQueryable<TKey> GetListWithSelect<T, TInner, TKey, TResult>(JoinList<T, TInner, TKey, TResult> Joinlist, Expression<Func<T, TKey>> resultSelector, Expression<Func<T, bool>> whereclause) where T : class
        //{
           
        //    //Example
        //    //var list = dc.Orders.Join(dc.Order_Details,o => o.OrderID, od => od.OrderID,(o, od) => new {OrderID = o.OrderID,OrderDate = o.OrderDate,Quantity = od.Quantity})
        //    //.Join(dc.Products,a => a.ProductID, p => p.ProductID,(a, p) => new {OrderID = a.OrderID,OrderDate = a.OrderDate,ProductName = p.ProductName});
        //    try
        //    {
        //        DbSet<T> dbset = GetObjectContext().Set<T>();
        //        if (whereclause != null)
        //        {
        //            dbset.Where(whereclause);
        //        }
        //       // DbSet<T> dbset = GetObjectContext().Set<T>();
        //        //foreach (JoinQuery<T, TInner, TKey, TResult> join in Joinlist.Joins)
        //        //{
        //        //    db.Join(join.Inner, join.OuterKeyResults, join.InnerKeyResults, join.ResultSelector);
        //        //}
        //        return dbset.Select(resultSelector);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
            
        //}
        #endregion

        #region GetList SelectMany Methods (IQueryable<TKey>)

        /// <summary>
        /// Returns the list with all fields by using SelectMany method.
        /// Example : returns all fields customersWithOrders.SelectMany(c => orders.Where(o => o.CustomerId == c.Id));
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="whereclause">The whereclause.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        public virtual IEnumerable<TKey> GetListSelectMany<T, TKey>(Expression<Func<T, bool>> whereclause, Expression<Func<T, IEnumerable<TKey>>> selector) where T : class
        {
            //Example
            //returns all fields customersWithOrders.SelectMany(c => orders.Where(o => o.CustomerId == c.Id));
            try
            {
                DbSet<T> dbset = GetObjectContext().Set<T>();
                if (whereclause != null)
                {
                    dbset.Where(whereclause);
                }
                return dbset.SelectMany(selector);
            }
            catch (Exception ex)
            {
               throw ex;
            }
            
        }


        /// <summary>
        /// Returns the list with only the fields specified in the resultSelector by using the SelectMany method.
        /// Example : returns customersWithOrderDescriptions.SelectMany(c => orders.Where(o => o.CustomerId == c.Id),(c, o) => new { CustomerId = c.Id, OrderDescription = o.Description });
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="whereclause">The whereclause.</param>
        /// <param name="collectionselector">The collectionselector.</param>
        /// <param name="resultSelector">The result selector.</param>
        /// <returns></returns>
        public virtual IEnumerable<TResult> GetListSelectMany<T, TKey, TResult>(Expression<Func<T, bool>> whereclause,
           Expression<Func<T, IEnumerable<TKey>>> collectionselector, Expression<Func<T, TKey, TResult>> resultSelector) where T : class
        {
            //Example
            //returns customersWithOrderDescriptions.SelectMany(c => orders.Where(o => o.CustomerId == c.Id),(c, o) => new { CustomerId = c.Id, OrderDescription = o.Description });

            try
            {
                DbSet<T> dbset = GetObjectContext().Set<T>();
                if (whereclause != null)
                {
                    dbset.Where(whereclause);
                }

                return dbset.SelectMany(collectionselector, resultSelector);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        
        #endregion
        
        #region GetList Methods (IQueryable<T>)
        
        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="includepaths">The includepaths.</param>
        /// <param name="whereclause">The whereclause.</param>
        /// <param name="orderBy">The order by.</param>
        /// <returns></returns>
        public virtual IQueryable<T> GetList<T, TKey>(List<Expression<Func<T, object>>> includepaths, Expression<Func<T, bool>> whereclause, Expression<Func<T, TKey>> orderBy, bool isReadUnCommitted = false) where T : class
        {
            try
            {
                if (isReadUnCommitted)
                {
                    GetObjectContext().Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED");
                }
                DbSet<T> dbset = GetObjectContext().Set<T>();
                
                if (includepaths != null && includepaths.Count > 0)
                {
                    foreach (Expression<Func<T, object>> path in includepaths)
                    {
                        dbset.Include(path).Load();
                    }
                }

                if (whereclause == null || orderBy == null)
                {
                    return dbset;
                }
                else
                {
                    return dbset.Where(whereclause).OrderBy(orderBy);
                }
             
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="includepaths">The includepaths.</param>
        /// <param name="whereclause">The whereclause.</param>
        /// <param name="isReadUnCommitted">if set to <c>true</c> [is read function committed].</param>
        /// <returns></returns>
        public virtual IQueryable<T> GetList<T, TKey>(List<Expression<Func<T, object>>> includepaths, Expression<Func<T, bool>> whereclause, bool isReadUnCommitted = false) where T : class
        {
            try
            {
                if (isReadUnCommitted)
                {
                    GetObjectContext().Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED");
                }
                DbSet<T> dbset = GetObjectContext().Set<T>();

                if (includepaths != null && includepaths.Count > 0)
                {
                    foreach (Expression<Func<T, object>> path in includepaths)
                    {
                        dbset.Include(path).Load();
                    }
                }
                if (whereclause != null)
                {
                    return dbset.Where(whereclause);
                }
				else
				{
                    return dbset;
				}

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="includepaths">The includepaths.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public virtual IQueryable<T> GetList<T, TKey>(List<Expression<Func<T, object>>> includepaths, Expression<Func<T, TKey>> orderBy, bool isReadUnCommitted = false) where T : class
        {
            try
            {
                if(isReadUnCommitted)
                {
                    GetObjectContext().Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED");
                }
                DbSet<T> dbset = GetObjectContext().Set<T>();

                if (includepaths != null && includepaths.Count > 0)
                {
                    foreach (Expression<Func<T, object>> path in includepaths)
                    {
                        dbset.Include(path).Load();
                    }
                }
                IOrderedQueryable<T> list;
                if (orderBy != null)
                {
                    list = dbset.OrderByDescending<T, TKey>(orderBy);
                }
				else
				{
                    list = dbset;
                }
                
                return list;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="includepaths">The includepaths.</param>
        /// <param name="isReadUnCommitted">if set to <c>true</c> [is read function committed].</param>
        /// <returns></returns>
        public virtual IQueryable<T> GetList<T, TKey>(List<Expression<Func<T, object>>> includepaths,bool isReadUnCommitted = false) where T : class
        {
            try
            {
                if (isReadUnCommitted)
                {
                    GetObjectContext().Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED");
                }
                DbSet<T> dbset = GetObjectContext().Set<T>();

                if (includepaths != null && includepaths.Count > 0)
                {
                    foreach (Expression<Func<T, object>> path in includepaths)
                    {
                        dbset.Include(path).Load();
                    }
                }
               
                return dbset;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
       
        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="orderBy">The order by.</param>
        /// <returns></returns>
        public virtual IQueryable<T> GetList<T, TKey>(Expression<Func<T, bool>> whereclause, Expression<Func<T, TKey>> orderBy, bool isReadUnCommitted = false) where T : class
        {
            try
            {
                if (whereclause == null ||orderBy == null)
                {
                    return this.GetList<T>(isReadUnCommitted);
                }
                if (isReadUnCommitted)
                {
                    GetObjectContext().Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED");
                }
                return GetObjectContext().Set<T>().Where(whereclause).OrderBy(orderBy);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
       
        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="orderBy">The order by.</param>
        /// <returns></returns>
        public virtual IQueryable<T> GetList<T, TKey>(Expression<Func<T, TKey>> orderBy, bool isReadUnCommitted = false) where T : class
        {
            try
            {
                if (isReadUnCommitted)
                {
                    GetObjectContext().Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED");
                }
                if (orderBy == null)
                {
                    return this.GetList<T>(isReadUnCommitted);
                }
                else
                {
                    return GetObjectContext().Set<T>().OrderBy(orderBy);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public virtual IQueryable<T> GetList<T>(Expression<Func<T, bool>> whereclause, bool isReadUnCommitted = false) where T : class
        {
            try
            {
               
                if (isReadUnCommitted)
                {
                    GetObjectContext().Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED");
                }
                if (whereclause == null)
                {
                    return this.GetList<T>(isReadUnCommitted);
                }
                else
                {
                    return GetObjectContext().Set<T>().Where(whereclause);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual IQueryable<T> GetList<T>(bool isReadUnCommitted = false) where T : class
        {
            try
            {
                if (isReadUnCommitted)
                {
                    GetObjectContext().Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED");
                }
               return GetObjectContext().Set<T>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        /// <summary>
        /// Executes the linq query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlQuery">The SQL query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public virtual IQueryable<T> ExecuteLinqQuery<T>(string sqlQuery, object[] parameters) where T : new()
        {
            return GetObjectContext().Database.SqlQuery<T>(sqlQuery, parameters).AsQueryable<T>();
        }

        public virtual IQueryable<T> ExecuteLinqQuery<T>(string sqlQuery) where T : new()
        {
            object[] parameters = { };
            return GetObjectContext().Database.SqlQuery<T>(sqlQuery, parameters).AsQueryable<T>();
        }

        public virtual int ExecuteLinqSqlQuery(string sqlQuery, object[] parameters)
        {
            return GetObjectContext().Database.ExecuteSqlCommand(sqlQuery, parameters);
        }

        //TODO add functionality to call SP
        //public OperationStatus ExecuteStoreCommand(string cmdText, params object[] parameters)
        //{
        //    var opStatus = new OperationStatus { Status = true };

        //    try
        //    {
        //        opStatus.RecordsAffected = GetObjectContext().ExecuteStoreCommand(cmdText, parameters);
        //    }
        //    catch (Exception exp)
        //    {
        //        throw exp;
        //    }
            
        //}
        
        #endregion
       
        #region Action methods

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public virtual int AddEntity<T>(T entity) where T : class
        {
             return this.UoW.Add(entity);
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public virtual int UpdateEntity<T>(T entity) where T : class
        {
            //TODO Robin add check for concurrency handling
           return this.UoW.Update(entity);
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public virtual int DeleteEntity<T>(T entity) where T : class
        {
            return this.UoW.Delete(entity);
        }

        //public virtual OperationStatus Update<T>(T entity, params string[] propsToUpdate) where T: class
        //{
        //    OperationStatus opStatus = new OperationStatus { Status = true };

        //    try
        //    {
        //        GetObjectContext().CreateObjectSet<T>().Attach(entity);
        //        var entry = GetObjectContext().ObjectStateManager.GetObjectStateEntry(entity);
        //        foreach (var propName in propsToUpdate)
        //        {
        //            entry.SetModifiedProperty(propName);
        //        }
        //        opStatus.Status = DataContext.SaveChanges() > 0;
        //    }
        //    catch (OptimisticConcurrencyException conexp)
        //    {
        //        opStatus = OperationStatus.CreateFromException("Error updating " + typeof(T) + ".", conexp);
        //    }
        //    catch (Exception exp)
        //    {
        //        opStatus = OperationStatus.CreateFromException("Error updating " + typeof(T) + ".", exp);
        //    }

        //    return opStatus;
        //}

        #endregion


    }
}
