namespace WEBBANHANG.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    [Table("DichVu")]
    public partial class DichVu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DichVu()
        {
            DatDichVus = new HashSet<DatDichVu>();
            Media = new HashSet<Medium>();
        }

        public int DichVuID { get; set; }

        [Required(ErrorMessage = "Tên dịch vụ là bắt buộc!")]
        [StringLength(300)]
        public string TenDichVu { get; set; }

        [DisplayName("Upload Image")]
        public string AnhDaiDien { get; set; }

        [StringLength(500)]
        public string MoTaNgan { get; set; }

        public bool? TrangThai { get; set; }

        public string BaiViet { get; set; }

        [StringLength(50)]
        public string ToaNha { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DatDichVu> DatDichVus { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Medium> Media { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
    }
}
