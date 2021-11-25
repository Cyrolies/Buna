using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;


namespace Common
{
    public static class QueryFilters
    {
        public static IEnumerable<T> Sort<T>(IEnumerable<T> source, string sortBy, string sortDirection) where T : class
        {
                var param = Expression.Parameter(typeof(T), "item");

                var parts = sortBy.Split('.');
                Expression parent = parts.Aggregate<string, Expression>(param, Expression.Property);
                Expression conversion = Expression.Convert(parent, typeof(object));

                var tryExpression = Expression.TryCatch(Expression.Block(typeof(object), conversion),
                                                        Expression.Catch(typeof(object), Expression.Constant(null)));

                var sortExpression = Expression.Lambda<Func<T, object>>(tryExpression, param);
                          
                switch (sortDirection)
                {
                    case "Ascending":
                        return source.AsQueryable<T>().OrderBy<T, object>(sortExpression);
                    default:
                        return source.AsQueryable<T>().OrderByDescending<T, object>(sortExpression);

                }
       }

        public static Expression<Func<TEntity, TResult>> GetExpression<TEntity, TResult>(string prop)
        {
            var param = Expression.Parameter(typeof(TEntity), "p");
            var parts = prop.Split('.');

            Expression parent = parts.Aggregate<string, Expression>(param, Expression.Property);
            Expression conversion = Expression.Convert(parent, typeof(object));

            var tryExpression = Expression.TryCatch(Expression.Block(typeof(object), conversion),
                                                    Expression.Catch(typeof(object), Expression.Constant(null)));

            return Expression.Lambda<Func<TEntity, TResult>>(tryExpression, param);
        }

        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string orderByProperty,
                  string orderByDirection) where TEntity : class
        {
            string command = orderByDirection.Equals("Ascending") ? "OrderBy" : "OrderByDescending";

            var type = typeof(TEntity);

            var property = type.GetProperty(orderByProperty);

            var parameter = Expression.Parameter(type, "p");

            var propertyAccess = Expression.MakeMemberAccess(parameter, property);

            var orderByExpression = Expression.Lambda(propertyAccess, parameter);

            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },

                                   source.Expression, Expression.Quote(orderByExpression));

            return source.Provider.CreateQuery<TEntity>(resultExpression);

        }

    }
}
