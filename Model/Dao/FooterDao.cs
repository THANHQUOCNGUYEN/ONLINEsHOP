using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class FooterDao
    {
        ShopOnlineDbContext db = null;
        public FooterDao()
        {
            db = new ShopOnlineDbContext();
        }

        public Footer GetFooter()
        {
            return db.Footers.SingleOrDefault(x => x.Status == true);
        }
    }
}
