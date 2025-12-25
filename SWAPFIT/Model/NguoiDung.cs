using SWAPFIT.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWAPFIT.Models
{
    public class NguoiDung
    {
        [Key]
        public int MaNguoiDung { get; set; }

        [Required]
        [StringLength(100)]
        public string TenDangNhap { get; set; }

        [Required]
        [StringLength(255)]
        public string MatKhau { get; set; }

        [StringLength(255)]
        public string? HoTen { get; set; }

        [StringLength(255)]
        public string? Email { get; set; }

        [StringLength(20)]
        public string? SoDienThoai { get; set; }

        public string? AnhDaiDien { get; set; }

        public string? VaiTro { get; set; } = "User";
        public string? TrangThai { get; set; } = "Hoạt động";

        public DateTime? NgayTao { get; set; }

        [InverseProperty("NguoiDung")]
        public ICollection<BaiViet>? BaiViets { get; set; }

        public ICollection<DonHang>? DonMua { get; set; }

        public ICollection<DonHang>? DonBan { get; set; }
        public TaiKhoanNganHang? TaiKhoanNganHang { get; set; }

    }
}
