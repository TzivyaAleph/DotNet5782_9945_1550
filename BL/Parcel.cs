using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class Parcel
        {
            public int Id { get; set; }
            public CustomerParcel Recipient { get; set; }
            public CustomerParcel Sender { get; set; }
            public Weight Weight { get; set; }
            public Priority Priority { get; set; }
            public DroneParcel DroneInParcel { get; set; }
            public DateTime Requested { get; set; }//the time when the parcel has been made and ready.
            public DateTime Scheduled { get; set; }
            public DateTime PickedUp { get; set; }
            public DateTime Delivered { get; set; }

            public override string ToString()
            {
                string result = "";
                result += $"ID is {Id},\n";
                result += $"recipient is {Recipient}\n";
                result += $"sender is {Sender}\n";
                result += $"weight is {Weight},\n";
                result += $"priority is {Priority},\n";
                result += $"the drone who is carrying the parcel is {DroneInParcel},\n";
                result += $"requested time is {Requested}\n";
                result += $"scheduled time is {Scheduled}\n";
                result += $"picked up time is {PickedUp}\n";
                result += $"delivered time is {Delivered}\n";
                return result;
            }
        }
    }
 
}
