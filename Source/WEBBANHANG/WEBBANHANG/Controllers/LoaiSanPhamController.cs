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
    public class LoaiSanPhamController : Controller
    {
        BanHangEntity db;
        List<LoaiSanPham> getlstLoaiSanPham()
        {
            db = new BanHangEntity();
            var lst = db.LoaiSanPhams.OrderBy(x => x.LoaiSanPhamID).ToList();
            return lst;
        }

        // GET: LoaiSanPham
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
                var lstLoaiSanPham = getlstLoaiSanPham().AsQueryable();
                if (!String.IsNullOrEmpty(searchString))
                {
                    lstLoaiSanPham = lstLoaiSanPham.Where(s => s.TenLoai.ToUpper().Contains(searchString.ToUpper()));
                }
                switch (sortOrder)
                {
                    case "name_desc":
                        lstLoaiSanPham = lstLoaiSanPham.OrderByDescending(s => s.TenLoai);
                        break;
                    default:  // Name ascending 
                        lstLoaiSanPham = lstLoaiSanPham.OrderBy(s => s.TenLoai);
                        break;
                }

                int pageSize = 5;
                int pageNumber = (page ?? 1);
                return View(lstLoaiSanPham.ToPagedList(pageNumber, pageSize));
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
        public ActionResult Create(LoaiSanPham loaiSanPham)
        {

            if (Session["username"] == null)
                return RedirectToAction("/Index", "Users");
            else
            {
                if(ModelState.IsValid)
                {
                    if (loaiSanPham.ImageFile != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(loaiSanPham.ImageFile.FileName);
                        string extension = Path.GetExtension(loaiSanPham.ImageFile.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        loaiSanPham.AnhDaiDien = "~/FileUpload/" + fileName;
                        fileName = Path.Combine(Server.MapPath("~/FileUpload/"), fileName);
                        loaiSanPham.ImageFile.SaveAs(fileName);
                    }

                    using (var db = new BanHangEntity())
                    {
                        db.LoaiSanPhams.Add(loaiSanPham);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                return View(loaiSanPham);
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
                        LoaiSanPham loaiSanPham = db.LoaiSanPhams.Find(id);
                        return View(loaiSanPham);
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
        public ActionResult Edit(LoaiSanPham dv)
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
                        var loaiSanPham = db.LoaiSanPhams.Include("LoaiSanPham2").FirstOrDefault(x => x.LoaiSanPhamID == id);
                        return View(loaiSanPham);
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
                        var loaiSanPham = db.LoaiSanPhams.Include("LoaiSanPham2").FirstOrDefault(x => x.LoaiSanPhamID == id);
                        return View(loaiSanPham);
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
                LoaiSanPham loaiSanPham = db.LoaiSanPhams.Find(id);
                if (loaiSanPham != null)
                {
                    db.LoaiSanPhams.Remove(loaiSanPham);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("/Index");
        }
    }
}