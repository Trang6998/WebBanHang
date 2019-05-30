namespace WEBBANHANG.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NhaCungCap")]
    public partial class NhaCungCap
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NhaCungCap()
        {
            SanPhams = new HashSet<SanPham>();
        }
        public int NhaCungCapID { get; set; }

        [Required(ErrorMessage = "Tên nhà cung cấp là bắt buộc!")]
        [StringLength(300)]
        public string TenNhaCungCap { get; set; }

        [StringLength(500)]
        public string DiaChi { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc!")]
        [StringLength(13, MinimumLength = 8, ErrorMessage = "Trường số điện thoại có độ dài từ 8 - 13 ký tự!")]
        [RegularExpression(@"^[0-9]+[0-9'\s]*$", ErrorMessage = "Số điện thoại chỉ gồm ký tự số!")]
        public string SoDienThoai { get; set; }

        [Required(ErrorMessage = "Email nhà cung cấp là bắt buộc!")]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        public string GhiChu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}
