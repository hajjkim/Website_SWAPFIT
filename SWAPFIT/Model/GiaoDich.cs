using System;
using System.ComponentModel.DataAnnotations;

namespace SWAPFIT.Models
{
    public class GiaoDich
    {
        [Key]
        public int MaGiaoDich { get; set; }

        public int MaYeuCau { get; set; }

        public DateTime? NgayGiaoDich { get; set; }

        [StringLength(100)]
        public string? TrangThai { get; set; }

        public decimal? GiaTri { get; set; }
    }
}
