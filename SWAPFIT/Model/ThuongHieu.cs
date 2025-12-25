using System.ComponentModel.DataAnnotations;

namespace SWAPFIT.Models
{
    public class ThuongHieu
    {
        [Key]
        public int MaThuongHieu { get; set; }

        [StringLength(255)]
        public string? TenThuongHieu { get; set; }

        public string? MoTa { get; set; }
    }
}
