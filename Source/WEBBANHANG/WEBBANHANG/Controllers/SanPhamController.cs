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
    public class SanPhamController : Controller
    {
        BanHangEntity db;
        List<SanPham> getlstSanpham()
        {
            db = new BanHangEntity();
            var lst = db.SanPhams.SqlQuery("Select * from SanPham").ToList<SanPham>();
            return lst;
        }
        // GET: SanPham
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? loaiSanPhamID, int? khoangGiaTu, int? khoangGiaDen,
            int? page)
        {
            db = new BanHangEntity();
            if (Session["username"] == null)
                return RedirectToAction("/Index", "Users");
            else
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "name";
                ViewBag.GiaBanSortParm = sortOrder == "giaban" ? "giaban_desc" : "giaban";
                ViewBag.KyHieuSortParm = sortOrder == "kyhieu" ? "kyhieu_desc" : "kyhieu";
                ViewBag.loaiSanPhamID = new SelectList(db.LoaiSanPhams, "LoaiSanPhamID", "TenLoai", loaiSanPhamID);
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
                string query = "Select * from SanPham ";
                string qrten = "";
                string qrgiatu = "";
                string qrgiaden = "";
                if (!String.IsNullOrEmpty(searchString))
                    qrten = "(TenSanPham like N'%" + searchString + "%' or KyHieuSanPham like N'%" + searchString + "%')";

                if (khoangGiaTu.HasValue)
                    qrgiatu = "GiaBan >= " + khoangGiaTu;

                if (khoangGiaDen.HasValue)
                    qrgiaden = "GiaBan <= " + khoangGiaDen;

                if (loaiSanPhamID.HasValue)
                    query = query + ", LoaiSanPham where LoaiSanPham.LoaiSanPhamID = SanPham.LoaiSanPhamID and SanPham.LoaiSanPhamID = " + loaiSanPhamID;
                if (!String.IsNullOrEmpty(searchString) || (khoangGiaTu.HasValue) || (khoangGiaDen.HasValue))
                    query = query + (loaiSanPhamID.HasValue ? " and " : " where ")  + qrten + (qrgiatu != "" ? (qrten != "" ? " and " : "") + qrgiatu : "") + (qrgiaden != "" ? (qrgiatu != "" ? " and " : "") + qrgiaden : "");

                switch (sortOrder)
                {
                    case "name_desc":
                        query = query + " order by TenSanPham desc ";
                        break;
                    case "name":
                        query = query + " order by TenSanPham ";
                        break;
                    case "kyhieu":
                        query = query + " order by KyHieuSanPham ";
                        break;
                    case "kyhieu_desc":
                        query = query + " order by KyHieuSanPham desc ";
                        break;
                    case "giaban":
                        query = query + " order by GiaBan ";
                        break;
                    case "giaban_desc":
                        query = query + " order by GiaBan desc ";
                        break;
                    default:  // new pro
                        query = query + " order by LaSanPhamMoi desc";
                        break;
                }

                int pageSize = 5;
                int pageNumber = (page ?? 1);
                var lstLoaiSanPham = db.SanPhams.SqlQuery(query);
                return View(lstLoaiSanPham.ToPagedList(pageNumber, pageSize));
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
                    SanPham sp = new SanPham();
                    sp.LoaiSanPhamCollection = db.LoaiSanPhams.ToList<LoaiSanPham>();
                    sp.DonViTinhCollection = db.DonViTinhs.ToList<DonViTinh>();
                    sp.NhaCungCapCollection = db.NhaCungCaps.ToList<NhaCungCap>();
                   
                    return View(sp);
                }
            }
        }
        [HttpPost]
        public ActionResult Create(SanPham sanPham)
        {
            if (Session["username"] == null)
                return RedirectToAction("/Index", "Users");
            else
            {
                if (ModelState.IsValid)
                {
                    if (sanPham.ImageFile != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(sanPham.ImageFile.FileName);
                        string extension = Path.GetExtension(sanPham.ImageFile.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        sanPham.AnhSanPham = "~/FileUpload/" + fileName;
                        fileName = Path.Combine(Server.MapPath("~/FileUpload/"), fileName);
                        sanPham.ImageFile.SaveAs(fileName);
                    }

                    using (var db = new BanHangEntity())
                    {
                        db.SanPhams.Add(sanPham);
                        db.SaveChanges();
                        return RedirectToAction("Edit", "SanPham", new { id = sanPham.SanPhamID });
                    }
                }
                else
                {
                    using (var db = new BanHangEntity())
                    {
                        sanPham.LoaiSanPhamCollection = db.LoaiSanPhams.ToList<LoaiSanPham>();
                        sanPham.DonViTinhCollection = db.DonViTinhs.ToList<DonViTinh>();
                        sanPham.NhaCungCapCollection = db.NhaCungCaps.ToList<NhaCungCap>();
                        return View(sanPham);
                    }
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
                        SanPham sanPham = db.SanPhams.Include("ThuocTinhSanPhams").Include("ThuocTinhSanPhams.ThuocTinh").FirstOrDefault(x => x.SanPhamID == id);
                        sanPham.LoaiSanPhamCollection = db.LoaiSanPhams.ToList<LoaiSanPham>();
                        sanPham.DonViTinhCollection = db.DonViTinhs.ToList<DonViTinh>();
                        sanPham.NhaCungCapCollection = db.NhaCungCaps.ToList<NhaCungCap>();

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
        public ActionResult Edit(SanPham sanPham)
        {
            if (Session["username"] == null)
                return RedirectToAction("/Index", "Users");
            else
            {
                if (ModelState.IsValid)
                {
                    if (sanPham.ImageFile != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(sanPham.ImageFile.FileName);
                        string extension = Path.GetExtension(sanPham.ImageFile.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        sanPham.AnhSanPham = "~/FileUpload/" + fileName;
                        fileName = Path.Combine(Server.MapPath("~/FileUpload/"), fileName);
                        sanPham.ImageFile.SaveAs(fileName);
                    }
                    using (var db = new BanHangEntity())
                    {
                        db.Entry(sanPham).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    using (var db = new BanHangEntity())
                    {
                        sanPham.LoaiSanPhamCollection = db.LoaiSanPhams.ToList<LoaiSanPham>();
                        sanPham.DonViTinhCollection = db.DonViTinhs.ToList<DonViTinh>();
                        sanPham.NhaCungCapCollection = db.NhaCungCaps.ToList<NhaCungCap>();
                        return View(sanPham);
                    }
                }
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
                        var sanPham = db.SanPhams.Include("LoaiSanPham").Include("DonViTinh").Include("NhaCungCap").FirstOrDefault(x => x.SanPhamID == id);
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
            if (Session["username"] == null)
                return RedirectToAction("/Index", "Users");
            else
            {
                using (var db = new BanHangEntity())
                {
                    try
                    {
                        var sanPham = db.SanPhams.Include("LoaiSanPham").Include("DonViTinh").Include("NhaCungCap").FirstOrDefault(x => x.SanPhamID == id);
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
        public ActionResult Delete(float id)
        {
            using (var db = new BanHangEntity())
            {
                var sanPham = db.SanPhams.Include("LoaiSanPham").Include("DonViTinh").Include("NhaCungCap").FirstOrDefault(x => x.SanPhamID == id);
                if (sanPham != null)
                {
                    if (sanPham.ChiTietDonDatHangs.Count() != 0)
                    {
                        ViewData["Loi1"] = "Sản phẩm đã có người đặt, không thể xóa";
                        return View(sanPham);
                    }
                    else
                    {
                        db.SanPhams.Remove(sanPham);
                        db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("/Index");
        }
    }
}