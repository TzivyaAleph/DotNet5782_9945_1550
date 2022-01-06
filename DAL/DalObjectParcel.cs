using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    partial class DalObject
    {
        /// <summary>
        /// coppies the parcel array
        /// </summary>
        /// <returns></returns the coppied array>
        public IEnumerable<Parcel> CopyParcelArray(Func<Parcel, bool> predicate = null)
        {
            List<Parcel> newLIst = new List<Parcel>(DataSource.Parcels);
            if (predicate == null)
                return newLIst;
            return newLIst.Where(predicate);
        }

        /// <summary>
        /// recieves a parcel and updates the parcels delivery time
        /// </summary>
        /// <param name="p"></param>
        public void Delivered(Parcel p)
        {
            p.Delivered = DateTime.Now;
            p.DroneID = 0;
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
            if (!(DataSource.Parcels.Exists(p => p.Id == id)))
            {
                throw new UnvalidIDException($"id {id} is not valid !!");
            };
            parcelToReturn = DataSource.Parcels.Find(c => c.Id == id);
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
            p.Id = id;
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
            p.DroneID = d.Id;
            p.Requested = DateTime.Now;
            UpdateParcel(p);
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
            if (!(DataSource.Parcels.Exists(p => p.Id == parcel.Id)))
            {
                throw new ExistingObjectException($"id { parcel.Id}  does not exist!!");
            }
            int index = DataSource.Parcels.FindIndex(item => item.Id == parcel.Id);
            DataSource.Parcels[index] = parcel;
        }

    }
}
