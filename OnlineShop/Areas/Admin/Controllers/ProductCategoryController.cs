using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class ProductCategoryController : Controller
    {
        // GET: Admin/ProductCategory
        public ActionResult Index()
        {
            var lst = new ProductCategoryDao().ListAll();
            return View(lst);
        }

        public ActionResult Details(int id)
        {
            var productCategory = new ProductCategoryDao().ViewDetail(id);
            return View(productCategory);
        }

        public ActionResult Create()
        {
            return View();
        }

        private void setViewBag(long? ParentID = null)
        {
            ViewBag.ParentID = new SelectList(new ProductCategoryDao().ListAll(), "ID", "Name");
        }

        [HttpPost]
        public JsonResult getParent()
        {
            var list = new ProductCategoryDao().ListAll();
            return Json(new
            {   
                status = true,
                data = list
            }) ;
        }

        [HttpPost]
        public ActionResult Create(ProductCategory productCategory)
        {
            if(ModelState.IsValid)
            {
                if(new ProductCategoryDao().Create(productCategory))
                {
                    return RedirectToAction("Index");
                }
            }

            return View(productCategory);
        }
        public ActionResult Update(int id)
        {
            var lst = new ProductCategoryDao().ViewDetail(id);
            return View(lst);
        }
        
        [HttpPost]
        public ActionResult Update(ProductCategory productCategory)
        {
            if(ModelState.IsValid)
            {
                if(new ProductCategoryDao().Update(productCategory))
                {
                    return RedirectToAction("Index");
                }
            }
            //không cần dùng setViewBag
            return View(productCategory);
        }

        [HttpPost]
        public JsonResult getDetails(int id)
        {
            var lst = new ProductCategoryDao().ViewDetail(id);

            return Json(new
            {
                status = true,
                data = lst
            });
        }
        
    }
}