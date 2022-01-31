using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using PagedList;
using System.Web;
namespace Model.Dao
{
    public class ContentDao
    {
        ShopOnlineDbContext db = null;
        public ContentDao()
        {
            db = new ShopOnlineDbContext();
        }

        //Phân trang
        public IEnumerable<Content> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Content> model = db.Contents;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
        public IEnumerable<Content> ListAllByTag(string tag, int page, int pageSize)
        {
            var model = (from a in db.Contents
                         join b in db.Content_Tag
                         on a.ID equals b.ContentID
                         where b.TagID == tag
                         select new
                         {
                             Name = a.Name,
                             MetaTitle = a.MetaTitle,
                             Image = a.Image,
                             Description = a.Description,
                             CreateDate = a.CreatedDate,
                             ID = a.ID,
                             CreatedBy = a.CreatedBy
                         }).AsEnumerable().Select(x => new Content() {
                             Name = x.Name,
                             MetaTitle = x.MetaTitle,
                             Image = x.Image,
                             Description = x.Description,
                             CreatedBy = x.CreatedBy,
                             CreatedDate = x.CreateDate,
                             ID = x.ID
                         });

            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public Content GetByID(long id)
        {
            return db.Contents.Find(id);
        }
        //ListAll mà không có phân trang
        /// <summary>
        /// List All content for client
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<Content> ListAllPaging(int page, int pageSize)
        {
            IQueryable<Content> model = db.Contents;
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        //xử lý tag
        public long Create(Content content)
        {
            //Xử lý alias
            if(string.IsNullOrEmpty(content.MetaTitle))//Nếu không truyền vào MetaTile
            {
                content.MetaTitle = StringHelper.ToUnsignString(content.Name);
            }
            content.CreatedDate = DateTime.Now;
            content.ViewCount = 0;//

            db.Contents.Add(content);
            db.SaveChanges();

            if(!string.IsNullOrEmpty(content.Tags))
            {
                string[] tags = content.Tags.Split(',');
                foreach(var tag in tags)
                {
                    var tagId = StringHelper.ToUnsignString(tag);
                    var exitstedTag = this.CheckTag(tagId);

                    //Insert to to tag table
                    if(!exitstedTag)
                    {
                        this.InsertTag(tagId, tag);    
                    }

                    //insert to content Tag
                    this.InsertContentTag(content.ID, tagId);
                }
            }

            return content.ID;
        }
        public long Edit(Content content)
        {
            //Xử lý alias
            if (string.IsNullOrEmpty(content.MetaTitle))//Nếu không truyền vào MetaTile
            {
                content.MetaTitle = StringHelper.ToUnsignString(content.Name);
            }
            content.CreatedDate = DateTime.Now;
            db.SaveChanges();

            if (!string.IsNullOrEmpty(content.Tags))
            {
                this.RemoveAllContentTag(content.ID);
                string[] tags = content.Tags.Split(',');
                foreach (var tag in tags)
                {
                    var tagId = StringHelper.ToUnsignString(tag);
                    var exitstedTag = this.CheckTag(tagId);

                    //Insert to to tag table
                    if (!exitstedTag)
                    {
                        this.InsertTag(tagId, tag);
                    }

                    //insert to content Tag
                    this.InsertContentTag(content.ID, tagId);
                }
            }

            return content.ID;
        }

        public void RemoveAllContentTag(long contentId)
        {
            db.Content_Tag.RemoveRange(db.Content_Tag.Where(x => x.ContentID == contentId));
            db.SaveChanges();
        }
        public void InsertContentTag(long contentId,string tagId)
        {
            var contentTag = new Content_Tag();
            contentTag.ContentID = contentId;
            contentTag.TagID = tagId;
            db.Content_Tag.Add(contentTag);
            db.SaveChanges();

        }
        public void InsertTag(string id,string name)
        {
            var tag = new Tag();
            tag.ID = id;
            tag.Name = name;
            db.Tags.Add(tag);
            db.SaveChanges();
        }
        public bool CheckTag(string id)
        {
            return db.Tags.Count(x => x.ID == id) > 0;
        }

        public List<Tag> ListTag(long contentId)
        {
            var model = (from a in db.Tags
                        join b in db.Content_Tag
                        on a.ID equals b.TagID
                        where b.ContentID == contentId
                        select new
                        {
                            ID = b.TagID,
                            Name = a.Name
                        }).AsEnumerable().Select(x=>new Tag() {
                            ID = x.ID,
                            Name = x.Name
                        });

            return model.ToList();
        }

        public Tag GetTag(string id)
        {
            return db.Tags.Find(id);
        }
    }
}
