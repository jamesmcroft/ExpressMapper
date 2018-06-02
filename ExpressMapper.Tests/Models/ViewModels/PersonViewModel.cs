namespace ExpressMapper.Tests.Models.ViewModels
{
    using System;

    public class PersonViewModel : ContactViewModel, IEquatable<PersonViewModel>
    {
        public PersonViewModel()
        {
            this.IsOrganization = false;
            this.IsPerson = true;
        }

        public string Name { get; set; }
        public PersonViewModel Relative { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PersonViewModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((this.Name != null ? this.Name.GetHashCode() : 0) * 397) ^ (this.Relative != null ? this.Relative.GetHashCode() : 0);
            }
        }

        public bool Equals(PersonViewModel other)
        {
            return this.ContactEquals(other) && this.Name == other.Name && (this.Relative == null || this.Relative.Equals(other.Relative));
        }
    }
}
