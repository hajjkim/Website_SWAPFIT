using SWAPFIT.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWAPFIT.Models
{
    public class BaoCaoTaiKhoan
    {
        public int Id { get; set; }

        // Foreign key to NguoiDung (the user who reported)
        public int MaNguoiBaoCao { get; set; }

        // Foreign key to NguoiDung (the user being reported)
        public int MaNguoiBiBaoCao { get; set; }

        public string LyDo { get; set; } = string.Empty;
        public string? MoTaChiTiet { get; set; }

        public DateTime NgayTao { get; set; } = DateTime.Now;

        // "Moi", "DangXuLy", "DaXuLy"
        public string TrangThai { get; set; } = "Moi";

        // Navigation properties
        [ForeignKey(nameof(MaNguoiBaoCao))]
        public NguoiDung? NguoiBaoCao { get; set; }

        [ForeignKey(nameof(MaNguoiBiBaoCao))]
        public NguoiDung? NguoiBiBaoCao { get; set; }

        // List of images associated with the report
        public List<BaoCaoTaiKhoanAnh>? BaoCaoTaiKhoanAnhs { get; set; }
    }
}
