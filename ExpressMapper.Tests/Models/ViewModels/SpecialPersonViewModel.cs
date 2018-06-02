namespace ExpressMapper.Tests.Models.ViewModels
{
    public class SpecialPersonViewModel : PersonViewModel
    {
        public int AffectionLevel { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SpecialPersonViewModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ this.AffectionLevel;
            }
        }

        public bool Equals(SpecialPersonViewModel other)
        {
            return base.Equals(other) && this.AffectionLevel == other.AffectionLevel;
        }
    }
}
