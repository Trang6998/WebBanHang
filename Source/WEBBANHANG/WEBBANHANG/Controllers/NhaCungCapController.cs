using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBBANHANG.Models.Entity;

namespace WEBBANHANG.Controllers
{
    public class NhaCungCapController : Controller
    {
        BanHangEntity db;
        List<NhaCungCap> GetNhaCungCaps()
        {
            db = new BanHangEntity();
            var lst = db.NhaCungCaps.ToList();
            return lst;
        }

        // GET: NhaCungCap
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
                var lstNhaCungCap = GetNhaCungCaps().AsQueryable();
                if (!String.IsNullOrEmpty(searchString))
                {
                    lstNhaCungCap = lstNhaCungCap.Where(s => s.TenNhaCungCap.ToUpper().Contains(searchString.ToUpper()));
                }
                switch (sortOrder)
                {
                    case "name_desc":
                        lstNhaCungCap = lstNhaCungCap.OrderByDescending(s => s.TenNhaCungCap);
                        break;
                    default:  // Name ascending 
                        lstNhaCungCap = lstNhaCungCap.OrderBy(s => s.TenNhaCungCap);
                        break;
                }

                int pageSize = 5;
                int pageNumber = (page ?? 1);
                return View(lstNhaCungCap.ToPagedList(pageNumber, pageSize));
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
        public ActionResult Create(NhaCungCap nhaCungCap)
        {
            if (Session["username"] == null)
                return RedirectToAction("/Index", "Users");
            else
            {
                if(ModelState.IsValid)
                {
                    using (var db = new BanHangEntity())
                    {
                        db.NhaCungCaps.Add(nhaCungCap);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                return View(nhaCungCap);
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
                        NhaCungCap ncc = db.NhaCungCaps.Find(id);
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
        public ActionResult Edit(NhaCungCap nhaCungCap)
        {
            if (Session["username"] == null)
                return RedirectToAction("/Index", "Users");
            else
            {
                if (ModelState.IsValid)
                {
                    using (var db = new BanHangEntity())
                    {
                        db.Entry(nhaCungCap).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(nhaCungCap);
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
                        NhaCungCap ncc = db.NhaCungCaps.Find(id);
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
                        NhaCungCap ncc = db.NhaCungCaps.Find(id);
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
                NhaCungCap nhaCungCap = db.NhaCungCaps.Find(id);
                if (nhaCungCap != null)
                {
                    db.NhaCungCaps.Remove(nhaCungCap);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("/Index");
        }
    }
}