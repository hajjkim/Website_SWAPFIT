using System;
using System.ComponentModel.DataAnnotations;

namespace SWAPFIT.Models
{
    public class DoCu
    {
        [Key]
        public int MaDoCu { get; set; }

        public int MaNguoiDung { get; set; }
        public int? MaDanhMuc { get; set; }
        public int? MaThuongHieu { get; set; }

        [StringLength(255)]
        public string? TenSanPham { get; set; }

        public string? MoTa { get; set; }

        public string? TinhTrang { get; set; }

        [StringLength(50)]
        public string? PhuongThucTraoDoi { get; set; }

        [StringLength(50)]
        public string? LoaiBaiDang { get; set; }

        public int? DiaChiId { get; set; }

        public decimal? GiaTriUocTinh { get; set; }

        public DateTime? NgayDang { get; set; } = DateTime.Now;

        [StringLength(50)]
        public string? TrangThai { get; set; }

        [StringLength(255)]
        public string? AnhSanPham { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime NgayTao { get; set; } = DateTime.Now;

        // 🆕 THÊM SIZE
        [StringLength(20)]
        public string? Size { get; set; }
    }
}
