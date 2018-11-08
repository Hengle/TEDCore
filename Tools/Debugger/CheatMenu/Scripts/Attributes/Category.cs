
namespace TEDCore.UnitTesting
{
    public class Category : System.Attribute
    {
        public string Name;
        public Category(string name)
        {
            Name = name;
        }
    }
}