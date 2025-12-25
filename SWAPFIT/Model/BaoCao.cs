using System;
using System.ComponentModel.DataAnnotations;

namespace SWAPFIT.Models
{
    public class BaoCao
    {
        [Key]
        public int MaBaoCao { get; set; }

        public int MaNguoiBaoCao { get; set; }

        public int? MaDoCu { get; set; }

        [StringLength(255)]
        public string? LyDo { get; set; }

        public string? ChiTiet { get; set; }

        [StringLength(100)]
        public string? TrangThai { get; set; }

        public DateTime? NgayTao { get; set; }
    }
}
