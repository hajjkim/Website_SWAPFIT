using System.ComponentModel.DataAnnotations;

namespace SWAPFIT.Models
{
    public class UuDaiSanPham
    {
        [Key]
        public int MaUuDaiSanPham { get; set; }
        public int MaUuDai { get; set; }
        public UuDai UuDai { get; set; }

        public int MaDoCu { get; set; }

        public DoCu DoCu { get; set; }
    }

}
