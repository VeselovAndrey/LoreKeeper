// -----------------------------------------------------------------------
// <copyright file="SqlClauseBuilder.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License:  Microsoft Public License (MS-PL)
// Contacts: http://andrey.moveax.com  andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("LoreKeeper.EF7.Tests")]

namespace LoreKeeper.EF7.AdvancedFeatures
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    internal class SqlClauseBuilder
    {
        public string Build(Expression expression)
        {
            return this.Build(expression, true);
        }

        private string Build(Expression expression, bool isExpression)
        {
            switch (expression.NodeType) {
                case ExpressionType.MemberAccess:
                    return isExpression ?
                        $"({this.GetMemberAccessExpressionString(expression)} = 1)" :
                        this.GetMemberAccessExpressionString(expression);

                case ExpressionType.Constant:
                    return this.GetConstantExpressionString(expression);

                case ExpressionType.Convert:
                    return this.GetConvertExpressionString(expression);

                case ExpressionType.Not:
                    return this.GetNotExpressionString(expression);

                case ExpressionType.Invoke:
                    return this.GetInvokationExpressionString(expression);

                default:
                    var operation = (BinaryExpression)expression;

                    var left = operation.Left;
                    string leftText = this.Build(left, false);

                    var right = operation.Right;
                    string rightText = this.Build(right, false);

                    return this.CombineValues(leftText, expression.NodeType, rightText);
            }
        }

        private string GetInvokationExpressionString(Expression expression)
        {
            var invocationExpression = expression as InvocationExpression;
            if (invocationExpression == null)
                throw new ArgumentException("expression is not InvocationExpression");

            var lambdaExpression = invocationExpression.Expression as LambdaExpression;
            if (lambdaExpression == null)
                throw new ArgumentException("expression is not contain LambdaExpression inside InvocationExpression");

            return this.Build(lambdaExpression.Body, true);
        }

        private string CombineValues(string leftText, ExpressionType expressionType, string rightText)
        {
            if (string.Equals(rightText, "null", StringComparison.Ordinal)) {
                if (expressionType == ExpressionType.Equal)
                    return $"({leftText} IS NULL)";

                if (expressionType == ExpressionType.NotEqual)
                    return $"({leftText} IS NOT NULL)";
            }

            return $"({leftText} {this.ToSqlOperator(expressionType)} {rightText})";
        }

        private string GetNotExpressionString(Expression expression)
        {
            var unaryExp = expression as UnaryExpression;
            if (unaryExp == null)
                throw new ArgumentException("expression is not UnaryExpression");

            var pattern = unaryExp.Operand.NodeType == ExpressionType.MemberAccess ?
                "({0} = 0)" : "(NOT {0})";

            return string.Format(pattern, this.Build(unaryExp.Operand, false));
        }

        private string GetConvertExpressionString(Expression expression)
        {
            var unaryExp = expression as UnaryExpression;
            if (unaryExp == null)
                throw new ArgumentException("expression is not UnaryExpression");

            return this.Build(unaryExp.Operand, false);
        }

        private string GetMemberAccessExpressionString(Expression expression)
        {
            var memberExp = expression as MemberExpression;
            if (memberExp == null)
                throw new ArgumentException("expression is not MemberExpression");

            var parameterExp = memberExp.Expression as ParameterExpression;
            if (parameterExp != null)
                return memberExp.Member.Name;

            var constExpression = memberExp.Expression as ConstantExpression;
            if (constExpression != null) {
                var property = constExpression.Type.GetField(memberExp.Member.Name,
                    BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                var value = property.GetValue(constExpression.Value);

                return this.ObjectToString(value);
            }

            var memberExpression = memberExp.Expression as MemberExpression;
            if (memberExpression != null) {
                var obj = this.GetMemberAccessExpressionValue(memberExpression);

                var property = memberExpression.Type.GetProperty(memberExp.Member.Name);
                var value = property.GetValue(obj);

                return this.ObjectToString(value);
            }

            var propertyInfo = memberExp.Member as PropertyInfo;
            if (propertyInfo != null && memberExp.Expression == null) {
                var value = propertyInfo.GetValue(null);

                return this.ObjectToString(value);
            }

            throw new ArgumentException("Unknown expression type");
        }

        private object GetMemberAccessExpressionValue(MemberExpression expression)
        {
            var constExpression = expression.Expression as ConstantExpression;
            if (constExpression != null) {
                var property = constExpression.Type.GetField(expression.Member.Name);
                var value = property.GetValue(constExpression.Value);
                return value;
            }

            var memberExpression = expression.Expression as MemberExpression;
            if (memberExpression != null) {
                var obj = this.GetMemberAccessExpressionValue(memberExpression);
                return obj;
            }

            throw new ArgumentException("Unknown expression type");
        }

        private string GetConstantExpressionString(Expression expression)
        {
            var constExp = expression as ConstantExpression;
            if (constExp == null)
                throw new ArgumentException("expression is not ConstantExpression");

            return this.ObjectToString(constExp.Value);

        }

        private string ObjectToString(object obj)
        {
            if (obj == null)
                return "null";

            var objType = obj.GetType();

            if (typeof(DateTime) == objType)
                return $"'{((DateTime) obj).ToString("yyyy.MM.dd HH:mm:ss")}'";

            if (typeof(Boolean) == objType)
                return (bool)obj ? "1" : "0";

            Type baseType = null;

#if DNXCORE50
            baseType = objType.GetTypeInfo().BaseType;
#else
            baseType = objType.BaseType;
#endif
            if (typeof(Enum) == baseType) {
                var enumValue = (Enum)obj;
                Type underlyingType = Enum.GetUnderlyingType(objType);
                object numberValue = Convert.ChangeType(enumValue, underlyingType);
                return numberValue.ToString();
            }

            return $"'{obj}'";
        }

        private string ToSqlOperator(ExpressionType expType)
        {
            switch (expType) {
                case ExpressionType.AndAlso:
                case ExpressionType.And:
                    return "AND";

                case ExpressionType.Equal:
                    return "=";

                case ExpressionType.GreaterThan:
                    return ">";

                case ExpressionType.GreaterThanOrEqual:
                    return ">=";

                case ExpressionType.LessThan:
                    return "<";

                case ExpressionType.LessThanOrEqual:
                    return "<=";

                case ExpressionType.Not:
                    return "NOT";

                case ExpressionType.NotEqual:
                    return "!=";

                case ExpressionType.OrElse:
                    return "OR";

                default:
                    throw new ArgumentException($"Unsupported expression type {expType}");
            }
        }
    }
}