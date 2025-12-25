using System;
using System.ComponentModel.DataAnnotations;

namespace SWAPFIT.Models
{
    public class NhatKyHeThong
    {
        [Key]
        public int MaNhatKy { get; set; }

        [StringLength(255)]
        public string? HanhDong { get; set; }

        [StringLength(255)]
        public string? ThucHienBoi { get; set; }

        public DateTime? ThoiGian { get; set; }

        public string? MoTa { get; set; }
    }
}
