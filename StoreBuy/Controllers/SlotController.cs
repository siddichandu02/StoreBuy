using StoreBuy.Domain;
using StoreBuy.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StoreBuy.Controllers
{
    public class SlotController : ApiController
    {

        IStoreFilledSlotRepository SlotRepository = null;IStoreSearchRepository StoreRepository;
        
        public SlotController(IStoreFilledSlotRepository SlotRepository,IStoreSearchRepository StoreRepository)
        {
            this.StoreRepository = StoreRepository;
            this.SlotRepository = SlotRepository;           
        }
        public bool Get(long StoreId,string SlotDate,string SlotTime)
        {
            StoreInfo Store = StoreRepository.GetById(StoreId); 
           bool isFilled= SlotRepository.CheckStoreSlotFilled(Store,SlotTime, SlotDate);
            return isFilled;
        }
    
    }
}
