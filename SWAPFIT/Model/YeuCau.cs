using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWAPFIT.Models
{
    public class YeuCau
    {
        [Key]
        public int MaYeuCau { get; set; }

        public int NguoiGuiId { get; set; }
        public int NguoiNhanId { get; set; }

        public int? MaDoCu { get; set; }

        [StringLength(255)]
        public string? NoiDung { get; set; }

        [StringLength(100)]
        public string? TrangThai { get; set; }

        public DateTime? NgayGui { get; set; }

        // 🟢 Navigation properties (để EF nhận dạng trong OnModelCreating)
        [ForeignKey(nameof(NguoiGuiId))]
        public NguoiDung? NguoiGui { get; set; }

        [ForeignKey(nameof(NguoiNhanId))]
        public NguoiDung? NguoiNhan { get; set; }
    }
}
