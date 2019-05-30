using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBBANHANG.Models.Entity;

namespace WEBBANHANG.Controllers
{
    public class DonViTinhController : Controller
    {
        BanHangEntity db;
        List<DonViTinh> getlstDVT()
        {
            db = new BanHangEntity();
            var lst = db.DonViTinhs.ToList();
            return lst;
        }
        // GET: DonViTinh
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
                var lstDonViTinh = getlstDVT().AsQueryable();
                if (!String.IsNullOrEmpty(searchString))
                {
                    lstDonViTinh = lstDonViTinh.Where(s => s.TenDonVi.ToUpper().Contains(searchString.ToUpper()));
                }
                switch (sortOrder)
                {
                    case "name_desc":
                        lstDonViTinh = lstDonViTinh.OrderByDescending(s => s.TenDonVi);
                        break;
                    default:  // Name ascending 
                        lstDonViTinh = lstDonViTinh.OrderBy(s => s.TenDonVi);
                        break;
                }

                int pageSize = 5;
                int pageNumber = (page ?? 1);
                return View(lstDonViTinh.ToPagedList(pageNumber, pageSize));
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
        public ActionResult Create(DonViTinh donViTinh)
        {
            if (Session["username"] == null)
                return RedirectToAction("/Index", "Users");
            else
            {
                if (ModelState.IsValid)
                {
                    using (var db = new BanHangEntity())
                    {
                        db.DonViTinhs.Add(donViTinh);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                return View(donViTinh);
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
                        DonViTinh us = db.DonViTinhs.Find(id);
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
        public ActionResult Edit(DonViTinh dvt)
        {
            if (Session["username"] == null)
                return RedirectToAction("/Index", "Users");
            else
            {
                if(ModelState.IsValid)
                {
                    using (var db = new BanHangEntity())
                    {
                        db.Entry(dvt).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(dvt);
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
                        DonViTinh us = db.DonViTinhs.Find(id);
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
                        DonViTinh us = db.DonViTinhs.Find(id);
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
                DonViTinh donViTinh = db.DonViTinhs.Find(id);
                if (donViTinh != null)
                {
                    if (db.SanPhams.Any(x => x.DonViTinhID == donViTinh.DonViTinhID))
                    {
                        TempData["mgs"] = "Đơn vị đã được sử dụng bởi sản phẩm!";
                        return RedirectToAction("/Delete", "DonViTinh", new { id = donViTinh.DonViTinhID });
                    }
                    else
                    {
                        db.DonViTinhs.Remove(donViTinh);
                        db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("/Index");
        }
    }
}