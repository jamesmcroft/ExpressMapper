using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressMapper
{
    public class SourceTypeMapper<T, TN> : TypeMapperBase<T, TN>, ITypeMapper<T, TN>
    {
        private readonly Dictionary<string, MemberBinding> _bindingExpressions =
            new Dictionary<string, MemberBinding>();

        public SourceTypeMapper(IMappingService service, IMappingServiceProvider serviceProvider)
            : base(service, serviceProvider)
        {
        }

        public override CompilationTypes MapperType => CompilationTypes.Source;

        protected override void InitializeRecursiveMappings(IMappingServiceProvider serviceProvider)
        {
            MethodInfo mapMethod = typeof(IMappingServiceProvider).GetInfo().GetMethods()
                .First(mi => mi.Name == "Map" && mi.GetParameters().Length == 1)
                .MakeGenericMethod(typeof(T), typeof(TN));

            MethodCallExpression methodCall = Expression.Call(Expression.Constant(serviceProvider), mapMethod, this.SourceParameter);

            this.RecursiveExpressionResult.Add(Expression.Assign(this.DestFakeParameter, methodCall));
        }

        protected override void CompileInternal()
        {
            if (this.ResultMapFunction != null) return;

            BinaryExpression destVariable = this.GetDestionationVariable();

            this.ProcessCustomMembers();
            this.ProcessCustomFunctionMembers();
            this.ProcessFlattenedMembers();
            this.ProcessAutoProperties();

            this.CreateQueryableProjection();

            List<Expression> expressions = new List<Expression> { destVariable };

            if (this.BeforeMapHandler != null)
            {
                Expression<Action<T, TN>> beforeExpression = (src, dest) => this.BeforeMapHandler(src, dest);
                InvocationExpression beforeInvokeExpr = Expression.Invoke(beforeExpression, this.SourceParameter, destVariable.Left);
                expressions.Add(beforeInvokeExpr);
            }

            expressions.AddRange(this.PropertyCache.Values);

            IEnumerable<Expression> customProps = this.CustomPropertyCache.Where(k => !this.IgnoreMemberList.Contains(k.Key))
                .Select(k => k.Value);
            expressions.AddRange(customProps);

            if (this.AfterMapHandler != null)
            {
                Expression<Action<T, TN>> afterExpression = (src, dest) => this.AfterMapHandler(src, dest);
                InvocationExpression afterInvokeExpr = Expression.Invoke(afterExpression, this.SourceParameter, destVariable.Left);
                expressions.Add(afterInvokeExpr);
            }

            this.ResultExpressionList.AddRange(expressions);
            expressions.Add(destVariable.Left);

            List<ParameterExpression> variables = new List<ParameterExpression>();

            BlockExpression finalExpression = Expression.Block(variables, expressions);

            ParameterExpression destExpression = destVariable.Left as ParameterExpression;

            PreciseSubstituteParameterVisitor substituteParameterVisitor = new PreciseSubstituteParameterVisitor(
                new KeyValuePair<ParameterExpression, ParameterExpression>(this.SourceParameter, this.SourceParameter),
                new KeyValuePair<ParameterExpression, ParameterExpression>(destExpression, destExpression));

            BlockExpression resultExpression = substituteParameterVisitor.Visit(finalExpression) as BlockExpression;

            Expression<Func<T, TN, TN>> expression = Expression.Lambda<Func<T, TN, TN>>(
                resultExpression,
                this.SourceParameter,
                this.DestFakeParameter);
            this.ResultMapFunction = expression.Compile();
        }

        private void CreateQueryableProjection()
        {
            try
            {
                foreach (KeyValuePair<MemberExpression, Expression> customMember in this.CustomMembers)
                {
                    this.ProcessProjectingMember(customMember.Value, customMember.Key.Member as PropertyInfo);
                }

                foreach (KeyValuePair<MemberExpression, Expression> flattenedMember in this.FlattenMembers)
                {
                    this.ProcessProjectingMember(flattenedMember.Value, flattenedMember.Key.Member as PropertyInfo);
                }

                foreach (KeyValuePair<MemberInfo, MemberInfo> autoMember in this.AutoMembers)
                {

                    if (this._bindingExpressions.ContainsKey(autoMember.Value.Name)
                        || this.IgnoreMemberList.Contains(autoMember.Value.Name)) continue;

                    PropertyInfo destination = autoMember.Value as PropertyInfo;
                    MemberExpression propertyOrField = Expression.PropertyOrField(this.SourceParameter, autoMember.Key.Name);
                    this.ProcessProjectingMember(propertyOrField, destination);
                }

                this.QueryableExpression = Expression.Lambda<Func<T, TN>>(
                    Expression.MemberInit(Expression.New(typeof(TN)), this._bindingExpressions.Values),
                    this.SourceParameter);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Queryable projection is not supported for such mapping. Exception: {0}", ex);
            }
        }

        private void ProcessProjectingMember(Expression sourceExp, PropertyInfo destProp)
        {
            Expression memberQueryableExpression =
                this.MappingService.GetMemberQueryableExpression(sourceExp.Type, destProp.PropertyType);

            Type tCol = sourceExp.Type.GetInfo().GetInterfaces().FirstOrDefault(
                           t => t.GetInfo().IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                       ?? (sourceExp.Type.GetInfo().IsGenericType && sourceExp.Type.GetInfo().GetInterfaces()
                               .Any(t => t == typeof(IEnumerable))
                               ? sourceExp.Type
                               : null);

            Type tnCol =
                destProp.PropertyType.GetInfo().GetInterfaces().FirstOrDefault(
                    t => t.GetInfo().IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                ?? (destProp.PropertyType.GetInfo().IsGenericType && destProp.PropertyType.GetInfo().GetInterfaces()
                        .Any(t => t == typeof(IEnumerable))
                        ? destProp.PropertyType
                        : null);

            if (sourceExp.Type != typeof(string) && tCol != null && tnCol != null)
            {
                Type sourceGenericType = sourceExp.Type.IsArray
                                            ? sourceExp.Type.GetElementType()
                                            : sourceExp.Type.GetInfo().GetGenericArguments()[0];
                Type destGenericType = destProp.PropertyType.IsArray
                                          ? destProp.PropertyType.GetElementType()
                                          : destProp.PropertyType.GetInfo().GetGenericArguments()[0];

                Expression genericMemberQueryableExpression =
                    this.MappingService.GetMemberQueryableExpression(sourceGenericType, destGenericType);

                MethodInfo selectMethod = null;
                foreach (ParameterInfo p in from m in typeof(Enumerable).GetInfo().GetMethods().Where(m => m.Name == "Select")
                                  from p in m.GetParameters().Where(p => p.Name.Equals("selector"))
                                  where p.ParameterType.GetInfo().GetGenericArguments().Count() == 2
                                  select p) selectMethod = (MethodInfo)p.Member;

                Expression selectExpression = Expression.Call(
                    null,
                    selectMethod.MakeGenericMethod(sourceGenericType, destGenericType),
                    new[] { sourceExp, genericMemberQueryableExpression });

                bool destListAndCollTest = typeof(ICollection<>).MakeGenericType(destGenericType).GetInfo()
                    .IsAssignableFrom(destProp.PropertyType);

                if (destListAndCollTest)
                {
                    var toArrayMethod = typeof(Enumerable).GetInfo().GetMethod(destProp.PropertyType.IsArray ? "ToArray" : "ToList");
                    selectExpression = Expression.Call(
                        null,
                        toArrayMethod.MakeGenericMethod(destGenericType),
                        selectExpression);
                }

                try
                {
                    this._bindingExpressions.Add(destProp.Name, Expression.Bind(destProp, selectExpression));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
            else
            {
                Expression expression;
                if (memberQueryableExpression != null)
                {
                    LambdaExpression lambdaExpression = memberQueryableExpression as LambdaExpression;
                    ProjectionAccessMemberVisitor projectionAccessMemberVisitor = new ProjectionAccessMemberVisitor(sourceExp.Type, sourceExp);
                    Expression clearanceExp = projectionAccessMemberVisitor.Visit(lambdaExpression.Body);
                    expression = Expression.Condition(
                        Expression.Equal(sourceExp, Expression.Constant(null, sourceExp.Type)),
                        Expression.Constant(null, destProp.PropertyType),
                        clearanceExp);
                }
                else
                {
                    expression = sourceExp;
                }

                try
                {
                    this._bindingExpressions.Add(destProp.Name, Expression.Bind(destProp, expression));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
        }
    }
}