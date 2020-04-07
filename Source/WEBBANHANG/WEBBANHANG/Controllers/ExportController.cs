using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Hosting;
using WEBBANHANG.Models;
using System.IO;
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.Style;
using System.Diagnostics;
using System.Globalization;
using WEBBANHANG.Models.Entity;
using WEBBANHANG.Models;

namespace WEBBANHANG.Controllers
{

    public class ExportController : Controller
    {
        [HttpGet]
        public FileResult ExportXuLyDonHang(string searchDateS , string searchDateF , int option)
        {
            using (var db = new BanHangEntity())
            {
                string sWebRootFolder = HostingEnvironment.ApplicationPhysicalPath + "\\FileUpload";
                IQueryable<DonDatHang> results = db.DonDatHangs;

                results = results.Include(x => x.User);
                if (option == -1)
                    results = results.Where(o => o.TinhTrang != TrangThaiDonHang.CHUA_GUI && o.TinhTrang != TrangThaiDonHang.THANH_LY_HUY_HANG);
                else
                    results = results.Where(o => o.TinhTrang == option);

                string[] strTu, strDen;
                DateTime? tu = null, den = null;

                if (!string.IsNullOrWhiteSpace(searchDateS) && searchDateS != "undefined" && searchDateS != "null")
                {
                    strTu = searchDateS.Split(' ');
                    tu = DateTime.Parse(strTu[0]);
                    if (tu.HasValue)
                        results = results.Where(o => o.NgayDat >= tu);
                }

                if (!string.IsNullOrWhiteSpace(searchDateF) && searchDateF != "undefined" && searchDateF != "null")
                {
                    strDen = searchDateF.Split(' ');
                    den = DateTime.Parse(strDen[0]);
                    if (den.HasValue)
                        results = results.Where(o => o.NgayDat <= den);
                }
                results = results.OrderBy(o => o.TinhTrang);

                ExportFileDonHang(results.ToList(), "DanhSachDonDatHang", sWebRootFolder, tu, den);
                string fPath = sWebRootFolder + "\\" + "DanhSachDonDatHang.xlsx";
                System.IO.FileInfo fi = new System.IO.FileInfo(fPath);
                return File(fPath, System.Net.Mime.MediaTypeNames.Application.Octet, "DanhSachDonDatHang" + fi.Extension);
            }
        }
        public static void ExportFileDonHang(List<DonDatHang> lst, string FName, string sWebRootFolder, DateTime? tuNgay = null, DateTime? denNgay = null)
        {
            string sFileName = FName + @".xlsx";
            //string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }

            string[] header = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18" };
            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Mẫu 01");
                int dem = 1;
                //worksheet.View.FreezePanes(2, 1);
                foreach (string s in header)
                {
                    worksheet.Cells[9, dem].Value = header[dem - 1];
                    worksheet.Cells[9, dem].Style.Font.Italic = true;
                    dem++;
                }
                worksheet.Cells["A1:D1"].Merge = true;
                worksheet.Cells["A1:D1"].Value = "CÔNG TY CP CHĂM SÓC BẤT ĐỘNG SẢN";

                worksheet.Cells["O1:P1"].Merge = true;
                worksheet.Cells["O1:P1"].Value = "Mẫu 01 - Care+";

                worksheet.Cells["A2:D2"].Merge = true;
                worksheet.Cells["A2:D2"].Value = "Bộ phận phát triển dịch vụ CARE+";

                worksheet.Cells["A3:R3"].Merge = true;
                worksheet.Cells["A3:R3"].Value = "DANH SÁCH CHI TIÊT CÁC ĐƠN HÀNG (DÀNH ĐỂ ĐI ĐƯA HÀNG ĐẾN CÁC CĂN HỘ)";

                worksheet.Cells["A4:R4"].Merge = true;
                worksheet.Cells["A4:R4"].Value = "Từ ngày " + tuNgay?.ToString() + " đến ngày " + denNgay?.ToString();

                worksheet.Cells[7, 1, 8, 1].Merge = true;
                worksheet.Cells[7, 1, 8, 1].Value = "STT";

                worksheet.Cells[7, 2, 8, 2].Merge = true;
                worksheet.Cells[7, 2, 8, 2].Value = "Mã đơn hàng";

                worksheet.Cells[7, 3, 8, 3].Merge = true;
                worksheet.Cells[7, 3, 8, 3].Value = "Họ tên khách hàng";

                worksheet.Cells[7, 4, 8, 4].Merge = true;
                worksheet.Cells[7, 4, 8, 4].Value = "Số điện thoại";

                worksheet.Cells[7, 5, 8, 5].Merge = true;
                worksheet.Cells[7, 5, 8, 5].Value = "Địa chỉ";

                worksheet.Cells[7, 6, 8, 6].Merge = true;
                worksheet.Cells[7, 6, 8, 6].Value = "Thời gian đặt";

                worksheet.Cells[7, 7, 8, 7].Merge = true;
                worksheet.Cells[7, 7, 8, 7].Value = "Hẹn thời gian nhận";

                worksheet.Cells[7, 8, 8, 8].Merge = true;
                worksheet.Cells[7, 8, 8, 8].Value = "Ghi chú khách hàng";

                worksheet.Cells["I7:N7"].Merge = true;
                worksheet.Cells["I7:N7"].Value = "Chi tiết hàng hóa đặt mua";

                worksheet.Cells[8, 9].Value = "Sản phẩm";
                worksheet.Cells[8, 10].Value = "Nhà cung cấp";
                worksheet.Cells[8, 11].Value = "Số lượng";
                worksheet.Cells[8, 12].Value = "Đơn giá";
                worksheet.Cells[8, 13].Value = "Thành tiền";
                worksheet.Cells[8, 14].Value = "Tổng tiền";

                worksheet.Cells[7, 15, 8, 15].Merge = true;
                worksheet.Cells[7, 15, 8, 15].Value = "Người đưa hàng ký";

                worksheet.Cells[7, 16, 8, 16].Merge = true;
                worksheet.Cells[7, 16, 8, 16].Value = "Người nhận hàng ký";

                worksheet.Cells[7, 17, 8, 17].Merge = true;
                worksheet.Cells[7, 17, 8, 17].Value = "Ghi chú giao nhận hàng";

                worksheet.Cells[7, 18, 8, 18].Merge = true;
                worksheet.Cells[7, 18, 8, 18].Value = "Trạng thái đơn";
                using (var range = worksheet.Cells["A3:R5"])
                {
                    range.Style.Font.SetFromFont(new Font("Times New Roman", 12));
                    range.Style.Font.Bold = true;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }

                using (var range = worksheet.Cells["A7:R9"])
                {
                    // Set Border
                    range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Font.SetFromFont(new Font("Times New Roman", 12));
                    range.Style.Font.Bold = true;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.AutoFitColumns();
                }
                worksheet.Row(7).Height = 37;
                worksheet.Row(8).Height = 32;
                // add a new worksheet to the empty workbook
                int soChiTiet = 0;
                int i = 1;
                int stt = 0;
                double? tongTien = 0;
                foreach (DonDatHang donDatHang in lst)
                {
                    soChiTiet = donDatHang.ChiTietDonDatHangs != null ? donDatHang.ChiTietDonDatHangs.Count() : 0;

                    if (soChiTiet == 0)
                        soChiTiet = 1;

                    for (int j = 1; j <= 18; j++)
                        using (var range = worksheet.Cells[i + 9, j, i + 9 + soChiTiet - 1, j])
                        {
                            range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            range.AutoFitColumns();
                            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        }

                    {
                        worksheet.Cells[i + 9, 1, i + 9 + soChiTiet - 1, 1].Merge = true;
                        worksheet.Cells[i + 9, 1, i + 9 + soChiTiet - 1, 1].Value = stt + 1;

                        worksheet.Cells[i + 9, 2, i + 9 + soChiTiet - 1, 2].Merge = true;
                        if (donDatHang.SoHieuDon != null)
                            worksheet.Cells[i + 9, 2, i + 9 + soChiTiet - 1, 2].Value = donDatHang.SoHieuDon;

                        worksheet.Cells[i + 9, 3, i + 9 + soChiTiet - 1, 3].Merge = true;
                        if (donDatHang.User != null)
                            worksheet.Cells[i + 9, 3, i + 9 + soChiTiet - 1, 3].Value = donDatHang.User.HoTen != null ? donDatHang.User.HoTen : donDatHang.User.Email;
                        worksheet.Cells[i + 9, 4, i + 9 + soChiTiet - 1, 4].Merge = true;
                        if (donDatHang.User != null)
                            worksheet.Cells[i + 9, 4, i + 9 + soChiTiet - 1, 4].Value = donDatHang.User.SoDienThoai != null ? donDatHang.User.SoDienThoai : "";
                        worksheet.Cells[i + 9, 5, i + 9 + soChiTiet - 1, 5].Merge = true;
                        if (donDatHang.User != null)
                            worksheet.Cells[i + 9, 5, i + 9 + soChiTiet - 1, 5].Value = donDatHang.User.DiaChi != null ? donDatHang.User.DiaChi : "";

                        worksheet.Cells[i + 9, 6, i + 9 + soChiTiet - 1, 6].Merge = true;
                        if (donDatHang.NgayDat != null)
                            worksheet.Cells[i + 9, 6, i + 9 + soChiTiet - 1, 6].Value = donDatHang.NgayDat?.ToString();

                        worksheet.Cells[i + 9, 7, i + 9 + soChiTiet - 1, 7].Merge = true;
                        worksheet.Cells[i + 9, 7, i + 9 + soChiTiet - 1, 7].Value = (donDatHang.HenLayTu != null && donDatHang.HenLayDen != null) ?
                                                                (donDatHang.HenLayTu?.ToString() + '-' + donDatHang.HenLayDen?.ToString()) :
                                                                (donDatHang.HenLayTu != null ? "Sau " + donDatHang.HenLayTu?.ToString() :
                                                                (donDatHang.HenLayDen != null ? "Trước " + donDatHang.HenLayDen?.ToString() : "Mọi thời gian"));
                        worksheet.Cells[i + 9, 8, i + 9 + soChiTiet - 1, 8].Merge = true;
                        if (donDatHang.GhiChu != null)
                            worksheet.Cells[i + 9, 8, i + 9 + soChiTiet - 1, 8].Value = donDatHang.GhiChu;

                        var chiTietDonDatHangs = donDatHang.ChiTietDonDatHangs.ToList();

                        if ((donDatHang.ChiTietDonDatHangs != null ? donDatHang.ChiTietDonDatHangs.Count() : 0) != 0)
                            for (int k = 0; k < soChiTiet; k++)
                            {
                                if (chiTietDonDatHangs[k] != null)
                                {
                                    worksheet.Cells[i + 9 + k, 9].Value = chiTietDonDatHangs[k].SanPham != null ? chiTietDonDatHangs[k].SanPham.TenSanPham : "";
                                    worksheet.Cells[i + 9 + k, 10].Value = chiTietDonDatHangs[k].SanPham != null ? (chiTietDonDatHangs[k].SanPham.NhaCungCap != null ? (chiTietDonDatHangs[k].SanPham.NhaCungCap.TenNhaCungCap ) : "" ) : "";
                                    worksheet.Cells[i + 9 + k, 11].Value = chiTietDonDatHangs[k].SoLuong != null ? chiTietDonDatHangs[k].SoLuong : 0;
                                    worksheet.Cells[i + 9 + k, 12].Value = chiTietDonDatHangs[k].GiaXuat != null ? chiTietDonDatHangs[k].GiaXuat : 0;
                                    worksheet.Cells[i + 9 + k, 12].Style.Numberformat.Format = "###,###,###";
                                    worksheet.Cells[i + 9 + k, 13].Value = chiTietDonDatHangs[k].SoLuong != null && chiTietDonDatHangs[k].SanPham != null ? (chiTietDonDatHangs[k].SoLuong * chiTietDonDatHangs[k].GiaXuat) : 0;
                                    worksheet.Cells[i + 9 + k, 13].Style.Numberformat.Format = "###,###,###";
                                }
                            }

                        worksheet.Cells[i + 9, 14, i + 9 + soChiTiet - 1, 14].Merge = true;
                        worksheet.Cells[i + 9, 14, i + 9 + soChiTiet - 1, 14].Value = Convert.ToDecimal(donDatHang.ChiTietDonDatHangs.Select(y => y.SoLuong * (y.GiaXuat)).Sum());
                        worksheet.Cells[i + 9, 14, i + 9 + soChiTiet - 1, 14].Style.Numberformat.Format = "###,###,###";
                        worksheet.Cells[i + 9, 14, i + 9 + soChiTiet - 1, 14].Style.Font.Bold = true;
                        tongTien += donDatHang.ChiTietDonDatHangs.Select(y => y.SoLuong * y.GiaXuat).Sum();

                        worksheet.Cells[i + 9, 15, i + 9 + soChiTiet - 1, 15].Merge = true;
                        worksheet.Cells[i + 9, 15, i + 9 + soChiTiet - 1, 15].Value = "";
                        worksheet.Cells[i + 9, 16, i + 9 + soChiTiet - 1, 16].Merge = true;
                        worksheet.Cells[i + 9, 16, i + 9 + soChiTiet - 1, 16].Value = "";
                        worksheet.Cells[i + 9, 17, i + 9 + soChiTiet - 1, 17].Merge = true;
                        worksheet.Cells[i + 9, 17, i + 9 + soChiTiet - 1, 17].Value = "";

                        worksheet.Cells[i + 9, 18, i + 9 + soChiTiet - 1, 18].Merge = true;
                        worksheet.Cells[i + 9, 18, i + 9 + soChiTiet - 1, 18].Value = donDatHang.TinhTrang == TrangThaiDonHang.CHUA_GUI ? "Giỏ hàng" :
                                                         (donDatHang.TinhTrang == TrangThaiDonHang.DANG_XU_LY ? "Đang xử lý" :
                                                         (donDatHang.TinhTrang == TrangThaiDonHang.DA_DAT_HANG ? "Chưa nhận đơn" :
                                                         (donDatHang.TinhTrang == TrangThaiDonHang.DA_GIAO_CHUA_TRA_TIEN ? "Chưa thanh toán" :
                                                         (donDatHang.TinhTrang == TrangThaiDonHang.DA_GIAO_DA_TRA_TIEN ? "Hoàn thành" :
                                                         (donDatHang.TinhTrang == TrangThaiDonHang.THANH_LY_HUY_HANG ? "Thanh lý - hủy hàng" : "")))));

                    }
                    stt++;
                    i = i + soChiTiet;
                }
                worksheet.Cells[i + 9, 1, i + 9, 3].Merge = true;
                worksheet.Cells[i + 9, 1, i + 9, 3].Value = "TỔNG";

                worksheet.Cells[i + 9, 14].Value = tongTien;
                worksheet.Cells[i + 9, 14].Style.Numberformat.Format = "###,###,###";

                using (var range = worksheet.Cells[i + 9, 1, i + 9, 18])
                {
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.AutoFitColumns();
                    range.Style.Font.Bold = true;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }

                worksheet.Cells[i + 12, 3].Value = "NGƯỜI LẬP";
                worksheet.Cells[i + 12, 14, i + 12, 15].Merge = true;
                worksheet.Cells[i + 12, 14, i + 12, 15].Value = "PHỤ TRÁCH DỊCH VỤ";

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                worksheet.Cells[worksheet.Dimension.Address].Style.WrapText = true;

                package.Save();
            }
        }

        [HttpGet]
        public FileResult ExportMau03(string searchDateS = null, string searchDateF = null, int? option = null, int? sanPhamID = null)
        {
            using (var db = new BanHangEntity())
            {
                IQueryable<DonDatHang> results = db.DonDatHangs;
                string tenHang = "";
                if (option.HasValue)
                {
                    if (option == -1)
                        results = results.Where(o => o.TinhTrang != TrangThaiDonHang.CHUA_GUI && o.TinhTrang != TrangThaiDonHang.THANH_LY_HUY_HANG);
                    else
                        results = results.Where(o => o.TinhTrang == option);
                }
                string[] strTu, strDen;
                DateTime? tu = null, den = null;

                if (!string.IsNullOrWhiteSpace(searchDateS) && searchDateS != "undefined" && searchDateS != "null")
                {
                    strTu = searchDateS.Split(' ');
                    tu = DateTime.Parse(strTu[0]);
                    if (tu.HasValue)
                        results = results.Where(o => o.NgayDat >= tu);
                }

                if (!string.IsNullOrWhiteSpace(searchDateF) && searchDateF != "undefined" && searchDateF != "null")
                {
                    strDen = searchDateF.Split(' ');
                    den = DateTime.Parse(strDen[0]);
                    if (den.HasValue)
                        results = results.Where(o => o.NgayDat <= den);
                }
                if (sanPhamID.HasValue)
                {
                    results = results.Where(o => o.ChiTietDonDatHangs.Any(y => y.SanPhamID == sanPhamID));
                    tenHang = db.SanPhams.Find(sanPhamID) != null ? db.SanPhams.Find(sanPhamID).TenSanPham : "";
                }
                results = results.OrderBy(o => o.TinhTrang).OrderByDescending(o => o.NgayDat);
                string sWebRootFolder = HostingEnvironment.ApplicationPhysicalPath + "\\FileUpload";

                ExportMau03(results.ToList(), "Mau03", sWebRootFolder, tu, den, sanPhamID, tenHang);
                string fPath = sWebRootFolder + "\\" + "Mau03.xlsx";
                System.IO.FileInfo fi = new System.IO.FileInfo(fPath);
                return File(fPath, System.Net.Mime.MediaTypeNames.Application.Octet, "Mau03" + fi.Extension);
            }
        }
        public static void ExportMau03(List<DonDatHang> lst, string FName, string sWebRootFolder, DateTime? tuNgay = null, DateTime? denNgay = null, int? sanPhamID = null, string tenHang = null)
        {
            string sFileName = FName + @".xlsx";
            //string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }

            string[] header = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Mẫu 03");
                int dem = 1;
                //worksheet.View.FreezePanes(2, 1);
                foreach (string s in header)
                {
                    worksheet.Cells[7, dem].Value = header[dem - 1];
                    dem++;
                }
                worksheet.Cells["A1:D1"].Merge = true;
                worksheet.Cells["A1:D1"].Value = "CÔNG TY CP CHĂM SÓC BẤT ĐỘNG SẢN";

                worksheet.Cells["J1"].Value = "Mẫu 03 - Care+";

                worksheet.Cells["A2:D2"].Merge = true;
                worksheet.Cells["A2:D2"].Value = "Bộ phận phát triển dịch vụ CARE+";

                worksheet.Cells["A3:J3"].Merge = true;
                worksheet.Cells["A3:J3"].Value = "BẢNG KÊ MUA BÁN HÀNG HÓA (" + tenHang + ")";

                worksheet.Cells["A4:J4"].Merge = true;
                worksheet.Cells["A4:J4"].Value = "Từ ngày " + tuNgay?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " đến ngày " + denNgay?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);


                worksheet.Cells[6, 1].Value = "STT";
                worksheet.Cells[6, 2].Value = "Mã đơn hàng";
                worksheet.Cells[6, 3].Value = "Thời gian đặt";
                worksheet.Cells[6, 4].Value = "Địa chỉ khách hàng";
                worksheet.Cells[6, 5].Value = "Đơn vị tính";
                worksheet.Cells[6, 6].Value = "Số lượng";
                worksheet.Cells[6, 7].Value = "Giá bán";
                worksheet.Cells[6, 8].Value = "Thành tiền bán";
                worksheet.Cells[6, 9].Value = "Ghi chú";
                worksheet.Cells[6, 10].Value = "Trạng thái đơn";

                using (var range = worksheet.Cells["A3:J4"])
                {
                    range.Style.Font.SetFromFont(new Font("Times New Roman", 12));
                    range.Style.Font.Bold = true;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }
                using (var range = worksheet.Cells["A6:J7"])
                {
                    // Set Border
                    range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Font.SetFromFont(new Font("Times New Roman", 12));
                    range.Style.Font.Bold = true;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.AutoFitColumns();
                }

                // add a new worksheet to the empty workbook
                int i = 0;
                double? tongTien = 0;
                int tongSoLuong = 0;
                for (i = 0; i < lst.Count; i++)
                {
                    for (int j = 1; j <= 10; j++)
                    {
                        worksheet.Cells[i + 8, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Cells[i + 8, j].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[i + 8, j].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        worksheet.Cells[i + 8, j].AutoFitColumns();
                    }

                    worksheet.Cells[i + 8, 1].Value = i + 1;
                    if (lst[i].SoHieuDon != null)
                        worksheet.Cells[i + 8, 2].Value = lst[i].SoHieuDon;
                    if (lst[i].NgayDat != null)
                        worksheet.Cells[i + 8, 3].Value = lst[i].NgayDat?.ToString();
                    if (lst[i].User != null)
                        worksheet.Cells[i + 8, 4].Value = (lst[i].User.HoTen != null ? (lst[i].User.HoTen) : "") + (lst[i].User.SoDienThoai != null ? " - " + lst[i].User.SoDienThoai : "");
                    if (sanPhamID.HasValue && lst[i].ChiTietDonDatHangs.FirstOrDefault(x => x.SanPhamID == sanPhamID) != null)
                    {
                        var chiTietDon = lst[i].ChiTietDonDatHangs.FirstOrDefault(x => x.SanPhamID == sanPhamID);
                        if (chiTietDon.SanPham != null && chiTietDon.SanPham.DonViTinh != null)
                            worksheet.Cells[i + 8, 5].Value = chiTietDon.SanPham.DonViTinh.TenDonVi;
                        if (chiTietDon.SoLuong != null)
                        {
                            worksheet.Cells[i + 8, 6].Value = chiTietDon.SoLuong;
                            tongSoLuong += (int)chiTietDon.SoLuong;
                        }
                        if (chiTietDon.GiaXuat != null)
                            worksheet.Cells[i + 8, 7].Value = chiTietDon.GiaXuat;
                        if (chiTietDon.SoLuong != null && chiTietDon.GiaXuat != null)
                        {
                            worksheet.Cells[i + 8, 8].Value = chiTietDon.SoLuong * chiTietDon.GiaXuat;
                            tongTien += (int)chiTietDon.SoLuong * chiTietDon.GiaXuat;
                        }
                    }
                    if (lst[i].GhiChu != null)
                        worksheet.Cells[i + 8, 9].Value = lst[i].GhiChu;
                    if (lst[i].TinhTrang != null)
                    {
                        if (lst[i].TinhTrang == TrangThaiDonHang.DA_DAT_HANG)
                            worksheet.Cells[i + 8, 10].Value = "Chưa nhận đơn";
                        else if (lst[i].TinhTrang == TrangThaiDonHang.DANG_XU_LY)
                            worksheet.Cells[i + 8, 10].Value = "Đang xử lý";
                        else if (lst[i].TinhTrang == TrangThaiDonHang.DA_GIAO_DA_TRA_TIEN)
                            worksheet.Cells[i + 8, 10].Value = "Hoàn thành";
                        else if (lst[i].TinhTrang == TrangThaiDonHang.DA_GIAO_CHUA_TRA_TIEN)
                            worksheet.Cells[i + 8, 10].Value = "Chưa thanh toán";
                        else
                            worksheet.Cells[i + 8, 10].Value = "Không xác định";
                    }

                }

                worksheet.Cells[i + 8, 1, i + 8, 3].Merge = true;
                worksheet.Cells[i + 8, 1, i + 8, 3].Value = "TỔNG";
                worksheet.Cells[i + 8, 6].Value = tongSoLuong;
                worksheet.Cells[i + 8, 8].Value = tongTien;
                worksheet.Cells[i + 8, 8].Style.Numberformat.Format = "###,###,###";

                using (var range = worksheet.Cells[i + 8, 1, i + 8, 10])
                {
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.AutoFitColumns();
                    range.Style.Font.Bold = true;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }

                worksheet.Cells[i + 10, 2].Value = "NGƯỜI LẬP";
                worksheet.Cells[i + 10, 8, i + 10, 9].Merge = true;
                worksheet.Cells[i + 10, 8, i + 10, 9].Value = "TRƯỞNG BỘ PHẬN";

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                worksheet.Cells[worksheet.Dimension.Address].Style.WrapText = true;
                package.Save();
            }
        }


    }
}
