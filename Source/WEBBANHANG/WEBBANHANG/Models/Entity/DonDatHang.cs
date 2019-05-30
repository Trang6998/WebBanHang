namespace WEBBANHANG.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DonDatHang")]
    public partial class DonDatHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DonDatHang()
        {
            ChiTietDonDatHangs = new HashSet<ChiTietDonDatHang>();
        }

        public int DonDatHangID { get; set; }

        public int? TaiKhoanDatHangID { get; set; }

        public int? TaiKhoanNhanVienID { get; set; }

        [StringLength(50)]
        public string SoHieuDon { get; set; }

        public DateTime? NgayDat { get; set; }

        public DateTime? HenLayTu { get; set; }

        public DateTime? HenLayDen { get; set; }

        public DateTime? HenTraTu { get; set; }

        public DateTime? HenTraDen { get; set; }

        public bool? LaTre { get; set; }

        public string LyDoTraTre { get; set; }

        public DateTime? NgayGioTraThucTe { get; set; }

        public string GhiChu { get; set; }

        public int? TinhTrang { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietDonDatHang> ChiTietDonDatHangs { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }
    }
}
