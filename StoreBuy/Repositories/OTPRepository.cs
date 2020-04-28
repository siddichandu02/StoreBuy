using StoreBuy.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoreBuy.Repositories
{
    public class OTPRepository:GenericRepository<OTPValidator>,IOTPRepository
    {
        public OTPRepository(IUnitOfWork UnitOfWork) : base(UnitOfWork)
        {

        }

        public void InsertOTP(long UserId,long OTP)
        {
            Session.CreateSQLQuery("Insert into OTPValidator values (" + UserId + "," + OTP + ",DEFAULT)").ExecuteUpdate();
        }
        public long ReturnByUserId(long UserId)
        {
            var Query=Session.CreateSQLQuery("select * from OTPValidator where UserId= "+UserId).AddEntity(typeof(OTPValidator));
            var OTP=Query.List<OTPValidator>();
            if(OTP.Count==0)
            {
                return -1;
            }
            else
            {
                return OTP[0].CurrentOtp;
            }
        }

        public void DeleteExpiredOTP()
        {
            Session.CreateSQLQuery("DELETE FROM OTPValidator  WHERE TimeStamp < DATEADD(mi, -1, GETDATE())").ExecuteUpdate();
        }
    }
}