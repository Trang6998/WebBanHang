using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBBANHANG.Models;
using WEBBANHANG.Models.Entity;

namespace WEBBANHANG.Controllers
{
    public class UsersController : Controller
    {
        BanHangEntity db;
        // GET: Users
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session["username"] = null;
            return RedirectToAction("/Index");
        }

        List<User> getlstUser()
        {
            db = new BanHangEntity();
            var lst = db.Users.ToList();
            return lst;
        }

        [HttpPost]
        public ActionResult Test()
        {
            string us = Request.Form["us"];
            string mk = Request.Form["mk"];
            List<User> lstUser = getlstUser();
            User u = lstUser.Find(x => x.TaiKhoan.Equals(us));
            if (u != null)
            {
                if (u.PassWord.Equals(mk))
                {
                    Session["usernameid"] = u.UserId;
                    Session["username"] = us;
                    Session["user"] = (User)u;
                    var donDatHang = db.DonDatHangs.Where(x => x.TaiKhoanDatHangID == u.UserId && x.TinhTrang == 0).FirstOrDefault();
                    if (donDatHang != null)
                    {
                        int? soluong = 0;
                        for (int i= 0; i < donDatHang.ChiTietDonDatHangs.Count(); i ++)
                        {
                            soluong = soluong + donDatHang.ChiTietDonDatHangs.ElementAt(i).SoLuong;
                        }
                        Session["soluong"] = soluong;
                    }
                    else
                    {
                        Session["soluong"] = 0;
                    }

                    TempData["msg"] = "Đăng nhập thành công nhé!";
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    TempData["msg"] = "Tên tài khoản và mật khẩu không hợp lệ!";
                    //return RedirectToAction("Index");
                    return PartialView("~/Views/Shared/LoginLoi.cshtml");
                }


            }
            else
            {
                TempData["msg"] = "Tên tài khoản và mật khẩu không hợp lệ!";
                //return RedirectToAction("Index");
                return PartialView("~/Views/Shared/LoginLoi.cshtml");
            }

        }

        public ActionResult Home()
        {
            if (Session["username"] == null)
                return RedirectToAction("/Index", "Users");
            else
                return View();
        }
        public ActionResult LeftQuanTri()
        {
            if (Session["username"] == null)
                return RedirectToAction("/Index", "Users");
            else
            {
                List<string> lst = new List<string>();
                lst.Add("Quản lý danh mục");
                lst.Add("Quản lý sản phẩm");
                lst.Add("Quản lý người dùng");
                return PartialView("~/Views/Shared/_LeftAdmin.cshtml", lst);
            }
        }
         public ActionResult List(string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (Session["username"] == null)
                return RedirectToAction("/Index", "Users");
            else
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;
                var lstUser = getlstUser().AsQueryable();
                if (!String.IsNullOrEmpty(searchString))
                {
                    lstUser = lstUser.Where(s => s.HoTen.ToUpper().Contains(searchString.ToUpper()));
                }
                switch (sortOrder)
                {
                    case "name_desc":
                        lstUser = lstUser.OrderByDescending(s => s.HoTen);
                        break;
                    default:  // Name ascending 
                        lstUser = lstUser.OrderBy(s => s.HoTen);
                        break;
                }

                int pageSize = 5;
                int pageNumber = (page ?? 1);
                return View(lstUser.ToPagedList(pageNumber, pageSize));
            }
        }

        public ActionResult Create()
        {
            if (Session["username"] == null)
                return RedirectToAction("/Index", "Users");
            else
                return View();
        }

        [HttpPost]
        public ActionResult Create(User us)
        {
            if (Session["username"] == null)
                return RedirectToAction("/Index", "Users");
            else
            {
                using (var db = new BanHangEntity())
                {
                    if (ModelState.IsValid)
                    {
                        us.NgayLap = DateTime.Now;
                        db.Users.Add(us);
                        db.SaveChanges();
                        return RedirectToAction("List");
                    }
                    else
                        return View(us);
                }
            }
            
        }

        public ActionResult Edit(int id)
        {
            if (Session["username"] == null)
                return RedirectToAction("/Index", "Users");
            else
            {
                using (var db = new BanHangEntity())
                {
                    try
                    {
                        User us = db.Users.Find(id);
                        return View(us);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return new HttpStatusCodeResult(404, "Error in cloud - GetPLUInfo" + ex.Message);
                    }
                }
                
            }
        }

        [HttpPost]
        public ActionResult Edit(User us)
        {
            using (var db = new BanHangEntity())
            {
                db.Entry(us).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("/List");
        }

        public ActionResult Details(int id)
        {
            if (Session["username"] == null)
                return RedirectToAction("/Index", "Users");
            else
            {
                using (var db = new BanHangEntity())
                {
                    try
                    {
                        User us = db.Users.Find(id);
                        return View(us);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return new HttpStatusCodeResult(404, "Error in cloud - GetPLUInfo" + ex.Message);
                    }
                }
            }
        }
        public ActionResult Delete(int id)
        {
            if (Session["username"] == null)
                return RedirectToAction("/Index", "Users");
            else
            {
                using (var db = new BanHangEntity())
                {
                    try
                    {
                        User us = db.Users.Find(id);
                        return View(us);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return new HttpStatusCodeResult(404, "Error in cloud - GetPLUInfo" + ex.Message);
                    }
                }
            }
        }

        [HttpPost]
        public ActionResult Delete(float id)
        {
            using (var db = new BanHangEntity())
            {
                User user = db.Users.Find(id);
                if (user != null)
                {
                    db.Users.Remove(user);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("/List");
        }

        public ActionResult Left()
        {
            List<string> lst = new List<string>();
            lst.Add("Dụng cụ điện");
            lst.Add("Thực phẩm");
            lst.Add("Đồ gia dụng");
            lst.Add("Dịch vụ");
            return PartialView("~/Views/Shared/Left.cshtml", lst);
        }
    }
}