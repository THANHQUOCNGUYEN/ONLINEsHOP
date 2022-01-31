﻿using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class MenuDao
    {
        ShopOnlineDbContext db = null;
        public MenuDao()
        {
            db = new ShopOnlineDbContext();
        }

        public List<Menu> ListByGroupId(int groupId)
        {
            return db.Menus.Where(x => x.Type == groupId && x.Status == true).OrderBy(x=>x.DisplayOrder).ToList();
        }
    }
}
