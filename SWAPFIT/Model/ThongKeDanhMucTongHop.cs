using System.ComponentModel.DataAnnotations;

namespace SWAPFIT.Models
{
    public class ThongKeDanhMucTongHop
    {
        [Key]
        public int MaDanhMuc { get; set; }
        public string TenDanhMuc { get; set; }
        public int TongSoBai { get; set; }
        public int TongSoLuotMua { get; set; }
    }
}
