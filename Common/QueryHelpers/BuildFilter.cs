using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.QueryHelpers
{
    public static class BuildFilter
    {
        #region "Build filter expression methods"
        public static Expression<Func<T, bool>> BuildWhereClause<T>(string propName, string opr, string value, Expression<Func<T, bool>> expr = null)
        {
            Expression<Func<T, bool>> func = null;
            try
            {
                Type t = typeof(T);
                var prop = t.GetProperty(propName);
                ParameterExpression tpe = Expression.Parameter(typeof(T));
                Expression left = Expression.Property(tpe, prop);
                if (prop.PropertyType.FullName.Contains("Int") || prop.PropertyType.FullName.Contains("Decimal"))//If int or decimal and only one digit then make sure its a number and not - or +  etc
                {
                    if (value.Length == 1)
                    {
                        Regex regex = new Regex(@"^\d$");
                        if (!regex.IsMatch(value)) //not numeric then set value to empty
                        {
                            value = "0";
                        }
                    }
                }
                Expression right = Expression.Convert(ToExprConstant(prop, value), prop.PropertyType);
                Expression<Func<T, bool>> innerExpr = Expression.Lambda<Func<T, bool>>(ApplyFilter(opr, left, right), tpe);
                               
                if (expr != null)
                {
                    func = AndAlso(expr, innerExpr);
                    //innerExpr = innerExpr.And(expr);
                }
				else
				{
                    func = innerExpr;
				}

            }
            catch(Exception ex)
            {
                throw ex;
            }
            return func;
        }
        //private static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> expr1,Expression<Func<T, bool>> expr2)
        //{
        //    // need to detect whether they use the same
        //    // parameter instance; if not, they need fixing
        //    ParameterExpression param = expr1.Parameters[0];
        //    if (ReferenceEquals(param, expr2.Parameters[0]))
        //    {
        //        // simple version
        //        return Expression.Lambda<Func<T, bool>>(
        //            Expression.AndAlso(expr1.Body, expr2.Body), param);
        //    }
        //    // otherwise, keep expr1 "as is" and invoke expr2
        //    return Expression.Lambda<Func<T, bool>>(
        //        Expression.AndAlso(
        //            expr1.Body,
        //            Expression.Invoke(expr2, param)), param);
        //}
        public static Expression<Func<T, bool>> AndAlso<T>(
        this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(left, right), parameter);
        }



        private class ReplaceExpressionVisitor
            : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression Visit(Expression node)
            {
                if (node == _oldValue)
                    return _newValue;
                return base.Visit(node);
            }
        }
        private static Expression ToExprConstant(PropertyInfo prop, string value)
        {
           
            object val = null;
            if(prop.PropertyType.FullName.Contains("Int"))
            {
                val = Convert.ChangeType(value, Type.GetType("System.Int32"));
                return Expression.Constant(val);
            }
            if (prop.PropertyType.FullName.Contains("DateTime"))
            {
                val = Convert.ChangeType(value, Type.GetType("System.DateTime"));
                return Expression.Constant(val);
            }
            if (prop.PropertyType.FullName.Contains("Decimal"))
            {
                val = Convert.ChangeType(value, Type.GetType("System.Decimal"));
                return Expression.Constant(val);
            }
            if (prop.PropertyType.FullName.Contains("Boolean"))
            {
                val = Convert.ChangeType(value, Type.GetType("System.Boolean"));
                return Expression.Constant(val);
            }
            if (prop.PropertyType.FullName.Contains("String"))
            {
                val = Convert.ChangeType(value, Type.GetType("System.String"));
                return Expression.Constant(val);
            }
            return Expression.Constant(val);
        }
        private static Expression ApplyFilter(string opr, Expression left, Expression right)
        {
            Expression InnerLambda = null;
            switch (opr)
            {
                case "==":
                case "=":
                    InnerLambda = Expression.Equal(left, right);
                    break;
                case "<":
                    InnerLambda = Expression.LessThan(left, right);
                    break;
                case ">":
                    InnerLambda = Expression.GreaterThan(left, right);
                    break;
                case ">=":
                    InnerLambda = Expression.GreaterThanOrEqual(left, right);
                    break;
                case "<=":
                    InnerLambda = Expression.LessThanOrEqual(left, right);
                    break;
                case "!=":
                    InnerLambda = Expression.NotEqual(left, right);
                    break;
                case "&&":
                    InnerLambda = Expression.And(left, right);
                    break;
                case "||":
                    InnerLambda = Expression.Or(left, right);
                    break;
                case "like":
                    var method = typeof(string).GetMethod("Contains", new[] { typeof(string)});
                    InnerLambda = Expression.Call(left, method, right);
                    break;
                case "StartsWith":
                    var method1 = typeof(string).GetMethod("StartsWith", new[] { typeof(string)});
                    InnerLambda = Expression.Call(left, method1,right);
                    break;
                case "EndsWith":
                    var method2 = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                    InnerLambda = Expression.Call(left, method2, right);
                    break;

            }
            return InnerLambda;
        }
        public static Expression<Func<T, TResult>> And<T, TResult>(this Expression<Func<T, TResult>> expr1, Expression<Func<T, TResult>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, TResult>>(Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }
        public static Func<T, TResult> ExpressionToFunc<T, TResult>(this Expression<Func<T, TResult>> expr)
        {
            return expr.Compile();
        }
        #endregion

        #region "Build Order By expression methods"
        private static LambdaExpression GenerateSelector<TEntity>(String propertyName, out Type resultType) where TEntity : class
        {
            // Create a parameter to pass into the Lambda expression (Entity => Entity.OrderByField).
            var parameter = Expression.Parameter(typeof(TEntity), "Entity");
            //  create the selector part, but support child properties
            PropertyInfo property;
            Expression propertyAccess;
            if (propertyName.Contains('.'))
            {
                // support to be sorted on child fields.
                String[] childProperties = propertyName.Split('.');
                property = typeof(TEntity).GetProperty(childProperties[0]);
                propertyAccess = Expression.MakeMemberAccess(parameter, property);
                for (int i = 1; i < childProperties.Length; i++)
                {
                    property = property.PropertyType.GetProperty(childProperties[i]);
                    propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
                }
            }
            else
            {
                property = typeof(TEntity).GetProperty(propertyName);
                propertyAccess = Expression.MakeMemberAccess(parameter, property);
            }
            resultType = property.PropertyType;
            // Create the order by expression.
            return Expression.Lambda(propertyAccess, parameter);
        }

        private static MethodCallExpression GenerateMethodCall<TEntity>(IQueryable<TEntity> source, string methodName, String fieldName) where TEntity : class
        {
            Type type = typeof(TEntity);
            Type selectorResultType;
            LambdaExpression selector = GenerateSelector<TEntity>(fieldName, out selectorResultType);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), methodName,
                                            new Type[] { type, selectorResultType },
                                            source.Expression, Expression.Quote(selector));
            return resultExp;
        }
        #endregion
    }
}
