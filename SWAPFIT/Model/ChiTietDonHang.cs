using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWAPFIT.Models
{
    public class ChiTietDonHang
    {
        [Key]
        public int MaChiTietDonHang { get; set; }

        public int MaDonHang { get; set; }
        public int MaBaiViet { get; set; }

        public int SoLuong { get; set; }

        public decimal Gia { get; set; }

        [ForeignKey("MaDonHang")]
        public DonHang DonHang { get; set; }

        [ForeignKey("MaBaiViet")]
        public BaiViet BaiViet { get; set; }
    }
}
