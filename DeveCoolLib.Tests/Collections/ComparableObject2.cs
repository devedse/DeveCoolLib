namespace DeveCoolLib.Tests.Collections
{
    public class ComparableObject2
    {
        public ComparableObject2(int key, string firstName, string lastName, int length)
        {
            Key = key;
            FirstName = firstName;
            LastName = lastName;
            Length = length;
        }
        public int Key { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Length { get; set; }

        public override string ToString()
        {
            return $"{Key} {FirstName} {LastName} {Length}";
        }
    }
}
