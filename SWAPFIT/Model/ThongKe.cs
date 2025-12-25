using System;
using System.ComponentModel.DataAnnotations;

namespace SWAPFIT.Models
{
    public class ThongKe
    {
        [Key]
        public int MaThongKe { get; set; }

        public int? TongNguoiDung { get; set; }

        public int? TongBaiViet { get; set; }

        public int? TongGiaoDich { get; set; }

        public DateTime? NgayThongKe { get; set; }
    }
}
