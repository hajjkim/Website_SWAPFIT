using System;
using System.ComponentModel.DataAnnotations;

namespace SWAPFIT.Models
{
    public class LuotTimKiem
    {
        [Key]
        public int MaTimKiem { get; set; }

        [StringLength(255)]
        public string? TuKhoa { get; set; }

        public int? SoLanTim { get; set; }

        public DateTime? LanCuoiTim { get; set; }
    }
}
