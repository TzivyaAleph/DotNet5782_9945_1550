using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BO
{
    public class ParcelForList
    {
        public int Id { get; set; }
        public string Sender { get; set; }
        public string Reciever { get; set; }
        public Weight Weight { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }

        public override string ToString()
        {
            string result = "";
            result += $"ID is: {Id}\n";
            result += $"recipient is: {Reciever}\n";
            result += $"sender is: {Sender}\n";
            result += $"weight is: {Weight}\n";
            result += $"priority is: {Priority}\n";
            result += $"the parcel is: {Status}\n";
            return result;
        }
    }
}

