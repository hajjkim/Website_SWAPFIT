using SWAPFIT.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWAPFIT.Models
{
    public class BaoCaoTaiKhoan
    {
        public int Id { get; set; }

        public int MaNguoiBaoCao { get; set; }

        public int MaNguoiBiBaoCao { get; set; }

        public string LyDo { get; set; } = string.Empty;
        public string? MoTaChiTiet { get; set; }

        public DateTime NgayTao { get; set; } = DateTime.Now;

        public string TrangThai { get; set; } = "Moi";

        [ForeignKey(nameof(MaNguoiBaoCao))]
        public NguoiDung? NguoiBaoCao { get; set; }

        [ForeignKey(nameof(MaNguoiBiBaoCao))]
        public NguoiDung? NguoiBiBaoCao { get; set; }

        public List<BaoCaoTaiKhoanAnh>? BaoCaoTaiKhoanAnhs { get; set; }
    }
}
