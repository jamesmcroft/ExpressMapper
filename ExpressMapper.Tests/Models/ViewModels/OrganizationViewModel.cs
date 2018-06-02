namespace ExpressMapper.Tests.Models.ViewModels
{
    using System;

    public class OrganizationViewModel : ContactViewModel, IEquatable<PersonViewModel>
    {
        public OrganizationViewModel()
        {
            this.IsOrganization = true;
            this.IsPerson = false;
        }

        public string Name { get; set; }
        public OrganizationViewModel Relative { get; set; }

        protected bool Equals(OrganizationViewModel other)
        {
            return string.Equals(this.Name, other.Name) && Equals(this.Relative, other.Relative);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((OrganizationViewModel)obj);
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
