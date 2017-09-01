using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressMapper.AppShared
{
    public class TestModel
    {
        public TestModel()
        {
            this.Images = new List<PoleImage>();
        }

        public Guid ID { get; set; }

        private string surname;

        public string Surname
        {
            get
            {
                if (!string.IsNullOrEmpty(this.surname))
                {
                    value = value.ToUpperInvariant().Trim();
                }

                return value;
            }
            set
            {
                this.surname = value;
            }
        }

        private string forename;

        /// <summary>
        /// The forename of the officers
        /// </summary>
        [DataMember(Order = 2)]
        public string Forename
        {
            get
            {
                return PropertyFormat.TitleCase(this.forename);
            }
            set
            {
                this.forename = value;
            }
        }

        /// <summary>
        /// The collar number of the officer
        /// </summary>
        [DataMember(Order = 3)]
        public string CollarNumber { get; set; }

        private string rank;

        /// <summary>
        /// The rank of the officer
        /// </summary>
        [DataMember(Order = 4)]
        public string Rank
        {
            get
            {
                return PropertyFormat.TitleCase(this.rank);
            }
            set
            {
                this.rank = value;
            }
        }

        private string force;

        /// <summary>
        /// The force the officer belongs to
        /// </summary>
        [DataMember(Order = 5)]
        public string Force
        {
            get
            {
                return PropertyFormat.TitleCase(this.force);
            }
            set
            {
                this.force = value;
            }
        }

        private string division;

        /// <summary>
        /// The division or area the officer belongs to
        /// </summary>
        [DataMember(Order = 6)]
        public string Division
        {
            get
            {
                return PropertyFormat.TitleCase(this.division);
            }
            set
            {
                this.division = value;
            }
        }

        private string department;

        /// <summary>
        /// The department the officer belongs to
        /// </summary>
        [DataMember(Order = 7)]
        public string Department
        {
            get
            {
                return PropertyFormat.TitleCase(this.department);
            }
            set
            {
                this.department = value;
            }
        }

        private string role;

        /// <summary>
        /// The role the officer is in
        /// </summary>
        [DataMember(Order = 8)]
        public string Role
        {
            get
            {
                return PropertyFormat.TitleCase(this.role);
            }
            set
            {
                this.role = value;
            }
        }

        /// <summary>
        /// The telephone number of the officer
        /// </summary>
        [DataMember(Order = 9)]
        public string Telephone { get; set; }

        /// <summary>
        /// The first mobile number of the officer
        /// </summary>
        [DataMember(Order = 10)]
        public string Mobile1 { get; set; }

        /// <summary>
        /// The second mobile number of the officer
        /// </summary>
        [DataMember(Order = 11)]
        public string Mobile2 { get; set; }

        /// <summary>
        /// The email address for the officer
        /// </summary>
        [DataMember(Order = 12)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is current officer.
        /// </summary>
        [DataMember(Order = 13)]
        public bool IsCurrentOfficer { get; set; }

        /// <summary>
        /// Gets or Sets the list of images for an officer
        /// </summary>
        [DataMember(Order = 14)]
        public List<PoleImage> Images { get; set; }

        /// <summary>
        /// ID of the event item
        /// </summary>   
        [DataMember(Order = 15)]
        public string UPN { get; set; }

        /// <summary>
        /// Gets or sets the station the office is based at.
        /// </summary>
        [DataMember(Order = 16)]
        public string Station { get; set; }

        /// <summary>
        /// Gets or sets a list of system identifiers
        /// </summary>
        [DataMember(Order = 17)]
        public List<SystemIdentifier> SystemIdentifiers { get; set; }

        /// <summary>
        /// Gets a formatted name.
        /// </summary>
        /// <returns>Formatted name</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            if (String.IsNullOrWhiteSpace(this.CollarNumber) == false)
            {
                sb.Append(this.CollarNumber);
                sb.Append(" ");
            }

            if (String.IsNullOrWhiteSpace(this.Surname) == false)
            {
                sb.Append(this.Surname);
                if (String.IsNullOrWhiteSpace(this.Forename) == false)
                {
                    sb.Append(", ");
                }
            }

            if (String.IsNullOrWhiteSpace(this.Forename) == false)
            {
                sb.Append(this.Forename);
            }

            if (sb.Length == 0)
            {
                return "Unknown";
            }

            return sb.ToString();
        }

        /// <summary>
        /// Overiden to determine value type and reference type equality
        /// </summary>
        /// <param name="obj">The object to compare with the current object</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (base.Equals(obj)) return true; //reference equality

            var otherOfficer = obj as Officer;
            if (otherOfficer == null) return false;

            if (this.UPN == null && otherOfficer.UPN == null)
            {
                return true;
            }

            if (this.UPN == null && otherOfficer.UPN != null)
            {
                return false;
            }

            if (this.UPN != null && otherOfficer.UPN == null)
            {
                return false;
            }

            return this.UPN != null && this.UPN.Equals(otherOfficer.UPN);
        }

        /// <summary>
        /// Overwritten to match the equals override
        /// </summary>
        /// <returns>32 bit int</returns>
        public override int GetHashCode()
        {
            return !string.IsNullOrWhiteSpace(this.CollarNumber) ? this.CollarNumber.GetHashCode() : 0;
        }
    }
}
