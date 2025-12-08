using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWAPFIT.Models
{
    public class ChiTietGioHang
    {
        [Key]
        public int MaChiTiet { get; set; }

        public int MaGioHang { get; set; }
        [ForeignKey(nameof(MaGioHang))]
        public GioHang? GioHang { get; set; }

        public int MaBaiViet { get; set; }
        [ForeignKey(nameof(MaBaiViet))]
        public BaiViet? BaiViet { get; set; }

        public int SoLuong { get; set; } = 1;
    }
}
