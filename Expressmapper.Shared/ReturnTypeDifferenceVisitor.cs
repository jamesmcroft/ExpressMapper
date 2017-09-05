using System.Linq.Expressions;

namespace ExpressMapper
{
    public class ReturnTypeDifferenceVisitor : ExpressionVisitor
    {
        private readonly Expression _srcExp;
        public bool DifferentReturnTypes { get; set; }

        public ReturnTypeDifferenceVisitor(Expression srcExp)
        {
            this._srcExp = srcExp;
        }


        protected override Expression VisitMember(MemberExpression node)
        {
            var memberExpression = this._srcExp as MemberExpression;
            if (memberExpression == null)
            {
                this.DifferentReturnTypes = true;
                return base.VisitMember(node);
            }
            this.DifferentReturnTypes = memberExpression != node;
            return base.VisitMember(node);
        }
    }
}
