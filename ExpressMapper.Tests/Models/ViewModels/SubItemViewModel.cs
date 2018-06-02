namespace ExpressMapper.Tests.Models.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class SubItemViewModel : IEquatable<SubItemViewModel>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Collection<UnitViewModel> Units { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SubItemViewModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.Name != null ? this.Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Units != null ? this.Units.GetHashCode() : 0);
                return hashCode;
            }
        }

        public bool Equals(SubItemViewModel other)
        {
            var units = true;
            if (this.Units != null && other.Units != null)
            {
                if (this.Units.Count == other.Units.Count)
                {
                    if (this.Units.Where((t, i) => !t.Equals(other.Units[i])).Any())
                    {
                        units = false;
                    }
                }
            }
            return this.Id == other.Id && this.Name == other.Name && units;
        }
    }
}
