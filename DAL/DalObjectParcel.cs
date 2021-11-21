﻿using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{
    public partial class DalObject
    {
        /// <summary>
        /// coppies the parcel array
        /// </summary>
        /// <returns></returns the coppied array>
        public IEnumerable<Parcel> CopyParcelArray()
        {
            List<Parcel> newLIst = new List<Parcel>(DataSource.Parcels);
            return newLIst;
        }

        /// <summary>
        /// recieves a parcel and updates the parcels delivery time
        /// </summary>
        /// <param name="p"></param>
        public void Delivered(Parcel p)
        {
            p.Delivered = DateTime.Now;
            UpdateParcel(p);
        }

        /// <summary>
        /// searches for the parcel in the array by the Id
        /// </summary>
        /// <param Name="parcelID"></param>
        /// <returns></returns parcel were looking for>
        public Parcel GetParcel(int id)
        {
            Parcel parcelToReturn = default;
            //searches the customer by the id
            if (!(DataSource.Parcels.Exists(p => p.ID == id)))
            {
                throw new UnvalidIDException($"id {id} is not valid !!");
            };
            parcelToReturn = DataSource.Parcels.Find(c => c.ID == id);
            return parcelToReturn;
        }

        /// <summary>
        /// gets a parcel and adds it to the list
        /// </summary>
        /// <param Name="p"></param>
        /// <returns></returns>
        public int AddParcel(Parcel p)
        {
            int id = ++DataSource.Config.RunningParcelID;
            p.ID = id;
            DataSource.Parcels.Add(p);
            return id;//return the id of the new  parcel.
        }

        /// <summary>
        /// recieves a parcel and a drone and attributes the parcel to the drone
        /// </summary>
        /// <param Name="p"></param>
        /// <param Name="d"></param>
        public void AttributingParcelToDrone(Parcel p, Drone d)//targil1
        {
            p.DroneID = d.ID;
            p.Requested = DateTime.Now;
            UpdateParcel(p);
        }

        /// <summary>
        /// searches for the non atributted parcels and coppies them into a new list.
        /// </summary>
        /// <returns></returns the new array>
        public IEnumerable<Parcel> FindNotAttributedParcels()
        {
            List<Parcel> notAttributed = new List<Parcel>();//new list to hold non attributed parcels
            foreach (Parcel p in DataSource.Parcels)//searches for the non attributed parcels
            {
                if (p.DroneID == 0)
                {
                    notAttributed.Add(p);
                }
            }
            return notAttributed;
        }

        /// <summary>
        /// recieves a parcel and updates the parcels picked up time
        /// </summary>
        /// <param Name="p"></param>
        public void PickedUp(Parcel p, Drone d)
        {
            p.PickedUp = DateTime.Now;//updates the parcels pickedUp time
            UpdateParcel(p);
        }

        /// <summary>
        /// updates a parcel in the list
        /// </summary>
        /// <param name="parcel"></param>
        public void UpdateParcel(Parcel parcel)
        {
            if (!(DataSource.Parcels.Exists(p => p.ID == parcel.ID)))
            {
                throw new UnvalidIDException("id { p.Id}  is not valid !!");
            }
            int index = DataSource.Parcels.FindIndex(item => item.ID == parcel.ID);
            DataSource.Parcels[index] = parcel;
        }


    }
}
