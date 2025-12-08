using System;
using System.ComponentModel.DataAnnotations;

namespace SWAPFIT.Models
{
    public class ThongBao
    {
        [Key]
        public int MaThongBao { get; set; }

        public int MaNguoiDung { get; set; }

        public string? NoiDung { get; set; }

        public string? LienKet { get; set; }

        public bool? DaXem { get; set; }
        public string? LoaiThongBao { get; set; }
        public DateTime? NgayTao { get; set; }
    }
}
