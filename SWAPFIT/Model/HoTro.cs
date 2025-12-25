using System;
using System.ComponentModel.DataAnnotations;

namespace SWAPFIT.Models
{
    public class HoTro
    {
        [Key]
        public int MaHoTro { get; set; }

        public int MaNguoiDung { get; set; }

        [StringLength(255)]
        public string? ChuDe { get; set; }

        public string? NoiDung { get; set; }

        [StringLength(100)]
        public string? TrangThai { get; set; }

        public DateTime? NgayGui { get; set; }
    }
}
