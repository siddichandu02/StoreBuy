using StoreBuy.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoreBuy.Repositories
{
    public interface IOTPRepository:IGenericRepository<OTPValidator>
    {
        void InsertOTP(long UserID, long OTP);
        void DeleteExpiredOTP();
         long ReturnByUserId(long UserId);
    }
}