using SWAPFIT.Models;
using System.ComponentModel.DataAnnotations;

namespace SWAPFIT.Model
{
    public class BaiVietDTO
    {
        public int MaBaiViet { get; set; }
        public string TieuDe { get; set; }
        public string? LyDo { get; set; }
        public int MaNguoiDung { get; set; }
        public int SoLuong { get; set; }
        public string HoTen { get; set; }
        public string TenDangNhap { get; set; }
        public string TenDanhMuc { get; set; }
        public string NoiDung { get; set; }
        public decimal? GiaSanPham { get; set; }
        public string LoaiBaiDang { get; set; } 
        public DateTime? NgayTao { get; set; }
        public string? TrangThai { get; set; }
        public int? MaDanhMuc { get; set; }
        public int? MaThuongHieu { get; set; }
        public int? MaDiaChi { get; set; }
        public List<string> HinhAnhs { get; set; }
    }
}
