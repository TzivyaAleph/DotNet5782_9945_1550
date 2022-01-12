using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using System.Runtime.CompilerServices;

namespace Dal
{
    partial class DalObject
    {
        /// <summary>
        /// recieves a drone and a station and sends the drone to a chargeSlot in that station
        /// </summary>
        /// <param Name="d"></param>
        /// <param Name="s"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SendDroneToChargeSlot(Drone d, Station s)
        {
            s.ChargeSlots--;
            UpdateStation(s);
            DroneCharge dc = new DroneCharge();
            dc.DroneID = d.Id;
            dc.StationID = s.Id;

        }

        /// <summary>
        /// recieves a drone and a station and releses the drone from the chargeSlot
        /// </summary>
        /// <param Name="d"></param>
        /// <param Name="s"></param>
        /// <param Name="dc"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ReleaseDrone(Drone d, Station s)
        {
            s.ChargeSlots++;
            UpdateStation(s);//updates the available charge slots in the current staition
            if (!(DataSource.DroneCharges.Exists(dc => dc.DroneID == d.Id && dc.StationID == s.Id)))
            {
                throw new UnvalidIDException("dc is not valid !!");
            }
            int index = DataSource.DroneCharges.FindIndex(item => item.StationID == s.Id && item.DroneID == d.Id);
            DroneCharge help = DataSource.DroneCharges[index];
            help.DroneID = 0;
            help.StationID = 0;
            help.SentToCharge = null;
            DataSource.DroneCharges[index] = help;
        }

        /// <summary>
        /// searches for the droneCharge in the array by the station Id and drone id
        /// </summary>
        /// <param Name="stationID"></param>
        /// <returns></returs the drone charge object were looking for>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public DroneCharge GetDroneCharge(int stationID, int droneID)
        {
            DroneCharge droneChargeToReturn = new DroneCharge();
            //searches the station with the recieved id.
            foreach (DroneCharge dc in DataSource.DroneCharges)
                if (dc.StationID == stationID && dc.DroneID == droneID)
                {
                    droneChargeToReturn = dc;
                }
            return droneChargeToReturn;
        }

        /// <summary>
        /// returns the list of drone charges
        /// </summary>
        /// <returns>list of drone charges</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneCharge> GetDroneChargeList()
        {
            List<DroneCharge> newList = new List<DroneCharge>(DataSource.DroneCharges);
            return newList;
        }
    }
}
