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

        // Bài viết người dùng đăng
        [InverseProperty("NguoiDung")]
        public ICollection<BaiViet>? BaiViets { get; set; }

        // Đơn người dùng đã mua
        public ICollection<DonHang>? DonMua { get; set; }

        // Đơn người dùng bán (người khác mua sản phẩm của họ)
        public ICollection<DonHang>? DonBan { get; set; }
    }
}
