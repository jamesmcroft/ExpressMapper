using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressMapper
{
    public class DestinationTypeMapper<T, TN> : TypeMapperBase<T, TN>, ITypeMapper<T, TN>
    {
        #region Const

        private const string MapStr = "Map";

        #endregion

        public DestinationTypeMapper(IMappingService service, IMappingServiceProvider serviceProvider) : base(service, serviceProvider) { }

        public override CompilationTypes MapperType => CompilationTypes.Destination;

        protected override void InitializeRecursiveMappings(IMappingServiceProvider serviceProvider)
        {
            var mapMethod =
                typeof(IMappingServiceProvider).GetInfo().GetMethods()
                    .First(mi => mi.Name == MapStr && mi.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), typeof(TN));

            var methodCall = Expression.Call(Expression.Constant(serviceProvider), mapMethod, this.SourceParameter, this.DestFakeParameter);

            this.RecursiveExpressionResult.Add(Expression.Assign(this.DestFakeParameter, methodCall));
        }

        protected override void CompileInternal()
        {
            if (this.ResultMapFunction != null) return;

            this.ProcessCustomMembers();
            this.ProcessCustomFunctionMembers();
            this.ProcessFlattenedMembers();
            this.ProcessAutoProperties();

            var expressions = new List<Expression>();

            if (this.BeforeMapHandler != null)
            {
                Expression<Action<T, TN>> beforeExpression = (src, dest) => this.BeforeMapHandler(src, dest);
                var beforeInvokeExpr = Expression.Invoke(beforeExpression, this.SourceParameter, this.DestFakeParameter);
                expressions.Add(beforeInvokeExpr);
            }

            expressions.AddRange(this.PropertyCache.Values);

            var customProps = this.CustomPropertyCache.Where(k => !this.IgnoreMemberList.Contains(k.Key)).Select(k => k.Value);
            expressions.AddRange(customProps);

            if (this.AfterMapHandler != null)
            {
                Expression<Action<T, TN>> afterExpression = (src, dest) => this.AfterMapHandler(src, dest);
                var afterInvokeExpr = Expression.Invoke(afterExpression, this.SourceParameter, this.DestFakeParameter);
                expressions.Add(afterInvokeExpr);
            }

            this.ResultExpressionList.AddRange(expressions);
            this.ResultExpressionList.Insert(0, this.GetDestionationVariable());

            expressions.Add(this.DestFakeParameter);

            var finalExpression = Expression.Block(expressions);

            var substituteParameterVisitor =
                new PreciseSubstituteParameterVisitor(
                    new KeyValuePair<ParameterExpression, ParameterExpression>(this.SourceParameter, this.SourceParameter),
                    new KeyValuePair<ParameterExpression, ParameterExpression>(this.DestFakeParameter, this.DestFakeParameter));

            //var substituteParameterVisitor = new SubstituteParameterVisitor(SourceParameter, DestFakeParameter);

            var resultExpression = substituteParameterVisitor.Visit(finalExpression);

            var expression = Expression.Lambda<Func<T, TN, TN>>(resultExpression, this.SourceParameter, this.DestFakeParameter);
            this.ResultMapFunction = expression.Compile();
        }
    }
}
