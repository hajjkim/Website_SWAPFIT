using System.ComponentModel.DataAnnotations;

namespace SWAPFIT.Models
{
    public class TheLoaiTinTuc
    {
        [Key]
        public int MaTheLoai { get; set; }

        [StringLength(255)]
        public string? TenTheLoai { get; set; }
    }
}
