namespace WEBBANHANG.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DatDichVu")]
    public partial class DatDichVu
    {
        public int DatDichVuID { get; set; }

        public int? UserID { get; set; }

        public int? DichVuID { get; set; }

        public DateTime? NgayDat { get; set; }

        public string YeuCau { get; set; }

        public string GhiChu { get; set; }

        public virtual DichVu DichVu { get; set; }

        public virtual User User { get; set; }
    }
}
