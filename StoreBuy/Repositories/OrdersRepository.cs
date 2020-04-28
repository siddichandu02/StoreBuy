using NHibernate;
using StoreBuy.Domain;
using StoreBuy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace StoreBuy.Repositories
{
    public class OrdersRepository : GenericRepository<Orders>, IOrdersRepository
    {

        public OrdersRepository(IUnitOfWork UnitOfWork) : base(UnitOfWork)
        { }

    
        public bool Notify(Orders Order,List<ItemCatalogueModel> Items)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("siddichandu03@gmail.com");
                message.To.Add(new MailAddress("siddichandu02@gmail.com"));
                message.Subject = "OrderDetails";
                string MailBody=""+Order.OrderId+" "+Order.SlotDate+""+Order.SlotTime+"\n";
                foreach(ItemCatalogueModel Item in Items)
                {
                    MailBody += " " + Item.ItemName + " " + Item.Quantity+"\n";
                }
                message.Body = MailBody;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com";
                smtp.Credentials = new NetworkCredential("siddichandu03@gmail.com", "chandu@12345");
                smtp.EnableSsl = true;
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public int GetSlotCount(Orders order)
        {
            try
            {
                var SlotsFillCount = Session.Query<Orders>().Where(x => x.Store == order.Store && x.SlotTime == order.SlotTime && x.SlotDate == order.SlotDate).Count();
                return SlotsFillCount;
            }
            catch (Exception exception)
            {
                throw exception;
            }

        }

    }
}