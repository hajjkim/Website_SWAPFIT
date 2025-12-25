namespace SWAPFIT.Models
{
    public class OrderStatusReport
    {
        public string TrangThai { get; set; }
        public int SoLuong { get; set; }
        public decimal TongTien { get; set; } // Tổng tiền của các đơn hàng ở trạng thái này
    }

}
