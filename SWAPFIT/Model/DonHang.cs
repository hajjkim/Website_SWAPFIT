using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWAPFIT.Models
{
    public class DonHang
    {
        [Key]
        public int MaDonHang { get; set; }

        // Cột này có thể bị thừa, nếu muốn liên kết với người dùng (người mua) thông qua MaNguoiMua và MaNguoiBan thì không cần phải có cột MaNguoiDung nữa.
        // public int? MaNguoiDung { get; set; }  // Không cần thiết nếu đã có MaNguoiMua và MaNguoiBan

        public int? MaNguoiMua { get; set; }
        public int? MaNguoiBan { get; set; }

        [ForeignKey("MaNguoiMua")]
        public NguoiDung? NguoiMua { get; set; }

        [ForeignKey("MaNguoiBan")]
        public NguoiDung? NguoiBan { get; set; }


        [Required]
        [StringLength(255)]
        public string DiaChiGiaoHang { get; set; }

        [Required]
        [StringLength(50)]
        public string TrangThai { get; set; } = "Đang xử lý";

        [DataType(DataType.DateTime)]
        public DateTime NgayDat { get; set; } = DateTime.Now;

        public decimal TongTien { get; set; }

        [StringLength(50)]
        public string? PhuongThucThanhToan { get; set; }

        [StringLength(50)]
        public string? PhuongThucGiaoHang { get; set; }

        // Quan hệ với bảng ChiTietDonHang (một đơn hàng có nhiều chi tiết)
        public List<ChiTietDonHang> ChiTietDonHangs { get; set; } = new();

        public string? LyDoHuy { get; set; }
    }
}
