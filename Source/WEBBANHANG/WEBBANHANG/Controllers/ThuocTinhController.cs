using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBBANHANG.Models.Entity;

namespace WEBBANHANG.Controllers
{
    public class ThuocTinhController : Controller
    {
        BanHangEntity db;
        List<ThuocTinh> GetThuocTinhs()
        {
            db = new BanHangEntity();
            var lst = db.ThuocTinhs.ToList();
            return lst;
        }

        // GET: ThuocTinh
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
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
                var lstUser = GetThuocTinhs().AsQueryable();
                if (!String.IsNullOrEmpty(searchString))
                {
                    lstUser = lstUser.Where(s => s.TenThuocTinh.ToUpper().Contains(searchString.ToUpper()));
                }
                switch (sortOrder)
                {
                    case "name_desc":
                        lstUser = lstUser.OrderByDescending(s => s.TenThuocTinh);
                        break;
                    default:  // Name ascending 
                        lstUser = lstUser.OrderBy(s => s.TenThuocTinh);
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
        public ActionResult Create(ThuocTinh thuocTinh)
        {
            if (Session["username"] == null)
                return RedirectToAction("/Index", "Users");
            else
            {
                if(ModelState.IsValid)
                {
                    using (var db = new BanHangEntity())
                    {
                        db.ThuocTinhs.Add(thuocTinh);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                return View(thuocTinh);
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
                        ThuocTinh ncc = db.ThuocTinhs.Find(id);
                        return View(ncc);
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
        public ActionResult Edit(ThuocTinh thuocTinh)
        {
            if (Session["username"] == null)
                return RedirectToAction("/Index", "Users");
            else
            {
                if(ModelState.IsValid)
                {
                    using (var db = new BanHangEntity())
                    {
                        db.Entry(thuocTinh).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(thuocTinh);
            }
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
                        ThuocTinh ncc = db.ThuocTinhs.Find(id);
                        return View(ncc);
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
                        ThuocTinh ncc = db.ThuocTinhs.Find(id);
                        return View(ncc);
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
                ThuocTinh thuocTinh = db.ThuocTinhs.Find(id);
                if (thuocTinh != null)
                {
                    ThuocTinhSanPham thuocTinhSanPham = db.ThuocTinhSanPhams.FirstOrDefault(x => x.ThuocTinhID == thuocTinh.ThuocTinhID);
                    if (thuocTinhSanPham != null)
                    {
                        db.ThuocTinhSanPhams.Remove(thuocTinhSanPham);
                    }
                    db.ThuocTinhs.Remove(thuocTinh);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("/Index");
        }
    }
}