using System.ComponentModel.DataAnnotations;

namespace SWAPFIT.Models
{
    public class DanhMuc
    {
        [Key]
        public int MaDanhMuc { get; set; }

        [Required]
        [StringLength(255)]
        public string TenDanhMuc { get; set; }

        public string? MoTa { get; set; }
    }
}
