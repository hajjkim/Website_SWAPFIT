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

        // SỬA: Bỏ dấu ? → không nullable nữa
        public bool DaXem { get; set; } = false;           // ← Thêm default false
        public DateTime NgayTao { get; set; } = DateTime.Now; // ← Thêm default

        public string? LoaiThongBao { get; set; }
    }
}