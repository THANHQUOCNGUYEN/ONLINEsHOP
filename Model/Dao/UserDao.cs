using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Model.EF;
using PagedList;
using PagedList.Mvc;
namespace Model.Dao
{
    public class UserDao
    {
        ShopOnlineDbContext db = null;
        public UserDao()
        {
            db = new ShopOnlineDbContext();
        }
        public long Insert(User entity)
        {
            db.Users.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public bool Update(User entity)
        {
            try
            {
                var user = db.Users.Find(entity.ID);
                user.Name = entity.Name;
                if(!string.IsNullOrEmpty(entity.Password))
                {
                    user.Password = entity.Password;
                }
                //không show password
                user.Address = entity.Address;
                user.Email = entity.Email;
                user.ModifiedBy = entity.ModifiedBy;
                user.ModifiedDate = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            catch(Exception)
            {
                //logging
                return false;
            }
           
        }
        //login facebook
        public long InsertForFacebook(User entity)
        {
            var user = db.Users.SingleOrDefault(x => x.UserName == entity.UserName);
            if(user == null)
            {
                db.Users.Add(entity);
                db.SaveChanges();

                return entity.ID;

            }
            else
            {
                return user.ID;
            }
        }
        public IEnumerable<User> ListAllPaging(string searchString,int page,int pageSize)
        {
            IQueryable<User> model = db.Users;
            if(!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.UserName.Contains(searchString) || x.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page,pageSize);
        }
        public User GetById(string userName)
        {
            return db.Users.SingleOrDefault(x=>x.UserName == userName);//lay 1 ban ghi don
        }

        public User ViewDetail(int id)
        {
            return db.Users.Find(id);
        }
        public int Login(string userName,string Password,bool isLoginAdmin = false)
        {
            var result = db.Users.SingleOrDefault(x => x.UserName == userName);
            if (result == null)
                return 0; //Tài khoản không tồn tại
            else
            {
                if(isLoginAdmin == true)
                {
                    if (result.GroupID == CommonConstants.ADMIN_GROUP || result.GroupID == CommonConstants.MOD_GROUP)
                    {

                        if (result.Status == false)
                            return -1; // Tài khoản đang bị khóa
                        else
                        {
                            if (result.Password == Password)
                            {
                                return 1;
                            }
                            else
                            {
                                return -2;//Mật khẩu không đúng
                            }
                        }
                    }
                    else
                    {
                        return -3;//khong phai thang admin va mod
                    }
                }

                else
                {
                    if (result.Status == false)
                        return -1; // Tài khoản đang bị khóa
                    else
                    {
                        if (result.Password == Password)
                        {
                            return 1;
                        }
                        else
                        {
                            return -2;//Mật khẩu không đúng
                        }
                    }
                }
            }

        }

        public bool Delete(int id)
        {
            try
            {
                var user = db.Users.Find(id);
                db.Users.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch(Exception )
            {
                return false;
            }
            
        }

        public bool ChangeStatus(long id)
        {
            var user = db.Users.Find(id);
            user.Status = !user.Status;
            db.SaveChanges();
            return user.Status;
        }

        public bool CheckUserName(string userName)
        {
            return db.Users.Count(x => x.UserName == userName) > 0;
        }

        public bool CheckEmail(string Email)
        {
            return db.Users.Count(x => x.Email == Email) > 0;
        }

        //Lấy danh sách các credential
        public List<string> GetListCredential(string userName)
        {
            //neu dung khoa chinh se don gian hon trong viec ket noi ban rat nhieu 
            var user = db.Users.Single(x => x.UserName == userName);
            var data = (from a in db.Credentials
                        join b in db.UserGroups on a.UserGroupID equals b.ID
                        join c in db.Roles on a.RoleID equals c.ID
                        where b.ID == user.GroupID
                        select new
                        {
                            RoleID = a.RoleID,
                            UserGroupID = a.UserGroupID
                        }).AsEnumerable().Select(x => new Credential() {
                            RoleID = x.RoleID,
                            UserGroupID = x.UserGroupID
                        });

            return data.Select(x=>x.RoleID).ToList();
        }
    }
}
