namespace WEBBANHANG.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;
    using System.Web.Mvc;

    [Table("SanPham")]
    public partial class SanPham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SanPham()
        {
            ChiTietDonDatHangs = new HashSet<ChiTietDonDatHang>();
            Media = new HashSet<Medium>();
            ThuocTinhSanPhams = new HashSet<ThuocTinhSanPham>();
        }

        public int SanPhamID { get; set; }
        
        public int? LoaiSanPhamID { get; set; }

        public int? DonViTinhID { get; set; }

        [Required(ErrorMessage = "Ký hiệu sản phẩm là bắt buộc!")]
        [StringLength(50)]
        public string KyHieuSanPham { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc!")]
        [StringLength(300)]
        public string TenSanPham { get; set; }

        [DisplayName("Upload Image")]
        public string AnhSanPham { get; set; }

        [StringLength(200)]
        public string MoTaNgan { get; set; }

        [AllowHtml]
        public string MoTa { get; set; }

        [StringLength(100)]
        public string NhaSanXuat { get; set; }

        public bool? AnHienSanPham { get; set; }

        public bool? AnHienNhaSanXuat { get; set; }

        [Required(ErrorMessage = "Giá bán sản phẩm là bắt buộc!")]
        [RegularExpression(@"^[0-9]+[0-9'\s]*$", ErrorMessage = "Giá sản phẩm chỉ gồm ký tự số!")]
        public double? GiaBan { get; set; }

        [StringLength(50)]
        public string ToaNha { get; set; }

        public int? ThuTu { get; set; }

        public DateTime? NgayDang { get; set; }

        public bool? LaSanPhamMoi { get; set; }

        public int? NhaCungCapID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietDonDatHang> ChiTietDonDatHangs { get; set; }

        public virtual DonViTinh DonViTinh { get; set; }

        public virtual LoaiSanPham LoaiSanPham { get; set; }
        public virtual NhaCungCap NhaCungCap { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Medium> Media { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ThuocTinhSanPham> ThuocTinhSanPhams { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }

        [NotMapped]
        public List<LoaiSanPham> LoaiSanPhamCollection { get; set; }
        [NotMapped]
        public List<DonViTinh> DonViTinhCollection { get; set; }
        [NotMapped]
        public List<NhaCungCap> NhaCungCapCollection { get; set; }
    }
}
