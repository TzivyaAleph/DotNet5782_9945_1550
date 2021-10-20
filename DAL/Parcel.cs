using System;
using System.Collections.Generic;
using System.Text;

namespace IDAL
{
    namespace DO
    {
        public struct Parcel
        {
            public int ID { get;  set; }
            public int senderID { get;  set; }//the sender of the parcel
            public int targetID { get;  set; }//the costumer rwho orded the parcel
            public WeightCategories weight { get;  set; }//the weight of the parcel
            public Priorities priority { get; set; }
            public DateTime requested { get; set; }//the time when the parcel has been made and ready.
            public int droneID { get; set; }//the drone who takes this parcel
            public DateTime scheduled { get; set; }
            public DateTime pickedUp { get; set; }
            public DateTime delivered { get; set; }

            public override string ToString()
            {
                string result = "";
                result += $"parcel ID is: {ID} \n";
                result += $"sender ID is: {senderID} \n";
                result += $"target ID is: {targeID} \n";
                result += $"parcel weight is: {weight} \n";
                result += $"drone ID is: {droneID} \n";
                result += $"priority is: {priority} \n";
                result += $"requested at: {requested} \n";
                result += $"scheduled at: {scheduled} \n";
                result += $"pickedUp at: {pickedUp} \n";
                result += $"delivered at: {delivered} \n";
                return result;
            }
        }
    }
}
