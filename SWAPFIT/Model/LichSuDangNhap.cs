using System;
using System.ComponentModel.DataAnnotations;

namespace SWAPFIT.Models
{
    public class LichSuDangNhap
    {
        [Key]
        public int MaLichSu { get; set; }

        public int MaNguoiDung { get; set; }

        public DateTime? ThoiGianDangNhap { get; set; }

        [StringLength(100)]
        public string? DiaChiIP { get; set; }
    }
}
