using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWAPFIT.Models
{
    public class GioHang
    {
        [Key]
        public int MaGioHang { get; set; }

        [ForeignKey("NguoiDung")]
        public int MaNguoiDung { get; set; }

        public NguoiDung? NguoiDung { get; set; }

        public DateTime NgayCapNhat { get; set; } = DateTime.Now;

        public virtual ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; } = new List<ChiTietGioHang>();
    }
}
