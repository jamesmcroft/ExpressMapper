using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressMapper
{
    public class PreciseSubstituteParameterVisitor : ExpressionVisitor
    {
        private KeyValuePair<ParameterExpression, ParameterExpression> _src;
        private KeyValuePair<ParameterExpression, ParameterExpression> _dest;

        public PreciseSubstituteParameterVisitor(KeyValuePair<ParameterExpression, ParameterExpression> src, KeyValuePair<ParameterExpression, ParameterExpression> dest)
        {
            this._src = src;
            this._dest = dest;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return (this._src.Key.Name == node.Name && this._src.Key.Type == node.Type) ? this._src.Value : (this._dest.Key.Name == node.Name && this._dest.Key.Type == node.Type) ? this._dest.Value : base.VisitParameter(node);
        }
    }
}
