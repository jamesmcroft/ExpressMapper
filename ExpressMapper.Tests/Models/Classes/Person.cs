namespace ExpressMapper.Tests.Models.Classes
{
    public class Person : Contact
    {
        public Person()
        {
            this.IsOrganization = false;
            this.IsPerson = true;
        }

        public string Name { get; set; }
        public Person Relative { get; set; }
    }
}
