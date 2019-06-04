using WEBBANHANG.Models;
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
    public class DonDatHangController : Controller
    {
        BanHangEntity db;
        List<DonDatHang> getlstDonDatHang()
        {
            db = new BanHangEntity();
            var lst = db.DonDatHangs.OrderBy(x => x.DonDatHangID).ToList();
            return lst;
        }

        // GET: DonDatHang
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, string currentSFilter, string searchDateS, string currentFFilter, string searchDateF, int? option, int? page)
        {
            if (Session["username"] == null)
                return RedirectToAction("/Index", "Users");
            else
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
                ViewBag.SoHieuSortParm = sortOrder == "sohieu"? "sohieu_desc" : "sohieu";
                ViewBag.TienSortParm = sortOrder == "tien" ? "tien_desc" : "tien";

                if (searchString != null || searchDateS != null || searchDateF != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                    searchDateS = currentSFilter;
                    searchDateF = currentFFilter;
                }

                ViewBag.CurrentFilter = searchString;
                ViewBag.CurrentSFilter = searchDateS;
                ViewBag.CurrentFFilter = searchDateF;

                var lstDonDatHang = getlstDonDatHang().AsQueryable().Where(x => x.TinhTrang != 0);
                if (!String.IsNullOrEmpty(searchString))
                {
                    lstDonDatHang = lstDonDatHang.Where(s => s.User.HoTen.ToUpper().Contains(searchString.ToUpper()));
                }
                if (!String.IsNullOrEmpty(searchDateS))
                {
                    lstDonDatHang = lstDonDatHang.Where(s => s.NgayDat >= Convert.ToDateTime(searchDateS));
                }
                if (!String.IsNullOrEmpty(searchDateF))
                {
                    lstDonDatHang = lstDonDatHang.Where(s => s.NgayDat <= Convert.ToDateTime(searchDateF));
                }
                if (option.HasValue)
                {
                    if (option == -1)
                        lstDonDatHang = lstDonDatHang.Where(o => o.TinhTrang != TrangThaiDonHang.CHUA_GUI && o.TinhTrang != TrangThaiDonHang.THANH_LY_HUY_HANG);
                    else
                        lstDonDatHang = lstDonDatHang.Where(o => o.TinhTrang == option);
                }

                switch (sortOrder)
                {
                    case "date_desc":
                        lstDonDatHang = lstDonDatHang.OrderBy(s => s.NgayDat);
                        break;
                    case "sohieu":
                        lstDonDatHang = lstDonDatHang.OrderBy(s => s.SoHieuDon);
                        break;
                    case "sohieu_desc":
                        lstDonDatHang = lstDonDatHang.OrderByDescending(s => s.SoHieuDon);
                        break;
                    case "tien":
                        lstDonDatHang = lstDonDatHang.OrderBy(s => (s.ChiTietDonDatHangs.Sum(x => x.SoLuong * x.GiaXuat)));
                        break;
                    case "tien_desc":
                        lstDonDatHang = lstDonDatHang.OrderByDescending(s => (s.ChiTietDonDatHangs.Sum(x => x.SoLuong * x.GiaXuat)));
                        break;
                    default:  // Name ascending 
                        lstDonDatHang = lstDonDatHang.OrderByDescending(s => s.NgayDat);
                        break;
                }

                int pageSize = 5;
                int pageNumber = (page ?? 1);
                return View(lstDonDatHang.ToPagedList(pageNumber, pageSize));
            }
        }

        public ActionResult Create()
        {
            if (Session["username"] == null)
                return RedirectToAction("/Index", "Users");
            else
                return View();
        }
        public ActionResult NhanDon(int id)
        {
            if (Session["username"] == null)
                return RedirectToAction("/Index", "Users");
            else
            {
                db = new BanHangEntity();
                DonDatHang donDatHang = db.DonDatHangs
                                          .FirstOrDefault(x => x.DonDatHangID == id);
                if (donDatHang != null)
                {
                    donDatHang.TinhTrang = 2;
                    db.SaveChanges();
                }
                return RedirectToAction("/Index", "DonDatHang");
            }
        }
        [HttpPost]
        public ActionResult Create(DonDatHang donDatHang)
        {

            if (Session["username"] == null)
                return RedirectToAction("/Index", "Users");
            else
            {
                using (var db = new BanHangEntity())
                {
                    db.DonDatHangs.Add(donDatHang);
                    db.SaveChanges();
                    return RedirectToAction("Index");
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
                        DonDatHang donDatHang = db.DonDatHangs.Include("User").Include("ChiTietDonDatHangs").Include("ChiTietDonDatHangs.SanPham").FirstOrDefault(x => x.DonDatHangID == id);
                        return View(donDatHang);
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
        public ActionResult Edit(DonDatHang donDatHang)
        {
            if (Session["username"] == null)
                return RedirectToAction("/Index", "Users");
            else
            {
                using (var db = new BanHangEntity())
                {
                    db.Entry(donDatHang).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
        }
        
        public ActionResult DeleteChiTietDon(int idChiTiet, int idDonHang)
        {
            if (Session["username"] == null)
                return RedirectToAction("/Index", "Users");
            else
            {
                using (var db = new BanHangEntity())
                {
                    ChiTietDonDatHang chiTietDonDatHang = db.ChiTietDonDatHangs.Find(idChiTiet);
                    if (chiTietDonDatHang != null)
                    {
                        db.ChiTietDonDatHangs.Remove(chiTietDonDatHang);
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("/Edit/" + idDonHang);
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
                        DonDatHang donDatHang = db.DonDatHangs.Include("User").Include("ChiTietDonDatHangs").Include("ChiTietDonDatHangs.SanPham").FirstOrDefault(x => x.DonDatHangID == id);
                        return View(donDatHang);
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
                DonDatHang donDatHang = db.DonDatHangs.FirstOrDefault(x => x.DonDatHangID == id);
                if (donDatHang != null)
                {
                    db.ChiTietDonDatHangs.RemoveRange(donDatHang.ChiTietDonDatHangs);
                    db.DonDatHangs.Remove(donDatHang);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("/Index");
        }
    }
}