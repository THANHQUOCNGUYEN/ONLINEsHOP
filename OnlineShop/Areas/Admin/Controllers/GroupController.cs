using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class GroupController : BaseController
    {
        // GET: Group
        ShopOnlineDbContext context = new ShopOnlineDbContext();
        public ActionResult Index()
        {
            var lstGroup = context.UserGroups.ToList();
            return View(lstGroup);
        }
        [HttpPost]
        public JsonResult getList(string id)
        {
            var lst = context.Credentials.Where(s => s.UserGroupID == id).Select(s => s.RoleID);

            return Json(new{
                status = true,
                data = lst
            });
        }
        public ActionResult PhanQuyen(string id)
        {
            var lst = context.Roles.ToList();
            ViewBag.id = id;
            return View(lst);
        }

        [HttpPost]
        public JsonResult updateRoles(string Credentials)
        {
            var c = new JavaScriptSerializer().Deserialize<List<Credential>>(Credentials);
            var a = c.FirstOrDefault().UserGroupID;
            context.Credentials.RemoveRange(context.Credentials.Where(s=>s.UserGroupID == a));
            context.Credentials.AddRange(c);
            context.SaveChanges();
            return Json(new
            {
                status = true
            });
        }

    }
}