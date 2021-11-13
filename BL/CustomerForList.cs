using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class CustomerForList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int ParcelProvided { get; set; }
        public int ParcelNotProvided { get; set; }
        public int ParcelRecieved { get; set; }
        public int ParcelOnTheWay { get; set; }
    }
}
