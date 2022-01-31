using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Model.Dao
{
    public class ProductCategoryDao
    {
        ShopOnlineDbContext db = null;
        public ProductCategoryDao()
        {
            db = new ShopOnlineDbContext();
        }

        public List<ProductCategory> ListAll()
        {
            return db.ProductCategories.Where(x => x.Status == true).OrderBy(x => x.DisplayOrder).ToList();
        }

        public ProductCategory ViewDetail(long id)
        {
            return db.ProductCategories.Find(id);
        }

        public bool Create(ProductCategory productCategory)
        {
            try
            {
                productCategory.CreatedDate = DateTime.Now;
                db.ProductCategories.Add(productCategory);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool Update(ProductCategory _productCategory)
        {
            try
            {
                var productCategory = db.ProductCategories.Find(_productCategory.ID);
                productCategory.Name = _productCategory.Name;
                productCategory.MetaTitle = _productCategory.MetaTitle;
                productCategory.ParentID = _productCategory.ParentID;
                productCategory.ModifiedDate = DateTime.Now;
                productCategory.Status = _productCategory.Status;
                productCategory.ShowOnHome = _productCategory.ShowOnHome;

                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
