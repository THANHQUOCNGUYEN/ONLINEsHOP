using Grpc.Core;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace Model.Dao
{
    public interface IProduct
    {
        List<Product> ListNewProduct(int top);
        List<Product> ListByCategoryId(long categoryID, ref int totalRecord, int pageIndex = 1, int pageSize = 1);
        List<Product> ListFeatureProduct(int top);
        List<Product> Search(string keyword, ref int totalRecord, int pageIndex = 1, int pageSize = 1);
        List<Product> ListRelatedProducts(long productId);
        List<string> ListName(string keyword);
        long Create(Product model);
        List<string> LoadMoreImages(int idProduct);
        List<Product> GetListPaging(int page, ref int numberPage, int pageSize);
        bool Update(Product _product);
        Product ViewDetail(long id);
    }
    public class ProductDao:IProduct
    {
        private readonly ShopOnlineDbContext db;
        public ProductDao(ShopOnlineDbContext context)
        {
            db = context;
        }

        public List<Product> ListNewProduct(int top)
        {
            return db.Products.OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }

        public List<Product> ListByCategoryId(long categoryID, ref int totalRecord, int pageIndex = 1,int pageSize = 1)
        {
            totalRecord = db.Products.Where(x => x.CategoryID == categoryID).Count();
            var model = db.Products.Where(x => x.CategoryID == categoryID).OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1)*pageSize).Take(pageSize).ToList();
            return model;
        }
        /// <summary>
        /// Get List Product by category
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public List<Product> ListFeatureProduct(int top)
        {
            return db.Products.Where(x => x.TopHot != null && x.TopHot > DateTime.Now).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }
        //phuong thuc search
        public List<Product> Search(string keyword, ref int totalRecord, int pageIndex = 1, int pageSize = 1)
        {
            totalRecord = db.Products.Where(x => x.Name.Contains(keyword)).Count();
            var model = db.Products.Where(x => x.Name.Contains(keyword)).OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return model;
        }
        //san-pham-lien-quan
        public List<Product> ListRelatedProducts(long productId)
        {
            var product = db.Products.Find(productId);
            return db.Products.Where(x => x.ID != productId && x.CategoryID == product.CategoryID).ToList();
        }

        public Product ViewDetail(long id)
        {
            return db.Products.Find(id);
        }

        public List<string> ListName(string keyword)
        {
            return db.Products.Where(x => x.Name.Contains(keyword)).Select(x => x.Name).ToList();
        }
        //Quản lý danh sách sản phẩm

        public List<Product> GetListPaging(int page,ref int numberPage,int pageSize)
        {
            var lst = db.Products.OrderBy(x => x.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            int totalRecored = db.Products.Count();
            numberPage = totalRecored % pageSize == 0 ? totalRecored / pageSize : totalRecored / pageSize + 1;
            return lst;
        }
        public long Create(Product model)
        {
            db.Products.Add(model);
            db.SaveChanges();
            return model.ID;
        }

        public List<string> LoadMoreImages(int idProduct)
        {
            var moreImages =  db.Products.Single(s=>s.ID == idProduct).MoreImages;
            var listmoreImages = new List<string>();
            if (moreImages != null)
            {
                listmoreImages = new JavaScriptSerializer().Deserialize<List<string>>(moreImages);
            }

            return listmoreImages;
        }

        public bool Update(Product _product)
        {
            var product = db.Products.Find(_product.ID);
            product.Name = _product.Name;
            product.Code = _product.Code;
            product.MetaTitle = _product.MetaTitle;
            product.Description = _product.Description;
            product.Image = _product.Image;
            product.MoreImages = _product.MoreImages;
            product.PromotionPrice = _product.PromotionPrice;
            product.Price = _product.Price;
            product.IncludeVAT = _product.IncludeVAT;
            product.Quantity = _product.Quantity;
            product.CategoryID = _product.CategoryID;
            product.Detail = _product.Detail;
            product.Quantity = _product.Quantity;
            product.ModifiedDate = _product.ModifiedDate;
            product.Status = _product.Status;
            product.TopHot = _product.TopHot;
            product.ViewCount = _product.ViewCount;

            db.SaveChanges();
            return true;
        }
    }
}
