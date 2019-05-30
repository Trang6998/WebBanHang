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
    public class ThuocTinhSanPhamController : Controller
    {
        BanHangEntity db;
        List<ThuocTinhSanPham> getlstThuocTinhSanPham()
        {
            db = new BanHangEntity();
            var lst = db.ThuocTinhSanPhams.SqlQuery("Select * from ThuocTinhSanPham").ToList<ThuocTinhSanPham>();
            return lst;
        }
        // GET: ThuocTinhSanPham
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? loaiThuocTinhSanPhamID, int? khoangGiaTu, int? khoangGiaDen,
            int? page)
        {
            db = new BanHangEntity();
            if (Session["username"] == null)
                return RedirectToAction("/Index", "Users");
            else
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.GiaBanSortParm = sortOrder == "giaban" ? "giaban_desc" : "giaban";
                ViewBag.KyHieuSortParm = sortOrder == "kyhieu" ? "kyhieu_desc" : "kyhieu";
                ViewBag.khoangGiaTu = khoangGiaTu;
                ViewBag.khoangGiaDen = khoangGiaDen;
                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;
                string query = "Select * from ThuocTinhSanPham ";
                string qrten = "";
                string qrgiatu = "";
                string qrgiaden = "";
                if (!String.IsNullOrEmpty(searchString))
                    qrten = "(TenThuocTinhSanPham like N'%" + searchString + "%' or KyHieuThuocTinhSanPham like N'%" + searchString + "%')";

                if (khoangGiaTu.HasValue)
                    qrgiatu = "GiaBan >= " + khoangGiaTu;

                if (khoangGiaDen.HasValue)
                    qrgiaden = "GiaBan <= " + khoangGiaDen;

                if (loaiThuocTinhSanPhamID.HasValue)
                    query = query + ", LoaiThuocTinhSanPham where LoaiThuocTinhSanPham.LoaiThuocTinhSanPhamID = ThuocTinhSanPham.LoaiThuocTinhSanPhamID and ThuocTinhSanPham.LoaiThuocTinhSanPhamID = " + loaiThuocTinhSanPhamID;
                if (!String.IsNullOrEmpty(searchString) || (khoangGiaTu.HasValue) || (khoangGiaDen.HasValue))
                    query = query + (loaiThuocTinhSanPhamID.HasValue ? " and " : " where ") + qrten + (qrgiatu != "" ? (qrten != "" ? " and " : "") + qrgiatu : "") + (qrgiaden != "" ? (qrgiatu != "" ? " and " : "") + qrgiaden : "");

                switch (sortOrder)
                {
                    case "name_desc":
                        query = query + " order by TenThuocTinhSanPham desc ";
                        break;
                    case "kyhieu":
                        query = query + " order by KyHieuThuocTinhSanPham ";
                        break;
                    case "kyhieu_desc":
                        query = query + " order by KyHieuThuocTinhSanPham desc ";
                        break;
                    case "giaban":
                        query = query + " order by GiaBan ";
                        break;
                    case "giaban_desc":
                        query = query + " order by GiaBan desc ";
                        break;
                    default:  // Name ascending 
                        query = query + " order by TenThuocTinhSanPham ";
                        break;
                }

                int pageSize = 5;
                int pageNumber = (page ?? 1);
                var lstLoaiThuocTinhSanPham = db.ThuocTinhSanPhams.SqlQuery(query);
                return View(lstLoaiThuocTinhSanPham.ToPagedList(pageNumber, pageSize));
            }
        }
        public ActionResult Create()
        {
            if (Session["username"] == null)
                return RedirectToAction("/Index", "Users");
            else
            {
                using (var db = new BanHangEntity())
                {
                    ThuocTinhSanPham sp = new ThuocTinhSanPham();
                    return View(sp);
                }
            }
        }
        [HttpPost]
        public ActionResult Create(int id, int SanPhamID, int ThuocTinhID, string mota, string ghichu)
        {
            using (var db = new BanHangEntity())
            {
                if (id==0)
                {
                    ThuocTinhSanPham thuocTinhSanPham = new ThuocTinhSanPham();
                    thuocTinhSanPham.SanPhamID = SanPhamID;
                    thuocTinhSanPham.ThuocTinhID = ThuocTinhID;
                    thuocTinhSanPham.NoiDungMoTa = mota;
                    thuocTinhSanPham.GhiChu = ghichu;
                    db.ThuocTinhSanPhams.Add(thuocTinhSanPham);
                }
                else
                {
                    ThuocTinhSanPham thuocTinhSanPham = db.ThuocTinhSanPhams.Find(id);
                    thuocTinhSanPham.ThuocTinhID = ThuocTinhID;
                    thuocTinhSanPham.SanPhamID = SanPhamID;
                    thuocTinhSanPham.NoiDungMoTa = mota;
                    thuocTinhSanPham.GhiChu = ghichu;
                }
                db.SaveChanges();
                return RedirectToAction("Edit","SanPham", new { id = SanPhamID});
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
                        ThuocTinhSanPham sanPham = db.ThuocTinhSanPhams.Find(id);
                        return View(sanPham);
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
        public ActionResult Edit(ThuocTinhSanPham sanPham)
        {
            using (var db = new BanHangEntity())
            {
                db.Entry(sanPham).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
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
                        var sanPham = db.ThuocTinhSanPhams.Include("LoaiThuocTinhSanPham").Include("DonViTinh").Include("NhaCungCap").FirstOrDefault(x => x.ThuocTinhSanPhamID == id);
                        return View(sanPham);
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
            using (var db = new BanHangEntity())
            {
                ThuocTinhSanPham thuocTinhSanPham = db.ThuocTinhSanPhams.Find(id);
                if (thuocTinhSanPham != null)
                {
                    int idSanPham = thuocTinhSanPham.SanPhamID.Value;
                    db.ThuocTinhSanPhams.Remove(thuocTinhSanPham);
                    db.SaveChanges();
                    return RedirectToAction("Edit", "SanPham", new { id = idSanPham });
                }
                return RedirectToAction("Index", "SanPham");
            }
        }
    }
}