﻿using System;
using DalApi;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using DO;
using System.Runtime.CompilerServices;

namespace Dal
{
    /// <summary>
    /// 
    /// </summary>
     sealed partial class DalObject : IDal
    {
        #region singleton
        //lazt<T> is doing a lazy initialzation and hi sdefualt is thread safe
        internal static readonly Lazy<DalObject> singleInstance = new Lazy<DalObject>(() => new DalObject());
        public static DalObject Instance
        {
            get
            {
                return singleInstance.Value;
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        private DalObject()
        {
            DataSource.Initialize();
        }
        #endregion singleton

        /// <summary>
        /// calculate the distance between two points specified by latitude and longitude.
        /// https://stackoverflow.com/questions/27928/calculate-distance-between-two-latitude-longitude-points-haversine-formula
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="lon1"></param>
        /// <param name="lat2"></param>
        /// <param name="lon2"></param>
        /// <returns>the distance</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double getDistanceFromLatLonInKm(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371; // Radius of the earth in km
            var dLat = deg2rad(lat2 - lat1);  // deg2rad below
            var dLon = deg2rad(lon2 - lon1);
            var a =
              Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
              ;
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c; // Distance in km
            return d;
        }
        /// <summary>
        /// converts degree to radians
        /// https://stackoverflow.com/questions/27928/calculate-distance-between-two-latitude-longitude-points-haversine-formula
        /// </summary>
        /// <param name="deg"></param>
        /// <returns>the radians</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double deg2rad(double deg)
        {
            return deg * (Math.PI / 180);
        }
    }
}



