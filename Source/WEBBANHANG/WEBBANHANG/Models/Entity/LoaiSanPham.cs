namespace WEBBANHANG.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    [Table("LoaiSanPham")]
    public partial class LoaiSanPham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LoaiSanPham()
        {
            LoaiSanPham1 = new HashSet<LoaiSanPham>();
            SanPhams = new HashSet<SanPham>();
        }

        public int LoaiSanPhamID { get; set; }

        public int? LoaiSanPhamPID { get; set; }

        [Required(ErrorMessage = "Tên danh mục là bắt buộc!")]
        [StringLength(50)]
        public string TenLoai { get; set; }

        [StringLength(500)]
        public string MoTa { get; set; }

        [StringLength(500)]
        public string AnhDaiDien { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LoaiSanPham> LoaiSanPham1 { get; set; }

        public virtual LoaiSanPham LoaiSanPham2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SanPham> SanPhams { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
    }
}
