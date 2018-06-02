using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressMapper
{
    /// <summary>
    /// NullCheckNestedMemberVisitor
    /// </summary>
    public class NullCheckNestedMemberVisitor : ExpressionVisitor
    {
        private readonly bool _isNull;

        /// <summary>
        /// NullCheckNestedMemberVisitor constructor
        /// </summary>
        /// <param name="isNull"></param>
        public NullCheckNestedMemberVisitor(bool isNull)
        {
            this._isNull = isNull;
        }

        private readonly List<string> _uniquefList = new List<string>();
        
        /// <summary>
        /// CheckNullExpression
        /// </summary>
        public Expression CheckNullExpression { get; set; }

        protected override Expression VisitMember(MemberExpression node)
        {
            var memberExpression = node.Expression as MemberExpression;

            if (memberExpression != null)
            {
                if (!this._uniquefList.Contains(memberExpression.ToString()))
                {
                    this._uniquefList.Add(memberExpression.ToString());
                    var expression = this._isNull
                        ? Expression.Constant(null, memberExpression.Type)
                        : (Expression)Expression.Default(memberExpression.Type);
                    if (this.CheckNullExpression == null)
                    {
                        this.CheckNullExpression = Expression.Equal(memberExpression, expression);
                    }
                    else
                    {
                        this.CheckNullExpression =
                            Expression.OrElse(Expression.Equal(memberExpression, expression), this.CheckNullExpression);
                    }
                }
            }

            return base.VisitMember(node);
        }
    }
}
