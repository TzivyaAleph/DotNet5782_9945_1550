
namespace IBL.BO
{
    public class ParcelCustomer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Weight Weight { get; set; }
        public Priority Priority {get;set;}
        public Status Status { get; set; }
        public CustomerParcel CustomerParcel { get; set; }
    }                                                                                                                                                                                                               
}