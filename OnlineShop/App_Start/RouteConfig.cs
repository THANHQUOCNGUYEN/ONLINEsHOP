using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OnlineShop
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //nhớ thêm cái này để sử dụng captcha code
            routes.IgnoreRoute("{*botdetect}",
     new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });

            routes.MapRoute(
              name: "Product Category",
              url: "san-pham/{metatitile}-{id}",
              defaults: new { controller = "Product", action = "Category", id = UrlParameter.Optional },
              namespaces: new[] { "OnlineShop.Controllers" }
          );
            routes.MapRoute(
              name: "Cart",
              url: "gio-hang",
              defaults: new { controller = "Cart", action = "Index", id = UrlParameter.Optional },
              namespaces: new[] { "OnlineShop.Controllers" }
          );
            //dang nhâp
            routes.MapRoute(
             name: "Login",
             url: "dang-nhap",
             defaults: new { controller = "User", action = "Login", id = UrlParameter.Optional },
             namespaces: new[] { "OnlineShop.Controllers" }
         );
            routes.MapRoute(
             name: "Product Detail",
             url: "chi-tiet/{a}-{i}",
             defaults: new { controller = "Product", action = "Detail", id = UrlParameter.Optional },
             namespaces: new[] { "OnlineShop.Controllers" }
         );
            routes.MapRoute(
            name: "Register",
            url: "dang-ki",
            defaults: new { controller = "User", action = "Register", id = UrlParameter.Optional },
            namespaces: new[] { "OnlineShop.Controllers" }
        );
            routes.MapRoute(
         name: "tin-demo",
         url: "tin-tuc/page={page}",
         defaults: new { controller = "ContentDemo", action = "Index", id = UrlParameter.Optional },
         namespaces: new[] { "OnlineShop.Controllers" }
     );
            routes.MapRoute(
            name: "News",
            url: "tin-tuc",
            defaults: new { controller = "Content", action = "Index", id = UrlParameter.Optional },
            namespaces: new[] { "OnlineShop.Controllers" }
        );
            routes.MapRoute(
           name: "Search",
           url: "tim-kiem",
           defaults: new { controller = "Product", action = "Search", id = UrlParameter.Optional },
           namespaces: new[] { "OnlineShop.Controllers" }
       );
            routes.MapRoute(
             name: "Add Cart",
             url: "them-gio-hang",
             defaults: new { controller = "Cart", action = "AddItem", id = UrlParameter.Optional },
             namespaces: new[] { "OnlineShop.Controllers" }
         );
            routes.MapRoute(
          name: "Contact",
          url: "lien-he",
          defaults: new { controller = "Contact", action = "Index", id = UrlParameter.Optional },
          namespaces: new[] { "OnlineShop.Controllers" }
      );

            routes.MapRoute(
        name: "thanhquoc",
        url: "thanhquoc/thanhhung-{id}",
        defaults: new { controller = "Default", action = "chitiet", id = UrlParameter.Optional },
        namespaces: new[] { "OnlineShop.Controllers" }
    );
            routes.MapRoute(
           name: "Payment",
           url: "thanhtoan",
           defaults: new { controller = "Cart", action = "Payment", id = UrlParameter.Optional },
           namespaces: new[] { "OnlineShop.Controllers" }
       );
            //hoan thanh
           routes.MapRoute(
          name: "Payment Success",
          url: "hoan-thanh",
          defaults: new { controller = "Cart", action = "Success", id = UrlParameter.Optional },
          namespaces: new[] { "OnlineShop.Controllers" }
      );
            routes.MapRoute(
              name: "Content Detail",
              url: "tin-tuc/{metatitle}-{id}",
              defaults: new { controller = "Content", action = "Detail", id = UrlParameter.Optional },
              namespaces: new[] { "OnlineShop.Controllers" }
          );
            routes.MapRoute(
           name: "Tags",
           url: "tag/{tagId}",
           defaults: new { controller = "Content", action = "Tag", id = UrlParameter.Optional },
           namespaces: new[] { "OnlineShop.Controllers" }
       );
            //khong tham so
            routes.MapRoute(
             name: "About",
             url: "gioi-thieu",
             defaults: new { controller = "About", action = "Index", id = UrlParameter.Optional },
             namespaces: new[] { "OnlineShop.Controllers" }
         );
            routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
               namespaces: new[] { "OnlineShop.Controllers" }
           );    
        }
    }
}
