using NHibernate;
using StoreBuy.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoreBuy.Repositories
{
    public class StoreFilledSlotRepository : GenericRepository<StoreFilledSlot>, IStoreFilledSlotRepository
    {
        public StoreFilledSlotRepository(IUnitOfWork UnitOfWork) : base(UnitOfWork)
        { }

        public bool CheckStoreSlotFilled(StoreInfo Store, string SlotTime, string SlotDate)
        {
            try
            {
                var IsAvailabletemp = Session.CreateSQLQuery("Select * from StoreFilledSlot where SlotTime = " + SlotTime + " and SlotDate = " + SlotDate + " and StoreId = " + Store.StoreId).AddEntity(typeof(StoreFilledSlot));
                var IsAvailable = IsAvailabletemp.List<StoreFilledSlot>();
                if (IsAvailable.Count == 0)
                    return false;
                else
                    return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public void InsertSlot(StoreInfo Store, string SlotTime, string SlotDate)
        {
            try
            {
                Session.CreateSQLQuery("insert into StoreFilledSlot values ('" + SlotTime + "','" + SlotDate + "'," + Store.StoreId + ")").ExecuteUpdate();
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

    }
}