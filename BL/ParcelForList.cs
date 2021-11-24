using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
   public  class ParcelForList
    {
        public int Id { get; set; }
        public string Sender { get; set; }
        public string Reciever { get; set; }
        public Weight Weight { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }
    }
}
