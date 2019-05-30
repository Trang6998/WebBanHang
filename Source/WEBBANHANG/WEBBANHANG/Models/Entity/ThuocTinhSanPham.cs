namespace WEBBANHANG.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ThuocTinhSanPham")]
    public partial class ThuocTinhSanPham
    {
        public int ThuocTinhSanPhamID { get; set; }

        public int? SanPhamID { get; set; }

        public int? ThuocTinhID { get; set; }

        [StringLength(500)]
        public string NoiDungMoTa { get; set; }
        [StringLength(500)]
        public string GhiChu { get; set; }

        public virtual SanPham SanPham { get; set; }

        public virtual ThuocTinh ThuocTinh { get; set; }
    }
}
