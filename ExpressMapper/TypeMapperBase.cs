using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressMapper
{
    public abstract class TypeMapperBase<T, TN> : IMemberConfigParameters
    {
        #region Constants

        private const string RightStr = "right";

        private const string DstString = "dst";

        private const string SrcString = "src";

        #endregion

        private readonly object _lockObject = new object();

        private bool _compiling;

        protected ParameterExpression DestFakeParameter = Expression.Parameter(typeof(TN), DstString);

        protected IMappingService MappingService { get; set; }

        protected IMappingServiceProvider MappingServiceProvider { get; set; }

        protected Dictionary<MemberInfo, MemberInfo> AutoMembers = new Dictionary<MemberInfo, MemberInfo>();

        public bool BaseType { get; set; }


        public IMemberConfiguration<T, TN> MemberConfiguration { get; set; }

        public List<KeyValuePair<MemberExpression, Expression>> CustomMembers { get; private set; }

        public List<KeyValuePair<MemberExpression, Expression>> FlattenMembers { get; private set; }

        public List<KeyValuePair<MemberExpression, Expression>> CustomFunctionMembers { get; private set; }

        public Expression<Func<T, TN>> QueryableExpression { get; protected set; }

        public abstract CompilationTypes MapperType { get; }

        public bool Flattened { get; set; }

        public Type SourceType => typeof(T);

        public Type DestinationType => typeof(TN);

        public Expression QueryableGeneralExpression => this.QueryableExpression;

        #region Constructors

        protected TypeMapperBase(IMappingService service, IMappingServiceProvider serviceProvider)
        {
            this.ResultExpressionList = new List<Expression>();
            this.RecursiveExpressionResult = new List<Expression>();
            this.PropertyCache = new Dictionary<string, Expression>();
            this.CustomPropertyCache = new Dictionary<string, Expression>();
            this.IgnoreMemberList = new List<string>();
            this.MappingService = service;
            this.MappingServiceProvider = serviceProvider;
            this.InitializeRecursiveMappings(serviceProvider);

            this.CustomMembers = new List<KeyValuePair<MemberExpression, Expression>>();
            this.FlattenMembers = new List<KeyValuePair<MemberExpression, Expression>>();
            this.CustomFunctionMembers = new List<KeyValuePair<MemberExpression, Expression>>();
        }

        #endregion

        #region Properties

        protected ParameterExpression SourceParameter = Expression.Parameter(typeof(T), SrcString);

        protected List<Expression> RecursiveExpressionResult { get; private set; }

        protected List<Expression> ResultExpressionList { get; private set; }

        protected Func<T, TN, TN> ResultMapFunction { get; set; }

        public List<string> IgnoreMemberList { get; private set; }

        protected Dictionary<string, Expression> PropertyCache { get; private set; }

        protected Dictionary<string, Expression> CustomPropertyCache { get; private set; }

        protected Action<T, TN> BeforeMapHandler { get; set; }

        protected Action<T, TN> AfterMapHandler { get; set; }

        protected Func<T, TN> ConstructorFunc { get; set; }

        protected Expression<Func<T, TN>> ConstructorExp { get; set; }

        protected Func<object, object, object> NonGenericMapFunc { get; set; }

        public bool CaseSensetiveMember { get; set; }

        public bool CaseSensetiveOverride { get; set; }

        public CompilationTypes CompilationTypeMember { get; set; }

        public bool CompilationTypeOverride { get; set; }

        #endregion

        protected abstract void InitializeRecursiveMappings(IMappingServiceProvider serviceProvider);

        public void Flatten()
        {
            this.Flattened = true;
        }

        public void CaseSensetiveMemberMap(bool caseSensitive)
        {
            this.CaseSensetiveMember = caseSensitive;
            this.CaseSensetiveOverride = true;
        }

        public void CompileTo(CompilationTypes compileType)
        {
            this.CompilationTypeMember = compileType;
            this.CompilationTypeOverride = true;
        }

        public void Compile(CompilationTypes compilationType, bool forceByDemand = false)
        {
            if (!forceByDemand
                && ((this.CompilationTypeOverride && (this.MapperType & this.CompilationTypeMember) != this.MapperType)
                    || (!this.CompilationTypeOverride && (this.MapperType & compilationType) != this.MapperType)))
            {
                return;
            }

            if (this._compiling)
            {
                return;
            }

            try
            {
                this._compiling = true;
                try
                {
                    this.CompileInternal();
                }
                catch (Exception ex)
                {
                    throw new ExpressMapperException(
                        $"Error error occured trying to compile mapping for: source {typeof(T).FullName}, destination {typeof(TN).FullName}. See the inner exception for details.",
                        ex);
                }
            }
            finally
            {
                this._compiling = false;
            }
        }

        protected abstract void CompileInternal();

        public void AfterMap(Action<T, TN> afterMap)
        {
            if (afterMap == null)
            {
                throw new ArgumentNullException(nameof(afterMap));
            }

            if (this.AfterMapHandler != null)
            {
                throw new InvalidOperationException($"AfterMap already registered for {typeof(T).FullName}");
            }

            this.AfterMapHandler = afterMap;
        }

        public Tuple<List<Expression>, ParameterExpression, ParameterExpression> GetMapExpressions()
        {
            if (this._compiling || this.BaseType)
            {
                return new Tuple<List<Expression>, ParameterExpression, ParameterExpression>(
                    new List<Expression>(this.RecursiveExpressionResult),
                    this.SourceParameter,
                    this.DestFakeParameter);
            }

            this.Compile(this.MapperType);
            return new Tuple<List<Expression>, ParameterExpression, ParameterExpression>(
                new List<Expression>(this.ResultExpressionList),
                this.SourceParameter,
                this.DestFakeParameter);
        }

        public Func<object, object, object> GetNonGenericMapFunc()
        {
            if (this.NonGenericMapFunc != null)
            {
                return this.NonGenericMapFunc;
            }

            var parameterExpression = Expression.Parameter(typeof(object), SrcString);
            var srcConverted = Expression.Convert(parameterExpression, typeof(T));
            var srcTypedExp = Expression.Variable(typeof(T), "srcTyped");
            var srcAssigned = Expression.Assign(srcTypedExp, srcConverted);

            var destParameterExp = Expression.Parameter(typeof(object), DstString);
            var dstConverted = Expression.Convert(destParameterExp, typeof(TN));
            var dstTypedExp = Expression.Variable(typeof(TN), "dstTyped");
            var dstAssigned = Expression.Assign(dstTypedExp, dstConverted);

            var customGenericType = typeof(ITypeMapper<,>).MakeGenericType(typeof(T), typeof(TN));
            var castToCustomGeneric = Expression.Convert(Expression.Constant((ITypeMapper)this), customGenericType);
            var genVariable = Expression.Variable(customGenericType);
            var assignExp = Expression.Assign(genVariable, castToCustomGeneric);
            var methodInfo = customGenericType.GetTypeInfo().GetMethod("MapTo", new[] { typeof(T), typeof(TN) });

            var mapCall = Expression.Call(genVariable, methodInfo, srcTypedExp, dstTypedExp);
            var resultVarExp = Expression.Variable(typeof(object), "result");
            var convertToObj = Expression.Convert(mapCall, typeof(object));
            var assignResult = Expression.Assign(resultVarExp, convertToObj);

            var blockExpression = Expression.Block(
                new[] { srcTypedExp, dstTypedExp, genVariable, resultVarExp },
                new Expression[] { srcAssigned, dstAssigned, assignExp, assignResult, resultVarExp });
            var lambda = Expression.Lambda<Func<object, object, object>>(
                blockExpression,
                parameterExpression,
                destParameterExp);
            this.NonGenericMapFunc = lambda.Compile();

            return this.NonGenericMapFunc;
        }

        protected void AutoMapProperty(MemberInfo propertyGet, MemberInfo propertySet)
        {
            var callSetPropMethod = propertySet.GetMemberType() == ObjMemberTypes.Field
                                        ? Expression.Field(this.DestFakeParameter, propertySet as FieldInfo)
                                        : Expression.Property(this.DestFakeParameter, propertySet as PropertyInfo);
            var callGetPropMethod = propertyGet.GetMemberType() == ObjMemberTypes.Field
                                        ? Expression.Field(this.SourceParameter, propertyGet as FieldInfo)
                                        : Expression.Property(this.SourceParameter, propertyGet as PropertyInfo);

            this.MapMember(callSetPropMethod, callGetPropMethod);
        }

        public void MapMember<TSourceMember, TDestMember>(
            Expression<Func<TN, TDestMember>> left,
            Expression<Func<T, TSourceMember>> right)
        {
            if (left == null)
            {
                throw new ArgumentNullException("left");
            }

            if (right == null)
            {
                throw new ArgumentNullException(RightStr);
            }

            this.CustomMembers.Add(
                new KeyValuePair<MemberExpression, Expression>(left.Body as MemberExpression, right.Body));

            //MapMember(left.Body as MemberExpression, right.Body);
        }

        #region flatten code

        public void MapMemberFlattened(MemberExpression left, Expression right)
        {
            this.FlattenMembers.Add(new KeyValuePair<MemberExpression, Expression>(left, right));
        }

        protected List<string> NamesOfMembersAndIgnoredProperties()
        {
            var result = this.CustomMembers.Select(x => x.Key.Member.Name)
                .Union(this.CustomFunctionMembers.Select(x => x.Key.Member.Name)).ToList();
            result.AddRange(this.IgnoreMemberList);
            return result;
        }

        #endregion


        protected void MapMember(MemberExpression left, Expression right)
        {
            var mappingExpression = this.MappingService.GetMemberMappingExpression(left, right, false);
            this.CustomPropertyCache[left.Member.Name] = mappingExpression;
        }

        protected BinaryExpression GetDestionationVariable()
        {
            if (this.ConstructorExp != null)
            {
                var substVisitorSrc = new SubstituteParameterVisitor(this.SourceParameter);
                var constructorExp = substVisitorSrc.Visit(this.ConstructorExp.Body);

                return Expression.Assign(this.DestFakeParameter, constructorExp);
            }

            if (this.ConstructorFunc != null)
            {
                Expression<Func<T, TN>> customConstruct = t => this.ConstructorFunc(t);
                var invocationExpression = Expression.Invoke(customConstruct, this.SourceParameter);
                return Expression.Assign(this.DestFakeParameter, invocationExpression);
            }

            var createDestination = Expression.New(typeof(TN));
            return Expression.Assign(this.DestFakeParameter, createDestination);
        }

        protected void ProcessAutoProperties()
        {
            var getFields = typeof(T).GetRuntimeFields().Where(x => x.IsPublic && !x.IsStatic);
            var setFields = typeof(TN).GetRuntimeFields().Where(x => x.IsPublic && !x.IsStatic);

            var getProps = typeof(T).GetRuntimeProperties().Where(x => x.PropertyType.GetTypeInfo().IsPublic);

            var setProps = typeof(TN).GetRuntimeProperties().Where(x => x.PropertyType.GetTypeInfo().IsPublic);

            var sourceMembers = getFields.Cast<MemberInfo>().Union(getProps);
            var destMembers = setFields.Cast<MemberInfo>().Union(setProps);

            var stringComparison = this.GetStringCase();

            var comparer = new CultureAwareStringComparer(StringComparison.OrdinalIgnoreCase);

            foreach (var prop in sourceMembers)
            {
                if (this.IgnoreMemberList.Contains(prop.Name, comparer)
                    || this.CustomPropertyCache.Keys.Contains(prop.Name, comparer))
                {
                    continue;
                }

                var notUniqueDestMembers = destMembers.Where(x => string.Equals(x.Name, prop.Name, stringComparison));
                var notUniqueSrcMembers = sourceMembers.Where(x => string.Equals(x.Name, prop.Name, stringComparison));

                var getprop = GetTopMostMemberOfHierarchy(notUniqueSrcMembers);
                if (this.AutoMembers.ContainsKey(getprop))
                {
                    continue;
                }

                var setprop = GetTopMostMemberOfHierarchy(notUniqueDestMembers);

                var propertyInfo = setprop as PropertyInfo;
                if ((propertyInfo == null && setprop == null)
                    || (propertyInfo != null && (!propertyInfo.CanWrite || !propertyInfo.SetMethod.IsPublic)))
                {
                    this.IgnoreMemberList.Add(getprop.Name);
                    continue;
                }

                this.AutoMembers[getprop] = setprop;
                this.AutoMapProperty(getprop, setprop);
            }
        }

        private static MemberInfo GetTopMostMemberOfHierarchy(IEnumerable<MemberInfo> notUniqueMembers)
        {
            MemberInfo chosen = null;

            foreach (var notUniqueMember in notUniqueMembers)
            {
                if (chosen == null)
                {
                    chosen = notUniqueMember;
                }
                else
                {
                    chosen = chosen.DeclaringType.GetTypeInfo()
                                 .IsAssignableFrom(notUniqueMember.DeclaringType.GetTypeInfo())
                                 ? notUniqueMember
                                 : chosen;
                }
            }

            return chosen;
        }

        internal StringComparison GetStringCase()
        {
            StringComparison stringComparison;

            if (this.MappingServiceProvider.CaseSensetiveMemberMap && !this.CaseSensetiveOverride)
            {
                stringComparison = this.MappingServiceProvider.CaseSensetiveMemberMap
                                       ? StringComparison.CurrentCulture
                                       : StringComparison.OrdinalIgnoreCase;
            }
            else
            {
                stringComparison = this.CaseSensetiveMember
                                       ? StringComparison.CurrentCulture
                                       : StringComparison.OrdinalIgnoreCase;
            }

            return stringComparison;
        }

        public virtual void InstantiateFunc(Func<T, TN> constructor)
        {
            this.ConstructorFunc = constructor;
        }

        public virtual void Instantiate(Expression<Func<T, TN>> constructor)
        {
            this.ConstructorExp = constructor;
        }

        public virtual void BeforeMap(Action<T, TN> beforeMap)
        {
            if (beforeMap == null)
            {
                throw new ArgumentNullException(nameof(beforeMap));
            }

            if (this.BeforeMapHandler != null)
            {
                throw new InvalidOperationException($"BeforeMap already registered for {typeof(T).FullName}");
            }

            this.BeforeMapHandler = beforeMap;
        }

        public virtual void Ignore<TMember>(Expression<Func<TN, TMember>> left)
        {
            var memberExpression = left.Body as MemberExpression;
            this.IgnoreMemberList.Add(memberExpression.Member.Name);
        }

        public void Ignore(PropertyInfo left)
        {
            this.IgnoreMemberList.Add(left.Name);
        }

        public TN MapTo(T src, TN dest)
        {
            if (this.ResultMapFunction == null)
            {
                lock (this._lockObject)
                {
                    // force compilation by client code demand
                    this.Compile(this.MapperType, forceByDemand: true);
                }
            }

            return this.ResultMapFunction(src, dest);
        }

        public void MapFunction<TMember, TNMember>(Expression<Func<TN, TNMember>> left, Func<T, TMember> right)
        {
            var memberExpression = left.Body as MemberExpression;
            Expression<Func<T, TMember>> expr = (t) => right(t);

            var parameterExpression = Expression.Parameter(typeof(T));
            var rightExpression = Expression.Invoke(expr, parameterExpression);

            this.CustomFunctionMembers.Add(
                new KeyValuePair<MemberExpression, Expression>(memberExpression, rightExpression));

            //MapFunction<TMember, TNMember>(left, rightExpression, memberExpression);
        }

        protected void MapFunction(MemberExpression left, Expression rightExpression)
        {
            if (left.Member.DeclaringType != rightExpression.Type)
            {
                var mapComplexResult =
                    this.MappingService.GetDifferentTypeMemberMappingExpression(rightExpression, left, false);
                this.CustomPropertyCache[left.Member.Name] = mapComplexResult;
            }
            else
            {
                var binaryExpression = Expression.Assign(left, rightExpression);
                this.CustomPropertyCache[left.Member.Name] = binaryExpression;
            }
        }

        protected void ProcessCustomMembers()
        {
            this.CustomMembers = this.TranslateExpression(this.CustomMembers);
            foreach (var keyValue in this.CustomMembers)
            {
                this.MapMember(keyValue.Key, keyValue.Value);
            }
        }

        protected void ProcessCustomFunctionMembers()
        {
            this.CustomFunctionMembers = this.TranslateExpression(this.CustomFunctionMembers);
            foreach (var keyValue in this.CustomFunctionMembers)
            {
                this.MapMember(keyValue.Key, keyValue.Value);
            }
        }

        protected void ProcessFlattenedMembers()
        {
            if (this.Flattened)
            {
                var flattenMapper = new FlattenMapper<T, TN>(
                    this.NamesOfMembersAndIgnoredProperties(),
                    this.GetStringCase());
                foreach (var flattenInfo in flattenMapper.BuildMemberMapping())
                {
                    this.MapMemberFlattened(
                        flattenInfo.DestAsMemberExpression<TN>(),
                        flattenInfo.SourceAsExpression<T>());
                }

                this.FlattenMembers = this.TranslateExpression(this.FlattenMembers);
                foreach (var keyValue in this.FlattenMembers)
                {
                    this.MapMember(keyValue.Key, keyValue.Value);
                }
            }
        }

        protected List<KeyValuePair<MemberExpression, Expression>> TranslateExpression(
            IEnumerable<KeyValuePair<MemberExpression, Expression>> expressions)
        {
            var result = new List<KeyValuePair<MemberExpression, Expression>>(expressions.Count());
            foreach (var customMember in expressions)
            {
                var substVisitorDest = new SubstituteParameterVisitor(this.DestFakeParameter);
                var dest = substVisitorDest.Visit(customMember.Key) as MemberExpression;

                var substVisitorSrc = new SubstituteParameterVisitor(this.SourceParameter);
                var src = substVisitorSrc.Visit(customMember.Value);
                result.Add(new KeyValuePair<MemberExpression, Expression>(dest, src));
            }

            return result;
        }

        public void ImportMemberConfigParameters(IMemberConfigParameters baseClassConfiguration)
        {
            this.Flattened = baseClassConfiguration.Flattened;
            this.CaseSensetiveMember = baseClassConfiguration.CaseSensetiveMember;
            this.CaseSensetiveOverride = baseClassConfiguration.CaseSensetiveOverride;
            this.CompilationTypeOverride = baseClassConfiguration.CompilationTypeOverride;
            this.CompilationTypeMember = baseClassConfiguration.CompilationTypeMember;

            // todo : implement visitor to replace base type to the subclass' type
            this.CustomFunctionMembers =
                new List<KeyValuePair<MemberExpression, Expression>>(
                    baseClassConfiguration.CustomFunctionMembers.Count);
            this.CustomMembers =
                new List<KeyValuePair<MemberExpression, Expression>>(baseClassConfiguration.CustomMembers.Count);
            this.FlattenMembers =
                new List<KeyValuePair<MemberExpression, Expression>>(baseClassConfiguration.FlattenMembers.Count);

            this.IgnoreMemberList = new List<string>(baseClassConfiguration.IgnoreMemberList);

            var replaceDestMemberTypeVisitor =
                new ReplaceMemberTypeVisitor(this.DestinationType, this.DestFakeParameter);
            var replaceSrcMemberTypeVisitor = new ReplaceMemberTypeVisitor(this.SourceType, this.SourceParameter);

            foreach (var customMember in baseClassConfiguration.CustomMembers)
            {
                var destExp = replaceDestMemberTypeVisitor.Visit(customMember.Key) as MemberExpression;
                var srcExp = replaceSrcMemberTypeVisitor.Visit(customMember.Value);
                this.CustomMembers.Add(new KeyValuePair<MemberExpression, Expression>(destExp, srcExp));
            }

            foreach (var customMember in baseClassConfiguration.CustomFunctionMembers)
            {
                var destExp = replaceDestMemberTypeVisitor.Visit(customMember.Key) as MemberExpression;
                var srcExp = replaceSrcMemberTypeVisitor.Visit(customMember.Value);
                this.CustomFunctionMembers.Add(new KeyValuePair<MemberExpression, Expression>(destExp, srcExp));
            }

            foreach (var customMember in baseClassConfiguration.FlattenMembers)
            {
                var destExp = replaceDestMemberTypeVisitor.Visit(customMember.Key) as MemberExpression;
                var srcExp = replaceSrcMemberTypeVisitor.Visit(customMember.Value);
                this.FlattenMembers.Add(new KeyValuePair<MemberExpression, Expression>(destExp, srcExp));
            }
        }
    }

    /// <summary>
    /// ReplaceMemberTypeVisitor
    /// </summary>
    public class ReplaceMemberTypeVisitor : ExpressionVisitor
    {
        private readonly Type _replacementType;

        private readonly Expression _instanceExp;

        /// <summary>
        /// ReplaceMemberTypeVisitor constructor
        /// </summary>
        public ReplaceMemberTypeVisitor(Type replacementType, Expression instanceExp)
        {
            this._replacementType = replacementType;
            this._instanceExp = instanceExp;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            return node.Member.DeclaringType.GetTypeInfo().IsAssignableFrom(this._replacementType.GetTypeInfo())
                       ? Expression.PropertyOrField(this._instanceExp, node.Member.Name)
                       : base.VisitMember(node);
        }
    }
}