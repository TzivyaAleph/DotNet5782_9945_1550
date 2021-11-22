
namespace IBL.BO
{
    public class ParcelCustomer
    {
        public int Id { get; set; }
        public Weight Weight { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }
        public CustomerParcel CustomerParcel { get; set; }

        public override string ToString()
        {
            string result = " ";
            result += $"id is {Id}\n";
            result += $"name is {Name}\n";
            result += $"weight is {Weight}\n";
            result += $"priority is {Priority}\n";
            result += $"status is {Status}\n";
            result += $"sender is {CustomerParcel}\n";
            return result;
        }
    }
}