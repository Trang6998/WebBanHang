namespace WEBBANHANG.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChiTietDonDatHang")]
    public partial class ChiTietDonDatHang
    {
        public int ChiTietDonDatHangID { get; set; }

        public int? DonDatHangID { get; set; }

        public int? SanPhamID { get; set; }

        public int? SoLuong { get; set; }

        public double? GiaXuat { get; set; }

        public virtual DonDatHang DonDatHang { get; set; }

        public virtual SanPham SanPham { get; set; }
    }
}
