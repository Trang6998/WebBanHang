using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBBANHANG.Models.Entity;

namespace WEBBANHANG.Controllers
{
    public class DichVuController : Controller
    {
        BanHangEntity db;
        List<DichVu> getlstDichVu()
        {
            db = new BanHangEntity();
            var lst = db.DichVus.OrderBy(x => x.DichVuID).ToList();
            return lst;
        }

        // GET: DichVu
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (Session["username"] == null)
                return RedirectToAction("/Index", "Users");
            else
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                //ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;
                var lstDichVu = getlstDichVu().AsQueryable();
                if (!String.IsNullOrEmpty(searchString))
                {
                    lstDichVu = lstDichVu.Where(s => s.TenDichVu.ToUpper().Contains(searchString.ToUpper()));
                }
                switch (sortOrder)
                {
                    case "name_desc":
                        lstDichVu = lstDichVu.OrderByDescending(s => s.TenDichVu);
                        break;
                    //case "Date":
                    //    students = students.OrderBy(s => s.EnrollmentDate);
                    //    break;
                    //case "date_desc":
                    //    students = students.OrderByDescending(s => s.EnrollmentDate);
                    //    break;
                    default:  // Name ascending 
                        lstDichVu = lstDichVu.OrderBy(s => s.TenDichVu);
                        break;
                }

                int pageSize = 5;
                int pageNumber = (page ?? 1);
                return View(lstDichVu.ToPagedList(pageNumber, pageSize));
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
        public ActionResult Create(DichVu dichVu)
        {

            if (Session["username"] == null)
                return RedirectToAction("/Index", "Users");
            else
            {
               if(ModelState.IsValid)
                {
                    if (dichVu.ImageFile != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(dichVu.ImageFile.FileName);
                        string extension = Path.GetExtension(dichVu.ImageFile.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        dichVu.AnhDaiDien = "~/FileUpload/" + fileName;
                        fileName = Path.Combine(Server.MapPath("~/FileUpload/"), fileName);
                        dichVu.ImageFile.SaveAs(fileName);
                    }

                    using (var db = new BanHangEntity())
                    {
                        db.DichVus.Add(dichVu);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                return View(dichVu);
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
                        DichVu dichVu = db.DichVus.Find(id);
                        return View(dichVu);
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
        public ActionResult Edit(DichVu dv)
        {
            if (Session["username"] == null)
                return RedirectToAction("/Index", "Users");
            else
            {
                if (ModelState.IsValid)
                {
                    if (dv.ImageFile != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(dv.ImageFile.FileName);
                        string extension = Path.GetExtension(dv.ImageFile.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        dv.AnhDaiDien = "~/FileUpload/" + fileName;
                        fileName = Path.Combine(Server.MapPath("~/FileUpload/"), fileName);
                        dv.ImageFile.SaveAs(fileName);
                    }
                    using (var db = new BanHangEntity())
                    {
                        db.Entry(dv).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(dv);
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
                        var dichVu = db.DichVus.Find(id);
                        return View(dichVu);
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
                        var dichVu = db.DichVus.Find(id);
                        return View(dichVu);
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
                DichVu dichVu = db.DichVus.Find(id);
                if (dichVu != null)
                {
                    db.DichVus.Remove(dichVu);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("/Index");
        }
    }
}