namespace ExpressMapper.Tests.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class EngineViewModel : IEquatable<EngineViewModel>
    {
        public Guid Id { get; set; }
        public string Capacity { get; set; }
        public List<CylinderViewModel> Cylinders { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EngineViewModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.Capacity != null ? this.Capacity.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Cylinders != null ? this.Cylinders.GetHashCode() : 0);
                return hashCode;
            }
        }

        public bool Equals(EngineViewModel other)
        {
            var subItems = true;
            if (this.Cylinders != null && other.Cylinders != null)
            {
                if (this.Cylinders.Count == other.Cylinders.Count)
                {
                    if (this.Cylinders.Where((t, i) => !t.Equals(other.Cylinders[i])).Any())
                    {
                        subItems = false;
                    }
                }
            }
            return this.Id == other.Id && this.Capacity == other.Capacity && subItems;
        }
    }
}
