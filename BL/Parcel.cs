using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
   public class Parcel
    {
        public int Id { get; set; }
        public CustomerParcel Recipient { get; set; }
        public CustomerParcel Sender { get; set; }

    }
}
