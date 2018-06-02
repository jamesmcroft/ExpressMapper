namespace ExpressMapper.Tests.Models.ViewModels.Structs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public struct ItemViewModel : IEquatable<ItemViewModel>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime Created { get; set; }

        public List<FeatureViewModel> FeatureList { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ItemViewModel && Equals((ItemViewModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.Name != null ? this.Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ this.Created.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.FeatureList != null ? this.FeatureList.GetHashCode() : 0);
                return hashCode;
            }
        }

        public bool Equals(ItemViewModel other)
        {
            var equals =
                ((this.FeatureList != null && other.FeatureList != null)
                 && this.FeatureList.Count == other.FeatureList.Count)
                || ((other.FeatureList == null && this.FeatureList == null));
            if (equals && this.FeatureList != null && other.FeatureList != null
                && this.FeatureList.Where((t, i) => !t.Equals(other.FeatureList[i])).Any())
            {
                equals = false;
            }

            return this.Id == other.Id && this.Name == other.Name && this.Created == other.Created && equals;
        }
    }
}