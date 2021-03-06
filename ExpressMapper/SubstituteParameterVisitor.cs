﻿using System.Linq;
using System.Linq.Expressions;

namespace ExpressMapper
{
    public class SubstituteParameterVisitor : ExpressionVisitor
    {
        private readonly Expression[] _parametersToReplace;

        public SubstituteParameterVisitor(params Expression[] parametersToReplace)
        {
            this._parametersToReplace = parametersToReplace;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            var substitution = this._parametersToReplace.FirstOrDefault(p => p.Type == node.Type);
            return substitution ?? base.VisitParameter(node);
        }
    }
}
