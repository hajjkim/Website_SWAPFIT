using System.ComponentModel.DataAnnotations;

namespace SWAPFIT.Models
{
    public class UuDaiSanPham
    {
        [Key]
        public int MaUuDaiSanPham { get; set; }

        // Foreign key reference to UuDai
        public int MaUuDai { get; set; }

        // Navigation property to UuDai
        public UuDai UuDai { get; set; }

        // Foreign key reference to DoCu (product)
        public int MaDoCu { get; set; }

        // Navigation property to DoCu
        public DoCu DoCu { get; set; }
    }

}
