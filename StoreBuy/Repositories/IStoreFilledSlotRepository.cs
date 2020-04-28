using StoreBuy.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoreBuy.Repositories
{
    public interface IStoreFilledSlotRepository : IGenericRepository<StoreFilledSlot>
    {
        bool CheckStoreSlotFilled(StoreInfo Store,string SlotTime, string SlotDate);
        void InsertSlot(StoreInfo StoreInfo, string SlotTime, string SlotDate);

    }
}