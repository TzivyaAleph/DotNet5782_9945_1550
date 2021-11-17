﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    public partial class DalObject
    {
        /// <summary>
        /// recieves a drone and a station and sends the drone to a chargeSlot in that station
        /// </summary>
        /// <param Name="d"></param>
        /// <param Name="s"></param>
        public void SendDroneToChargeSlot(Drone d, Station s)
        {
            s.ChargeSlots--;
            UpdateStation(s);
            DroneCharge dc = new DroneCharge();
            dc.DroneID = d.ID;
            dc.StationID = s.ID;
            DataSource.DroneCharges.Add(dc);
        }

        /// <summary>
        /// recieves a drone and a station and releses the drone from the chargeSlot
        /// </summary>
        /// <param Name="d"></param>
        /// <param Name="s"></param>
        /// <param Name="dc"></param>
        public void ReleaseDrone(Drone d, Station s, DroneCharge dc)
        {
            s.ChargeSlots++;
            UpdateStation(s);//updates the available charge slots in the current staition
            if (!(DataSource.DroneCharges.Exists(dc => dc.DroneID == d.ID && dc.StationID == s.ID)))
            {
                throw new UnvalidIDException("dc is not valid !!");
            }
            int index = DataSource.DroneCharges.FindIndex(item => item.StationID == s.ID && item.DroneID == d.ID);
            DroneCharge help = DataSource.DroneCharges[index];
            help.DroneID = 0;
            help.StationID = 0;
            DataSource.DroneCharges[index] = help;
        }
    }
}
