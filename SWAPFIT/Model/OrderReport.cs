namespace SWAPFIT.Model
{
    public class OrderReport
    {
        public int MaNguoiDung { get; set; }
        public string TenDangNhap { get; set; }
        public string HoTen { get; set; }
        public int TongSoDon { get; set; }
        public int DonHienTai { get; set; }
        public int DonHoanThanh { get; set; }
        public int DonHuy { get; set; }
        public decimal TongTien { get; set; } // Tổng tiền cho tất cả các đơn hàng của người dùng
    }

}
