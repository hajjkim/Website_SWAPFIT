using System.ComponentModel.DataAnnotations;

namespace SWAPFIT.Models
{
    public class HinhAnh
    {
        [Key]
        public int MaHinhAnh { get; set; }

        public int MaDoCu { get; set; }

        [StringLength(255)]
        public string? DuongDan { get; set; }
    }
}
