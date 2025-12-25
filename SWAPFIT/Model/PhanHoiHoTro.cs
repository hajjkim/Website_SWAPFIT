using System;
using System.ComponentModel.DataAnnotations;

namespace SWAPFIT.Models
{
    public class PhanHoiHoTro
    {
        [Key]
        public int MaPhanHoi { get; set; }

        public int MaHoTro { get; set; }

        public string? NoiDung { get; set; }

        public DateTime? NgayPhanHoi { get; set; }
    }
}
