namespace ExpressMapper.Tests.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class UnitViewModel : IEquatable<UnitViewModel>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<SubUnitViewModel> SubUnits { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((UnitViewModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.Name != null ? this.Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.SubUnits != null ? this.SubUnits.GetHashCode() : 0);
                return hashCode;
            }
        }

        public bool Equals(UnitViewModel other)
        {
            var subUnits = true;
            if (this.SubUnits != null && other.SubUnits != null)
            {
                if (this.SubUnits.Count == other.SubUnits.Count)
                {
                    if (this.SubUnits.Where((t, i) => !t.Equals(other.SubUnits[i])).Any())
                    {
                        subUnits = false;
                    }
                }
            }
            return this.Id == other.Id && this.Name == other.Name && subUnits;
        }
    }
}
