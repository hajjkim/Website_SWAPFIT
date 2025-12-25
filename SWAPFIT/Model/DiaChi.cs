using System.ComponentModel.DataAnnotations;

namespace SWAPFIT.Models
{
    public class DiaChi
    {
        [Key]
        public int MaDiaChi { get; set; }

        public int MaNguoiDung { get; set; }

        [StringLength(100)]
        public string? Tinh { get; set; }

        [StringLength(100)]
        public string? Huyen { get; set; }

        [StringLength(100)]
        public string? Xa { get; set; }

        [StringLength(255)]
        public string? ChiTiet { get; set; }

        public bool LaMacDinh { get; set; } = false;

    }
}
