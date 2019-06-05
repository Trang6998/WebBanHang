using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBBANHANG.Models.Entity;
using WEBBANHANG.Models;
using WEBBANHANG.Models;

namespace WEBBANHANG.Controllers
{
    public class ListSanPham
    {
        public List<SanPham> SanPhams { get; set; }
        public int? SoLuong { get; set; }
    }
    public class HomeController : Controller
    {
        BanHangEntity db = new BanHangEntity();

        public ActionResult Index()
        {
            if (Session["username"] == null)
                return RedirectToAction("/Index", "Users");
            else
            {
                int id = (int)Session["usernameid"];
                var donDatHang = db.DonDatHangs.Where(x => x.TaiKhoanDatHangID == id && x.TinhTrang == 0).FirstOrDefault();

                return View();
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        public ActionResult Cart(int SanPhamID, int SoLuong)
        {
            using (var db = new BanHangEntity())
            {
                if (Session["username"] == null)
                {
                    return RedirectToAction("/Index", "Users");
                }
                else
                {
                    int id = (int)Session["usernameid"];
                    DonDatHang donDatHang = new DonDatHang();
                    donDatHang = db.DonDatHangs.Where(x => x.TaiKhoanDatHangID == id && x.TinhTrang == 0).FirstOrDefault();
                    if (donDatHang != null)
                    {
                        ChiTietDonDatHang chiTietDonDatHang = new ChiTietDonDatHang();
                        chiTietDonDatHang = donDatHang.ChiTietDonDatHangs.FirstOrDefault(x => x.SanPhamID == SanPhamID);

                        if (chiTietDonDatHang != null)
                        {
                            chiTietDonDatHang.SoLuong = chiTietDonDatHang.SoLuong + SoLuong;
                            db.Entry(chiTietDonDatHang).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            ChiTietDonDatHang chiTietDonDatHangadd = new ChiTietDonDatHang();
                            chiTietDonDatHangadd.DonDatHangID = donDatHang.DonDatHangID;
                            chiTietDonDatHangadd.SanPhamID = SanPhamID;
                            chiTietDonDatHangadd.SoLuong = SoLuong;
                            chiTietDonDatHangadd.GiaXuat = db.SanPhams.Find(SanPhamID).GiaBan;
                            db.ChiTietDonDatHangs.Add(chiTietDonDatHangadd);
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        DonDatHang dondat = new DonDatHang();
                        dondat.TaiKhoanDatHangID = id;
                        dondat.NgayDat = DateTime.Now;
                        var shd = "SHD" + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Year + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second;
                        dondat.SoHieuDon = shd;
                        dondat.TinhTrang = TrangThaiDonHang.CHUA_GUI;
                        ChiTietDonDatHang chiTietDonDatHang = new ChiTietDonDatHang();
                        chiTietDonDatHang.SanPhamID = SanPhamID;
                        chiTietDonDatHang.SoLuong = SoLuong;
                        chiTietDonDatHang.GiaXuat = db.SanPhams.Find(SanPhamID).GiaBan;
                        dondat.ChiTietDonDatHangs.Add(chiTietDonDatHang);
                        db.DonDatHangs.Add(dondat);
                        db.SaveChanges();
                    }
                    DonDatHang donDat = new DonDatHang();
                    donDat = db.DonDatHangs.Where(x => x.TaiKhoanDatHangID == id && x.TinhTrang == 0).FirstOrDefault();
                    int? soluong = 0;
                    foreach (var i in donDat.ChiTietDonDatHangs)
                    {
                        soluong = soluong + i.SoLuong;
                    }
                    Session["soluong"] = soluong;
                    return RedirectToAction("DetailProductView", new { sanPhamID = SanPhamID });
                }

            }
        }
        public ActionResult ListDonDat()
        {
            using (var db = new BanHangEntity())
            {
                if (Session["username"] == null)
                {
                    return RedirectToAction("/Index", "Users");
                }
                else
                {
                    int id = (int)Session["usernameid"];
                    var donDatHang = db.DonDatHangs.Include(x => x.ChiTietDonDatHangs.Select(y => y.SanPham)).Where(x => x.TaiKhoanDatHangID == id).ToList();
                    ViewBag.donDatHang = donDatHang;
                    return View();
                }
            }
        }


        public ActionResult ChiTietDon(int donDatHangID)
        {
            using (var db = new BanHangEntity())
            {
                if (Session["username"] == null)
                {
                    return RedirectToAction("/Index", "Users");
                }
                else
                {
                    var donDatHang = db.DonDatHangs.Include(y => y.ChiTietDonDatHangs.Select(z => z.SanPham)).Where(x => x.DonDatHangID == donDatHangID).FirstOrDefault();
                    //ListSanPham lstSP = new ListSanPham();
                    //foreach(var i in donDatHang.ChiTietDonDatHangs)
                    //{
                    //    SanPham sp = i.SanPham;
                    //    lstSP.SanPhams.Add(sp);
                    //    lstSP.SoLuong = i.SoLuong;

                    //}
                    ViewBag.donDatHang = donDatHang;
                    return View();
                }
            }
        }
        public ActionResult CartView()
        {
            using (var db = new BanHangEntity())
            {
                // Nếu chưa đăng nhập thì trả về trang đăng nhập
                if (Session["username"] == null)
                {
                    return RedirectToAction("/Index", "Users");
                }
                // Nếu đăng nhập rồi thì hiển thị danh sách hàng trong giỏ
                else
                {
                    int id = (int)Session["usernameid"];  // Session chứa id của tài khoản đăng nhập
                    var lstSanPham = db.ChiTietDonDatHangs.Include(x => x.SanPham).ToList();
                    var donDatHang = db.DonDatHangs.Where(x => x.TaiKhoanDatHangID == id && x.TinhTrang == 0).FirstOrDefault();
                    if (donDatHang != null)
                    {
                        lstSanPham = lstSanPham.Where(x => x.DonDatHangID == donDatHang.DonDatHangID).ToList();
                    }
                    else
                    {
                        lstSanPham = null;
                    }
                    double? TongTien = 0;
                    if (donDatHang != null)
                    {
                        foreach (var i in donDatHang.ChiTietDonDatHangs)
                        {
                            TongTien = TongTien + (i.GiaXuat * i.SoLuong);

                        }
                    }
                    ViewBag.TongTien = TongTien;
                    //ViewBag.lstSanPham = lstSanPham;
                    return View(lstSanPham);
                }
            }
        }
        public ActionResult DeleteItem(int SanPhamID)
        {

            using (var db = new BanHangEntity())
            {
                if (Session["username"] == null)
                {
                    return RedirectToAction("/Index", "Users");
                }
                else
                {
                    int id = (int)Session["usernameid"];
                    //var lstSanPham = db.SanPhams.Include(x => x.ChiTietDonDatHangs).ToList();
                    var donDatHang = db.DonDatHangs.Where(x => x.TaiKhoanDatHangID == id && x.TinhTrang == 0).FirstOrDefault();
                    var chiTiet = db.ChiTietDonDatHangs.Where(x => x.SanPhamID == SanPhamID && x.DonDatHangID == donDatHang.DonDatHangID).FirstOrDefault();
                    if (donDatHang != null)
                    {
                        donDatHang.ChiTietDonDatHangs.Remove(chiTiet);
                        db.SaveChanges();
                        int? soluong = 0;
                        foreach (var i in donDatHang.ChiTietDonDatHangs)
                        {
                            soluong = soluong + i.SoLuong;
                        }
                        Session["soluong"] = soluong;
                    }

                    return RedirectToAction("/CartView", "Home");
                }
            }

        }
        public ActionResult DatHang(DateTime? henTu, DateTime? henDen, string ghiChu)
        {

            using (var db = new BanHangEntity())
            {
                if (Session["username"] == null)
                {
                    return RedirectToAction("/Index", "Users");
                }
                else
                {
                    int id = (int)Session["usernameid"];
                    //var lstSanPham = db.SanPhams.Include(x => x.ChiTietDonDatHangs).ToList();
                    var donDatHang = db.DonDatHangs.Where(x => x.TaiKhoanDatHangID == id && x.TinhTrang == 0).FirstOrDefault();
                    //var chiTiet = db.ChiTietDonDatHangs.Where(x => x.SanPhamID == SanPhamID).FirstOrDefault();
                    if (donDatHang != null)
                    {
                        if (henTu.HasValue && henDen.HasValue && String.IsNullOrEmpty(ghiChu))
                        {
                            donDatHang.TinhTrang = 1;
                            donDatHang.HenLayTu = henTu;
                            donDatHang.HenLayDen = henDen;
                            db.Entry(donDatHang).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        else if (!henTu.HasValue && henDen.HasValue && String.IsNullOrEmpty(ghiChu))
                        {
                            donDatHang.TinhTrang = 1;
                            donDatHang.HenLayDen = henDen;
                            db.Entry(donDatHang).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        else if (henTu.HasValue && !henDen.HasValue && String.IsNullOrEmpty(ghiChu))
                        {
                            donDatHang.TinhTrang = 1;
                            donDatHang.HenLayTu = henTu;
                            db.Entry(donDatHang).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        else if (!henTu.HasValue && !henDen.HasValue && String.IsNullOrEmpty(ghiChu))
                        {
                            donDatHang.TinhTrang = 1;
                            db.Entry(donDatHang).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        else if (!henTu.HasValue && henDen.HasValue && (!String.IsNullOrEmpty(ghiChu)))
                        {
                            donDatHang.TinhTrang = 1;
                            donDatHang.GhiChu = ghiChu;
                            donDatHang.HenLayDen = henDen;
                            db.Entry(donDatHang).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        else if (henTu.HasValue && !henDen.HasValue && (!String.IsNullOrEmpty(ghiChu)))
                        {
                            donDatHang.TinhTrang = 1;
                            donDatHang.GhiChu = ghiChu;
                            donDatHang.HenLayTu = henTu;
                            db.Entry(donDatHang).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        else if (!henTu.HasValue && !henDen.HasValue && (!String.IsNullOrEmpty(ghiChu)))
                        {
                            donDatHang.TinhTrang = 1;
                            donDatHang.GhiChu = ghiChu;
                            db.Entry(donDatHang).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        else if (henTu.HasValue && henDen.HasValue && (!String.IsNullOrEmpty(ghiChu)))
                        {
                            donDatHang.TinhTrang = 1;
                            donDatHang.HenLayTu = henTu;
                            donDatHang.HenLayDen = henDen;
                            donDatHang.GhiChu = ghiChu;
                            db.Entry(donDatHang).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }

                    }
                    Session["soluong"] = 0;
                    return RedirectToAction("/Index", "Home");
                }
            }
        }
        public ActionResult CheckOutView()
        {
            if (Session["username"] == null)
                return RedirectToAction("/Index", "Users");
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult CheckOut(int id)
        {
            return View();
        }
        public ActionResult ListProductView(int? loaiSanPhamID, int? khoangGiaTu, int? khoangGiaDen, string laMoi, string sortOrder, string currentFilter, string tenSanPham, int? page)
        {
            if (Session["username"] == null)
                return RedirectToAction("/Index", "Users");
            else
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
                ViewBag.khoangGiaTu = khoangGiaTu;
                ViewBag.khoangGiaDen = khoangGiaDen;
                if(laMoi!= null)
                {
                    ViewBag.laMoi = true ;

                }
                if (tenSanPham != null)
                {
                    page = 1;
                }
                else
                {
                    tenSanPham = currentFilter;
                }

                ViewBag.CurrentFilter = tenSanPham;
                if (loaiSanPhamID.HasValue)
                {
                    //lstSP = db.SanPhams.Where(x => x.LoaiSanPhamID == loaiSanPhamID).ToList();
                    string query = "Select * from SanPham where LoaiSanPhamID = " + loaiSanPhamID;
                    string qrten = "";
                    string qrgiatu = "";
                    string qrgiaden = "";
                    string qrlaMoi = "";
                    if (!String.IsNullOrEmpty(tenSanPham))
                        qrten = " and TenSanPham like N'%" + tenSanPham + "%'";

                    if (khoangGiaTu.HasValue)
                        qrgiatu = " and GiaBan >= " + khoangGiaTu;

                    if (khoangGiaDen.HasValue)
                        qrgiaden = " and GiaBan <= " + khoangGiaDen;
                    if (!String.IsNullOrEmpty(laMoi))
                    {
                        if (laMoi == "on")
                        {
                            qrlaMoi = " and LaSanPhamMoi =" + 0;
                        }
                        if (laMoi != "")
                        {
                            qrlaMoi = " and LaSanPhamMoi =" + 1;
                        }
                    }
                    if (!String.IsNullOrEmpty(tenSanPham) || (khoangGiaTu.HasValue) || (khoangGiaDen.HasValue) || (!String.IsNullOrEmpty(laMoi)))
                        query = query + " " + qrten + (qrgiatu != "" ? (qrten != "" ? "  " : "") + qrgiatu : "") + (qrgiaden != "" ? (qrgiatu != "" ? "  " : "") + qrgiaden : "") + (qrlaMoi != "" ? (qrgiaden != "" ? "  " : "") + qrlaMoi : "");

                    int pageSize = 4;
                    int pageNumber = (page ?? 1);
                    //ViewBag.lstSP = lstSP;
                    ViewBag.loaiSanPhamID = loaiSanPhamID;
                    var listSP = db.SanPhams.SqlQuery(query);
                    return View(listSP.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    return View();
                }


            }
        }
        [HttpPost]
        public ActionResult ListProduct(int id)
        {
            return View();
        }
        public ActionResult DetailProductView(int sanPhamID)
        {
            if (Session["username"] == null)
                return RedirectToAction("/Index", "Users");
            else
            {
                var sanPham = db.SanPhams.Include(x => x.NhaCungCap).Include(x => x.ThuocTinhSanPhams.Select(y => y.ThuocTinh)).Where(x => x.SanPhamID == sanPhamID).FirstOrDefault();
                ViewBag.sanPham = sanPham;
                return View(sanPham);
            }
        }
        [HttpPost]
        public ActionResult DetailProduct(int id)
        {
            return View();
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
    }
}