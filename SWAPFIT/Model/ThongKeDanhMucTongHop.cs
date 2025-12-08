namespace SWAPFIT.Models
{
    public class ThongKeDanhMucTongHop
    {
        public int MaDanhMuc { get; set; }
        public string TenDanhMuc { get; set; }

        public int TongSoBai { get; set; }      // tổng số bài đăng thuộc danh mục
        public int TongSoLuotMua { get; set; }  // số đơn mua (lượt mua) thuộc danh mục
    }
}
