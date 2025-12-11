using System;
using System.ComponentModel.DataAnnotations;

namespace SWAPFIT.Models
{
    public class DanhGia
    {
        [Key]
        public int MaDanhGia { get; set; }

        public int MaNguoiDung { get; set; }

        public int MaDoCu { get; set; }

        public int? Diem { get; set; }

        public string? BinhLuan { get; set; }

        public DateTime? NgayDanhGia { get; set; }
    }
}
