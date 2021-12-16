using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BO
{
    public class DroneParcel
    {
        public int Id { get; set; }
        public double Battery { get; set; }
        public Location Location { get; set; }

        public override string ToString()
        {
            string result = "";
            result += $"ID is: {Id}\n";
            result += $"battery level is: {Battery}\n";
            result += $"location is:\n {Location}";
            return result;
        }
    }
}

