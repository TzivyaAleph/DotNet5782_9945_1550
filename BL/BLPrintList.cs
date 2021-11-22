using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL;

namespace BL
{
    public partial class BL
    {
        /// <summary>
        /// createse list of stations and update each fields by dal data.
        /// </summary>
        /// <returns>the creates list</returns>
        public IEnumerable<StationForList> GetStationList()
        {
            List<StationForList> stationsToReturn = new List<StationForList>();
            StationForList stationToAdd = new();
            foreach(var stat in myDal.CopyStationArray())
            {
                stationToAdd.Id = stat.ID;
                stationToAdd.Name = stat.StationName;
                stationToAdd.AvailableChargingSlots = stat.ChargeSlots;
                // counts the drones that are charging in the current station
                int countNumOfDronesInStation = 0;
                foreach (var dc in myDal.GetDroneChargeList())
                {
                    if (dc.StationID == stat.ID)
                        countNumOfDronesInStation++;
                }
                Station station = new();
                stationToAdd.UnAvailableChargingSlots = countNumOfDronesInStation - stat.ChargeSlots;
                stationsToReturn.Add(stationToAdd);
            }
            return stationsToReturn;
        }
    }
}
