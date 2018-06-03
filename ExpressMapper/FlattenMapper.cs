using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ExpressMapper
{
    internal class FlattenMapper<TSource, TDest>
    {
        private readonly StringComparison _stringComparison;

        private readonly IEnumerable<PropertyInfo> _allDestProps;

        private readonly IEnumerable<PropertyInfo> _allSourceProps;

        private readonly List<PropertyInfo> _filteredDestProps;

        private List<FlattenMemberInfo> _foundFlattens;

        public FlattenMapper(ICollection<string> namesOfPropertiesToIgnore, StringComparison stringComparison)
        {
            this._stringComparison = stringComparison;
            this._allSourceProps = GetPropertiesRightAccess<TSource>();
            this._allDestProps = GetPropertiesRightAccess<TDest>();

            //ExpressMapper with match the top level properties, so we ignore those
            this._filteredDestProps = this.FilterOutExactMatches(this._allDestProps, this._allSourceProps);

            if (!namesOfPropertiesToIgnore.Any()) return;

            //we also need to remove the destinations that have a .Member or .Ignore applied to them
            if (stringComparison == StringComparison.OrdinalIgnoreCase)
                namesOfPropertiesToIgnore = namesOfPropertiesToIgnore.Select(x => x.ToLowerInvariant()).ToList();
            this._filteredDestProps = this._filteredDestProps.Where(
                x => !namesOfPropertiesToIgnore.Contains(
                         this._stringComparison == StringComparison.OrdinalIgnoreCase
                             ? x.Name.ToLowerInvariant()
                             : x.Name)).ToList();
        }

        public List<FlattenMemberInfo> BuildMemberMapping()
        {
            this._foundFlattens = new List<FlattenMemberInfo>();
            var filteredSourceProps = this.FilterOutExactMatches(this._allSourceProps, this._allDestProps);
            this.ScanSourceProps(filteredSourceProps);
            return this._foundFlattens;
        }

        private void ScanSourceProps(
            List<PropertyInfo> sourcePropsToScan,
            string prefix = "",
            PropertyInfo[] sourcePropPath = null)
        {
            foreach (var destProp in this._filteredDestProps.ToList())

                //scan source property name against dest that has no direct match with any of the source property names
                if (this._filteredDestProps.Contains(destProp))

                    //This allows for entries to be removed from the list
                    this.ScanSourceClassRecursively(
                        sourcePropsToScan,
                        destProp,
                        prefix,
                        sourcePropPath ?? new PropertyInfo[] { });
        }

        private void ScanSourceClassRecursively(
            IEnumerable<PropertyInfo> sourceProps,
            PropertyInfo destProp,
            string prefix,
            PropertyInfo[] sourcePropPath)
        {

            foreach (var matchedStartSrcProp in sourceProps.Where(
                x => destProp.Name.StartsWith(prefix + x.Name, this._stringComparison)))
            {
                var matchStart = prefix + matchedStartSrcProp.Name;
                if (string.Equals(destProp.Name, matchStart, this._stringComparison))
                {
                    //direct match of name

                    var underlyingType = Nullable.GetUnderlyingType(destProp.PropertyType);
                    if (destProp.PropertyType == matchedStartSrcProp.PropertyType
                        || underlyingType == matchedStartSrcProp.PropertyType || Mapper.MapExists(
                            matchedStartSrcProp.PropertyType,
                            destProp.PropertyType))
                    {
                        //matched a) same type, or b) dest is a nullable version of source 
                        this._foundFlattens.Add(new FlattenMemberInfo(destProp, sourcePropPath, matchedStartSrcProp));
                        this._filteredDestProps.Remove(destProp); //matched, so take it out
                    }

                    return;
                }

                if (matchedStartSrcProp.PropertyType == typeof(string))

                    //string can only be directly matched
                    continue;

                if (matchedStartSrcProp.PropertyType.GetTypeInfo().IsClass)
                {
                    var classProps = GetPropertiesRightAccess(matchedStartSrcProp.PropertyType);
                    var clonedList = sourcePropPath.ToList();
                    clonedList.Add(matchedStartSrcProp);
                    this.ScanSourceClassRecursively(classProps, destProp, matchStart, clonedList.ToArray());
                }
                else if (matchedStartSrcProp.PropertyType.GetTypeInfo().ImplementedInterfaces
                    .Any(i => i.Name == "IEnumerable"))
                {
                    //its an enumerable class so see if the end relates to a LINQ method
                    var endOfName = destProp.Name.Substring(matchStart.Length);
                    var enumeableMethod =
                        FlattenLinqMethod.EnumerableEndMatchsWithLinqMethod(endOfName, this._stringComparison);
                    if (enumeableMethod != null)
                    {
                        this._foundFlattens.Add(
                            new FlattenMemberInfo(destProp, sourcePropPath, matchedStartSrcProp, enumeableMethod));
                        this._filteredDestProps.Remove(destProp); //matched, so take it out
                    }
                }
            }
        }

        private static IEnumerable<PropertyInfo> GetPropertiesRightAccess<T>()
        {
            return GetPropertiesRightAccess(typeof(T));
        }

        private static IEnumerable<PropertyInfo> GetPropertiesRightAccess(Type classType)
        {
            return classType.GetRuntimeProperties().Where(x => x.GetMethod.IsPublic && !x.GetMethod.IsStatic);
        }

        private List<PropertyInfo> FilterOutExactMatches(
            IEnumerable<PropertyInfo> propsToFilter,
            IEnumerable<PropertyInfo> filterAgainst)
        {
            var filterNames = filterAgainst.Select(
                    x => this._stringComparison == StringComparison.OrdinalIgnoreCase
                             ? x.Name.ToLowerInvariant()
                             : x.Name)
                .ToArray();
            return propsToFilter.Where(
                x => !filterNames.Contains(
                         this._stringComparison == StringComparison.OrdinalIgnoreCase
                             ? x.Name.ToLowerInvariant()
                             : x.Name)).ToList();

        }
    }
}