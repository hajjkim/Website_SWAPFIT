using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWAPFIT.Models
{
    public class BaiViet
    {
        internal readonly string HoTen;
        internal readonly string TenDangNhap;
        internal readonly string TenDanhMuc;
        internal readonly string? DuongDan;

        [Key]
        public int MaBaiViet { get; set; }

        [Required]
        [ForeignKey("NguoiDung")] 
        public int MaNguoiDung { get; set; }

        public NguoiDung? NguoiDung { get; set; }

        public int? MaDanhMuc { get; set; }
        public int? MaThuongHieu { get; set; }
        public int? MaDiaChi { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề.")]
        public string? TieuDe { get; set; }

        public string? NoiDung { get; set; }

        [StringLength(50)]
        public string? LoaiBaiDang { get; set; } // “Tặng” hoặc “Bán”

        public decimal? GiaSanPham { get; set; } // Giá nếu là “Bán”

        [StringLength(100)]
        public string? TrangThai { get; set; }

        public DateTime? NgayTao { get; set; }

        [StringLength(20)]
        public string? Size { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int SoLuong { get; set; }

        public ICollection<AnhBaiViet>? AnhBaiViets { get; set; }
        [ForeignKey(nameof(MaDanhMuc))]
        public DanhMuc? DanhMuc { get; set; }

        [ForeignKey(nameof(MaThuongHieu))]
        public ThuongHieu? ThuongHieu { get; set; }

        [ForeignKey(nameof(MaDiaChi))]
        public DiaChi? DiaChi { get; set; }
        public string? LyDo { get; set; } 
    }
}
