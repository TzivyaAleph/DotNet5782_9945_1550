using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    public partial class DalObject
    {
        /// <addDrone>
        /// add new drone and updates 
        /// </summary>
        /// <returns></returns>
        public void AddDrone(Drone d)
        {
            if (DataSource.Drones.Exists(drone => drone.Id == d.Id))
            {
                throw new ExistingObjectException($"drone {d.Id} allready exists !!");
            }
            DataSource.Drones.Add(d);
        }

        /// <summary>
        /// coppies the drone array
        /// </summary>
        /// <returns></returns the coppied array>
        public IEnumerable<Drone> CopyDroneArray()
        {
            List<Drone> newList = new List<Drone>(DataSource.Drones);
            return newList;
        }

        /// <summary>
        /// searches for the drone in the array by the Id
        /// </summary>
        /// <param Name="droneID"></param>
        /// <returns></returnsthe drone were looking for>
        public Drone GetDrone(int droneID)
        {
            if (!(DataSource.Drones.Exists(d => d.Id == droneID)))
            {
                throw new UnvalidIDException("id { d.Id}  is not valid !!");
            }
            int index = DataSource.Drones.FindIndex(item => item.Id == droneID);
            return DataSource.Drones[index];
        }

        /// <summary>
        /// creates a new array with the drone's electricity use  
        /// </summary>
        /// <returns>the new array</returns>
        public double[] GetElectricityUse()
        {
            double[] electricityUse = new double[5];
            electricityUse[0] = DataSource.Config.Avalaible;
            electricityUse[1] = DataSource.Config.Light;
            electricityUse[2] = DataSource.Config.Medium;
            electricityUse[3] = DataSource.Config.Heavy;
            electricityUse[4] = DataSource.Config.ChargingRate;
            return electricityUse;
        }

        /// <summary>
        /// updates the drone in the list
        /// </summary>
        /// <param name="drone"></param>
        public void UpdateDrone(Drone drone)
        {
            if (!(DataSource.Drones.Exists(d => d.Id == drone.Id)))
            {
                throw new UnvalidIDException("id { d.Id}  is not valid !!");
            }
            int index = DataSource.Drones.FindIndex(item => item.Id == drone.Id);
            DataSource.Drones[index] = drone;
        }
    }
}
