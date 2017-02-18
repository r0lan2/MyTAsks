using System;
using System.Linq.Expressions;

namespace BigLamp.Extensions.Reflection
{
    public static class ReflectionExtensions
    {
        public static string GetPropertyName<T, TProperty>(this T owner, Expression<Func<T, TProperty>> expression) 
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                var unaryExpression = expression.Body as UnaryExpression;
                if (unaryExpression != null)
                {
                    memberExpression = unaryExpression.Operand as MemberExpression;
                    if (memberExpression == null)
                        throw new NotImplementedException();
                }
                else
                    throw new NotImplementedException();
            }

            var propertyName = memberExpression.Member.Name;
            return propertyName;
        }
        
        public static string GetMethodName<T>(this T owner, Expression<Action<T>> expression)
        {
            return GetMethodName(expression);
        }
        public static string GetMethodName<T>(Expression<Action<T>> expression)
        {
            if (expression.NodeType != ExpressionType.Lambda || expression.Body.NodeType != ExpressionType.Call)
                return null;
            var methodCallExp = (MethodCallExpression)expression.Body;
            return methodCallExp.Method.Name;
        }
    }
}
