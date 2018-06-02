namespace ExpressMapper.Tests.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ExpressMapper.Tests.Models.ViewModels.Structs;

    public class CityViewModel : IEquatable<CityViewModel>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<FeatureViewModel> FeaturesList { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CityViewModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.Name != null ? this.Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.FeaturesList != null ? this.FeaturesList.GetHashCode() : 0);
                return hashCode;
            }
        }

        public bool Equals(CityViewModel other)
        {
            var featureEquals = ((this.FeaturesList != null && other.FeaturesList != null) && this.FeaturesList.Count == other.FeaturesList.Count) || ((other.FeaturesList == null && this.FeaturesList == null));
            if (featureEquals && this.FeaturesList != null && other.FeaturesList != null && this.FeaturesList.Where((t, i) => !t.Equals(other.FeaturesList[i])).Any())
            {
                featureEquals = false;
            }
            return this.Id == other.Id && this.Name == other.Name && featureEquals;
        }
    }
}
