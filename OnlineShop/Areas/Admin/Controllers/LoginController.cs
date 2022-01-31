﻿using Model.Dao;
using OnlineShop.Areas.Admin.Data;
using OnlineShop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(LoginModel model)
        {
            if(ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.Login(model.UserName,Encryptor.MD5Hash(model.Password),true);
                if (result == 1)
                {
                    var user = dao.GetById(model.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.UserID = user.ID;
                    userSession.GroupID = user.GroupID;

                    var listCredentials = dao.GetListCredential(model.UserName);//Lấy danh sách các quyền của user
                    Session.Add(CommomConstants.Session_Credentials, listCredentials);
                    Session.Add(CommomConstants.USER_SESSION,userSession);
                    return RedirectToAction("Index","Home");
                }
                else if(result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại .");
                }
                else if(result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản đang bị khóa.");
                }
                else if(result == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng.");
                }
                else if(result == -3)
                {
                    ModelState.AddModelError("", "Tài khoản của của bạn không có quyền đăng nhập.");
                }
                else
                {
                    ModelState.AddModelError("", "Đăng nhập không đúng.");
                }  
            }

            return View("Index");
        }
    }
}