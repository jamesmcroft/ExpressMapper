using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ExpressMapper
{
    public class ProjectionAccessMemberVisitor : ExpressionVisitor
    {
        private readonly Expression _exp;

        //private ParameterExpression _src;
        private readonly Type _type;

        public ProjectionAccessMemberVisitor(Type type, Expression exp)
        {
            this._exp = exp;
            this._type = type;

            //_src = src;
        }

        //protected override Expression VisitParameter(ParameterExpression node)
        //{
        //    if (node.Type == _type)
        //    {
        //        return _src;
        //    }
        //    return base.VisitParameter(node);
        //}

        protected override Expression VisitMember(MemberExpression node)
        {
            return node.Expression != null && node.Expression.Type == this._type
                       ? Expression.PropertyOrField(this._exp, node.Member.Name)
                       : base.VisitMember(node);
        }
    }
}