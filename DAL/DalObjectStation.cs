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
        /// gets a station and adds it to the array
        /// </summary>
        /// <param Name="s">Station to add</param>
        public void AddStation(Station s)
        {
            if (DataSource.Stations.Exists(station => station.ID == s.ID))
            {
                throw new ExistingObjectException($"station {s.StationName} allready exists !!");
            }
            DataSource.Stations.Add(s);
        }

        /// <summary>
        /// coppies the station array
        /// </summary>
        /// <returns></returns the coppied array>
        public IEnumerable<Station> CopyStationArray()
        {
            List<Station> newList = new List<Station>(DataSource.Stations);
            return newList;
        }

        /// <summary>
        /// creates an array by searching for available charge slots in the station list.
        /// </summary>
        /// <returns></returns the new list>
        public IEnumerable<Station> FindAvailableStations()
        {
            List<Station> availableStations = new List<Station>();//new list to hold Available Stations
            for (int i = 0; i < DataSource.Stations.Count; i++)
                if (DataSource.Stations[i].ChargeSlots > 0)
                {
                    availableStations.Add(DataSource.Stations[i]);
                }
            return availableStations;
        }

        /// <summary>
        /// find closest station to a recieved customer
        /// </summary>
        /// <param name="customerTemp"></param>
        /// <returns>return the closeset station</returns>
        public Station GetClossestStation(Customer customerTemp)
        {
            IDAL.DO.Station minStation = new IDAL.DO.Station();
            double minDistance = Math.Sqrt((Math.Pow(customerTemp.Lattitude - DataSource.Stations.First().Lattitude, 2) + Math.Pow(customerTemp.Longtitude - DataSource.Stations.First().Longitude, 2))); ;
            foreach (var st in DataSource.Stations)
            {
                double distance = Math.Sqrt((Math.Pow(customerTemp.Lattitude - st.Lattitude, 2) + Math.Pow(customerTemp.Longtitude - st.Longitude, 2)));
                if (minDistance > distance)
                {
                    minDistance = distance;
                    minStation = st;
                }
            }
            return minStation;
        }

        /// <summary>
        /// searches for the station in the array by the Id
        /// </summary>
        /// <param Name="stationID"></param>
        /// <returns></returs the station were looking for>
        public Station GetStation(int stationID)
        {
            if (!(DataSource.Stations.Exists(s => s.ID == stationID)))
            {
                throw new UnvalidIDException("id { s.Id}  is not valid !!");
            }
            int index = DataSource.Stations.FindIndex(item => item.ID == stationID);
            return DataSource.Stations[index];
        }

        /// <summary>
        /// updates a station in the stations' list
        /// </summary>
        /// <param name="station"></param>
        public void UpdateStation(Station station)
        {
            if (!(DataSource.Stations.Exists(s => s.ID == station.ID)))
            {
                throw new UnvalidIDException("id { s.Id}  is not valid !!");
            }
            int index = DataSource.Stations.FindIndex(item => item.ID == station.ID);
            DataSource.Stations[index] = station;
        }

    }
}