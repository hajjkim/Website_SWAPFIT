using System.ComponentModel.DataAnnotations;

namespace SWAPFIT.Models
{
    public class YeuThich
    {
        [Key]
        public int MaYeuThich { get; set; }

        public int MaNguoiDung { get; set; }

        public int MaDoCu { get; set; }
    }
}
