using System.Collections.Generic;

namespace SWAPFIT.Models
{
    public class ThongKeBaiVietNguoiDung
    {
        public int MaNguoiDung { get; set; }
        public string TenDangNhap { get; set; }
        public string HoTen { get; set; }

        public int TongSoBai { get; set; }
        public int ChoDuyet { get; set; }
        public int DangHienThi { get; set; }
        public int TuChoi { get; set; }

        // Dùng cho trang chi tiết 1 user
        public List<BaiViet> DanhSachBaiViet { get; set; } = new List<BaiViet>();
    }
}
