using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    public partial class DalObject : IDal
    {

        /// <summary>
        /// constructor
        /// </summary>
        public DalObject()
        {
            DataSource.Initialize();
        }

       


        //public void AddBaseStation(BaseStation b)
        //{
        //    throw new NotImplementedException();
        //}

        //public void AddClient(Client c)
        //{
        //    throw new NotImplementedException();
        //}

        //public void AddPackage(Package p)
        //{
        //    throw new NotImplementedException();
        //}

        //public void AddSkimmer(Quadocopter q)
        //{
        //    throw new NotImplementedException();
        //}

        //public void AssignPackageSkimmer(int idp, int idq)
        //{
        //    throw new NotImplementedException();
        //}

        //public List<BaseStation> BaseStationFreeCharging()
        //{
        //    throw new NotImplementedException();
        //}

        //public void CollectionPackage(int idp)
        //{
        //    throw new NotImplementedException();
        //}

        //public BaseStation GetBaseStation(int IDb)
        //{
        //    throw new NotImplementedException();
        //}

        //public IEnumerable<BaseStation> GetBaseStationList()
        //{
        //    throw new NotImplementedException();
        //}

        //public Client GetClient(int IDc)
        //{
        //    throw new NotImplementedException();
        //}

        //public IEnumerable<Client> GetClientList()
        //{
        //    throw new NotImplementedException();
        //}

        //public Package GetPackage(int idp)
        //{
        //    throw new NotImplementedException();
        //}

        //public IEnumerable<Package> GetPackageList()
        //{
        //    throw new NotImplementedException();
        //}

        //public IEnumerable<Quadocopter> GetQuadocopterList()
        //{
        //    throw new NotImplementedException();
        //}

        //public Quadocopter GetQuadrocopter(int IDq)
        //{
        //    throw new NotImplementedException();
        //}

        //public void PackageDelivery(int idp)
        //{
        //    throw new NotImplementedException();
        //}

        //public List<Package> PackagesWithoutSkimmer()
        //{
        //    throw new NotImplementedException();
        //}

        //public void SendingSkimmerForCharging(int idq, int idBS)
        //{
        //    throw new NotImplementedException();
        //}

        //public void SkimmerRelease(int idq, int IdBS)
        //{
        //    throw new NotImplementedException();
        //}
    }

    
}



