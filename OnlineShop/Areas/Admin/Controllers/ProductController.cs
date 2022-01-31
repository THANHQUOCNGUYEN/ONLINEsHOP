using Common;
using Model.Dao;
using Model.EF;
using OfficeOpenXml;
using OnlineShop.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Excel = Microsoft.Office.Interop.Excel;
namespace OnlineShop.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        // GET: Admin/Product
        private readonly IProduct _product;
        public ProductController(IProduct product)
        {
            _product = product;
        }
        public ActionResult Index(int page = 1,int pageSize = 4)
        {
            int numberPage = 0;
            var model = _product.GetListPaging(page,ref numberPage,pageSize);
            ViewBag.numberPage = numberPage;
            ViewBag.currentPage = page;
            if(TempData["success"] != null)
            {
                ViewBag.success = TempData["success"];
            } 
            if (TempData["result"] != null)
            {
                ViewBag.error = TempData["result"];
            }
            ViewBag.categories = new ProductCategoryDao().ListAll();
            return View(model);
        }
        public ActionResult Create()
        {
            setViewBag();
            return View();
        }


        [HttpPost]
        [ValidateInput(false)] //cai nay cho ckeditor
        public ActionResult Create(Product model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                
                model.CreatedBy = ((UserLogin)Session[CommomConstants.USER_SESSION]).UserName; //lays session trong user
                model.ViewCount = 0; //ban dau viewcount = 0;
                var dao = _product.Create(model);
                if (dao > 0)
                {
                    return RedirectToAction("Index");
                }
            }

            setViewBag();
            return View();
        }
        protected void setViewBag(long? selectID = null)
        {
            var dao = new ProductCategoryDao().ListAll();
            ViewBag.CategoryID = new SelectList(dao, "ID", "Name");
        }

        public ActionResult LoadMoreImages(int ID)
        {
            return View(_product.LoadMoreImages(ID));
        }

        [HttpPost]
        public JsonResult getAlias(string alias)
        {
            var alia = StringHelper.ToUnsignString(alias);
            

            return Json(new
            {
                status = true,
                data = alia
            });
        }


        [HttpPost]
        public JsonResult getProductCategories()
        {
            var lst = new ProductCategoryDao().ListAll();
            return Json(new
            {
                status = true,
                data = lst
            });
        }

        public ActionResult Update(int id)
        {
            var p = _product.ViewDetail(id);
            return View(p);
        }

        [HttpPost]
        [ValidateInput(false)]
       public ActionResult Update(Product product)
        {
            if (ModelState.IsValid)
            {
                var result = _product.Update(product);
                if (result)
                {
                    TempData["success"] = "Thêm thành công";
                    return RedirectToAction("Index", "Product");
                }
            }

            setViewBag(product.CategoryID);
            return View(product);
        }

        [HttpPost]
        public JsonResult GetList(int id)
        {
            var lstProducts = _product.ViewDetail(id);
            return Json(new
            {
                status = true,
                data = lstProducts
            });
        }

        //dọc file excex insert vào database
        [HttpPost]
        public ActionResult Import(HttpPostedFileBase excelfile,int? danhmuc)
        {
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                TempData["result"] = "Please select a excel file !";
                return RedirectToAction("Index");
            }
            else
            {
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    if(danhmuc == null)
                    {
                        TempData["result"] = "Vui lòng chọn danh mục !";
                        return RedirectToAction("Index");
                    }

                    string path = Server.MapPath("~/Data/" + excelfile.FileName);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }

                    excelfile.SaveAs(path);
                    //Read data form excel file
                    Excel.Application application = new Excel.Application();
                    Excel.Workbook workbook = application.Workbooks.Open(path);
                    Microsoft.Office.Interop.Excel.Worksheet worksheet = workbook.ActiveSheet;
                    Excel.Range range = worksheet.UsedRange;

                    List<Product> products = new List<Product>();
                    for (int row = 3; row <= range.Rows.Count; row++)
                    {
                        Product p = new Product();
                        p.Name = ((Excel.Range)range.Cells[row, 1]).Text;
                        p.Code = ((Excel.Range)range.Cells[row, 2]).Text;
                        p.MetaTitle = ((Excel.Range)range.Cells[row, 3]).Text;
                        p.Description = ((Excel.Range)range.Cells[row, 4]).Text;
                        p.Price = decimal.Parse(((Excel.Range)range.Cells[row, 5]).Text);
                        p.PromotionPrice = decimal.Parse(((Excel.Range)range.Cells[row, 6]).Text);
                        p.Quantity = int.Parse(((Excel.Range)range.Cells[row, 7]).Text);
                        p.Warranty = int.Parse(((Excel.Range)range.Cells[row, 8]).Text);
                        p.Status = Boolean.Parse(((Excel.Range)range.Cells[row, 9]).Text);
                        var moreImage = (string)(((Excel.Range)range.Cells[row, 10]).Text);
           
                        List<string> result = new List<string>();
                        foreach (var item in moreImage.Split(','))
                        {
                            result.Add(item);
                        }

                        p.MoreImages = new JavaScriptSerializer().Serialize(result);
                        p.CategoryID = danhmuc.Value;
                        products.Add(p);
                    }

                    ShopOnlineDbContext db = new ShopOnlineDbContext();
                    db.Products.AddRange(products);
                    db.SaveChanges();

                    TempData["result"] = "Thêm dữ liệu thành công !";
                    return RedirectToAction("Index");
                    
                }  
                else 
                {
                    TempData["result"] = "File type is incorrect !";
                    return RedirectToAction("Index");
                }
            }
            //return View();
        }
       
    }
}