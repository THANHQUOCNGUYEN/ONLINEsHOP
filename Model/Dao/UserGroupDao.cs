using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class UserGroupDao
    {
        ShopOnlineDbContext db = null;
        public UserGroupDao()
        {
            db = new ShopOnlineDbContext();
        }
        public string Create(UserGroup grUs)
        {
            db.UserGroups.Add(grUs);
            db.SaveChanges();

            return grUs.ID;
        }

        public void Delete(int id)
        {
            var grUs = db.UserGroups.Find(id);
            db.UserGroups.Remove(grUs);
            db.SaveChanges();
        }
        //Phân Quyền cho nhóm người dùng

        public List<UserGroup> Getlist()
        {

            return db.UserGroups.ToList();
        }


        public List<Credential> GetListCredential(string groupID)
        {
            var lstCredentials = db.Credentials.Where(s => s.UserGroupID == groupID).ToList();

            return lstCredentials;
        }

        public void Save(List<Credential> credentials,string userGroup)
        {
            db.Credentials.RemoveRange(db.Credentials.Where(s=>s.UserGroupID == userGroup));
            foreach(var item in credentials)
            {
                db.Credentials.Add(item);
            }

            db.SaveChanges();
        }
    }
}
