using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressMapper
{
    internal class FlattenMemberInfo
    {
        /// <summary>
        /// The Destination property in the DTO
        /// </summary>
        private readonly PropertyInfo _destMember;

        /// <summary>
        /// The list of properties in order to get to the source property we want
        /// </summary>
        private readonly ICollection<PropertyInfo> _sourcePathMembers;

        /// <summary>
        /// Optional Linq Method to apply to an enumerable source (null if no Linq method on the end)
        /// </summary>
        private readonly FlattenLinqMethod _linqMethodSuffix;

        public FlattenMemberInfo(PropertyInfo destMember, PropertyInfo[] sourcePathMembers, PropertyInfo lastMemberToAdd, 
            FlattenLinqMethod linqMethodSuffix  = null)
        {
            this._destMember = destMember;
            this._linqMethodSuffix = linqMethodSuffix;

            var list = sourcePathMembers.ToList();
            list.Add(lastMemberToAdd);
            this._sourcePathMembers = list;
        }

        public override string ToString()
        {
            var linqMethodStr = this._linqMethodSuffix?.ToString() ?? string.Empty;
            return $"dest => dest.{this._destMember.Name}, src => src.{string.Join(".", this._sourcePathMembers.Select(x => x.Name))}{linqMethodStr}";
        }

        public MemberExpression  DestAsMemberExpression<TDest>()
        {
            return Expression.Property(Expression.Parameter(typeof(TDest), "dest"), this._destMember);
        }

        public Expression SourceAsExpression<TSource>()
        {
            var paramExpression = Expression.Parameter(typeof(TSource), "src");
            return this.NestedExpressionProperty(paramExpression, this._sourcePathMembers.Reverse().ToArray());
        }

        //-------------------------------------------------------
        //private methods

        private Expression NestedExpressionProperty(Expression expression, PropertyInfo[] properties)
        {
            if (properties.Length > 1)
            {
                return Expression.Property(
                    this.NestedExpressionProperty(
                        expression,
                        properties.Skip(1).ToArray()
                        ),
                    properties[0]);
            }

            //we are at the end 
            var finalProperty = Expression.Property(expression, properties[0]);

            return this._linqMethodSuffix == null
                ? (Expression)finalProperty
                : this._linqMethodSuffix.AsMethodCallExpression(finalProperty, properties[0], this._destMember);
        }
    }
}