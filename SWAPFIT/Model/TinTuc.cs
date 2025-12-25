using System;
using System.ComponentModel.DataAnnotations;

namespace SWAPFIT.Models
{
    public class TinTuc
    {
        [Key]
        public int MaTinTuc { get; set; }

        [StringLength(255)]
        public string? TieuDe { get; set; }

        public string? NoiDung { get; set; }

        public string? HinhAnh { get; set; }

        public int? MaTheLoai { get; set; }

        public DateTime? NgayDang { get; set; }
        [StringLength(50)]
        public string? KichThuoc { get; set; }
        [StringLength(100)]
        public string? LoaiSanPham { get; set; }
    }
}
