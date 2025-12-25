using System.ComponentModel.DataAnnotations;

namespace SWAPFIT.Models
{
    public class PhuongThucThanhToan
    {
        [Key]
        public int MaPhuongThuc { get; set; }

        [StringLength(100)]
        public string? TenPhuongThuc { get; set; }

        [StringLength(255)]
        public string? MoTa { get; set; }
    }
}
