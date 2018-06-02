namespace ExpressMapper.Tests.Models.ViewModels
{
    using System;

    public class CylinderViewModel : IEquatable<CylinderViewModel>
    {
        public Guid Id { get; set; }
        public decimal Capacity { get; set; }
        public EngineViewModel Engine { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CylinderViewModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.Id.GetHashCode();
                hashCode = (hashCode * 397) ^ this.Capacity.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.Engine != null ? this.Engine.GetHashCode() : 0);
                return hashCode;
            }
        }

        public bool Equals(CylinderViewModel other)
        {
            return this.Id == other.Id && this.Capacity == other.Capacity && ((this.Engine == null && other.Engine == null) || this.Engine.Equals(other.Engine));
        }
    }
}
