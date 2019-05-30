namespace WEBBANHANG.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Medium
    {
        [Key]
        public int MediaID { get; set; }

        public int? DichVuID { get; set; }

        public int? SanPhamID { get; set; }

        public int? LaAnh { get; set; }

        public string DuongLink { get; set; }

        [StringLength(100)]
        public string MoTa { get; set; }

        public virtual DichVu DichVu { get; set; }

        public virtual SanPham SanPham { get; set; }
    }
}
