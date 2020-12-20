using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using ServiceStack;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// Class which parses lambda expression to Filter class
    /// </summary>
    public class WebEasFilterExpression
    {
        public static Filter DecodeFilter(Expression exp)
        {
            var sql = new WebEasFilterExpression();
            var filter = sql.Visit(exp);
            if (filter == null) return null;
            if (filter is FilterElement) return new Filter((FilterElement)filter);
            //otherwise
            return filter as Filter;
        }

        private WebEasFilterExpression() { }

        object Visit(Expression exp)
        {
            if (exp == null)
            {
                return string.Empty;
            }
            switch (exp.NodeType)
            {
            case ExpressionType.Add:
            case ExpressionType.AddChecked:
            case ExpressionType.And:
            case ExpressionType.AndAlso:
            case ExpressionType.ArrayIndex:
            case ExpressionType.Coalesce:
            case ExpressionType.Divide:
            case ExpressionType.Equal:
            case ExpressionType.ExclusiveOr:
            case ExpressionType.GreaterThan:
            case ExpressionType.GreaterThanOrEqual:
            case ExpressionType.LeftShift:
            case ExpressionType.LessThan:
            case ExpressionType.LessThanOrEqual:
            case ExpressionType.Modulo:
            case ExpressionType.Multiply:
            case ExpressionType.MultiplyChecked:
            case ExpressionType.NotEqual:
            case ExpressionType.Or:
            case ExpressionType.OrElse:
            case ExpressionType.RightShift:
            case ExpressionType.Subtract:
            case ExpressionType.SubtractChecked:
                return this.VisitBinary(exp as BinaryExpression);
            case ExpressionType.ArrayLength:
            case ExpressionType.Convert:
            case ExpressionType.ConvertChecked:
            case ExpressionType.Negate:
            case ExpressionType.NegateChecked:
            case ExpressionType.Not:
            case ExpressionType.Quote:
            case ExpressionType.TypeAs:
                return this.VisitUnary(exp as UnaryExpression);
            case ExpressionType.Call:
                return this.VisitMethodCall(exp as MethodCallExpression);
            case ExpressionType.Constant:
                return this.VisitConstant(exp as ConstantExpression);
            case ExpressionType.Lambda:
                return this.VisitLambda(exp as LambdaExpression);
            case ExpressionType.MemberAccess:
                return this.VisitMemberAccess(exp as MemberExpression);
            case ExpressionType.MemberInit:
                return this.VisitMemberInit(exp as MemberInitExpression);
            case ExpressionType.New:
                return this.VisitNew(exp as NewExpression);
            case ExpressionType.NewArrayInit:
            case ExpressionType.NewArrayBounds:
                return this.VisitNewArray(exp as NewArrayExpression);
            case ExpressionType.Parameter:
                return this.VisitParameter(exp as ParameterExpression);
            }
            return exp.ToString();
        }
        object VisitLambda(LambdaExpression lambda)
        {
            if (lambda.Body.NodeType == ExpressionType.MemberAccess)
            {
                MemberExpression memberExpression = lambda.Body as MemberExpression;
                if (memberExpression.Expression != null)
                {
                    string arg = this.VisitMemberAccess(memberExpression).ToString();
                    return new FilterElement(arg, FilterOperator.Eq, true);
                }
            }
            return this.Visit(lambda.Body);
        }
        object VisitBinary(BinaryExpression b)
        {
            string text = this.BindOperant(b.NodeType);
            object obj;
            object obj2;
            if (text == "AND" || text == "OR")
            {
                MemberExpression memberExpression = b.Left as MemberExpression;
                if (memberExpression != null && memberExpression.Expression != null && memberExpression.Expression.NodeType == ExpressionType.Parameter)
                {
                    obj = new FilterElement(this.VisitMemberAccess(memberExpression).ToString(), FilterOperator.Eq, true);
                }
                else
                {
                    obj = this.Visit(b.Left);
                }
                memberExpression = (b.Right as MemberExpression);
                if (memberExpression != null && memberExpression.Expression != null && memberExpression.Expression.NodeType == ExpressionType.Parameter)
                {
                    obj2 = new FilterElement(this.VisitMemberAccess(memberExpression).ToString(), FilterOperator.Eq, true);
                }
                else
                {
                    obj2 = this.Visit(b.Right);
                }
                if (!(obj is IFilterElement) && !(obj2 is IFilterElement))
                {
                    return Expression.Lambda(b, new ParameterExpression[0]).Compile().DynamicInvoke(new object[0]);
                }
                return text == "AND"
                    ? Filter.AndElements((IFilterElement)obj, (IFilterElement)obj2)
                    : Filter.OrElements((IFilterElement)obj, (IFilterElement)obj2);
            }
            else
            {
                obj = this.Visit(b.Left);
                obj2 = this.Visit(b.Right);
            }

            string a;
            if ((a = text) != null && (a == "MOD" || a == "COALESCE"))
            {
                return FilterElement.Custom("{0}({1},{2})", text, obj, obj2);
            }
            return new FilterElement(obj.ToString(), new FilterOperator(text), obj2);
        }
        object VisitMemberAccess(MemberExpression m)
        {
            if (m.Expression == null || (m.Expression.NodeType != ExpressionType.Parameter && m.Expression.NodeType != ExpressionType.Convert))
            {
                UnaryExpression body = Expression.Convert(m, typeof(object));
                Expression<Func<object>> expression = Expression.Lambda<Func<object>>(body, new ParameterExpression[0]);
                Func<object> func = expression.Compile();
                return func();
            }

            if (m.Expression.NodeType == ExpressionType.Convert)
            {
                UnaryExpression unaryExpression = m.Expression as UnaryExpression;
                if (unaryExpression != null)
                {
                    return VisitUnary(unaryExpression);
                }
            }

            PropertyInfo member = (PropertyInfo)m.Member;
            if (member.HasAttribute<AliasAttribute>())
            {
                return member.GetCustomAttribute<AliasAttribute>().Name;
            }
            return member.Name;
        }
        object VisitMemberInit(MemberInitExpression exp)
        {
            return Expression.Lambda(exp, new ParameterExpression[0]).Compile().DynamicInvoke(new object[0]);
        }
        object VisitNew(NewExpression nex)
        {
            UnaryExpression body = Expression.Convert(nex, typeof(object));
            Expression<Func<object>> expression = Expression.Lambda<Func<object>>(body, new ParameterExpression[0]);
            object result;
            try
            {
                Func<object> func = expression.Compile();
                result = func();
            }
            catch (InvalidOperationException)
            {
                List<object> list = this.VisitExpressionList(nex.Arguments);
                StringBuilder stringBuilder = new StringBuilder();
                foreach (object current in list)
                {
                    if (stringBuilder.Length > 0)
                    {
                        stringBuilder.Append(",");
                    }
                    stringBuilder.Append(current);
                }
                result = stringBuilder.ToString();
            }
            return result;
        }
        object VisitParameter(ParameterExpression p)
        {
            return p.Name;
        }
        object VisitConstant(ConstantExpression c)
        {
            //if (c.Value == null)
            //{
            //	return new PartialSqlString("null");
            //}
            return c.Value;
        }
        object VisitUnary(UnaryExpression u)
        {
            ExpressionType nodeType = u.NodeType;
            if (nodeType != ExpressionType.Convert)
            {
                if (nodeType == ExpressionType.Not)
                {
                    object obj = this.Visit(u.Operand);
                    if (!(obj is IFilterElement))
                    {
                        return !(bool)obj;
                    }
                    return FilterElement.Custom("NOT (" + obj + ")");
                }
            }
            else
            {
                if (u.Method != null)
                {
                    return Expression.Lambda(u, new ParameterExpression[0]).Compile().DynamicInvoke(new object[0]);
                }
            }
            return this.Visit(u.Operand);
        }
        object VisitMethodCall(MethodCallExpression m)
        {
            if (this.IsStaticArrayMethod(m))
            {
                return this.VisitStaticArrayMethodCall(m);
            }
            if (this.IsEnumerableMethod(m))
            {
                return this.VisitEnumerableMethodCall(m);
            }
            return Expression.Lambda(m, new ParameterExpression[0]).Compile().DynamicInvoke(new object[0]);
        }
        List<object> VisitExpressionList(ReadOnlyCollection<Expression> original)
        {
            List<object> list = new List<object>();
            int i = 0;
            int count = original.Count;
            while (i < count)
            {
                if (original[i].NodeType == ExpressionType.NewArrayInit || original[i].NodeType == ExpressionType.NewArrayBounds)
                {
                    list.AddRange(this.VisitNewArrayFromExpressionList(original[i] as NewArrayExpression));
                }
                else
                {
                    list.Insert(0, this.Visit(original[i]));
                }
                i++;
            }
            return list;
        }
        object VisitNewArray(NewArrayExpression na)
        {
            List<object> list = this.VisitExpressionList(na.Expressions);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (object current in list)
            {
                stringBuilder.Append((stringBuilder.Length > 0) ? ("," + current) : current);
            }
            return stringBuilder.ToString();
        }
        List<object> VisitNewArrayFromExpressionList(NewArrayExpression na)
        {
            return this.VisitExpressionList(na.Expressions);
        }
        string BindOperant(ExpressionType e)
        {
            if (e <= ExpressionType.Coalesce)
            {
                if (e == ExpressionType.Add)
                {
                    return "+";
                }
                if (e == ExpressionType.AndAlso)
                {
                    return "AND";
                }
                if (e == ExpressionType.Coalesce)
                {
                    return "COALESCE";
                }
            }
            else
            {
                switch (e)
                {
                case ExpressionType.Divide:
                    return "/";
                case ExpressionType.Equal:
                    return "=";
                case ExpressionType.ExclusiveOr:
                case ExpressionType.Invoke:
                case ExpressionType.Lambda:
                case ExpressionType.LeftShift:
                case ExpressionType.ListInit:
                case ExpressionType.MemberAccess:
                case ExpressionType.MemberInit:
                    break;
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.Modulo:
                    return "MOD";
                case ExpressionType.Multiply:
                    return "*";
                default:
                    switch (e)
                    {
                    case ExpressionType.NotEqual:
                        return "<>";
                    case ExpressionType.Or:
                        break;
                    case ExpressionType.OrElse:
                        return "OR";
                    default:
                        if (e == ExpressionType.Subtract)
                        {
                            return "-";
                        }
                        break;
                    }
                    break;
                }
            }
            return e.ToString();
        }
        private bool IsStaticArrayMethod(MethodCallExpression m)
        {
            return m.Object == null && m.Method.Name == "Contains" && m.Arguments.Count == 2;
        }
        object VisitStaticArrayMethodCall(MethodCallExpression m)
        {
            string name;
            if ((name = m.Method.Name) != null && name == "Contains")
            {
                List<object> list = this.VisitExpressionList(m.Arguments);
                object colName = list[0];
                Expression expression = m.Arguments[0];
                if (expression.NodeType == ExpressionType.MemberAccess)
                {
                    expression = (m.Arguments[0] as MemberExpression);
                }
                return this.ToInPartialString(expression, colName);
            }
            throw new NotSupportedException();
        }
        bool IsEnumerableMethod(MethodCallExpression m)
        {
            return m.Object != null && m.Object.Type.IsOrHasGenericInterfaceTypeOf(typeof(IEnumerable<>)) && m.Object.Type != typeof(string) && m.Method.Name == "Contains" && m.Arguments.Count == 1;
        }
        object VisitEnumerableMethodCall(MethodCallExpression m)
        {
            string name;
            if ((name = m.Method.Name) != null && name == "Contains")
            {
                List<object> list = this.VisitExpressionList(m.Arguments);
                object quotedColName = list[0];
                return this.ToInPartialString(m.Object, quotedColName);
            }
            throw new NotSupportedException();
        }
        object ToInPartialString(Expression memberExpr, object colName)
        {
            UnaryExpression body = Expression.Convert(memberExpr, typeof(object));
            Expression<Func<object>> expression = Expression.Lambda<Func<object>>(body, new ParameterExpression[0]);
            Func<object> func = expression.Compile();
            List<object> list = Flatten(func() as IEnumerable);
            StringBuilder stringBuilder = new StringBuilder();
            if (list.Count > 0)
            {
                using (List<object>.Enumerator enumerator = list.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        object current = enumerator.Current;
                        if (stringBuilder.Length > 0)
                        {
                            stringBuilder.Append(",");
                        }
                        stringBuilder.Append(GetQuotedValue(current));
                    }
                }
            }
            else stringBuilder.Append("NULL");

            return FilterElement.Custom("[{0}] IN ({1})", colName, stringBuilder);
        }
        static List<object> Flatten(IEnumerable list)
        {
            List<object> list2 = new List<object>();
            if (list == null)
            {
                return list2;
            }
            foreach (object current in list)
            {
                if (current != null)
                {
                    IEnumerable enumerable = current as IEnumerable;
                    if (enumerable != null && !(current is string))
                    {
                        list2.AddRange(enumerable.Cast<object>());
                    }
                    else
                    {
                        list2.Add(current);
                    }
                }
            }
            return list2;
        }
        static string GetQuotedValue(object value)
        {
            if (value == null)
            {
                return "NULL";
            }
            Type fieldType = value.GetType();
            if (fieldType == typeof(Guid))
            {
                Guid guid = (Guid)value;
                return string.Format("CAST('{0}' AS UNIQUEIDENTIFIER)", guid);
            }
            if (fieldType == typeof(DateTime))
            {
                return string.Format(CultureInfo.InvariantCulture, "'{0:yyyy-MM-dd HH:mm:ss}'", (DateTime)value);
            }
            if (fieldType == typeof(DateTimeOffset))
            {
                return string.Format(CultureInfo.InvariantCulture, "'{0:yyyy-MM-dd HH:mm:ss zzz}'", (DateTimeOffset)value);
            }
            if (fieldType == typeof(bool))
            {
                return (bool)value ? "1" : "0";
            }
            if (fieldType == typeof(string))
            {
                return string.Format("'{0}'", value.ToString().Replace("'", "''"));
            }
            if (fieldType == typeof(byte[]))
            {
                return "0x" + BitConverter.ToString((byte[])value).Replace("-", "");
            }
            //else
            switch (fieldType.GetTypeCode())
            {
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    if (fieldType.IsNumericType())
                    {
                        if (value is TimeSpan)
                        {
                            return ((TimeSpan)value).Ticks.ToString(CultureInfo.InvariantCulture);
                        }
                        return Convert.ChangeType(value, fieldType).ToString();
                    }
                    break;
                case TypeCode.Single:
                    return ((float)value).ToString(CultureInfo.InvariantCulture);
                case TypeCode.Double:
                    return ((double)value).ToString(CultureInfo.InvariantCulture);
                case TypeCode.Decimal:
                    return ((decimal)value).ToString(CultureInfo.InvariantCulture);
            }
            if (fieldType == typeof(TimeSpan))
            {
                return ((TimeSpan)value).Ticks.ToString(CultureInfo.InvariantCulture);
            }
            return "'" + value.ToString().Replace("'", "''") + "'";
        }
    }
}