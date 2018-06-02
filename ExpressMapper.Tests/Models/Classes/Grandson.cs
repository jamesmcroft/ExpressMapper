namespace ExpressMapper.Tests.Models.Classes
{
    public class Grandson
    {
        public int MyInt { get; set; }
        public string MyString { get; set; }

        public static Grandson CreateOne()
        {
            return new Grandson
            {
                MyInt = 3,
                MyString = "Grandson"
            };
        }
    }
}