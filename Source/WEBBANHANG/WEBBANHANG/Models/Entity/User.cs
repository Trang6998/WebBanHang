namespace WEBBANHANG.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Users")]
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            DatDichVus = new HashSet<DatDichVu>();
            DonDatHangs = new HashSet<DonDatHang>();
            DonDatHangs1 = new HashSet<DonDatHang>();
        }

        public int UserId { get; set; }

        public DateTime? NgayLap { get; set; }

        [Required(ErrorMessage = "Tên người dùng là bắt buộc!")]
        [StringLength(50)]
        public string HoTen { get; set; }

        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        public string GhiChu { get; set; }

        [Required(ErrorMessage = "Địa chỉ user là bắt buộc!")]
        public string DiaChi { get; set; }

        public int? LoaiUser { get; set; }

        [StringLength(50)]
        public string PassWord { get; set; }

        [StringLength(50)]
        public string TaiKhoan { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc!")]
        [StringLength(13, MinimumLength = 8, ErrorMessage = "Trường số điện thoại có độ dài từ 8 - 13 ký tự!")]
        [RegularExpression(@"^[0-9]+[0-9'\s]*$", ErrorMessage = "Số điện thoại chỉ gồm ký tự số!")]
        public string SoDienThoai { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DatDichVu> DatDichVus { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DonDatHang> DonDatHangs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DonDatHang> DonDatHangs1 { get; set; }
    }
}
