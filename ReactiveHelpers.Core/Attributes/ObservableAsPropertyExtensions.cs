using System;
using System.Linq.Expressions;
using System.Reflection;
namespace ReactiveHelpers.Core
{
    public static class ObservableAsPropertyExtensions
    {

       /* static PropertyInfo GetPropertyInfo(this LambdaExpression expression)
        {
            var current = expression.Body;
            var unary = current as UnaryExpression;
            if (unary != null)
                current = unary.Operand;
            var call = (MemberExpression)current;
            return (PropertyInfo)call.Member;
        }*/
    }
}
