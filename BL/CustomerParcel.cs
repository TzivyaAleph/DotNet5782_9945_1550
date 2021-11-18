namespace IBL.BO
{
    public class CustomerParcel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            string result = "";
            result += $"id is {Id}\n";
            result += $"name is {Name}\n";
            return result;
        }
    }
}