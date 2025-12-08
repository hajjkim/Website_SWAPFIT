using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWAPFIT.Models
{
    public class UuDai
    {
        [Key]
        public int MaUuDai { get; set; }

        [Required, StringLength(100)]
        public string TenUuDai { get; set; } = string.Empty;

        [StringLength(255)]
        public string? MoTa { get; set; }

        [StringLength(20)]
        public string LoaiUuDai { get; set; } = "PhanTram"; // PhanTram hoặc TienMat

        [Column(TypeName = "decimal(10,2)")]
        public decimal GiaTri { get; set; }

        [DataType(DataType.Date)]
        public DateTime NgayBatDau { get; set; }

        [DataType(DataType.Date)]
        public DateTime NgayKetThuc { get; set; }

        [StringLength(20)]
        public string TrangThai { get; set; } = "HoatDong";
        public string AnhBia { get; set; }
        public int? GioiHanSoLuong { get; set; }
    }
}
