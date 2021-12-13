namespace IBL
{
    namespace BO
    {
        public class CustomerParcel
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public override string ToString()
            {
                string result = "";
                result += $"name is {Name}\n";
                result += $"id is {Id}\n";
                return result;
            }
        }
    }
}