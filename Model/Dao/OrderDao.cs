using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class OrderDao
    {
        ShopOnlineDbContext db = null;
        public OrderDao()
        {
            db = new ShopOnlineDbContext();
        }
        public long Insert(Order order)
        {
            db.Orders.Add(order);
            db.SaveChanges();//save xong thi no moi lay duoc ID
            return order.ID;//return true hay false
        }


    }
}
