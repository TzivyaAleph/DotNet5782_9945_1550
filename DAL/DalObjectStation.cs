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
        /// gets a station and adds it to the array
        /// </summary>
        /// <param Name="s">Station to add</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddStation(Station s)
        {
            if (DataSource.Stations.Exists(station => station.Id == s.Id))
            {
                throw new ExistingObjectException($"station {s.Id} allready exists !!");
            }
            DataSource.Stations.Add(s);
        }

        /// <summary>
        /// coppies the station array
        /// </summary>
        /// <returns></returns the coppied array>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> CopyStationArray(Func<Station, bool> predicate = null)
        {
            List<Station> newList = new List<Station>(DataSource.Stations);
            if (predicate == null)
                return newList;
            return newList.Where(predicate);
        }

        /// <summary>
        /// puts a updated station in the stations list
        /// </summary>
        /// <param name="station">updated station to add</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStation(Station station)
        {
            if (!(DataSource.Stations.Exists(s => s.Id == station.Id)))
            {
                throw new UnvalidIDException($"id {station.Id} is not valid !!");
            }
            int index = DataSource.Stations.FindIndex(item => item.Id == station.Id);
            DataSource.Stations[index] = station;
        }

        /// <summary>
        /// searches for the station in the array by the Id
        /// </summary>
        /// <param Name="stationID"></param>
        /// <returns></returs the station were looking for>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station GetStation(int stationID)
        {
            if (!(DataSource.Stations.Exists(s => s.Id == stationID)))
            {
                throw new UnvalidIDException("id { s.Id}  is not valid !!");
            }
            int index = DataSource.Stations.FindIndex(item => item.Id == stationID);
            return DataSource.Stations[index];
        }

        /// <summary>
        /// creates list with all the available station and return the closest station in the list to the recieved drone.
        /// </summary>
        /// <param name="drone">for finding the closest </param>
        /// <param name="longtitude"></param>
        /// <param name="lattitude"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station GetClossestStation(double lattitude, double longtitude, List<Station> stations)
        {
            DO.Station minStation = new Station();
            double minDistance = Math.Sqrt(Math.Pow(lattitude - stations.First().Lattitude, 2) + Math.Pow(longtitude - stations.First().Longitude, 2));
            minStation = stations.First();
            foreach (var st in DataSource.Stations)
            {
                double distance = Math.Sqrt(Math.Pow(lattitude - st.Lattitude, 2) + Math.Pow(longtitude - st.Longitude, 2));
                if (minDistance > distance)
                {
                    minDistance = distance;
                    minStation = st;
                }
            }
            return minStation;
        }
    }
}
