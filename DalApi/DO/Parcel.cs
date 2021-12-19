using System;
using System.Collections.Generic;
using System.Text;


namespace DO
{
    public struct Parcel
    {
        public int Id { get; set; }
        public int SenderID { get; set; }//the sender of the parcel
        public int TargetID { get; set; }//the costumer rwho orded the parcel
        public Weight Weight { get; set; }//the weight of the parcel
        public Priority Priority { get; set; }
        public DateTime? Requested { get; set; }//the time when the parcel has been made and ready.
        public int DroneID { get; set; }//the drone who takes this parcel
        public DateTime? Scheduled { get; set; }
        public DateTime? PickedUp { get; set; }
        public DateTime? Delivered { get; set; }

        public override string ToString()
        {
            string result = "";
            result += $"parcel ID is: {Id} \n";
            result += $"sender ID is: {SenderID} \n";
            result += $"target ID is: {TargetID} \n";
            result += $"parcel weight is: {Weight} \n";
            result += $"drone ID is: {DroneID} \n";
            result += $"priority is: {Priority} \n";
            result += $"requested at: {Requested} \n";
            result += $"scheduled at: {Scheduled} \n";
            result += $"pickedUp at: {PickedUp} \n";
            result += $"delivered at: {Delivered} \n";
            return result;
        }
    }
}

