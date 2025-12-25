using System;
using System.ComponentModel.DataAnnotations;

namespace SWAPFIT.Models
{
    public class ThongBao
    {
        [Key]
        public int Id { get; set; }

        public int MaNguoiDung { get; set; }

        [Required]
        public string NoiDung { get; set; } = "";

        public string? LienKet { get; set; }

        public bool DaXem { get; set; } = false;

        public DateTime NgayTao { get; set; } = DateTime.Now;

        public string? Loai { get; set; }  
        public int? ThamChieuId { get; set; } 
    }
}
